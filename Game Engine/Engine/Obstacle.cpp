#include "pch.h"
#include "Obstacle.h"

Obstacle::Obstacle(ResourceManager* EngineResources, sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles,
	sf::Vector2f SpawnPosition)
{
	Sprite.setTexture(EngineResources->WoodCrateTexture);
	Sprite.setOrigin(sf::Vector2f(Sprite.getTexture()->getSize().x / 2, Sprite.getTexture()->getSize().y / 2));
	Sprite.setRotation(0);
	Sprite.setScale(sf::Vector2f(1.0f, 1.0f));
	setPosition(SpawnPosition);
	Sprite.setPosition(getPosition());


	ObjectX = getPosition().x;
	ObjectY = getPosition().y;
	Health = 0;
	RotationSpeed = 0;
	MoveSpeed = 0;

	//Check If spawned in other object
	for (int i = 0; i < WorldObstacles.size(); i++)
	{
		Obstacle* CurrentObstacle = WorldObstacles[i];
		if (CurrentObstacle->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()))
		{
			Sprite.setPosition(sf::Vector2f(rand() % (Window->getSize().x / 2), rand() % (Window->getSize().y / 2)));
		}
	}
}

void Obstacle::Update()
{
	setPosition(ObjectX, ObjectY);
}
