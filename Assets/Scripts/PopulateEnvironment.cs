using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopulateEnvironment : MonoBehaviour
{
    public GameObject[] PlantsToClone;
    public List<GameObject> Vegetation = new List<GameObject>();
    public int VegetationDensitiy = 100;
    private Vector3 m_groundSize;
    private int m_vegetationMask;

    // Use this for initialization
    void Start ()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        Transform ground =  gameObject.transform.Find("Ground");
        m_groundSize = Vector3.Scale(transform.localScale, ground.GetComponent<MeshFilter>().mesh.bounds.size);

        m_vegetationMask = 1 << 9;

        Vegetate();
	}
	
	private void Vegetate()
    {
        Debug.Log("Beginning Vegetation Phase.");

        for (int i = 0; i < VegetationDensitiy; i++)
        {
            Vegetation.Add(SpawnPlant(m_groundSize));
        }

        Debug.Log("Finished Vegetation Phase.");
    }

    private void Update()
    {
        if (Vegetation.Count < VegetationDensitiy)
        {
            Vegetation.Add(SpawnPlant(m_groundSize));
        }
        else if (Vegetation.Count > VegetationDensitiy)
        {
            DeSpawnPlant();
        }
    }
    private GameObject SpawnPlant(Vector3 groundSize)
    {
        float randomX = Random.Range(-(groundSize.x / 2), groundSize.x / 2);
        float randomZ = Random.Range(-(groundSize.z / 2), groundSize.z / 2);
        Vector3 randomSpot = new Vector3(randomX, 0.0f, randomZ);

        int failedAttempt = 0;

        while (failedAttempt < 100 && Physics.CheckSphere(randomSpot, 0.5f, m_vegetationMask))
        {
            randomSpot.x = Random.Range(-(groundSize.x / 2), groundSize.x / 2);
            randomSpot.z = Random.Range(-(groundSize.z / 2), groundSize.z / 2);

            failedAttempt++;
        }

        if (failedAttempt == 100)
            Debug.Log("Gave up finding a random spot.");

        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject plant = Instantiate(GetPlant(), randomSpot, randomRotation);
        plant.transform.SetParent(transform, true);

        return plant;
    }

    private GameObject GetPlant()
    {
        return PlantsToClone[Random.Range(0, PlantsToClone.Length)];
    }

    private void DeSpawnPlant()
    {
        GameObject plant = Vegetation[Random.Range(0, Vegetation.Count)];
        Vegetation.Remove(plant);

        GameObject.Destroy(plant);
    }
}
