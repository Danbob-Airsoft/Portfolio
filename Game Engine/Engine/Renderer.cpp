#include "pch.h"
#include "Renderer.h"

sf::RenderWindow* Renderer::CreateWindow(BackgroundTile* Background, ResourceManager* EngineResource, std::vector<char> BackgroundData)
{
	//Create Window
	sf::RenderWindow* NewWindow = new sf::RenderWindow(sf::VideoMode(1900, 1000), "The Random Engine", sf::Style::Titlebar | sf::Style::Close);
	
	//IMGUI Creation
	ImGui::SFML::Init(*NewWindow);
	WindowGUI = new EditorGUI();
	WindowGUI->GUIWindowOpen = false;

	return NewWindow;
}

void Renderer::UpdateWindow(sf::RenderWindow* window, InputManager* EngineInput, Player* PlayerChar, ResourceManager* EngineResource, 
	BackgroundTile* Background, sf::Time CurrentTime, sf::Clock DeltaClock, Scene* BattleScene)
{
	//-------------------------------------- Clear the window ----------------------------------
	window->clear();

	//-------------------------------------- Call Input Check ----------------------------------
	EngineInput->PollEvents(window, PlayerChar, CurrentTime, Projectiles ,EngineResource, BattleScene->WorldObstacles, BattleScene->AIAgents, WindowGUI);

	//-------------------------------------- Draw Background -----------------------------------
	for (int Sprite = 0; Sprite < BattleScene->BackgroundSprites.size(); Sprite++)
	{
		sf::Sprite CurrentTile = BattleScene->BackgroundSprites.at(Sprite);
		window->draw(CurrentTile, CurrentTile.getTransform());
	}

	//-------------------------------------- Call Player Draw ----------------------------------
	window->draw(PlayerChar->Sprite, PlayerChar->getTransform());

	//-------------------------------------- Call Renderables Updates --------------------------
	//For each AI in Vector of agents
	for (int i = 0; i < BattleScene->AIAgents.size(); i++)
	{
		//Get Current AI
		AIAgent* CurrentAgent = BattleScene->AIAgents[i];
		//Check if paused
		if (!PlayerChar->GamePaused)
		{
			//If Not
			//Update Agent
			CurrentAgent->Think(window, BattleScene->WorldObstacles, BattleScene->AIAgents, Projectiles, EngineResource, CurrentTime);
		}

		//Draw Agent to window
		window->draw(CurrentAgent->Sprite, CurrentAgent->getTransform());

	}

	//---------------------------------------- Draw Obstacles -----------------------------------
	for (int i = 0; i < BattleScene->WorldObstacles.size(); i++)
	{
		Obstacle* CurrentObstacle = BattleScene->WorldObstacles[i];
		//Update position incase of GUI change to positions
		CurrentObstacle->Update();
		//Draw
		window->draw(CurrentObstacle->Sprite, CurrentObstacle->Sprite.getTransform());
	}

	//-------------------------------------- Draw Projectiles ----------------------------------
	//For each projectile in Projectiles vector
	for (int i = 0; i < Projectiles.size(); i++)
	{
		//Get Current Shell
		Projectile* Shell = Projectiles[i];
		if (!PlayerChar->GamePaused)
		{
			//Update Shell
			Shell->Think(window, BattleScene->WorldObstacles);
		}
		//Check if shell needs destroying
		if (Shell->NeedsDestroying)
		{
			//Call Deconstructor
			Shell->~Projectile();
			//Remove from Vector
			Projectiles.erase(Projectiles.begin() + i);

		}
		else
		{
			//Draw Shell
			window->draw(Shell->ProjectileSprite, Shell->ProjectileSprite.getTransform());
		}
	}

	//---------------------------------------- Check for Player hit -----------------------------
	PlayerChar->CheckforHit(Projectiles);
	//---------------------------------------- Check for Player Death ---------------------------
	if (PlayerChar->IsDead)
	{
		//Empty Projectiles
		Projectiles.clear();

		//Reload Scene OR load new scene data
		BattleScene->LoadScene("SceneData\\SceneInfo.txt", "SceneData\\BackgroundInfo.txt", "SceneData\\CrateSpawnInfo.txt", "SceneData\\AISpawnInfo.txt");
		//Create Scene
		BattleScene->CreateScene(window, EngineResource, CurrentTime, Background);
		//Reset Player Data
		PlayerChar->ResetPlayer(EngineResource, window, CurrentTime, BattleScene->WorldObstacles, BattleScene->AIAgents, BattleScene->PlayerSpawn);
	}

	//----------------------------------------- GUI -----------------------------------------------
	if (WindowGUI->GUIWindowOpen) 
	{
		//---------------------------------------- Update GUI -------------------------------------
		ImGui::SFML::Update(*window, DeltaClock.restart());

		//---------------------------------------- Draw GUI ---------------------------------------
		WindowGUI->CreateNewInspectorGUI(WindowGUI->GUIWindowOpen);


		ImGui::SFML::Render(*window);
	}
}

void Renderer::DisplayWindow(sf::RenderWindow* window)
{
	//Display new Window
	window->display();
}
