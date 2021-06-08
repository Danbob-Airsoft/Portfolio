#pragma once
#define _USE_MATH_DEFINES
#include "SFML/Graphics.hpp"
#include <math.h>
#include <cmath>
#include "ResourceManager.h"
#include <SFML/System/Time.hpp>
#include "Projectile.h"
#include <SFML/Audio.hpp>
#include <SFML/Graphics/Transformable.hpp>
#include "Obstacle.h"
#include "AIAgent.h"
#include "Renderable.h"

class Player: public Renderable
{
public:
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///	Player inherits from Renderable class to allow access to Health, MoveSpeed, Rotation Speed, 
	///	ID, X and Y positions that can be accessed by the GUI and Shot times						
	///////////////////////////////////////////////////////////////////////////////////////////////////


	bool GamePaused; /*!< Keeps track of if the game is paused or running*/

	bool IsDead; /*!< Keeps track of players death status*/

	Player(); /*!< Empty Constructor*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Reset Player											
	///		Responsible for restoring the player to its starting values
	///																								
	///		-Takes a Resource manager to access sprites and sounds
	///		-Takes a window to generate new positions with windows size as a limit if AI or
	///		obstacles spawn overlapping the player
	///		-Takes SFML time to set up players stored time which dictates when it can next fire
	///		-Takes a vector of world objects to check global bounds for overlaps
	///		-Takes a vector of AI Agents to check global bounds for overlaps
	///		-Takes a vector2f for the players starting spawn point
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void ResetPlayer(ResourceManager* EngineResources, sf::RenderWindow* Window, sf::Time CurrentTime, std::vector<Obstacle*> WorldObstacles,
		std::vector<AIAgent*> AIAgents, sf::Vector2f SpawnPoint);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///											Update												
	///		Responsible for updating the player when specific inputs occur
	///																								
	///		-Takes an SFML key to determine which event to trigger
	///		-Takes a vector of world objects to check global bounds for overlaps
	///		-Takes a vector of AI Agents to check global bounds for overlaps
	///		-Takes a window to check if moving out of bounds or to pass to other functions
	///		-Takes SFML time to compare to players stored time which dictates when it can next fire
	///		-Takes a vector of projectiles to pass into shoot function
	///		-Takes a Resource manager to pass into shoot function
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Update(sf::Keyboard::Key Key, sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents, 
		sf::Time CurrentTime, std::vector<Projectile*>& Projectiles, ResourceManager* EngineResource);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///											Shoot												
	///		Responsible for checking if enough time has passed for the enemy to fire again and
	///		creating a new projectile if agent can fire
	///																								
	///		-Takes a Vector of projectiles to pass to new projectile
	///		-Takes a Resource manager to pass to new projectile
	///		-Takes sfml time to compare to agents stored time which dictates when it can next fire
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Shoot(std::vector<Projectile*> &Projectiles, ResourceManager* EngineResource, sf::Time CurrentTime);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///											Rotate												
	///		Responsible for rotating the player in a specific direction
	///																								
	///		-Takes a float which dictates how much to rotate the player by on each trigger
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Rotate(float Rotation); /*!< Rotates player by its rotation speed*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Play Sound												
	///		Responsible for playing a given sound
	///																								
	///		-Takes an sfml sound clip to play
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void PlaySound(sf::Sound &SoundToPlay); /*!< Plays designated sound*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Take Damage											
	///		Responsible for Reducing the players health
	///																								
	///		-Takes a float which dictates how much health to remove
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void TakeDamage(float DamageToTake);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										  Hit Check												
	///		Responsible for checking if a players bounds intersects a projectiles
	///																								
	///		-Takes a vector of projectiles to check for intersects with player global bounds
	///
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void CheckforHit(std::vector<Projectile*>& Projectiles);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///									Overlapping Check											
	///		Responsible for checking if a players bounds intersects another
	///																								
	///		-Takes a render window to use size as a maximum when generation new positions
	///		-Takes a Vector of World obstacles to check global bounds for intersections with agents
	///		global bounds
	///		-Takes a Vector of AI Agents to check global bounds for intersections with agents global
	///		bounds
	///
	///////////////////////////////////////////////////////////////////////////////////////////////////
	bool CollisionCheck(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents);

private:
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///									Find Forward Vector											
	///		Responsible for calculating the players forward vector based on it current rotation
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	sf::Vector2f FindForwardVector();

	sf::Sound ShotSound; /*!< Firing Sound*/

	sf::Sound MoveSound; /*!< Move Sound*/

	sf::Vector2f CurrentPosition; /*!< Position stored at the beginning of update in case of an overlap*/
};

