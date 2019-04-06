using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreatures : MonoBehaviour
{
    [SerializeField] private int CreatureDensitiy = 10;
    public GameObject[] CreaturesToSpawn;
    private Vector3 m_groundSize;
    private int m_creatureMask;

    // Use this for initialization
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        Transform ground = gameObject.transform.Find("Ground");
        m_groundSize = Vector3.Scale(transform.localScale, ground.GetComponent<MeshFilter>().mesh.bounds.size);

        m_creatureMask = 1 << 10;
    }

    private void Update()
    {
        int creatureCount = GameObject.FindGameObjectsWithTag("Creature").Length;
        if (creatureCount < CreatureDensitiy)
        {
            SpawnCreature(m_groundSize);
        }
        else if (creatureCount > CreatureDensitiy)
        {
            DeSpawnCreature();
        }
    }
    private GameObject SpawnCreature(Vector3 groundSize)
    {
        float randomX = Random.Range(-(groundSize.x / 2), groundSize.x / 2);
        float randomZ = Random.Range(-(groundSize.z / 2), groundSize.z / 2);
        Vector3 randomSpot = new Vector3(randomX, 0.0f, randomZ);

        int failedAttempt = 0;

        while (failedAttempt < 100 && Physics.CheckSphere(randomSpot, 0.0f, m_creatureMask))
        {
            randomSpot.x = Random.Range(-(groundSize.x / 2), groundSize.x / 2);
            randomSpot.z = Random.Range(-(groundSize.z / 2), groundSize.z / 2);

            failedAttempt++;
        }

        if (failedAttempt == 100)
            Debug.Log("Gave up finding a random spot.");

        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject creature = Instantiate(GetCreature(), randomSpot, randomRotation);
        creature.transform.SetParent(transform, true);

        return creature;
    }

    private GameObject GetCreature()
    {
        return CreaturesToSpawn[Random.Range(0, CreaturesToSpawn.Length)];
    }

    private void DeSpawnCreature()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");

        GameObject creature = creatures[Random.Range(0, creatures.Length)];

        GameObject.Destroy(creature);
    }
}
