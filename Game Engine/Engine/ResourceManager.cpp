#include "pch.h"
#include "ResourceManager.h"

ResourceManager::ResourceManager()
{
    //--------------------------- Tank Sprites ----------------------
    if (!PlayerTexture.loadFromFile("Sprites\\PlayerSprite.png"))
    {
        std::cout << "Failed to Load Player Texture" << std::endl;
    }
    if (!EnemyTexture.loadFromFile("Sprites\\EnemySprite.png"))
    {
        std::cout << "Failed to Load Enemy Texture" << std::endl;
    }
    //--------------------------- Background Sprites ----------------------
    if (!GrassTexture.loadFromFile("Sprites\\GrassTile.png"))
    {
        std::cout << "Failed to Load Grass Texture" << std::endl;
    }
    if (!SandTexture.loadFromFile("Sprites\\SandTile.png")) 
    {
        std::cout << "Failed to Load Dirt Texture" << std::endl;
    }
    //-------------------------- Sand Road Textures -----------------------
    if (!SandRoadCross.loadFromFile("Sprites\\SandRoadCross.png"))
    {
        std::cout << "Failed to Load Sand Road Cross Texture" << std::endl;
    }
    if (!SandRoadEast.loadFromFile("Sprites\\SandRoadEast.png"))
    {
        std::cout << "Failed to Load Sand Road East Texture" << std::endl;
    }
    if (!SandRoadET.loadFromFile("Sprites\\SandRoadET.png"))
    {
        std::cout << "Failed to Load Sand Road East T Junction Texture" << std::endl;
    }
    if (!SandRoadLLeftC.loadFromFile("Sprites\\SandRoadLLeftC.png"))
    {
        std::cout << "Failed to Load Sand Road Right Turn Texture" << std::endl;
    }
    if (!SandRoadLRightC.loadFromFile("Sprites\\SandRoadLRightC.png"))
    {
        std::cout << "Failed to Load Sand Road Right Turn Texture" << std::endl;
    }
    if (!SandRoadNorth.loadFromFile("Sprites\\SandRoadNorth.png"))
    {
        std::cout << "Failed to Load Sand Road North Texture" << std::endl;
    }
    if (!SandRoadNT.loadFromFile("Sprites\\SandRoadNT.png"))
    {
        std::cout << "Failed to Load Sand Road North T Texture" << std::endl;
    }
    if (!SandRoadST.loadFromFile("Sprites\\SandRoadST.png"))
    {
        std::cout << "Failed to Load Sand Road South T Texture" << std::endl;
    }
    if (!SandRoadULeftC.loadFromFile("Sprites\\SandRoadULeftC.png"))
    {
        std::cout << "Failed to Load Sand Road Upper Left Corner Texture" << std::endl;
    }
    if (!SandRoadURightC.loadFromFile("Sprites\\SandRoadURightC.png"))
    {
        std::cout << "Failed to Load Sand Road Upper Right Corner Texture" << std::endl;
    }
    if (!SandRoadWT.loadFromFile("Sprites\\SandRoadWT.png"))
    {
        std::cout << "Failed to Load Sand Road West T Texture" << std::endl;
    }

    //--------------------------- Crate Sprites ----------------------
    if (!WoodCrateTexture.loadFromFile("Sprites\\WoodCrate.png"))
    {
        std::cout << "Failed to Load Wooden Crate Texture" << std::endl;
    }
    if (!MetalCrateTexture.loadFromFile("Sprites\\MetalCrate.png"))
    {
        std::cout << "Failed to Load Metal Crate Texture" << std::endl;
    }

    //--------------------------- Projectile Sprites ----------------------
    if (!ProjectileTexture.loadFromFile("Sprites\\Projectile.png"))
    {
        std::cout << "Failed to Load Projectile Texture" << std::endl;
    }

    //--------------------------- Audio Sprites ----------------------
    if (!ShotSound.loadFromFile("Audio\\ShotSound.wav")) 
    {
        std::cout << "Failed to load Shot Sound" << std::endl;
    }
    if (!MoveSound.loadFromFile("Audio\\MovementSound.wav"))
    {
        std::cout << "Failed to load Movement Sound" << std::endl;
    }
}
