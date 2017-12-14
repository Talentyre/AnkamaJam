using UnityEngine;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour
{
    [SerializeField]
    private List<TrapModel> m_traps;
    [SerializeField]
    public Transform m_trapContainer;

    public List<Trap> Init()
    {
        var list = new List<Trap>();

        if (m_traps.Count == 0)
            return list;

        var positions = GameSingleton.Instance.GetTrapPositions();
        for (int i = 0; i < 10; i++)
        {
            var trapIndex = Helper.random(m_traps.Count);
            var positionIndex = Helper.random(positions.Length);


            var trap = m_traps[trapIndex];
            var position = positions[positionIndex];
            list.Add(SpawnTrap(trap, position));
        }

        return list;
    }

    private Trap SpawnTrap(TrapModel model, Vector3Int position)
    {
        var trapGO = Instantiate(model.gameObject);
        trapGO.transform.SetParent(m_trapContainer);
        trapGO.transform.position = position;
        var modelI = trapGO.GetComponent<TrapModel>();
        var trap = trapGO.AddComponent<Trap>();
        trap.Init(modelI, position);
        return trap;
    }
        
}