using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
    public GameObject m_gameOver;
    public GameObject m_victory;

    public static bool Victory;

    // Use this for initialization
    void Start()
    {
        m_gameOver.SetActive(!Victory);
        m_victory.SetActive(Victory);

    }
    
}
