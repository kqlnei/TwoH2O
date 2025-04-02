using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("プレイヤーオブジェクト設定")]
    [SerializeField] private GameObject player1Model;
    [SerializeField] private GameObject player2Model;

    [Header("衝突設定")]
    [SerializeField] private LayerMask collisionLayer; // 衝突対象のレイヤー
    [SerializeField] private float collisionRadiusMultiplier = 1.0f;

    public event Action<GameObject, Collider> OnPlayerCollision; // 衝突時のイベント通知

    private Collider player1Collider;
    private Collider player2Collider;

    private Collider[] collisionResults = new Collider[10]; // NonAlloc用配列

    // プレイヤー1とプレイヤー2の衝突したコライダーを保持するリスト
    private List<Collider> player1HitColliders = new List<Collider>();
    private List<Collider> player2HitColliders = new List<Collider>();

    private void Start()
    {
        if (player1Model != null)
            player1Collider = player1Model.GetComponent<Collider>();

        if (player2Model != null)
            player2Collider = player2Model.GetComponent<Collider>();
    }

    private void Update()
    {
        if (player1Collider != null)
            CheckCollisions(player1Collider, player1HitColliders);

        if (player2Collider != null)
            CheckCollisions(player2Collider, player2HitColliders);
    }

    // プレイヤー1が衝突したオブジェクトを取得
    public List<Collider> GetPlayer1HitColliders()
    {
        return player1HitColliders;
    }

    // プレイヤー2が衝突したオブジェクトを取得
    public List<Collider> GetPlayer2HitColliders()
    {
        return player2HitColliders;
    }

    private void CheckCollisions(Collider playerCollider, List<Collider> hitCollidersList)
    {
        // 衝突半径をプレイヤーのバウンディングエクステントに基づき計算
        float radius = playerCollider.bounds.extents.magnitude * collisionRadiusMultiplier;
        int numHits = Physics.OverlapSphereNonAlloc(playerCollider.transform.position, radius, collisionResults, collisionLayer);

        // 衝突したコライダーリストをクリア
        hitCollidersList.Clear();

        for (int i = 0; i < numHits; i++)
        {
            Collider hitCollider = collisionResults[i];
            if (hitCollider != playerCollider)
            {
                // 衝突したオブジェクトをリストに追加
                hitCollidersList.Add(hitCollider);

                // ログ出力とイベント発火
                Debug.Log($"衝突したオブジェクト: {hitCollider.gameObject.name}, タグ: {hitCollider.tag}");
                OnPlayerCollision?.Invoke(playerCollider.gameObject, hitCollider);
            }
        }
    }

    // シーンビュー上に衝突範囲を描画（デバッグ用）
    private void OnDrawGizmos()
    {
        if (player1Model != null)
        {
            Collider col = player1Model.GetComponent<Collider>();
            if (col != null)
            {
                Gizmos.color = Color.red;
                float radius = col.bounds.extents.magnitude * collisionRadiusMultiplier;
                Gizmos.DrawWireSphere(col.transform.position, radius);
            }
        }

        if (player2Model != null)
        {
            Collider col = player2Model.GetComponent<Collider>();
            if (col != null)
            {
                Gizmos.color = Color.blue;
                float radius = col.bounds.extents.magnitude * collisionRadiusMultiplier;
                Gizmos.DrawWireSphere(col.transform.position, radius);
            }
        }
    }
}
