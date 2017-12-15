﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameSingleton : MonoBehaviour
{
    private static GameSingleton m_instance;
	public static GameSingleton Instance { get { return m_instance; } }

    public GridInformation GridInformation;
    public CharacterSpawner CharacterSpawner;
    public GridLayout GridLayout;
    public TrapManager TrapManager;
    public InputManager InputManager;

    public float LogicFps = 30;

    public GameObject SoulGainFeedbackPrefab;
    public CharacterModel TetsCharacterBehaviour;

    private const long BaseReputation = 100;
    private long _reputation;

    public long Reputation
    {
        get
        {
            return _reputation;
        }
        set { _reputation = (long) Mathf.Max(_reputation, 0); }
    }

    private const long MaxSouls = 1000;
    private long _souls;

    public long Souls
    {
        get { return _souls; }
        set { _souls = (long) Mathf.Min(_souls, MaxSouls); }
    }
    
    
    private void Awake()
    {
        m_instance = this;
        _lastTick = Time.realtimeSinceStartup;
        _tickInterval = 1 / LogicFps;
        CharacterSpawner.Init();
        m_traps.AddRange(TrapManager.Init());
        InputManager.OnTrapCell += OnTrapCell;
        _reputation = BaseReputation;

        Invoke("Pouet",2);
        Invoke("Pouet",4);
    }

    private void Pouet()
    {
        var characterBehaviour = TetsCharacterBehaviour.gameObject.AddComponent<CharacterBehaviour>();
        characterBehaviour.Init(TetsCharacterBehaviour, Vector3Int.zero);
        
        ComputeCharacterDeath(characterBehaviour);
    }

    private readonly List<CharacterBehaviour> m_characters = new List<CharacterBehaviour>();
    private readonly List<Trap> m_traps = new List<Trap>();
    private float _lastTick;
    private float _tickInterval;

    public void OnTrapCell(Vector3Int position)
    {
        var pos = new Vector2Int(position.x, position.y);
        if (!TrapExistsAt(pos))
        {
            var trap = TrapManager.SpawnRandomTrap(position);
            if (trap != null)
                m_traps.Add(trap);
        }
        else
        {
            Debug.Log("A trap is already present at the position.");
        }
    }

    public IEnumerable<CharacterBehaviour> GetCharactersAt(Vector2Int position)
    {
        return m_characters.Where((c) => c.PositionInt.Equals(position));
    } 

    public IEnumerable<CharacterBehaviour> GetCharactersAt(List<Vector2Int> positions)
    {
        return m_characters.Where((c) => positions.Contains(c.PositionInt));
    }
    
    public bool TrapExistsAt(Vector2Int position)
    {
        return m_traps.Any((trap) => position.Equals(trap.Position));
    }

    private void Update()
    {
        if (_lastTick + _tickInterval < Time.realtimeSinceStartup)
        {
            GameLoop();
            _lastTick = Time.realtimeSinceStartup;
        }
    }
    
   public void GameLoop()
    {
        var spawnedCharacters = CharacterSpawner.SpawnCharacterLoop();
        m_characters.AddRange(spawnedCharacters);

        for (int i = 0, size = m_traps.Count; i < size; ++i)
        {
            var trap = m_traps[i];
            trap.Act();
        }

        for (int i = m_characters.Count-1; i >= 0; --i)
        {
            var c = m_characters[i];
            if (c.IsDead)
            {
                Debug.Log("Character Dead " + c);
                c.OnDeath();
                ComputeCharacterDeath(c);
                m_characters.RemoveAt(i);
            }
            else
            {
                c.MoveLoop();   
            }
        }
    }

    private void ComputeCharacterDeath(CharacterBehaviour characterBehaviour)
    {
        var modelSoul = characterBehaviour.Model.Soul;
        var sourcePosition = characterBehaviour.transform.position;
        
        OnSoulModified(modelSoul, sourcePosition);
    }

    public void OnSoulModified(int soulDelta, Vector3 sourcePosition)
    {
        Souls += soulDelta;

        GameObject soulGain = Instantiate(SoulGainFeedbackPrefab);

        soulGain.transform.position = sourcePosition + Vector3.up * 0.5f;

        soulGain.transform.DOLocalMoveY(soulGain.transform.position.y + 0.5f, 1f);
        var componentInChildren = soulGain.GetComponent<Text>();
        componentInChildren.text = (soulDelta >= 0 ? "+ "  : "- ")+ soulDelta;
        componentInChildren.DOFade(1f, 0.5f).OnComplete(() =>
        {
            componentInChildren.DOFade(0f, 0.5f).OnComplete(() => Destroy(soulGain));
        });
    }

    public MovingSidewalk GetMovingSideWalkAt(Vector2Int mPositionInt)
    {
        var vector3Int = new Vector3Int(mPositionInt.x,mPositionInt.y,0);
        GameObject movingGameObject = (GameObject) GridInformation.GetPositionProperty(vector3Int, TilemapProperty.MovingSidewalkProperty, (Object) null);
        if (movingGameObject == null)
            return null;
        return movingGameObject.GetComponent<MovingSidewalk>();
    }
    
    public Vector3Int[] GetTrapPositions()
    {
        return GridInformation.GetAllPositions(TilemapProperty.TrapProperty);
    }

    public Vector3Int[] GetSpawnPosition()
    {
        return GridInformation.GetAllPositions(TilemapProperty.StartProperty);
    }
}