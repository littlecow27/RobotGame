using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour 
{
    public Transform[] spawnPoints;
    [SyncVar] public bool gameStarted;

    public float startTimer = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(spawnPoints.Length != 2) {
            Debug.Log("Error! Spawnpoints are not equal to two.");
        }
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted && NetworkManager.singleton.numPlayers == 2) {
            StartGame();
        }
    }

    void StartGame() {
        Debug.Log("Game Started!");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int spawnIndex = 0;
        foreach(GameObject player in players) {
            player.transform.position = spawnPoints[spawnIndex].position;
            spawnIndex++;
        }
        if(startTimer <= 0) {
            gameStarted = true;
        } else {
            startTimer -= Time.deltaTime;
        }
    }
}
