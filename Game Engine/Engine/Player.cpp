#include "pch.h"
#include "Player.h"

Player::Player()
{
}

void Player::ResetPlayer(ResourceManager* EngineResources, sf::RenderWindow* Window, sf::Time CurrentTime, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents, sf::Vector2f SpawnPoint)
{
    {
        setPosition(SpawnPoint);
        Sprite.setPosition(SpawnPoint);
        Sprite.setTexture(EngineResources->PlayerTexture);
        Sprite.setOrigin(sf::Vector2f(Sprite.getTexture()->getSize().x / 2, Sprite.getTexture()->getSize().y / 2));
        Sprite.setScale(sf::Vector2f(1.0f, 1.0f));

        ID = 1;
        
        ObjectX = getPosition().x;
        ObjectY = getPosition().y;

        MoveSpeed = 5;
        RotationSpeed = 4;
        Health = 50;

        GamePaused = false;
        IsDead = false;

        TimetoNextShot = CurrentTime;
        ShotCooldown = sf::seconds(2.0f);


        //Audio Setup
        ShotSound.setBuffer(EngineResources->ShotSound);
        MoveSound.setBuffer(EngineResources->MoveSound);
        MoveSound.setVolume(20);


        //Check If spawned in other object
        for (int i = 0; i < WorldObstacles.size(); i++)
        {
            Obstacle* CurrentObstacle = WorldObstacles[i];
            if (CurrentObstacle->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()))
            {
                CurrentObstacle->Sprite.setPosition(sf::Vector2f(rand() % (Window->getSize().x / 2), rand() % (Window->getSize().y / 2)));
            }
        }

        for (int i = 0; i < AIAgents.size(); i++)
        {
            AIAgent* CurrentAgent = AIAgents[i];
            //Set AI target to player
            CurrentAgent->Target = &Sprite;
            //Check if Spawned on AI
            if (CurrentAgent->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()))
            {
                //Relocate AI if so
                CurrentAgent->Sprite.setPosition(sf::Vector2f(rand() % (Window->getSize().x / 2), rand() % (Window->getSize().y / 2)));
            }
        }

    }
}

void Player::Update(sf::Keyboard::Key Key, sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents,
    sf::Time CurrentTime, std::vector<Projectile*>& Projectiles, ResourceManager* EngineResource)
{
    setPosition(ObjectX, ObjectY);
    Sprite.setPosition(getPosition());
    //Check if paused
    if (GamePaused == false) 
    {
        switch (Key)
        {
            //----------------------------- Unknown Key -----------------------------
        case sf::Keyboard::Unknown:
            break;

            //------------------------------- Movement --------------------------------
        case sf::Keyboard::W:
            //Store current position
            CurrentPosition = getPosition();
            //Move Forward
            setPosition(getPosition() - (MoveSpeed * FindForwardVector()));
            PlaySound(MoveSound);
            if (CollisionCheck(Window, WorldObstacles, AIAgents)) 
            {
                setPosition(CurrentPosition + (MoveSpeed * FindForwardVector()));
            }
            break;

        case sf::Keyboard::A:
            Rotate(-RotationSpeed);
            PlaySound(MoveSound);
            break;

        case sf::Keyboard::S:
            //Store current position
            CurrentPosition = getPosition();
            //Move Backwards
            setPosition(Sprite.getPosition() + (MoveSpeed * FindForwardVector()));
            PlaySound(MoveSound);
            if (CollisionCheck(Window, WorldObstacles, AIAgents)) 
            {
                setPosition(CurrentPosition - (MoveSpeed * FindForwardVector()));
            }
            break;

        case sf::Keyboard::D:
            Rotate(RotationSpeed);
            PlaySound(MoveSound);
            break;

            //------------------------------- Alternate Movement -------------------------------
        case sf::Keyboard::Up:
            //Store current position
            CurrentPosition = Sprite.getPosition();
            //Move Forward
            Sprite.setPosition(Sprite.getPosition() - (MoveSpeed * FindForwardVector()));
            PlaySound(MoveSound);
            if (CollisionCheck(Window, WorldObstacles, AIAgents)) 
            {
                setPosition(CurrentPosition + (MoveSpeed * FindForwardVector()));
            }
            break;

        case sf::Keyboard::Down:
            //Store current position
            CurrentPosition = Sprite.getPosition();
            //Move Backwards
            Sprite.setPosition(Sprite.getPosition() + (MoveSpeed * FindForwardVector()));
            PlaySound(MoveSound);
            if (CollisionCheck(Window, WorldObstacles, AIAgents)) 
            {
                setPosition(CurrentPosition - (MoveSpeed * FindForwardVector()));
            }
            break;

        case sf::Keyboard::Left:
            Rotate(-RotationSpeed);
            PlaySound(MoveSound);
            break;

        case sf::Keyboard::Right:
            Rotate(RotationSpeed);
            PlaySound(MoveSound);
            break;

            //------------------------------------------ Shooting -----------------------------------------
        case sf::Keyboard::Space:
            Shoot(Projectiles, EngineResource, CurrentTime);
            break;
        }
    }

    //------------------------Game Paused --------------------------------
    //Must be checked outside switchcase as it can be done when game is paused
    if (Key == sf::Keyboard::P) 
    {
        if (GamePaused == false) 
        {
            std::cout << "Game Paused" << std::endl;
            GamePaused = true;
        }
        else
        {
            std::cout << "Game Resumed" << std::endl;
            GamePaused = false;
        }
    }

    ObjectX = getPosition().x;
    ObjectY = getPosition().y;

}

sf::Vector2f Player::FindForwardVector()
{
    sf::Vector2f Forward;

    Forward.x = sin(Sprite.getRotation() * M_PI /180);
    Forward.y = -cos(Sprite.getRotation() * M_PI / 180);

    return Forward;
}

void Player::Shoot(std::vector<Projectile*>& Projectiles, ResourceManager* EngineResource, sf::Time CurrentTime)
{
    if (CurrentTime >= TimetoNextShot) 
    {
        sf::Vector2f CannonLocation = (getPosition() - (FindForwardVector() * 47.0f));

        Projectile* NewProjectile = new Projectile(EngineResource, CannonLocation);
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


void Player::Rotate(float Rotation)
{
    Sprite.rotate(Rotation);
}

void Player::PlaySound(sf::Sound &SoundToPlay)
{
    if (SoundToPlay.getStatus() != 2)
    {
        SoundToPlay.play();
    }
}

void Player::TakeDamage(float DamageToTake)
{
    Health -= DamageToTake;
    std::cout << Health << std::endl;
    if (Health <= 0) 
    {
        std::cout << "Player is Dead" << std::endl;
        IsDead = true;
    }
}

void Player::CheckforHit(std::vector<Projectile*>& Projectiles)
{
    for (int i = 0; i < Projectiles.size(); i++) 
    {
        Projectile* CurrentProj = Projectiles[i];
        if (CurrentProj->ProjectileSprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()) 
            && CurrentProj->SpawnedVehicle != this)
        {
            TakeDamage(1);
            CurrentProj->NeedsDestroying = true;
        }
    }
}

bool Player::CollisionCheck(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents)
{
    //Check if outside barrier
    if (getPosition().x < (0 + Sprite.getTexture()->getSize().x / 4)
        || getPosition().y < (0 + Sprite.getTexture()->getSize().y / 4)
        || getPosition().x >(Window->getSize().x / 2) - (Sprite.getTexture()->getSize().x / 3)
        || getPosition().y >(Window->getSize().y / 2) - (Sprite.getTexture()->getSize().y / 3))
    {
        return true;
    }
    //Otherwise check position against world obstacles
    else
    {
        for (int i = 0; i < WorldObstacles.size(); i++)
        {
            Obstacle* CurrentObstacle = WorldObstacles[i];
            if (CurrentObstacle->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()))
            {
                return true;
            }
        }

        for (int i = 0; i < AIAgents.size(); i++)
        {
            AIAgent* CurrentAgent = AIAgents[i];
            if (CurrentAgent->Sprite.getGlobalBounds().intersects(Sprite.getGlobalBounds()))
            {
                return true;
            }
        }
    }
    return false;
}



