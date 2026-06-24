using TMPro;
using Mirror;
using UnityEngine;

public class WinMenuScript : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject scorePanel;
    public GameObject pausePanel;
    public GameObject instructionsPanel;
    public TMP_Text winText;
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        winPanel.SetActive(false);
        scorePanel.SetActive(true);
        pausePanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    void Update()
    {
        if (gameManager != null && gameManager.GetComponent<GameManager>().winner != 0)
        {
            winPanel.SetActive(true);
            scorePanel.SetActive(false);
            pausePanel.SetActive(false);
            instructionsPanel.SetActive(false);
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
    public void PauseGame()
    {
        if(instructionsPanel.activeSelf)
        {
            instructionsPanel.SetActive(false);
            pausePanel.SetActive(true);
            return;
        }
        pausePanel.SetActive(!pausePanel.activeSelf);
    }
    public void OpenInstructions()
    {
        pausePanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }
}