using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int Score;

    public void AddScore(int ScoreToAdd)
    {
        Score += ScoreToAdd;
    }
}
