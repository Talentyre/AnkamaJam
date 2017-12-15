using System.Collections;
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

    public GameObject OverhadFeedbackPrefab;

    private const long MaxReputation = 100;
    private long _reputation;

    public long Reputation
    {
        get
        {
            return _reputation;
        }
        set { _reputation = (long) Mathf.Min(_reputation, MaxReputation); }
    }

    private const long MaxSouls = 1000;
    private long _souls;

    public long Souls
    {
        get { return _souls; }
        set { _souls = (long) Mathf.Min(_souls, MaxSouls); }
    }
    
    public bool IsGameOver { get { return _reputation <= 0; } }
    
    
    private void Awake()
    {
        m_instance = this;
        _lastTick = Time.realtimeSinceStartup;
        _tickInterval = 1 / LogicFps;
        CharacterSpawner.Init();
        m_traps.AddRange(TrapManager.Init());
        InputManager.OnTrapCell += OnTrapCell;
        _reputation = MaxReputation;
    }

    private readonly List<CharacterBehaviour> m_characters = new List<CharacterBehaviour>();
    private readonly List<Trap> m_traps = new List<Trap>();
    private float _lastTick;
    private float _tickInterval;
    private bool _gameOverLaunched;

    public void OnTrapCell(Vector3Int position)
    {
        /*var pos = new Vector2Int(position.x, position.y);
        if (!TrapExistsAt(pos))
        {
            var trap = TrapManager.SpawnRandomTrap(position);
            if (trap != null)
                m_traps.Add(trap);
        }
        else
        {
            Debug.Log("A trap is already present at the position.");
        }*/
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
        if (_gameOverLaunched)
            return;
        if (IsGameOver)
        {
            OnGameOver();
            return;
        }
        
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
            } else if (c.IsVictory)
            {
                OnCharacterVictory(c);
                m_characters.RemoveAt(i);
                Destroy(c.gameObject);
            }
            else
            {
                c.MoveLoop();   
            }
        }
    }

    private void OnGameOver()
    {
        _gameOverLaunched = true;
        
        // todo score et compagnie !
        var showPanels = SceneHandler.Instance.gameObject.GetComponent<ShowPanels>();
        SceneHandler.Instance.Load(SceneHandler.StartScene, () => showPanels.ShowMenu());
    }

    private void OnCharacterVictory(CharacterBehaviour characterBehaviour)
    {
        Reputation++;
        var sourcePosition = characterBehaviour.transform.position;
        
        var text = "Victory !!!";
        LaunchOverheadFeedback(text, Color.yellow,sourcePosition);
        StartCoroutine(ReputationFeedback(sourcePosition));
    }

    private IEnumerator ReputationFeedback(Vector3 sourcePosition)
    {
        yield return new WaitForSeconds(2f);
        LaunchOverheadFeedback("+ 1",Color.red, sourcePosition);
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

        var text = (soulDelta >= 0 ? "+ " : "- ") + soulDelta;
        LaunchOverheadFeedback(text, Color.cyan,sourcePosition);
    }

    private void LaunchOverheadFeedback(string text, Color color, Vector3 sourcePosition)
    {
        GameObject soulGain = Instantiate(OverhadFeedbackPrefab);

        soulGain.transform.position = sourcePosition + Vector3.up * 0.5f;

        soulGain.transform.DOLocalMoveY(soulGain.transform.position.y + 0.5f, 1f);
        var componentInChildren = soulGain.GetComponent<Text>();
        componentInChildren.text = text;
        componentInChildren.color = color;
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

    public bool CanSpawnTrapAt(TrapModel model, Vector3Int position)
    {
        var positions = Helper.ToVector3Int(model.ActivationPositions(new Vector2Int(position.x, position.y)));
        var trapPositions = GridInformation.GetAllPositions(TilemapProperty.TrapProperty);

        if (!positions.All((pos) => trapPositions.Contains(pos)))
            return false;

        List<Vector3Int> allTrapsPositions = new List<Vector3Int>();
        m_traps.ForEach((t) => allTrapsPositions.AddRange(Helper.ToVector3Int(t.ActivationPositions)));
        return !positions.Any((pos) => allTrapsPositions.Contains(pos));

    }

    public void RequestSpawnAt(TrapModel model, Vector3Int position)
    {
        if (!CanSpawnTrapAt(model, position))
            return;
        // Check souls
        var trap = TrapManager.SpawnTrap(model, position);
        if (trap != null)
            m_traps.Add(trap);
        
    }
}