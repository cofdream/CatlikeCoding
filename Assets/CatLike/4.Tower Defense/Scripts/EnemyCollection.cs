using System.Collections.Generic;

[System.Serializable]
public class EnemyCollection
{
    List<Enemy> enemyList = new List<Enemy>();

    public void Add(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].GameUpdate())
            {
                int lastIndex = enemyList.Count - 1;
                enemyList[i] = enemyList[lastIndex];
                enemyList.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }
}