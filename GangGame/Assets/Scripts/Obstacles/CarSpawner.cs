using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private float firstCarSpawnTime;
    [SerializeField, Range(1,6)] private float carSpawnDelay = 2;
    private float carSpawnTimer;

    private bool isLocked = true;

    void Start()
    {
        this.carSpawnTimer = this.firstCarSpawnTime;
        GameManager.Instance.start += this.UnLock;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked) return;

        this.carSpawnTimer -= Time.deltaTime;

        if (this.carSpawnTimer <= 0.0f)
        {
            this.SpawnCar();
        }
    }

    public void UnLock()
    {
        this.isLocked = false;
    }

    public void SpawnCar()
    {
        GameObject gr = Instantiate(carPrefab, this.transform.position, this.transform.rotation);
        this.carSpawnTimer = carSpawnDelay;
    }
}
