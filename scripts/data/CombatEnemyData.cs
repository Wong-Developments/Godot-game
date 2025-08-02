namespace Game.Scripts.Data;

public class CombatEnemyData
{
    public string EnemyScenePath { get; set; }
    public string DisplayName { get; set; }
    public int MaxHealth { get; set; }

    public CombatEnemyData(string scenePath, string displayName, int maxHealth)
    {
        EnemyScenePath = scenePath;
        DisplayName = displayName;
        MaxHealth = maxHealth;
    }
}
