using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
	public Transform PnjsTransform;
	public List<CharacterModel> CharacterModels;
	private Dictionary<CharacterModel, float> m_charactersNextSpawn = new Dictionary<CharacterModel, float>();
	private Vector3Int[] _spawnPositions;

	public void Init()
	{
		CharacterModels.ForEach(c =>
		{
			m_charactersNextSpawn.Add(c, Time.time+c.SpawnDelay);
		});
		_spawnPositions = GameSingleton.Instance.GetSpawnPosition();
		if (_spawnPositions.Length == 0)
			throw new Exception("No spawn position defined !!!");
	}
	
	public List<CharacterBehaviour> SpawnCharacterLoop()
	{
		List<CharacterBehaviour> characterBehaviours = new List<CharacterBehaviour>();
		var models = m_charactersNextSpawn.Keys.ToArray();
		foreach (var model in models)
		{
			var nextSpawn = m_charactersNextSpawn[model];
			if (nextSpawn <= Time.time)
			{
				var position = _spawnPositions[Helper.random(_spawnPositions.Length)];
				characterBehaviours.Add(SpawnCharacter(model, position));
				m_charactersNextSpawn[model] = Time.time + model.SpawnInterval;
			}
		}
		return characterBehaviours;
	}

	private CharacterBehaviour SpawnCharacter(CharacterModel characterModel, Vector3Int position)
	{
		var character = Instantiate(characterModel.gameObject,PnjsTransform);
		var characterBehaviour = character.AddComponent<CharacterBehaviour>();
		characterBehaviour.Init(characterModel, position);
		character.transform.position = position;
		return characterBehaviour;
	}
}
