#pragma once
#include <SFML/Graphics.hpp>
#include "Player.h"
#include "Projectile.h"
#include "Obstacle.h"
#include "EditorGUI.h"

#include "IMGUI/imgui.h"
#include "IMGUI/imgui-SFML.h"

const class InputManager
{
public:
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Poll events											
	///		Responsible for polling Keyboard or Mouse Events and passes events where needed
	///																								
	///		-Takes a render window to poll events from
	///		-Takes a Player to call update from on keyboard events
	///		-Takes an sfml time to pass to objects where needed for updating with inputs
	///		-Takes a vector of projectiles to pass to player if needed with input
	///		-Takes a Resource manager to pass to objects which need for updating
	///		-Takes a vector of projectiles to pass to objects where needed
	///		-Takes a vector of AI agents to pass to objects where needed
	///		-Takes a reference to an Editor GUI for GUI events
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void PollEvents(sf::RenderWindow* window, Player* PlayerChar, sf::Time CurrentTime, std::vector<Projectile*> &Projectiles, 
		ResourceManager* EngineResource, std::vector<Obstacle*> WorldObstacles , std::vector<AIAgent*> AIAgents, EditorGUI* InspectorGUI);
};

