#pragma once
#include <SFML/Graphics.hpp>
#include "InputManager.h"
#include "Player.h"
#include "ResourceManager.h"
#include <iostream>
#include "Backgroundtile.h"
#include <vector>
#include "Projectile.h"
#include "AIAgent.h"
#include <SFML/System/Time.hpp>
#include "Obstacle.h"

#include "EditorGUI.h"
#include "Scene.h"

#include "IMGUI/imgui.h"
#include "IMGUI/imgui-SFML.h"

class Renderer
{
public:

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Window Creator											
	///		Responsible for creating the renderer window
	///																								
	///		-Takes a Resource Manager to access loaded sprites
	///		-Takes a vector2f for projectile spawn location
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	sf::RenderWindow* CreateWindow(BackgroundTile* Background, ResourceManager* EngineResource, std::vector<char> BackgroundData);
	
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Window Update											
	///		Responsible for updating the window
	///																								
	///		-Takes a window to update
	///		-Takes an input manager to handle window events
	///		-Takes a player character to update and draw in the window
	///		-Takes a resource manager to pass to objects that require it
	///		-Takes a resource manager to pass to objects that require it
	///		-Takes a background to pass to new scene creation
	///		-Takes an SFML time to pass to objects that require it
	///		-Takes an SFML clock to pass to objects that require the time since the last frame
	///		-Takes a scene to load object information from
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void UpdateWindow(sf::RenderWindow* window, InputManager* EngineInput, Player* PlayerChar, ResourceManager* EngineResource, 
		BackgroundTile* Background, sf::Time CurrentTime, sf::Clock DeltaClock, Scene* BattleScene);
	
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Window Display											
	///		Responsible for displaying the window
	///																								
	///		-Takes a window to display
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void DisplayWindow(sf::RenderWindow* window);

private:
	
	std::vector<Projectile*> Projectiles; /*!< All Projectiles in scene*/
	
	sf::RenderWindow* window; /*!< Scene window*/
	
	EditorGUI* WindowGUI; /*!< Scene GUI Editor*/
};

