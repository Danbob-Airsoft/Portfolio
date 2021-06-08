#pragma once
#include <SFML/Graphics.hpp>
#include "ResourceManager.h"
#include "Obstacle.h"
#include "Renderable.h"

class Projectile
{
public:
	sf::Sprite ProjectileSprite; /*!< Projectile Sprite*/

	Renderable* SpawnedVehicle; /*!< Reference to Renderable that fired projectile*/

	sf::Vector2f ForwardVector; /*!< Forward Vector to travel on*/

	bool NeedsDestroying; /*!< Indicator if Projectile needs to be destroyed*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Constructor												
	///		Responsible for constructing projectiles
	///																								
	///		-Takes a Resource Manager to access loaded sprites
	///		-Takes a vector2f for projectile spawn location
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	Projectile(ResourceManager* EngineResource, sf::Vector2f SpawnPoint);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///									  Deconstructor												
	///		Responsible deconstructing projectile
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	~Projectile();

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///											Think												
	///		Responsible for playing a given sound
	///																								
	///		-Takes a render window for checking if projectile is outside window
	///		-Takes a vector of Obstacles for checking if global bounds intersect
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Think(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles);
};

