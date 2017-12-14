using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour
{

    private CharacterModel m_model;
    private Animator m_animator;

    private int m_currentLife;

    private Vector2 m_position;
    private Vector2Int m_positionInt;
    private int m_lastPositionCompute = 0;
    
    private Direction m_currentDirection;
    private bool m_dead;


    public Vector2 Position { get { return m_position; } }
    public Vector2Int PositionInt { get { return m_positionInt; } }
    public bool IsDead { get { return m_currentLife <= 0; }}
    
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    
    private void Init(CharacterModel model)
    {
        m_currentLife = m_model.MaxLife;
    }

    public void OnFear()
    {
        m_animator.SetBool("criPeur",true);
        
        // todo attendre le temps de l'animation peur 
    }
    
    public void Move()
    {
        // Change Position
        // TODO Si Target existe, Lerp entre start et fin
        // Si target manque, choix de la target (en fonction de la tile sur laquelle on est)
        // m_position = ???
        m_positionInt = new Vector2Int((int) m_position.x, (int) m_position.y);
    }

    public void Damage(int mDamage)
    {
        m_currentLife -= mDamage;
        // todo jouer FX de dégâts
    }
}
