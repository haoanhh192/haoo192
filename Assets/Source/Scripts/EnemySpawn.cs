using D2D;
using D2D.Core;
using SRF;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class EnemySpawn : Unit
{
    [SerializeField] private int maxEnemiesOnField;
    [SerializeField] private float delayBetweenSpawn = .4f;

    [SerializeField] private LevelSO debugLevel;

    public LevelSO Level => debugLevel;
    public float LevelTimer => currentLevelTimer;

    private int currentWaveIndex = 0;

    private int currentAmount = 0;

    private float currentWaveTimer = 0;
    private float currentLevelTimer = 0;
    private float timer;

    private Wave currentWave;

    private Camera currentCamera;

    private bool isStopped;

    private void Awake()
    {
        _enemySpawn = this;

        currentCamera = Camera.main;

        SetWave(debugLevel.Waves[currentWaveIndex]);

        _stateMachine.On<WinState>(() => isStopped = true);
        _stateMachine.On<LoseState>(() => isStopped = true);
    }
    private void Update()
    {
        if (isStopped || _stateMachine.Last.Is<PauseState>())
        {
            return;
        }

        currentWaveTimer += Time.deltaTime;
        currentLevelTimer += Time.deltaTime;

        if (currentWaveTimer >= currentWave.Duration)
        {
            currentWaveIndex++;
            currentWaveTimer = 0;

            if (debugLevel.Waves.Length <= currentWaveIndex)
            {
                currentWaveIndex = 0;
            }

            SetWave(debugLevel.Waves[currentWaveIndex]);
        }

        /*if (currentLevelTimer >= debugLevel.TotalDuration)
        {
            {
                isStopped = true;
                _stateMachine.Push(new WinState());
            }
        }*/

        if (currentAmount >= maxEnemiesOnField)
        {
            return;
        }

        if (timer <= Time.time)
        {
            SpawnEnemy(currentWave.Enemies.Random());

            timer = Time.time + delayBetweenSpawn;
        }
    }

    public void SetWave(Wave wave)
    {
        currentWave = wave;
    }
    public void EnemyDied()
    {
        currentAmount--;
    }
    private void SpawnEnemy(GameObject enemy)
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
        
        if (Physics.Raycast(ray, out RaycastHit hit, _gameData.GroundLayer))
        {
            var enemyGO = Instantiate(enemy, hit.point, Quaternion.identity);
            var enemyComp = enemyGO.GetComponent<EnemyComponent>();

            var multiplier = Mathf.Pow(_gameData.baseSpeedMultiplier, _db.PassedLevels.Value);
            enemyComp.SetSpeed(multiplier);
            enemyComp.SetHealth(multiplier);

            currentAmount++;
        }
    }
}