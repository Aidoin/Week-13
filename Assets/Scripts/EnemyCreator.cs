using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DpreadMode
{
    Number,
    Percent
}


public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2Int minMaxQuantityNumberEnemies = Vector2Int.one;
    [SerializeField] private Vector2 minMaxSpawnTime = Vector2Int.one;
    [SerializeField] private float minimumPossibleSpawnTime = 1;

    private float currentSpawnTime;
    private float spawnTimer;

    [Space(20)]
    [SerializeField] private bool shorteningSpawnInterval = true;
    [SerializeField] private float timeToReduce;
    private float reductionTimer;
    [SerializeField] private DpreadMode dpreadMode;
    [SerializeField] private float reducTheSpawnTimeBy = 0;


    private void Start() {
        currentSpawnTime = Random.Range(minMaxSpawnTime.x, minMaxSpawnTime.y);
    }

    void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > currentSpawnTime) {

            Spawn(Random.Range(minMaxQuantityNumberEnemies.x, minMaxQuantityNumberEnemies.y));
            currentSpawnTime = Random.Range(minMaxSpawnTime.x, minMaxSpawnTime.y);
            spawnTimer = 0;
        }

        if (shorteningSpawnInterval) {
            reductionTimer += Time.deltaTime;
            if (reductionTimer > timeToReduce) {
                if (dpreadMode == DpreadMode.Number) {
                    minMaxSpawnTime -= Vector2.one * reducTheSpawnTimeBy;
                } else if (dpreadMode == DpreadMode.Percent) {
                    minMaxSpawnTime -= new Vector2(minMaxSpawnTime.x / 100 * reducTheSpawnTimeBy, minMaxSpawnTime.y / 100 * reducTheSpawnTimeBy);
                } else { Debug.LogError("Поведение для DpreadMode(" + dpreadMode + ") не назначено"); }

                if (minMaxSpawnTime.x < minimumPossibleSpawnTime) {
                    if (minMaxSpawnTime.y < minimumPossibleSpawnTime) {
                        shorteningSpawnInterval = false;
                        minMaxSpawnTime = Vector2.one * minimumPossibleSpawnTime;
                    } else {
                        minMaxSpawnTime.x = minimumPossibleSpawnTime;
                    }
                } else if (minMaxSpawnTime.y < minimumPossibleSpawnTime) {
                    minMaxSpawnTime.y = minimumPossibleSpawnTime;
                }
                reductionTimer = 0;
            }
        }
    }

    public void Spawn(int number) {
        for (int i = 0; i < number; i++) {
            Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        if (Random.Range(0, 100) > 90) {
            minMaxQuantityNumberEnemies += Vector2Int.one;
        }
    }
}
