using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{
    private int m_level;
    private TrapModel m_model;

    private float m_nextActivation;

    private Vector2Int m_position;
    private List<Vector2Int> m_activationPositions;
    private List<Vector2Int> m_blockPositions;
    private int m_evolution = -1;

    private GameObject _trapMenuPrefab;
    private GameObject _trapMenu;

    public Vector2Int Position
    {
        get { return m_position; }
    }

    public List<Vector2Int> ActivationPositions
    {
        get { return m_activationPositions; }
    }

    public List<Vector2Int> BlockPositions
    {
        get { return m_blockPositions; }
    }
    
    public float Cooldown
    {
        get { return Evolved ? m_model.Evolution.Cooldown : m_model.Cooldown; }
    }

    public bool Evolved
    {
        get { return Model.EvolutionIndex >= 0; }
    }

    public float CooldownPercentage
    {
        get
        {
            if (m_model.Cooldown <= 0)
                return 1;
            var endTime = m_nextActivation;
            var startTime = m_nextActivation - m_model.Cooldown;
            var elapsedTime = Time.realtimeSinceStartup - startTime;
            return elapsedTime / m_model.Cooldown;
        }
    }

    private void Start()
    {
        _trapMenuPrefab = Resources.Load<GameObject>("Prefabs/UI/TrapMenu");
    }

    public void UpdateCooldownProgressBar()
    {
        m_model.CooldownImage.fillAmount = CooldownPercentage;
    }

    public void Init(TrapModel model, Vector3Int position)
    {
        m_model = model;
        m_nextActivation = Time.realtimeSinceStartup + m_model.Delay;
        m_activationPositions = new List<Vector2Int>();
        m_blockPositions = new List<Vector2Int>();

        m_position = new Vector2Int(position.x, position.y);
        m_activationPositions.AddRange(m_model.ActivationPositions(m_position));
        m_blockPositions.AddRange(m_model.BlockPositions(m_position));
    }

    public TrapModel Model
    {
        get { return m_model; }
    }

    public void Act(bool automatic = true)
    {
        var cooldown = IsInCooldown;
        m_model.Animators.ForEach(a => a.SetBool("Cooldown", cooldown));
        UpdateCooldownProgressBar();

        if (automatic != m_model.Automatic)
            return;

        if (IsInCooldown)
            return;

        var charactersInTrap = GameSingleton.Instance.GetCharactersAt(m_activationPositions).ToList();
        if (automatic && charactersInTrap.Count == 0)
            return;

        foreach (var characterBehaviour in charactersInTrap)
        {
            m_model.Activate(characterBehaviour, Evolved, Position, characterBehaviour.PositionInt);
        }

        m_model.Animators.ForEach(a => a.SetTrigger("TriggerTrap"));
        m_nextActivation = Time.realtimeSinceStartup + m_model.Cooldown;
    }

    public bool IsInCooldown
    {
        get { return m_model.Cooldown > 0 && m_nextActivation > Time.realtimeSinceStartup; }
    }

    public int SellCost
    {
        get { return (m_model.Souls + (Evolved ? m_model.Evolution.Souls : 0))/2; }
    }

    public void OnPurchase()
    {
        GameSingleton.Instance.OnSoulModified(-Model.Souls, transform.position);
    }

    public void Unhighlight()
    {
        m_model.Highlight.Unhighlight();
    }

    public void Highlight(bool effects, bool blocks)
    {
        m_model.Highlight.DoHighlight(effects, blocks);
    }


    public void DisactiveMenu()
    {
        if (_trapMenu != null)
            _trapMenu.gameObject.SetActive(false);
    }

    public void ActivateMenu()
    {
        if (_trapMenu == null)
        {
            _trapMenu = Instantiate(_trapMenuPrefab);
            _trapMenu.transform.SetParent(transform);
            _trapMenu.transform.position = transform.position + Vector3.up + Vector3.right*0.5f;
        }
        else
        {
            _trapMenu.gameObject.SetActive(true);
        }
        _trapMenu.GetComponent<TrapMenu>().UpdateInfos(this);
    }
}