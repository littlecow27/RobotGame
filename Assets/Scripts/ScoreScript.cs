using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(RobotScript p in FindObjectsByType<RobotScript>(FindObjectsSortMode.InstanceID))
        {
            if(p.playerNumber == 1)
            {
                p1ScoreText.text = p.score.ToString();
            }
            else if(p.playerNumber == 2)
            {
                p2ScoreText.text = p.score.ToString();
            }
        }
    }
}
