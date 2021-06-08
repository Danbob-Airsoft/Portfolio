#include "pch.h"
#include "AIAgent.h"

AIAgent::AIAgent(sf::RenderWindow* Window, ResourceManager* EngineResources, std::vector<AIAgent*> AIAgents, 
	std::vector<Obstacle*> WorldObstacles, sf::Time CurrentTime, sf::Vector2f SpawnPoint)
{
	srand(time(NULL));
	srand((unsigned)time(0));

	setPosition(SpawnPoint);
	Sprite.setPosition(getPosition());
	Sprite.setTexture(EngineResources->EnemyTexture);
	Sprite.setOrigin(Sprite.getTexture()->getSize().x / 2, Sprite.getTexture()->getSize().y / 2);
	Sprite.setScale(sf::Vector2f(1.0f, 1.0f));

	ShotSound.setBuffer(EngineResources->ShotSound);

	ObjectX = getPosition().x;
	ObjectY = getPosition().y;
	Health = 6;
	MoveSpeed = 0.0f;
	RotationSpeed = 1;
	ShotCooldown = sf::seconds(8.0f);
	TimetoNextShot = CurrentTime + ShotCooldown;

	Overlapping = false;
	OverlappingCheck(Window, WorldObstacles, AIAgents);
	while (Overlapping) 
	{
		OverlappingCheck(Window, WorldObstacles, AIAgents);
	}

}

void AIAgent::PlaySound(sf::Sound& SoundToPlay)
{
	if (SoundToPlay.getStatus() != 2)
	{
		SoundToPlay.play();
	}
}

void AIAgent::Respawn(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents)
{
	//Find new random position
	setPosition(sf::Vector2f(rand() % (Window->getSize().x / 2), rand() % (Window->getSize().y / 2)));
	Sprite.setPosition(getPosition());

	//Check for Spawn Overlaps
	Overlapping = false;
	OverlappingCheck(Window, WorldObstacles, AIAgents);
	while (Overlapping)
	{
		OverlappingCheck(Window, WorldObstacles, AIAgents);
	}

	//Set Health to full
	Health = 6;
}

void AIAgent::Shoot(std::vector<Projectile*> &Projectiles, ResourceManager* EngineResources, sf::Time CurrentTime)
{
	if (CurrentTime >= TimetoNextShot)
	{
		//Rotate to face player
		Rotate();

		sf::Vector2f CannonLocation = (getPosition() - (FindForwardVector() * 20.0f));

		Projectile* NewProjectile = new Projectile(EngineResources, CannonLocation);
		//Set Projectile forward vector
		NewProjectile->ForwardVector = FindForwardVector();
		NewProjectile->SpawnedVehicle = this;
		//Add to Projectiles Vector to be drawn each frame
		Projectiles.push_back(NewProjectile);
		//Increase time to next shot
		TimetoNextShot = CurrentTime + ShotCooldown;
		//Play Sound
		PlaySound(ShotSound);
	}
}

void AIAgent::Rotate()
{
	sf::Vector2f CurrentForwards = FindForwardVector() * 20.0f;

	float Angle = -atan2(Target->getPosition().x - Sprite.getPosition().x, Target->getPosition().y - Sprite.getPosition().y)
		* 180 /M_PI;
	//Rotate Agent
	Sprite.setRotation(Angle);
}

void AIAgent::OverlappingCheck(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents)
{
	//Check If spawned in other object
	for (int i = 0; i < WorldObstacles.size(); i++)
	{
		Obstacle* CurrentObstacle = WorldObstacles[i];
		if (CurrentObstacle->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()))
		{
			setPosition(sf::Vector2f(rand() % (Window->getSize().x / 2), rand() % (Window->getSize().y / 2)));
			Sprite.setPosition(getPosition());
			Overlapping = true;
			return;
		}
	}

	for (int i = 0; i < AIAgents.size(); i++)
	{
		AIAgent* CurrentAgent = AIAgents[i];
		if (CurrentAgent->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()) && CurrentAgent != this)
		{
			setPosition(sf::Vector2f(rand() % (Window->getSize().x / 2), rand() % (Window->getSize().y / 2)));
			Sprite.setPosition(getPosition());
			Overlapping = true;
			return;
		}
	}
	Overlapping = false;
}

sf::Vector2f AIAgent::FindForwardVector()
{
	sf::Vector2f Forwards;

	Forwards.x = sin(Sprite.getRotation() * M_PI / 180);
	Forwards.y = -cos(Sprite.getRotation() * M_PI / 180);

	return Forwards;
}


void AIAgent::TakeDamage(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents)
{
	Health -= 2;
	if (Health <= 0) 
	{
		std::cout << "Enemy is dead" << std::endl;
		Respawn(Window, WorldObstacles, AIAgents);
	}
}

void AIAgent::Think(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents, 
	std::vector<Projectile*> &Projectiles, ResourceManager* EngineResources, sf::Time CurrentTime)
{
	setPosition(ObjectX, ObjectY);
	Sprite.setPosition(getPosition());
	//Check for overlaps with Projectiles
	for (int i = 0; i < Projectiles.size(); i++) 
	{
		Projectile* CurrentProj = Projectiles[i];
		if (Sprite.getGlobalBounds().intersects(CurrentProj->ProjectileSprite.getGlobalBounds()) 
			&& CurrentProj->SpawnedVehicle != this)
		{
			CurrentProj->NeedsDestroying = true;
			TakeDamage(Window, WorldObstacles, AIAgents);
		}
	}

	Shoot(Projectiles, EngineResources, CurrentTime);

	ObjectX = getPosition().x;
	ObjectY = getPosition().y;
}
