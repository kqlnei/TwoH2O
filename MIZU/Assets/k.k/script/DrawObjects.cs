using UnityEngine;
using System.Collections.Generic;

public class DrawObjects : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject drawObjectPrefab;  // 置きたいオブジェクトのPrefab
    public float minDistance = 0.1f;     // オブジェクトを配置する最小距離

    private List<GameObject> drawObjects;
    private Vector3 lastPosition;

    void Start()
    {
        drawObjects = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // オブジェクトを削除してリセット
            foreach (var obj in drawObjects)
            {
                Destroy(obj);
            }
            drawObjects.Clear();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // カメラからの適切な距離を設定
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;

            if (drawObjects.Count == 0 || Vector3.Distance(worldPosition, lastPosition) > minDistance)
            {
                GameObject drawObject = Instantiate(drawObjectPrefab, worldPosition, Quaternion.identity);
                drawObjects.Add(drawObject);
                lastPosition = worldPosition;
            }
        }
    }
}
