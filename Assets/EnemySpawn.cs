using D2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class EnemySpawn : Unit
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesOnField;
    [SerializeField] private float delayBetweenSpawn = .4f;

    private int currentAmount = 0;
    private float timer;

    private Camera currentCamera;

    private List<Ray> drawRay = new();

    private void OnDrawGizmos()
    {
        foreach (var ray in drawRay)
        {
            Gizmos.DrawRay(ray);
        }
    }
    private void Awake()
    {
        _enemySpawn = this;

        currentCamera = Camera.main;
    }
    private void Update()
    {
        if (currentAmount >= maxEnemiesOnField)
        {
            return;
        }

        if (timer <= Time.time)
        {
            SpawnEnemy();
        }
    }

    public void EnemyDied()
    {
        currentAmount--;
    }
    private void SpawnEnemy()
    {
        float xPos;
        float yPos;
        float sign = Mathf.Sign(Random.Range(-1, 2));

        if (Random.Range(0, 100) > 50)
        {
            xPos = 0.5f + 0.6f * sign;
            yPos = Random.Range(0, 100) / 100f;
        }
        else
        {
            xPos = Random.Range(0, 100) / 100f;
            yPos = 0.5f + 0.6f * sign;
        }

        Vector3 direction = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, -10));
        
        Ray ray = new Ray(currentCamera.transform.position, currentCamera.transform.position - direction);
        drawRay.Add(ray);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _gameData.GroundLayer))
        {
            Instantiate(enemyPrefab, hit.point, Quaternion.identity);

            currentAmount++;
        }
    }
}