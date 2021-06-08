#pragma once
#include "SFML/Graphics.hpp"
#include "SFML/System/Time.hpp"

class Renderable: public sf::Transformable
{
public:
    /////////////////////////////////////////////////////////////////
    /// Renderable class inherits from SFML Transformable.
    /// This give it access to position, rotation, movement and
    /// other useful functions.
    /////////////////////////////////////////////////////////////////

    sf::Sprite Sprite; /*!< Renderable Sprite*/

    float MoveSpeed; /*!< Renderable Move Speed*/

    float RotationSpeed; /*!< Renderable Rotation Speed*/

    int Health; /*!< Renderable Health*/

    int ID; /*!< Renderable ID*/

    float ObjectX; /*!< Renderable X Position for GUI to use*/

    float ObjectY; /*!< Renderable Y Position for GUI to use*/

    sf::Time TimetoNextShot; /*!< Time until agent can fire again*/
    sf::Time ShotCooldown; /*!< Time to be added to current time to stop constant firing*/
};

