using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : DamageTrap
{
    public List<GameObject> ElectricFXs = new List<GameObject>();

    public override void Activate(CharacterBehaviour character)
    {
        var randomPrefab = ElectricFXs[Helper.random(ElectricFXs.Count)];
        GameObject go = Instantiate(randomPrefab);
        go.transform.position = character.transform.position + Vector3.down * 0.5f;

        character.OnElectrocute(0.5f);

        base.Activate(character);
    }
}