using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public List<GameObject> players = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(spawnPoints.Length != 2) {
            Debug.Log("Error! Spawnpoints are not equal to two.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        if(players.Count == 0 || (newPlayer != null && !newPlayer.Equals(players[0]))) {
            players.Add(newPlayer);
        }
        if(players.Count == 2) {
            StartGame();
            Debug.Log(players.Count);
        }
    }

    void StartGame() {
        Debug.Log("Game Started!");
    }
}
