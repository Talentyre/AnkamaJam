using UnityEngine;
using System.Collections;
using System.Xml.Schema;
using DG.Tweening;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    private CharacterModel m_model;
    private Animator m_animator;

    private int m_currentLife;

    private Vector2 m_position;
    private Vector2 m_positionWithALittleJoke;
    private Vector2Int m_positionInt;
    private int m_lastPositionCompute = 0;
    private float m_movePercentage;

    private Direction m_currentDirection;
    private bool m_dead;
    private Vector2? m_target;
    private Vector2? m_targetWithALittleJoke;

    private float m_speed;
    private Vector2 m_startPosition;
    private Vector2 m_startPositionWithALittleJoke;

    private CharacterStatesEnum m_characterStatesEnum = CharacterStatesEnum.WALKING;
    private float _nextStaticTime;
    private float _nextTalkTime;

    public GameObject BubblePrefab;
    private GameObject _bubbleInstance;
    private bool _talking;
    private bool m_tempVictory;
    private bool m_victory;
    private Object _bloodFxPrefab;
    private Object _stunFxPrefab;
    private Object _bloodFx2Prefab;
    private Object _deathFxPrefab;
    private Object _loveFxPrefab;
    private int m_fearCounter;
    private float m_stunEnd;

    public CharacterModel Model
    {
        get { return m_model; }
    }

    //public Vector2? Target { get { return m_target; } }
    // position en cours
    public Vector2 Position
    {
        get { return m_position; }
    }

    // position case
    public Vector2Int PositionInt
    {
        get { return m_positionInt; }
    }

    public bool IsDead
    {
        get { return m_currentLife <= 0; }
    }

    public bool IsTempVictory { get { return m_tempVictory; } }

    public bool IsVictoryLaunched { get; set; }

    public bool IsVictory
    {
        get { return m_victory; }
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.flipX = Random.value > 0.5f;
        _bloodFxPrefab = Resources.Load("Prefabs/FX/BloodFx");
        _bloodFx2Prefab = Resources.Load("Prefabs/FX/BloodFx2");
        _deathFxPrefab = Resources.Load("Prefabs/FX/DeathFx");
        _stunFxPrefab = Resources.Load("Prefabs/FX/StunFx");
    }

    private void RefreshNextStaticTime()
    {
        _nextStaticTime = Time.time + Random.Range(m_model.MinStaticInterval, m_model.MaxStaticInterval);
    }

    private void RefreshNextTalkTime()
    {
        _nextTalkTime = Time.time + Random.Range(m_model.MinTalkInterval, m_model.MaxTalkInterval);
    }

    public void Init(CharacterModel model, Vector3Int position)
    {
        m_positionInt = new Vector2Int(position.x, position.y);
        m_position = new Vector2(position.x, position.y);
        m_positionWithALittleJoke = new Vector2(position.x, position.y);

        m_model = model;
        m_currentLife = model.MaxLife;
        OnWalk();
        RefreshNextStaticTime();
        RefreshNextTalkTime();
    }

    private Coroutine m_currentCoroutine = null;
    public static int BubbleCount;
    private SpriteRenderer m_spriteRenderer;
    private GameObject _stunFx;

    private void StartMovementCoroutine(IEnumerator routine)
    {
        if (m_currentCoroutine != null)
            StopCoroutine(m_currentCoroutine);
        m_currentCoroutine = StartCoroutine(routine);
    }


    public void OnStun(float duration)
    {
        if (m_tempVictory)
            return;

        if (_stunFx == null)
        {
            _stunFx = (GameObject) Instantiate(_stunFxPrefab);
            _stunFx.transform.position = transform.position + Vector3.up * 0.5f;   
        } 
        
        m_characterStatesEnum = CharacterStatesEnum.STUN;
        m_animator.SetBool("scared", true);
        m_stunEnd = Time.time + duration;
        m_target = null;
    }

    public void OnStunEnd()
    {
        if (_stunFx != null)
        {
            Destroy(_stunFx);
            _stunFx = null;
        } 
        m_animator.SetBool("scared", false);
        m_characterStatesEnum = CharacterStatesEnum.WALKING;
    }

    public void OnElectrocute(float duration, int damage)
    {
        if (m_tempVictory)
            return;
        m_currentLife -= damage;

        m_characterStatesEnum = CharacterStatesEnum.STUN;
        m_animator.SetTrigger("electrocute");
        m_stunEnd = Time.time + duration;

        m_target = null;
    }

    public void OnFear(int power)
    {
        if (m_tempVictory)
            return;
        
        m_characterStatesEnum = CharacterStatesEnum.FEAR;
        m_animator.SetTrigger("cri");
        m_animator.SetInteger("speed", 2);
        m_target = null;
        m_fearCounter = power;
        StartMovementCoroutine(RunAfterFear());
    }

    public IEnumerator RunAfterFear()
    {
        yield return new WaitForSeconds(0.7f);
        OnRun();
    }

    public void OnRun()
    {
        m_characterStatesEnum = CharacterStatesEnum.RUNNING;
        m_speed = m_model.Speed * 2;
    }

    public void OnStopRun()
    {
        OnWalk();
    }

    public void OnWalk()
    {
        m_characterStatesEnum = CharacterStatesEnum.WALKING;
        m_animator.SetInteger("speed", 1);
        m_speed = m_model.Speed;
    }

    public void OnStatic(int maxDuration = 4)
    {
        if (m_characterStatesEnum == CharacterStatesEnum.STATIC)
            return;
        m_characterStatesEnum = CharacterStatesEnum.STATIC;
        m_animator.SetInteger("speed", 0);
        m_speed = m_model.Speed;

        var duration = Mathf.Min(Random.Range(2, 5), maxDuration);
        StartMovementCoroutine(GoToWalk(duration));
    }

    private IEnumerator GoToWalk(float range)
    {
        yield return new WaitForSeconds(range);
        if (m_model != null && m_characterStatesEnum == CharacterStatesEnum.STATIC)
        {
            RefreshNextStaticTime();
            OnWalk();
        }
    }

    public void MoveLoop()
    {
        if (!_talking && Time.time > _nextTalkTime && m_characterStatesEnum == CharacterStatesEnum.STATIC)
            OnTalk();


        if (m_characterStatesEnum == CharacterStatesEnum.STATIC)
            return;

        if (m_characterStatesEnum == CharacterStatesEnum.FEAR)
            return;

        if (m_characterStatesEnum == CharacterStatesEnum.STUN)
        {
            if (Time.time > m_stunEnd)
            {
                OnStunEnd();
            }
            return;
        }


        if (!m_tempVictory && Time.time > _nextStaticTime &&
            (m_characterStatesEnum == CharacterStatesEnum.WALKING))
        {
            OnStatic();
            return;
        }

        if (m_target == null)
        {
            switch (m_characterStatesEnum)
            {
                case CharacterStatesEnum.WALKING:
                    if (SelectWalkTarget())
                        return;
                    break;
                case CharacterStatesEnum.RUNNING:
                    if (SelectRunTarget(true))
                        return;
                    break;
            }
        }

        var target = m_target.GetValueOrDefault();
        var target2 = m_targetWithALittleJoke.GetValueOrDefault();

        m_movePercentage = Mathf.Min(1.0f, m_movePercentage + m_speed / 100f);

        m_position = Vector2.Lerp(m_startPosition, new Vector2(target.x, target.y), m_movePercentage);
        m_positionWithALittleJoke = Vector2.Lerp(m_startPositionWithALittleJoke, new Vector2(target2.x, target2.y),
            m_movePercentage);
        m_positionInt = new Vector2Int(Mathf.RoundToInt(m_position.x), Mathf.RoundToInt(m_position.y));

        var v = m_positionWithALittleJoke + Vector2.one * 0.5f;
        transform.position = new Vector3(v.x, v.y, v.y);

        if (Mathf.Approximately(m_movePercentage, 1.0f))
            switch (m_characterStatesEnum)
            {
                case CharacterStatesEnum.WALKING:
                    SelectWalkTarget();
                    return;
                case CharacterStatesEnum.RUNNING:
                    SelectRunTarget(true);
                    return;
            }
    }

    private void OnTalk()
    {
        if (m_tempVictory)
            return;
        RefreshNextTalkTime();
        if (BubbleCount >= 2)
            return;

        if (_bubbleInstance == null)
        {
            _bubbleInstance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Bubble"), transform);
            _bubbleInstance.transform.localPosition = Vector3.zero;
        }
        _talking = true;

        BubbleCount++;

        _bubbleInstance.gameObject.SetActive(true);
        var sentence = m_model.SpecialTalks.Count > 0
            ? m_model.SpecialTalks[Helper.random(m_model.SpecialTalks.Count)]
            : m_model.RandomSentences[Helper.random(m_model.RandomSentences.Length)];
        var randomSentence = sentence;
        _bubbleInstance.GetComponentInChildren<Text>().text =
            randomSentence;

        StartCoroutine(CloseBubble(randomSentence.Length));
    }

    private IEnumerator CloseBubble(int randomSentenceLength)
    {
        yield return new WaitForSeconds(randomSentenceLength * 0.1f);
        _bubbleInstance.gameObject.SetActive(false);
        _talking = false;

        BubbleCount--;
    }

    private bool SelectWalkTarget()
    {
        OnWalk();

        var movingSideWalkAt = GameSingleton.Instance.GetMovingSideWalkAt(m_positionInt);
        var endProperty = GameSingleton.Instance.GridInformation.GetPositionProperty(Helper.ToVector3Int(m_positionInt),
            TilemapProperty.EndProperty, 0);
        if (endProperty == 1 && !m_tempVictory)
        {
            m_tempVictory = true;
            OnStatic(2);
            return true;
        }
        
        if (movingSideWalkAt == null)
        {
            Debug.LogError("Impossible move at " + m_positionInt);
            if (!m_tempVictory)
                m_currentLife = 0;
            return true;
        }
        var previousTarget = m_target;
        m_target = m_positionInt + MovingSidewalk.GetVector2IntFromDirection(movingSideWalkAt.PickExit());
        ChangeDirectionByTarget(previousTarget, m_target);
        m_targetWithALittleJoke = m_target + Random.insideUnitCircle * 0.3f;
        m_startPosition = m_position;
        m_startPositionWithALittleJoke = m_positionWithALittleJoke;
        m_movePercentage = 0.0f;
        return false;
    }


    private bool SelectRunTarget(bool invertDirection)
    {
        Debug.Log("SelectRun " + Time.frameCount);
        if (m_fearCounter <= 0)
        {
            OnStopRun();
            return false;
        }

        OnRun();

        var startProperty = GameSingleton.Instance.GridInformation.GetPositionProperty(Helper.ToVector3Int(m_positionInt),
            TilemapProperty.StartProperty, 0);
        if (startProperty == 1)
        {
            m_target = null;
            OnStatic(2);
            return true;
        }
        
        var movingSideWalkAt = GameSingleton.Instance.GetMovingSideWalkAt(m_positionInt);
        if (movingSideWalkAt == null)
        {
            m_currentLife = 0;
            return true;
        }

        m_fearCounter--;

        var previousTarget = m_target;
        m_target = m_positionInt + MovingSidewalk.GetVector2IntFromDirection(movingSideWalkAt.PickEntrance());
        ChangeDirectionByTarget(previousTarget, m_target);
        m_targetWithALittleJoke = m_target + Random.insideUnitCircle * 0.3f;
        m_startPosition = m_position;
        m_startPositionWithALittleJoke = m_positionWithALittleJoke;
        m_movePercentage = 0.0f;
        return false;
    }

    private void ChangeDirectionByTarget(Vector2? previousTarget, Vector2? nextTarget)
    {
        if (previousTarget == null || nextTarget == null)
            return;
        if (Mathf.Approximately(nextTarget.GetValueOrDefault().x, previousTarget.GetValueOrDefault().x))
            return;
        m_spriteRenderer.flipX = nextTarget.GetValueOrDefault().x < previousTarget.GetValueOrDefault().x;
    }

    public void Damage(int mDamage)
    {
        m_currentLife -= mDamage;
        OnHit();
    }

    private void OnHit()
    {
        m_animator.SetTrigger("hit");
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            GameObject bloodFx = (GameObject) Instantiate(Random.Range(0,2) == 0 ? _bloodFx2Prefab : _bloodFxPrefab);
            var rendomVect = Random.insideUnitCircle * 0.25f;
            bloodFx.transform.position = transform.position + Vector3.up * 0.5f + new Vector3(rendomVect.x,rendomVect.y,0);   
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
        GameObject deathFx = (GameObject) Instantiate(_deathFxPrefab);
        deathFx.transform.position = transform.position + Vector3.down * 0.5f;
    }

    public void OnVictory()
    {
        if (_bubbleInstance != null)
            _bubbleInstance.gameObject.SetActive(false);
        
        StartCoroutine(VictoryCoroutine());
        /*
        GameObject loveFx = (GameObject)Instantiate(_loveFxPrefab);
        loveFx.transform.position = transform.position + Vector3.down*0.5f;
        */
    }

    private IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(2f);
        m_spriteRenderer.DOColor(new Color(0f, 0f, 0f, 0f), 2f);
        yield return new WaitForSeconds(2f);
        m_victory = true;
    }
}