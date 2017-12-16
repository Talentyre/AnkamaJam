using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : DamageTrap
{
    public List<GameObject> ElectricFXs = new List<GameObject>();

    public override void Activate(CharacterBehaviour character)
    {
        for (int i = 0; i < Random.Range(6, 10); i++)
        {
            var randomPrefab = ElectricFXs[Helper.random(ElectricFXs.Count)];
            GameObject go = Instantiate(randomPrefab);
            go.transform.position = transform.position +  new Vector3(Random.Range(-0.25f,2f),Random.Range(0f,1f),0);  
            
        }
        for (int i = 0; i < Random.Range(3, 6); i++)
        {
            var randomPrefab = ElectricFXs[Helper.random(ElectricFXs.Count)];
            GameObject go = Instantiate(randomPrefab);
            var r = Random.insideUnitCircle;
            go.transform.position = character.transform.position +  new Vector3(r.x,r.y,0)*0.7f;   
        }

        character.OnElectrocute(1f, Damage);

    }
}