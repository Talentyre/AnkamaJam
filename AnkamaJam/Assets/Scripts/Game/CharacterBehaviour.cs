using UnityEngine;
using System.Collections;
using System.Xml.Schema;

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

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void RefreshNextStaticTime()
    {
        _nextStaticTime = Time.time + Random.Range(m_model.MinStaticInterval, m_model.MaxStaticInterval);
    }

    public void Init(CharacterModel model, Vector3Int position)
    {
        m_positionInt = new Vector2Int(position.x, position.y);
        m_position = new Vector2(position.x, position.y);
        m_positionWithALittleJoke =  new Vector2(position.x, position.y);

        m_model = model;
        m_currentLife = model.MaxLife;
        OnWalk();
        RefreshNextStaticTime();
    }

    public void OnFear()
    {
        m_animator.SetBool("criPeur", true);
        m_target = m_positionInt;
    }

    public void OnRun()
    {
        m_characterStatesEnum = CharacterStatesEnum.RUNNING;
        m_speed = m_model.Speed * 2;
        // TODO trouver une target de fuite
        //m_target =
    }

    public void OnWalk()
    {
        m_characterStatesEnum = CharacterStatesEnum.WALKING;
        m_animator.SetBool("marche", true);
        m_speed = m_model.Speed;
    }

    public void OnStatic()
    {
        if (m_characterStatesEnum == CharacterStatesEnum.STATIC)
            return;
        m_characterStatesEnum = CharacterStatesEnum.STATIC;
        m_animator.SetBool("marche", false);
        m_speed = m_model.Speed;
        
        StartCoroutine(GoToWalk(Random.Range(3,5)));
    }

    private IEnumerator GoToWalk(float range)
    {
        yield return new WaitForSeconds(range);
        if (m_model != null)
        {
            RefreshNextStaticTime();
            OnWalk();
        }
    }

    public void Move()
    {
        if (m_characterStatesEnum == CharacterStatesEnum.STATIC)
            return;

        if (Time.time > _nextStaticTime)
        {
            OnStatic();
            return;
        }
        
        if (m_target == null && SelectWalkTarget()) 
            return;

        var target = m_target.GetValueOrDefault();
        var target2 = m_targetWithALittleJoke.GetValueOrDefault();
        
        m_movePercentage = Mathf.Min(1.0f, m_movePercentage + m_speed / 100f);

        m_position = Vector2.Lerp(m_startPosition, new Vector2(target.x, target.y), m_movePercentage);
        m_positionWithALittleJoke = Vector2.Lerp(m_startPositionWithALittleJoke, new Vector2(target2.x, target2.y), m_movePercentage);
        m_positionInt = new Vector2Int(Mathf.RoundToInt(m_position.x), Mathf.RoundToInt(m_position.y));

        var v = m_positionWithALittleJoke + Vector2.one*0.5f;
        transform.position = new Vector3(v.x, v.y, v.y);

        if (Mathf.Approximately(m_movePercentage, 1.0f)) 
            SelectWalkTarget();
    }

    private bool SelectWalkTarget()
    {
        OnWalk();
        
        var movingSideWalkAt = GameSingleton.Instance.GetMovingSideWalkAt(m_positionInt);
        if (movingSideWalkAt == null)
        {
            Debug.LogError("Impossible move at " + m_positionInt);
            m_currentLife = 0;
            return true;
        }
        m_target = m_positionInt + MovingSidewalk.GetVector2IntFromDirection(movingSideWalkAt.PickExit());
        m_targetWithALittleJoke = m_target + Random.insideUnitCircle * 0.3f;
        m_startPosition = m_position;
        m_startPositionWithALittleJoke = m_positionWithALittleJoke;
        m_movePercentage = 0.0f;
        return false;
    }

    public void Damage(int mDamage)
    {
        m_currentLife -= mDamage;
        // todo jouer FX de dégâts
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}