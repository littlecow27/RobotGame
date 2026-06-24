using TMPro;
using Mirror;
using UnityEngine;

public class WinMenuScript : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject scorePanel;
    public TMP_Text winText;
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        winPanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    void Update()
    {
        if (gameManager != null && gameManager.GetComponent<GameManager>().winner != 0)
        {
            winPanel.SetActive(true);
            scorePanel.SetActive(false);
            winText.text = "Player " + gameManager.GetComponent<GameManager>().winner + " Wins!";
        }
    }
    public void QuitToTitle()
    {
        try
        {
            NetworkManager.singleton.StopHost();
        }
        catch (System.Exception e)
        {
            NetworkManager.singleton.StopClient();
        }
    }
}