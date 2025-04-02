using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public float speed = 3f;  // 動くスピード
    public float distance = 5f;  // 移動距離

    private Vector3 startPosition;

    void Start()
    {
        // 初期位置を保存
        startPosition = transform.position;
    }

    void Update()
    {
        // Sin関数を使って左右に動かす
        float move = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPosition.x + move, startPosition.y, startPosition.z);
    }
}
