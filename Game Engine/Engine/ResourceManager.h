#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>

class ResourceManager
{
public:
	sf::Texture PlayerTexture; /*!< Player Tank Texture*/

	sf::Texture EnemyTexture; /*!< Enemy Tank Texture*/

	/*Background Textures*/
	sf::Texture GrassTexture; /*!< Plain Grass*/

	sf::Texture SandTexture; /*!< Plain Sand*/

	//Sand Road Textures
	sf::Texture SandRoadCross; /*!< Sand Cross Road*/

	sf::Texture SandRoadEast; /*!< Sand Road from East to West*/

	sf::Texture SandRoadET; /*!< Sand Road from North to South with T juction to West*/

	sf::Texture SandRoadLLeftC; /*!< Sand Road Lower Left corner (North to East turn)*/

	sf::Texture SandRoadLRightC; /*!< Sand Road Lower Right corner (North to West turn)*/

	sf::Texture SandRoadNorth; /*!< Sand Road from North to South*/

	sf::Texture SandRoadNT; /*!< Sand Road from East To West with T junction to South*/

	sf::Texture SandRoadST; /*!< Sand Road from East To West with T junction to North*/

	sf::Texture SandRoadULeftC; /*!< Sand Road Upper Left corner (South to East turn)*/

	sf::Texture SandRoadURightC; /*!< Sand Road Upper Right corner (South to West turn)*/

	sf::Texture SandRoadWT; /*!< Sand Road from North to South with T juction to East*/

	/*Crate Textures*/
	sf::Texture WoodCrateTexture; /*!< Wooden Crate Texture*/

	sf::Texture MetalCrateTexture; /*!< Metal Crate Texture*/

	//Projectile Texture
	sf::Texture ProjectileTexture; /*!< Projectile Texture*/

	/*Sounds*/
	sf::SoundBuffer MoveSound; /*!< Movement Sound*/

	sf::SoundBuffer ShotSound; /*!< Firing Sound*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Constructor												
	///		-Loads textures and sounds from files and stores them as variables above
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	ResourceManager();
};

