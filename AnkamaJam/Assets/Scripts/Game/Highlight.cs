using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    private GameObject m_blockHighlightPrefab;
    [SerializeField]
    private GameObject m_effectHighlightPrefab;
    [SerializeField]
    private Transform m_blockHighlightContainer;
    [SerializeField]
    private Transform m_effectHighlightContainer;

    private void Awake()
    {
        m_effectHighlightContainer.gameObject.SetActive(false);
        m_blockHighlightContainer.gameObject.SetActive(false);
    }

    public void Init(TrapAOE effectAOE, TrapAOE blockAOE) 
    {
        foreach (var pos in Helper.PositionsFromAOE(effectAOE, Vector2Int.zero))
        {
            var highlightPrefab = Instantiate(m_effectHighlightPrefab);
            highlightPrefab.transform.SetParent(m_effectHighlightContainer);
            highlightPrefab.transform.localPosition = Helper.ToVector3Int(pos);
        }

        foreach (var pos in Helper.PositionsFromAOE(blockAOE, Vector2Int.zero))
        {
            var highlightPrefab = Instantiate(m_blockHighlightPrefab);
            highlightPrefab.transform.SetParent(m_blockHighlightContainer);
            highlightPrefab.transform.localPosition = Helper.ToVector3Int(pos);
        }
    }

    public void DoHighlight(bool effects, bool blocks)
    {
        m_effectHighlightContainer.gameObject.SetActive(effects);
        m_blockHighlightContainer.gameObject.SetActive(blocks);
    }

    public void Unhighlight()
    {
        m_effectHighlightContainer.gameObject.SetActive(false);
        m_blockHighlightContainer.gameObject.SetActive(false);
    }
}