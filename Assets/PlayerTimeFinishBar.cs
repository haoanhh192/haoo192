using D2D.UI;

public class PlayerTimeFinishBar : FillBarBase
{
    private EnemySpawn _enemySpawn;
    private LevelSO level;

    private void Start()
    {
        _enemySpawn = FindObjectOfType<EnemySpawn>();
        level = _enemySpawn.Level;
    }

    protected override float Calculate()
    {
        return _enemySpawn.LevelTimer / level.TotalDuration;
    }
}