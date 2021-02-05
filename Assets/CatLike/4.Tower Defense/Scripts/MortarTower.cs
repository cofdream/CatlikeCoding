using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField, Range(0.5f, 2f)]
    float shotsPerSecond = 1f;

    [SerializeField]
    Transform mortat = default;

    public override TowerType TowerType => TowerType.Mortar;

    public override void GameUpdate()
    {
        Launch();
    }
    public void Launch()
    {
        Vector3 launchPoint = mortat.position;
        Vector3 targetPoint = new Vector3(launchPoint.x + 3f, 0f, launchPoint.z);

        Debug.DrawLine(launchPoint, targetPoint, Color.yellow);
    }
}