#include "pch.h"
#include "Scene.h"

Scene::Scene()
{
}

void Scene::LoadScene(std::string SceneInfoLoc, std::string BackgroundInfoLoc, std::string CrateInfoLoc, std::string AIInfoLoc)
{
	//Clear any previous scene data
	AIAgents.clear();
	WorldObstacles.clear();
	BackgroundSprites.clear();
	BackgroundData.clear();
	AIPositions.clear();
	CratePositions.clear();

	//Create new file streams
	std::ifstream SceneFile;
	std::ifstream BackgroundFile;
	std::ifstream AISpawnFile;
	std::ifstream CrateSpawnFile;

	//Check for scene data file
	SceneFile.open(SceneInfoLoc);
	//If found,
	if (SceneFile.is_open())
	{
		//Create Variable to store line data
		std::string LineData;
		//Set a line counter
		int Linecount = 0;
		//Pass through each line of the file
		while (std::getline(SceneFile, LineData))
		{
			//Alternating lines contain headers and then data
			Linecount++;
			//First Data is crate count
			if (Linecount == 2)
			{
				//Check if Lines data contains non numbers
				if (!CheckForChar(LineData))
				{
					//Use Line data for crate count
					int CrateCountIn = std::stoi(LineData);
					CrateCount = CrateCountIn;
				}
				else
				{
					//Scene file setup incorrectly or no data, use defaults
					std::cout << "AI Count invalid in scene data, using default" << std::endl;
					CrateCount = 10;
				}
			}
			//Second data is enemy count
			else if (Linecount == 4)
			{
				//Check if Lines data contains non numbers
				if (!CheckForChar(LineData))
				{
					//Use Line data for AI count
					int AICountIn = std::stoi(LineData);
					AICount = AICountIn;
				}
				else
				{
					//Scene file setup incorrectly or no data, use defaults
					std::cout << "AI Count invalid in scene data, using default" << std::endl;
					AICount = 4;
				}
			}
			//3rd data is players start position x
			else if (Linecount == 6)
			{
				//Check if Lines data contains non numbers
				if (!CheckForChar(LineData))
				{
					//Use Line data for Player X pos
					int XStart = std::stoi(LineData);
					PlayerSpawn.x = XStart;
				}
				else
				{
					//Scene file setup incorrectly or no data, use defaults
					std::cout << "Player X Position invalid in scene data, using default" << std::endl;
					PlayerSpawn.x = 200;
				}
			}
			//4th data is players start position y
			else if (Linecount == 8)
			{
				//Check if Lines data contains non numbers
				if (!CheckForChar(LineData))
				{
					//Use Line data for Player Y pos
					int YStart = std::stoi(LineData);
					PlayerSpawn.y = YStart;
				}
				else
				{
					//Scene file setup incorrectly or no data, use defaults
					std::cout << "Player Y Position invalid in scene data, using default" << std::endl;
					PlayerSpawn.y = 200;
				}
			}
		}
		//Close file when complete
		SceneFile.close();
	}
	//If file can't be found, use default values
	else
	{
		std::cout << "No scene data found. Using defaults" << std::endl;
		CrateCount = 10;
		AICount = 4;
		PlayerSpawn = sf::Vector2f(200.0f, 200.0f);
	}

	//Background data
	BackgroundFile.open(BackgroundInfoLoc);
	if (BackgroundFile.is_open())
	{
		std::string LineData;
		//Read file line by line
		while (std::getline(BackgroundFile, LineData))
		{
			//For each value on line
			for (int i = 0; i < LineData.size(); i++)
			{
				//Get indivdual character
				char Data = LineData.at(i);
				if (std::isalpha(Data))
				{
					//Add to Background data vector
					BackgroundData.push_back(Data);
				}
				else
				{
					//Scene file setup incorrectly or no data, use defaults
					std::cout << "Incorrect data type" << std::endl;
					BackgroundData.push_back('a');
				}
			}
		}
		//Close File
		BackgroundFile.close();
	}
	else
	{
		std::cout << "No Background data found. Using defaults" << std::endl;
		//Default data
	}

	//Crate Spawn Data
	CrateSpawnFile.open(CrateInfoLoc);
	//If file found
	if (CrateSpawnFile.is_open())
	{
		std::string LineData;
		int Linecount = 0;
		//Read Each Line
		while (std::getline(CrateSpawnFile, LineData))
		{
			Linecount++;
			//Ignore first line as it contains instructions and ignore blank lines
			if (Linecount > 1 && LineData != "")
			{
				//Check if data is only digits
				if (!CheckForChar(LineData))
				{
					//No characters found
					//Add to vector of coords
					CratePositions.push_back(std::stoi(LineData));
				}
				else
				{
					std::cout << "Line Contains None Digits... Ignoring Line Data" << std::endl;
				}
			}
		}
	}
	else
	{
		std::cout << "No Crate Data Detected, using random locations" << std::endl;
	}


	//AI Spawn Data
	AISpawnFile.open(AIInfoLoc);
	if (AISpawnFile.is_open())
	{
		int Linecount = 0;
		std::string LineData;
		//Read Each Line
		while (std::getline(AISpawnFile, LineData))
		{
			Linecount++;
			//Ignore first line as it contains instructions and ignore blank lines
			if (Linecount > 1 && LineData != "")
			{
				//Check if data is only digits
				if (!CheckForChar(LineData))
				{
					//No characters found
					//Add to vector of coords
					AIPositions.push_back(std::stoi(LineData));
				}
				else
				{
					std::cout << "Line Contains None Digits... Ignoring Line Data" << std::endl;
				}
			}
		}
	}
	else
	{
		std::cout << "No AI Data Detected, using random locations" << std::endl;
	}
	std::cout << BackgroundSprites.size() << std::endl;
}

void Scene::CreateScene(sf::RenderWindow* window, ResourceManager* EngineResources, sf::Time CurrentTime, BackgroundTile* Background)
{
	BackgroundSprites = Background->GenerateBackground(window, EngineResources, BackgroundData);
	srand(time(NULL));
	srand((unsigned)time(0));
	//------------------------------------ Create Obstacles -----------------------------
	float NumberOfCratePos = (float)CratePositions.size() / 2.0f;
	for (float i = 0; i < CrateCount; i++)
	{
		sf::Vector2f SpawnPoint;
		if (i < NumberOfCratePos)
		{
			SpawnPoint.x = CratePositions[i];
		}
		else
		{
			std::cout << "Missing X Co-ord for crate, Using Random" << std::endl;
			SpawnPoint.x = (rand() % (window->getSize().y / 2));
		}

		if (i <= (NumberOfCratePos)-1)
		{
			SpawnPoint.y = CratePositions[i + 1];
		}
		else
		{
			std::cout << "Missing Y Co-ord for crate, Using Random" << std::endl;
			SpawnPoint.y = (rand() % (window->getSize().y / 2));
		}

		Obstacle* CurrentObstacle = new Obstacle(EngineResources, window, WorldObstacles, SpawnPoint);
		CurrentObstacle->ID = 10 + i;
		WorldObstacles.push_back(CurrentObstacle);
	}

	//------------------------------------- Create AI -----------------------------------
	float NumberOfAIPos = (float)AIPositions.size() / 2.0f;
	for (int i = 0; i < AICount; i++)
	{
		sf::Vector2f SpawnPoint;
		if (i < NumberOfAIPos)
		{
			SpawnPoint.x = AIPositions[i];
		}
		else
		{
			std::cout << "Missing X Co-ord for agent, Using Random" << std::endl;
			SpawnPoint.x = (rand() % (window->getSize().x / 2));
		}

		if (i <= (NumberOfAIPos)-1)
		{
			SpawnPoint.y = AIPositions[i + 1];
		}
		else
		{
			std::cout << "Missing Y Co-ord for agent, Using Random" << std::endl;
			SpawnPoint.y = (rand() % (window->getSize().y / 2));
		}

		AIAgent* CurrentAgent = new AIAgent(window, EngineResources, AIAgents, WorldObstacles,
			CurrentTime, SpawnPoint);
		CurrentAgent->ID = 50 + i;
		AIAgents.push_back(CurrentAgent);
	}
}

bool Scene::CheckForChar(std::string LineData)
{
	//Scan through line data
	for (int i = 0; i < LineData.size(); i++) 
	{
		char CurrentChar = LineData[i];
		//If the current Char is not a digit
		if (!std::isdigit(CurrentChar)) 
		{
			//Return true to use defaults
			return true;
		}
	}
	return false;
}
