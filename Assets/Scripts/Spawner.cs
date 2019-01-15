using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] PlantsToClone;
    [SerializeField] private int VegetationDensitiy = 100;
    private Vector3 m_groundSize;
    private int m_vegetationMask = 1 << 9;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        Transform ground = gameObject.transform.Find("Ground");
        m_groundSize = Vector3.Scale(transform.localScale, ground.GetComponent<MeshFilter>().mesh.bounds.size);

        Vegetate();

        StartCoroutine(MonitorVegetation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MonitorVegetation()
    {
        while (true)
        {
            Vegetation[] vegetation = GameObject.FindObjectsOfType<Vegetation>();

            if (vegetation.Length < VegetationDensitiy)
            {
                SpawnPlant(m_groundSize);
            }
            else if (vegetation.Length > VegetationDensitiy)
            {
                DeSpawnPlant(vegetation);
            }

            yield return new WaitForSeconds(3);
        }
    }

    private void Vegetate()
    {
        Debug.Log("Beginning Vegetation Phase.");

        for (int i = 0; i < VegetationDensitiy; i++)
        {
            SpawnPlant(m_groundSize);
        }

        Debug.Log("Finished Vegetation Phase.");
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

    private void DeSpawnPlant(Vegetation[] vegetation)
    {
        GameObject plant = vegetation[Random.Range(0, vegetation.Length)].gameObject;

        GameObject.Destroy(plant);
    }
}
