#pragma once
#include "SFML/Graphics.hpp"
#include <cctype>
#include <fstream>
#include <iostream>
#include <string>
#include <cstdlib>
#include <vector>
#include <random>
#include "AIAgent.h"
#include "Obstacle.h"
#include "BackgroundTile.h"

#include "ResourceManager.h"
#include "SFML/System/Time.hpp"

class Scene
{
public:
	int AICount; /*!< Number of AI to generate*/

	int CrateCount; /*!< Number of crates to generate*/
	
	sf::Vector2f PlayerSpawn; /*!< Player Spawn Vector*/
	
	std::vector<char> BackgroundData; /*!< Background data as read from file to be used when generating background*/
	
	std::vector<float> CratePositions; /*!< Vector of Crate positions read from file*/
	
	std::vector<float> AIPositions; /*!< Vector of AI positions read from file*/

	
	std::vector<AIAgent*> AIAgents; /*!< All AI Agents in scene*/
	
	std::vector<Obstacle*> WorldObstacles; /*!< All obstacles in scene*/
	
	std::vector<sf::Sprite> BackgroundSprites; /*!< All Background sprites*/

	Scene();
	
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Load Scene												
	///		Responsible for loading scene information from files
	///																								
	///		-Takes a string for the file location of the scene information
	///		-Takes a string for the file location of the background information
	///		-Takes a string for the file location of the crate information
	///		-Takes a string for the file location of the AI Agent information
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void LoadScene(std::string SceneInfoLoc, std::string BackgroundInfoLoc, std::string CrateInfoLoc, std::string AIInfoLoc);
	
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Create Scene											
	///		Responsible for creating a scene using the currently loaded information
	///																								
	///		-Takes render window to draw the scene onto
	///		-Takes a resource manager for access to loaded texture and sound files needed when
	///		creating objects
	///		-Takes an sfml time for setting object timers
	///		-Takes a background tile to generate a new background from
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void CreateScene(sf::RenderWindow* window, ResourceManager* EngineResources, sf::Time CurrentTime, BackgroundTile* Background);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 CheckForChar											
	///		Responsible for checking line data passed into function for characters
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	bool CheckForChar(std::string LineData);
};

