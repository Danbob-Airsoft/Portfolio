// Random Engine.cpp : This file contains the 'main' function. Program execution begins and ends there.
#include "pch.h"
#include <iostream>
#include <SFML/Graphics.hpp>
#include "Renderer.h"
#include "InputManager.h"
#include "Player.h"
#include "ResourceManager.h"
#include "AIAgent.h"
#include <chrono>
#include <SFML/System/Time.hpp>
#include "Obstacle.h"
#include "Scene.h"

#include "IMGUI/imgui.h"
#include "IMGUI/imgui-SFML.h"

int main()
{
	//-------------------------------------- Start Time ------------------------------
	sf::Clock ProgramClock;
	sf::Clock DeltaClock;
	sf::Time CurrentTime;

	//---------------------------------------Manager setups----------------------------
	Scene* BattleScene = new Scene();
	BattleScene->LoadScene("SceneData\\SceneInfo.txt", "SceneData\\BackgroundInfo.txt", "SceneData\\CrateSpawnInfo.txt", "SceneData\\AISpawnInfo.txt");
	ResourceManager* EngineResources = new ResourceManager();
	Renderer* EngineRenderer = new Renderer();
	BackgroundTile* Background = new BackgroundTile();
	InputManager* EngineInput = new InputManager();

	//---------------------------------------Window Creation------------------------------
	sf::RenderWindow* window = EngineRenderer->CreateWindow(Background, EngineResources, BattleScene->BackgroundData);

	BattleScene->CreateScene(window, EngineResources, CurrentTime, Background);

	//------------------------------------- Other Setup ----------------------------------
	Player* PlayerCharacter = new Player();
	PlayerCharacter->ResetPlayer(EngineResources, window, CurrentTime, BattleScene->WorldObstacles,
		BattleScene->AIAgents, BattleScene->PlayerSpawn);

	//---------------------------------Main Game Loop-------------------------------------
	while (window->isOpen())
	{
		//Update Time
		CurrentTime = ProgramClock.getElapsedTime();
	

		//Update Window
		EngineRenderer->UpdateWindow(window, EngineInput, PlayerCharacter, EngineResources, Background, CurrentTime, DeltaClock, BattleScene);

		//Display new Window
		EngineRenderer->DisplayWindow(window);
	}
	ImGui::SFML::Shutdown();
	return 0;
}
