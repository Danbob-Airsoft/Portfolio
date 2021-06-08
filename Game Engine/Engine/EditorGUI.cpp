#include "pch.h"
#include "EditorGUI.h"

EditorGUI::EditorGUI()
{

}

void EditorGUI::CreateNewInspectorGUI(bool GUIOpen)
{
	//Create a new window
	ImGui::Begin("Object Inspector", &GUIOpen);
	ImGui::SetWindowFontScale(1.6);

	//Add GUI Target Name
	std::string NameString = "Object Name = " + TargetName;
	char const* NameChar = NameString.c_str();
	ImGui::TextColored(ImVec4(1, 0, 1, 1), NameChar);


	//Add GUI Target Xpos
	//float Xpos = GUITarget->getPosition().x;
	ImGui::InputFloat("X Pos", &GUITarget->ObjectX);

	//Add GUI Target Ypos
	ImGui::InputFloat("Y Pos", &GUITarget->ObjectY);

	//Add GUI Target Health
	ImGui::InputInt("TargetHealth", &GUITarget->Health);

	//Add GUI Target Move Speed
	ImGui::InputFloat("Move Speed", &GUITarget->MoveSpeed);

	//Add GUI Target Rotation Speed
	ImGui::InputFloat("Rotation Speed", &GUITarget->RotationSpeed);

	//Add GUI Target Cooldown Speed
	


	//Add GUI Target Sprite
	ImGui::Image(GUITarget->Sprite, sf::Color(255,255,255,255), sf::Color(0, 0, 0, 0));

	//End Gui Setup
	ImGui::End();

}

bool EditorGUI::GUIToggleCheck(sf::RenderWindow* window, Player* PlayerChar,
	std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents)
{
	auto MousePos = sf::Mouse::getPosition(*window);
	auto TranslatedPos = window->mapPixelToCoords(MousePos);

	//Check Player Character
	//Check if mouse is over and clicked
	if (PlayerChar->Sprite.getGlobalBounds().contains(TranslatedPos / 2.0f) && sf::Mouse::isButtonPressed(sf::Mouse::Left))
	{
		//If true, Check if GUI is enabled AND GUITarget is equal to clicked object
		if (GUIWindowOpen && PlayerChar == GUITarget)
		{
			//If same object has been clicked and GUI is active, disable GUI
			GUIWindowOpen = false;
		}
		else
		{
			//Else enable the GUI and Set target
			GUIWindowOpen = true;
			GUITarget = PlayerChar;
			TargetName = "Player";
		}
		//Return True
		return true;
	}

	//Else move on to check AI
	//Check if mouse is over and clicked
	else
	{
		for (int i = 0; i < AIAgents.size(); i++)
		{
			if (AIAgents[i]->Sprite.getGlobalBounds().contains(TranslatedPos / 2.0f) && sf::Mouse::isButtonPressed(sf::Mouse::Left))
			{
				//If true, Check if GUI is enabled
				if (GUIWindowOpen &&  AIAgents[i] == GUITarget)
				{
					GUIWindowOpen = false;
				}
				else
				{
					GUIWindowOpen = true;
					GUITarget = AIAgents[i];
					TargetName = "AI";
				}
				//Return True
				return true;
			}
		}
		
		//Else check Obstacles
		//Check if mouse is over and clicked
		for (int i = 0; i < WorldObstacles.size(); i++)
		{
			if (WorldObstacles[i]->Sprite.getGlobalBounds().contains(TranslatedPos / 2.0f) && sf::Mouse::isButtonPressed(sf::Mouse::Left))
			{
				//If true, Check if GUI is enabled
				if (GUIWindowOpen && WorldObstacles[i] == GUITarget)
				{
					GUIWindowOpen = false;
				}
				else 
				{
					GUIWindowOpen = true;
					GUITarget = WorldObstacles[i];
					TargetName = "Obstacle";
				}

				//Return True
				return true;
			}
		}
	}
	return false;
}
