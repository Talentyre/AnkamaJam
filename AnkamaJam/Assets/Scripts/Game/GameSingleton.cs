using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameSingleton : ScriptableObject
{
    private GameSingleton m_instance;
	public static GameSingleton Instance { get { return m_instance; } }

    private readonly List<CharacterBehaviour> m_characters = new List<CharacterBehaviour>();
    private readonly List<Trap> m_traps = new List<Trap>();

    public IEnumerable<CharacterBehaviour> GetCharactersAt(Vector2Int position)
    {
        return m_characters.Where((c) => c.PositionInt.Equals(position));
    } 

    public IEnumerable<CharacterBehaviour> GetCharactersAt(List<Vector2Int> positions)
    {
        return m_characters.Where((c) => positions.Contains(c.PositionInt));
    }
    
    public void GameLoop()
    {
        for (int i = 0, size = m_traps.Count; i < size; ++i)
        {
            var trap = m_traps[i];
            trap.Act();
        }

        for (int i = 0, size = m_characters.Count; i < size; ++i)
        {
            var c = m_characters[i];
            // TODO CheckLife, retrait du perso ?
            c.Move();
        }
    }
}