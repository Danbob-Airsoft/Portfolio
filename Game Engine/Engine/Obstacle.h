#pragma once
#include "ResourceManager.h"
#include "SFML/Graphics.hpp"
#include "Renderable.h"

class Obstacle: public Renderable
{
public:
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///	Obstacle inherits from Renderable class to allow access to Health, MoveSpeed,	
	///	Rotation Speed, ID, X and Y positions that can be accessed by the GUI and Shot times
	///////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Constructor												
	///		-Takes a Resource manager to gain access to loaded sprites
	///		-Takes a render window incase a random position needs to be generated
	///		-Takes a vector of world obstacles to check for intersections with on spawn
	///		-Takes a vector2f for initial spawn position, which may change if overlapping with an
	///		existing object
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	Obstacle(ResourceManager* EngineResources, sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, 
		sf::Vector2f SpawnPosition); /*!< Constructor*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										  Update												
	///		Resets the sprite position to its renderable stored x and y, which are alterable by the
	///		GUI
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Update();
};

