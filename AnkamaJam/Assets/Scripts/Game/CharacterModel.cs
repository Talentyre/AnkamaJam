using UnityEngine;
using System.Collections;

public class CharacterModel : MonoBehaviour
{

    [SerializeField]
    private int m_maxLife = 1;

    public int MaxLife { get { return m_maxLife; } }
    
    [SerializeField]
    private float m_speed = 1f;

    public float Speed { get { return m_speed; } }
    
    [SerializeField]
    private float m_spawnInterval = 1f;

    public float SpawnInterval { get { return m_spawnInterval; } }
    
    [SerializeField]
    private float m_spawnDelay = 1f;

    public float SpawnDelay { get { return m_spawnDelay; } }

    [SerializeField]
    private float m_minStaticInterval = 7f;

    public float MinStaticInterval { get { return m_minStaticInterval; } }
    
    [SerializeField]
    private float m_maxStaticInterval = 30f;

    public float MaxStaticInterval { get { return m_maxStaticInterval; } }
    
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
