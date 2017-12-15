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

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameSingleton.Instance.BeginDrag(true);
        m_currentDnD = GameSingleton.Instance.TrapManager.StartPreviewTrap(m_model);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameSingleton.Instance.BeginDrag(false);
        GameSingleton.Instance.RequestSpawnAt(m_model, ExtractCellPosition(eventData));
        Destroy(m_currentDnD.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = ExtractCellPosition(eventData);
        m_currentDnD.transform.position = pos;
    }

    private Vector3Int ExtractCellPosition(PointerEventData data)
    {
        var screenToWorldPoint = Camera.main.ScreenToWorldPoint(data.position);
        return GameSingleton.Instance.GridLayout.WorldToCell(screenToWorldPoint);
    }

}