using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrapElementUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Text m_cost;
    [SerializeField] 
    private TrapModel m_model;

    private Trap m_currentDnD;
    private Material _grayScaleMaterial;
    private Material _defaultMaterial;
    private Color _originalBgColor;
    private bool _enabled;

    private void Start()
    {
        m_cost.text = m_model.Souls.ToString();
        _grayScaleMaterial = Resources.Load<Material>("material/SpriteGrayscale");
        _defaultMaterial = m_image.material;
        _originalBgColor = GetComponent<Image>().color;
        SetEnabled(GameSingleton.Instance.Souls >= m_model.Souls);
        GameSingleton.Instance.OnSoulUpdate += l =>
        {
            SetEnabled(l >= m_model.Souls);
        };
    }

    private void SetEnabled(bool enable)
    {
        _enabled = enable;
        m_image.material = !enable ? _grayScaleMaterial : _defaultMaterial;
        GetComponent<Image>().color = !enable ? Color.gray : _originalBgColor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_enabled)
            return;
        GameSingleton.Instance.BeginDrag(true);
        m_currentDnD = GameSingleton.Instance.TrapManager.StartPreviewTrap(m_model);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_enabled)
            return;
        GameSingleton.Instance.BeginDrag(false);
        GameSingleton.Instance.RequestSpawnAt(m_model, ExtractCellPosition(eventData));
        Destroy(m_currentDnD.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_enabled)
            return;
        var pos = ExtractCellPosition(eventData);
        m_currentDnD.transform.position = pos;
    }

    private Vector3Int ExtractCellPosition(PointerEventData data)
    {
        var screenToWorldPoint = Camera.main.ScreenToWorldPoint(data.position);
        return GameSingleton.Instance.GridLayout.WorldToCell(screenToWorldPoint);
    }

}