using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private float firstCarSpawnTime;
    private float carSpawnTimer;

    void Start()
    {
        this.carSpawnTimer = this.firstCarSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        this.carSpawnTimer -= Time.deltaTime;

        if (this.carSpawnTimer <= 0.0f)
        {
            this.SpawnCar();
        }
    }

    public void SpawnCar()
    {
        GameObject gr = Instantiate(carPrefab, this.transform.position, this.transform.rotation);
        this.carSpawnTimer = Random.Range(2.5f, 5.5f);
    }
}
