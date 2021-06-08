#include "pch.h"
#include "InputManager.h"
#include <SFML/Graphics.hpp>
#include <iostream>
#include "Player.h"

void InputManager::PollEvents(sf::RenderWindow* window, Player* PlayerChar, sf::Time CurrentTime, std::vector<Projectile*> &Projectiles, 
	ResourceManager* EngineResource, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents, EditorGUI* InspectorGUI)
{
	sf::Event event;
	//Check for Window Closed
	while (window->pollEvent(event))
	{
		ImGui::SFML::ProcessEvent(event);
		//Check if event is GUI Toggle
		if (!InspectorGUI->GUIToggleCheck(window,PlayerChar, WorldObstacles, AIAgents)) 
		{
			//Else
			//Check Keyboard events
			if (event.type == sf::Event::KeyPressed)
			{
				PlayerChar->Update(event.key.code, window, WorldObstacles, AIAgents, CurrentTime, Projectiles, EngineResource);
				break;
			}
			//Check if window closed
			else if (event.type == sf::Event::Closed)
			{
				window->close();
				break;
			}
			else
			{
				break;
			}
		}
		else 
		{
			break;
		}
	}
}
