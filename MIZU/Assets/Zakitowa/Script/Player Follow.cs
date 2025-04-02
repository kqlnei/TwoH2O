using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // プレイヤーのTransformを格納
    public Vector3 offset;      // カメラとプレイヤーの間のオフセット

    void Start()
    {
        // カメラとプレイヤーの初期オフセットを設定
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // プレイヤーの位置にオフセットを加えてカメラの位置を更新
        transform.position = player.position + offset;
    }
}
