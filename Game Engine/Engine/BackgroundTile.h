#pragma once
#include "SFML/Graphics.hpp"
#include "ResourceManager.h"
#include <cctype>

class BackgroundTile
{
public:
	sf::Sprite TileSprite; /*!< Individual tile sprite*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Create Sprite											
	///		Responsible for creating a new sprite tile at a select position
	///																								
	///		-Takes a Resource manager to get tiles sprite texture
	///		-Takes an int for the tiles X position and an int for the tiles y position
	///		-Takes a character for sprite data which determines which sprite from the resource
	///		manager is used
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void CreateSprite(ResourceManager* EngineResource, int Xpos, int Ypos, char SpriteData);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///									Generate Background											
	///		Responsible for creating a vector of new sprites which form the scenes background
	///																								
	///		-Takes render window to calculate how many tiles need to be generated
	///		-Takes a Resource manager to pass to tile creation
	///		-Takes a vector of characters which are passed one by one into sprite creation as that
	///		sprites data
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	std::vector<sf::Sprite> GenerateBackground(sf::RenderWindow* window, ResourceManager* EngineResources, std::vector<char> BackgroundData);
};

