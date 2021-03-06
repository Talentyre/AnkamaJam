﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
    private float m_maxSpawnInterval = 1f;
    public float MaxSpawnInterval { get { return m_maxSpawnInterval; } }

    [SerializeField]
    private float m_spawnDelay = 1f;

    public float SpawnDelay { get { return m_spawnDelay; } }

    [SerializeField]
    private float m_minStaticInterval = 7f;

    public float MinStaticInterval { get { return m_minStaticInterval; } }
    
    [SerializeField]
    private float m_maxStaticInterval = 30f;

    public float MaxStaticInterval { get { return m_maxStaticInterval; } }
    
    
    [SerializeField]
    private float m_minTalkInterval = 15f;

    public float MinTalkInterval { get { return m_minTalkInterval; } }
    
    [SerializeField]
    private float m_maxTalkInterval = 30f;

    public float MaxTalkInterval { get { return m_maxTalkInterval; } }
    
    
    [SerializeField]
    private int m_soul = 1;

    public int Soul { get { return m_soul; } }
    
    [SerializeField]
    private int m_score = 100;

    public int Score { get { return m_score; } }

    [SerializeField]
    private Image m_lifeImage;
    public Image LifeImage
    {
        get { return m_lifeImage; }
    }

    private string[] m_randomSentences =
    {
        "Mais qu'est-ce que je fais ici ?",
        "Ils savent quand même vachement bien jouer la mort !",
        "O, D, I, L, Quest-ce que ça peut bien vouloir dire ?",
        "Arrêtez de pousser ! Y en aura pour tout le monde !",
        "Où sont les WCs ?",
        "Je suis pas venu içi pour souffrir okay !?",
        "J'ai un mauvais pressentiment",
    };

    public List<string> SpecialTalks = new List<string>();
    
    public string[] RandomSentences {get { return m_randomSentences; }}
    
    
    
}
