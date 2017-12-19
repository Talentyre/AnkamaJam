using UnityEngine;

public class CharacterSpawn
{
    private CharacterModel m_model;
    private Vector3Int m_position;

    public CharacterSpawn(CharacterModel m_model, Vector3Int m_position)
    {
        this.m_model = m_model;
        this.m_position = m_position;
    }

    public CharacterModel Model { get { return m_model; } }
    public Vector3Int Position { get { return m_position; } }
}
