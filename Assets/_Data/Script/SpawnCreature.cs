using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnCreature : MonoBehaviour
{
    public CreatureType creatureType;
    public List<Creatures> listCreature = new();
    List<GameObject> listCreatureSpawn = new();
    
    public float rangeSpawn = 10;
    public int spawnAmount = 5;
    private void OnValidate()
    {

        foreach (var creature in listCreature)
        {
            if (creature.Name != creature.type.ToString())
            {
                creature.Name = creature.type.ToString();
            }

        }
    }
    void Start()
    {
        int i = 0;
        while(i < spawnAmount)
        {
            i++;
            Spawn();
        }
        InvokeRepeating(nameof(Spawn), 0, 4f);
    }
    GameObject ObjPoolCreature()
    {
      
          foreach (var creature in listCreatureSpawn)
            {
                if (creature.activeSelf == false)
                {
                    return creature;
                }
            }
          // spwan first time
         
          return null;

    }
    void CreateCreature()
    {
        GameObject prefab = null;
        foreach (var v in listCreature)
        {
            if (v.type == creatureType)
            {
                prefab = v.creature;
            }
        }
        GameObject creature = Instantiate(prefab, RandomPosSpawn(), Quaternion.identity, transform);
        listCreatureSpawn.Add(creature);
    }
    Vector3 RandomPosSpawn()
        {       Vector3 newPos = new Vector3();
        newPos.x = Random.Range(transform.position.x - rangeSpawn, transform.position.x + rangeSpawn);
        newPos.z = Random.Range(transform.position.z - rangeSpawn, transform.position.z + rangeSpawn);

        Physics.Raycast(newPos, Vector3.down, out RaycastHit pos);
        newPos.y = pos.point.y + 1f;
                   
            return newPos;
        }
    void Spawn()
     {
            if (listCreatureSpawn.Count >= spawnAmount)
           {
            GameObject creature = ObjPoolCreature();
            if (creature == null) return;
            creature.transform.position = RandomPosSpawn();
            creature.SetActive(true);
           }
        else
        {
            CreateCreature();
        }
           

    }
}
    public enum CreatureType
    {
        RabitFamily,
        Rabit,
        Bear,
        BearBaby,
    }
    [System.Serializable]
    public class Creatures {
        public string Name;

        public CreatureType type;

        public GameObject creature;
    }


