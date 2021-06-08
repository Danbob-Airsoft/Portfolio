#include "pch.h"
#include "BackgroundTile.h"

void BackgroundTile::CreateSprite(ResourceManager* EngineResource, int Xpos, int Ypos, char SpriteData)
{
	sf::Texture* TileTexture = &EngineResource->GrassTexture;
	if (SpriteData == 'a') 
	{
		TileTexture = &EngineResource->GrassTexture;
	}
	else if(SpriteData == 'b')
	{
		TileTexture = &EngineResource->SandTexture;
	}
	else if (SpriteData == 'c')
	{
		TileTexture = &EngineResource->SandRoadCross;
	}
	else if (SpriteData == 'd')
	{
		TileTexture = &EngineResource->SandRoadEast;
	}
	else if (SpriteData == 'e')
	{
		TileTexture = &EngineResource->SandRoadET;
	}
	else if (SpriteData == 'f')
	{
		TileTexture = &EngineResource->SandRoadLLeftC;
	}
	else if (SpriteData == 'g')
	{
		TileTexture = &EngineResource->SandRoadLRightC;
	}
	else if (SpriteData == 'h')
	{
		TileTexture = &EngineResource->SandRoadNorth;
	}
	else if (SpriteData == 'i')
	{
		TileTexture = &EngineResource->SandRoadNT;
	}
	else if (SpriteData == 'j')
	{
		TileTexture = &EngineResource->SandRoadST;
	}
	else if (SpriteData == 'k')
	{
		TileTexture = &EngineResource->SandRoadULeftC;
	}
	else if (SpriteData == 'l')
	{
		TileTexture = &EngineResource->SandRoadURightC;
	}
	else if (SpriteData == 'm')
	{
		TileTexture = &EngineResource->SandRoadWT;
	}
	else 
	{
		std::cout << "Invalid value entered, using default grass" << std::endl;
		TileTexture = &EngineResource->GrassTexture;
	}


	TileSprite.setTexture(*TileTexture);
	TileSprite.setOrigin(TileTexture->getSize().x / 2, TileTexture->getSize().y / 2);
	TileSprite.setPosition(sf::Vector2f(Xpos, Ypos));
	TileSprite.setScale(1, 1);
	TileSprite.setRotation(0.0f);
}

std::vector<sf::Sprite> BackgroundTile::GenerateBackground(sf::RenderWindow* window, ResourceManager* EngineResources, std::vector<char> BackgroundData)
{
	std::vector<sf::Sprite> BackgroundSprites;
	//Create Background
	for (int ypos = 1; ypos <= (window->getSize().y / EngineResources->GrassTexture.getSize().x + 1); ypos++)
	{
		for (int xpos = 1; xpos <= (window->getSize().x / EngineResources->GrassTexture.getSize().y + 1); xpos++)
		{
			//Get data for sprite level with how many sprites are already made
			char SpriteData;
			//If size of imported background is smaller
			if (BackgroundData.size() != BackgroundSprites.size())
			{
				SpriteData = std::tolower(BackgroundData[BackgroundSprites.size()]);
			}
			//Fill remaining tiles with grass
			else
			{
				SpriteData = 'a';
			}

			BackgroundTile* NewTile = new BackgroundTile();
			NewTile->CreateSprite(EngineResources, xpos * 32, ypos * 32, SpriteData);
			BackgroundSprites.push_back(NewTile->TileSprite);
		}
	}

	return BackgroundSprites;
}

