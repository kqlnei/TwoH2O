using UnityEngine;

public class KK_PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform object1; // 追従するオブジェクト1
    [SerializeField] private Transform object2; // 追従するオブジェクト2
    [SerializeField] private float offset = 10f; // カメラとオブジェクトの距離

    private void LateUpdate()
    {
        if (object1 == null || object2 == null)
            return;

        // 2つのオブジェクトの中心を計算
        Vector3 centerPoint = (object1.position + object2.position) / 2;

        // 2つのオブジェクト間の距離を計算
        float distance = Vector3.Distance(object1.position, object2.position);

        // カメラのサイズを計算（画面に収めるための最小サイズを決定）
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Mathf.Max(distance / 2 + offset, 5f); // 画面に収めるためのサイズ

        // カメラの位置を設定
        transform.position = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
    }
}
