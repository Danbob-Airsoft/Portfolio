#pragma once
#include "ResourceManager.h"
#include "SFML/Graphics/Color.hpp"
#include "IMGUI/imgui.h"
#include "IMGUI/imgui-SFML.h"
#include "Player.h"
#include "SFML/Graphics.hpp"
#include <SFML/Graphics/Color.hpp>
#include "Projectile.h"
#include <string>
#include <cstring>
#include "Renderable.h"

class EditorGUI
{
public:

	Renderable* GUITarget; /*!< The target for the GUI to pull its info from*/

	std::string TargetName; /*!< GUI targets name*/

	bool GUIWindowOpen; /*!< Designates if the window is open and should be drawn*/

	EditorGUI(); /*!< Empty Constructor*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///								GUI Inspector Creation											
	///		Creates an IMGUI layout for the inspector GUI
	///
	///		-Takes a bool which dictates if the GUI should be draw
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void CreateNewInspectorGUI(bool GUIOpen); /*!< GUI Builder*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///								GUI Inspector Toggle											
	///		Checks each Renderable object to find if it has been clicked and toggles the GUI target
	///		if the same object is pressed twice or switches the information displayed to the new
	///		GUI target
	///		-Takes a render window to convert mouse screen position to position in window
	///		-Takes a Player to compare mouse location to global bounds
	///		-Takes a vector of Obstacles to compare mouse location to global bounds of each
	///		-Takes a vector of AI Agents to compare mouse location to global bounds of each
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	bool GUIToggleCheck(sf::RenderWindow* window, Player* PlayerChar,
		std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents);
};

