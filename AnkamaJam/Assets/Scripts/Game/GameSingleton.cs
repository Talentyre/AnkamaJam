using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameSingleton : MonoBehaviour
{
    private static GameSingleton m_instance;

    public static GameSingleton Instance
    {
        get { return m_instance; }
    }

    public GridInformation GridInformation;
    public CharacterSpawner CharacterSpawner;
    public GridLayout GridLayout;
    public TrapManager TrapManager;
    public InputManager InputManager;

    public float LogicFps = 30;

    public GameObject OverhadFeedbackPrefab;

    public Tilemap TrapTilemap;

    public const long MaxAlert = 100;
    private long _alert;

    public long Alert
    {
        get { return _alert; }
        set
        {
            _alert = (long) Mathf.Min(value, MaxAlert);
            if (OnAlertUpdate != null)
                OnAlertUpdate(_alert);
        }
    }

    public const long MaxSouls = 100;
    private long _souls;

    public long Souls
    {
        get { return _souls; }
        set { _souls = (long) Mathf.Min(value, MaxSouls); }
    }

    private long _score;

    public long Score
    {
        get { return _score; }
        set
        {
            _score = value;
            if (OnScoreUpdate != null)
                OnScoreUpdate(_score);
        }
    }

    public bool IsGameOver
    {
        get { return _alert >= MaxAlert; }
    }


    private void Awake()
    {
        m_instance = this;
        _lastTick = Time.realtimeSinceStartup;
        _tickInterval = 1 / LogicFps;
        CharacterSpawner.Init();
        m_traps.AddRange(TrapManager.Init());
        InputManager.OnCellClick += OnCellClick;
        BeginDrag(false);
    }

    private readonly List<CharacterBehaviour> m_characters = new List<CharacterBehaviour>();
    private readonly List<Trap> m_traps = new List<Trap>();
    private float _lastTick;
    private float _tickInterval;
    private bool _gameOverLaunched;
    public Action<long> OnSoulUpdate;
    public Action<int> OnComboUpdate;
    public Action<long> OnScoreUpdate;
    public Action<long> OnAlertUpdate;

    private int _combo;
    private const float ComboBaseDuration = 5;
    private float _comboDuration;

    private int Combo
    {
        get { return _combo; }
        set
        {
            _combo = value;
            OnComboUpdate(_combo);
        }
    }

    public void OnCellClick(Vector3Int position)
    {
        var trap = m_traps.FirstOrDefault((c) => c.Position.Equals(Helper.ToVector2Int(position)));
        if (trap != null)
            trap.Act(false);
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
        if (_comboDuration > 0)
        {
            _comboDuration -= Time.deltaTime;
            if (_comboDuration <= 0)
                Combo = 0;
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

        for (int i = m_characters.Count - 1; i >= 0; --i)
        {
            var c = m_characters[i];
            if (c.IsDead)
            {
                Debug.Log("Character Dead " + c);
                c.OnDeath();
                ComputeCharacterDeath(c);
                m_characters.RemoveAt(i);
            }
            else if (c.IsVictory)
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
        Alert++;
        var sourcePosition = characterBehaviour.transform.position;

        var text = "Victory !!!";
        LaunchOverheadFeedback(text, Color.yellow, sourcePosition);
        StartCoroutine(ReputationFeedback(sourcePosition));
    }

    private IEnumerator ReputationFeedback(Vector3 sourcePosition)
    {
        yield return new WaitForSeconds(2f);
        LaunchOverheadFeedback("+ 1", Color.red, sourcePosition);
    }

    private void ComputeCharacterDeath(CharacterBehaviour characterBehaviour)
    {
        var modelSoul = characterBehaviour.Model.Soul;
        var sourcePosition = characterBehaviour.transform.position;

        Combo++;
        Score += characterBehaviour.Model.Score * Combo;
        _comboDuration = ComboBaseDuration;

        OnSoulModified(modelSoul, sourcePosition);
    }

    public void OnSoulModified(int soulDelta, Vector3 sourcePosition)
    {
        Souls += soulDelta;
        if (OnSoulUpdate != null)
            OnSoulUpdate(Souls);

        var text = (soulDelta >= 0 ? "+ " : "- ") + soulDelta;
        LaunchOverheadFeedback(text, Color.cyan, sourcePosition);
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
        var vector3Int = new Vector3Int(mPositionInt.x, mPositionInt.y, 0);
        GameObject movingGameObject =
            (GameObject) GridInformation.GetPositionProperty(vector3Int, TilemapProperty.MovingSidewalkProperty,
                (Object) null);
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

    public void BeginDrag(bool drag)
    {
        TrapTilemap.color = drag ? Color.white : new Color(1f,1f,1f,0f);
    }
}