using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public GameObject m_gameOver;
    public GameObject m_victory;
    public Text m_scoreText;

    public static bool Victory;
    public static int Score;

    // Use this for initialization
    void Start()
    {
        m_gameOver.SetActive(!Victory);
        m_victory.SetActive(Victory);

        var bestScore = PlayerPrefs.GetInt("bestScore");
        bool newBestScore = bestScore < Score;
        m_scoreText.text = "Score : " + Score + "\n" +(newBestScore ? "New Best Score !!!" : "Best Score : "+bestScore);
        if (newBestScore)
            PlayerPrefs.SetInt("bestScore", Score);
    }
    
}
