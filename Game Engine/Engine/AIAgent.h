#pragma once
#define _USE_MATH_DEFINES
#include <SFML/Graphics.hpp>
#include "ResourceManager.h"
#include <random>
#include <iostream>
#include <SFML/System/Time.hpp>
#include <SFML/Audio.hpp>
#include <math.h>
#include <cmath>
#include "Obstacle.h"
#include "Renderable.h"
#include "Projectile.h"

class AIAgent: public Renderable
{
public:
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///	AIAgent inherits from Renderable class to allow access to Health, MoveSpeed, Rotation Speed,
	///	ID, X and Y positions that can be accessed by the GUI and Shot times
	///////////////////////////////////////////////////////////////////////////////////////////////////

	sf::Sprite* Target; /*!< Reference to Players sprite to be used when calculating rotation to face Player*/

	sf::Sound ShotSound; /*!< Sound to play when firing*/

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										Constructor												
	///		Reponsible for setup of new agents and adding agents to vector of agents in scene
	///																								
	///		-Takes a render window for generating random positions within window if agent spawns
	///		within an existing object
	///		-Takes Resource manager to access sounds and sprites
	///		-Takes Vector of AI agents to check for overlaps with existing agents on creation and
	///		to add constructed agent to vector
	///		-Takes Vector of world Obstacles to check for overlaps with existing obstacles
	///		-Takes Current time for shooting delay setup
	///		-Takes Vector2f for initial spawn location
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	AIAgent(sf::RenderWindow* Window, ResourceManager* EngineResources, std::vector<AIAgent*> AIAgents, 
		std::vector<Obstacle*> WorldObstacles, sf::Time CurrentTime, sf::Vector2f SpawnPoint);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Thinking												
	///		Responsible for the agents behaviour and updating the agent
	///																								
	///		-Takes a render window to pass into take damage function if needed for position
	///		generation
	///		-Takes Vector of world Obstacles to pass into take damage function if needed for
	///		position generation
	///		-Takes Vector of AI agents to pass into take damage function if needed for position
	///		generation
	///		-Takes a vector of projectiles to pass into shooting check
	///		-Takes a Resource Manager to pass into shooting check
	///		-Takes Current time to pass into shooting check
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Think(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents, 
		std::vector<Projectile*> &Projectiles, ResourceManager* EngineResources, sf::Time CurrentTime);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Play Sound												
	///		Responsible for playing a given sound
	///																								
	///		-Takes an sfml sound clip to play
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void PlaySound(sf::Sound& SoundToPlay);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Take Damage											
	///		Responsible for Reducing the agents health and checking if respawn is needed
	///																								
	///		-Takes a render window to pass into respawn function if needed for position
	///		generation
	///		-Takes Vector of world Obstacles to pass into respawn function if needed for position
	///		generation
	///		-Takes Vector of AI agents to pass into respawn function if needed for position
	///		generation
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void TakeDamage(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents);

private:
	///////////////////////////////////////////////////////////////////////////////////////////////////
	///										 Respawn												
	///		Responsible for respawning the agent at a new location and resetting its stats
	///																								
	///		-Takes a render window to pass into Overlapping Check
	///		-Takes Vector of world Obstacles to pass into Overlapping Check
	///		generation
	///		-Takes Vector of AI agents to pass into Overlapping Check
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Respawn(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents);

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
	void Shoot(std::vector<Projectile*> &Projectiles, ResourceManager* EngineResources, sf::Time CurrentTime);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///											Rotate												
	///		Responsible for rotating the agent towards its target
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void Rotate();

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///									Overlapping Check											
	///		Responsible for checking if an agents bounds intersects another
	///																								
	///		-Takes a render window to use size as a maximum when generation new positions
	///		-Takes a Vector of World obstacles to check global bounds for intersections with agents
	///		global bounds
	///		-Takes a Vector of AI Agents to check global bounds for intersections with agents global
	///		bounds
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void OverlappingCheck(sf::RenderWindow* Window, std::vector<Obstacle*> WorldObstacles, std::vector<AIAgent*> AIAgents);

	///////////////////////////////////////////////////////////////////////////////////////////////////
	///									Find Forward Vector										
	///		Responsible for calculating the agents forward vector based on it current rotation
	///																								
	///////////////////////////////////////////////////////////////////////////////////////////////////
	sf::Vector2f FindForwardVector();


	bool Overlapping; /*!< Bool that is true when Overlapping Check function finds agents global bound intesects with another objects*/

};

