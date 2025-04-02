using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; // 移動する開始地点
    public Transform pointB; // 移動する終了地点
    public float speed = 2.0f; // 移動速度
    private Vector3 target; // 現在のターゲット位置

    void Start()
    {
        // 初期のターゲットを設定（pointBに移動）
        target = pointB.position;
    }

    void Update()
    {
        // ターゲットに向かって移動
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // 床がターゲット地点に到達したら、ターゲットを反転
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (target == pointA.position)
            {
                target = pointB.position;
            }
            else
            {
                target = pointA.position;
            }
        }
    }

    // プレイヤーが床に触れた場合、床と一緒に移動させる
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform); // プレイヤーを床の子オブジェクトにする
        }
    }

    // プレイヤーが床から離れた場合、床の子オブジェクトから解除する
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); // プレイヤーを床の子オブジェクトから外す
        }
    }
}
