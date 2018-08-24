using System;
using UnityEngine;

public class NameOfExample : MonoBehaviour
{
    enum Difficulty { Easy, Medium, Hard };
    private void Start()
    {
        Debug.Log(nameof(Difficulty.Easy));
        RecordHighScore("John");
    }

    private void RecordHighScore(string playerName)
    {
        Debug.Log(nameof(playerName));
        if (playerName == null) throw new ArgumentNullException(nameof(playerName));
    }
}
