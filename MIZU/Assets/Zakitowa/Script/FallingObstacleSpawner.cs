using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    [Header("障害物のプレハブ")]
    public GameObject obstaclePrefab; // 障害物のプレハブ

    [Header("生成位置")]
    public Transform spawnPoint; // 障害物の生成位置

    [Header("生成間隔")]
    public float spawnInterval = 2f; // 障害物の生成間隔（秒）

    [Header("削除する高さ")]
    public float destroyHeight = -10f; // 障害物が削除される高さ

    private void Start()
    {
        // 指定された間隔で障害物を生成
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnInterval);
    }

    private void SpawnObstacle()
    {
        // 新しい障害物を生成
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);

        // 障害物が画面外に落ちるのを待ち、指定の高さに達したら削除
        StartCoroutine(DestroyObstacleAfterFall(obstacle));
    }

    private System.Collections.IEnumerator DestroyObstacleAfterFall(GameObject obstacle)
    {
        // 障害物が指定の高さまで落ちるまで待機
        while (obstacle != null && obstacle.transform.position.y > destroyHeight)
        {
            yield return null; // フレームを待つ
        }

        if (obstacle != null)
        {
            Destroy(obstacle); // 指定の高さに達したら障害物を削除
        }
    }
}
