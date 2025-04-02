using UnityEngine;

public class WaterControl : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private GameObject water;           // 上下する水オブジェクト
    [SerializeField] private float ascendAmount = 1.0f;     // 上昇時の移動量
    [SerializeField] private float descendAmount = 1.0f;    // 下降時の移動量
    [SerializeField] private float moveSpeed = 2.0f;        // 移動速度
    [SerializeField] private bool isAscending = true;       // trueなら上昇、falseなら下降

    [Header("Button Settings")]
    [SerializeField] private GameObject buttonTop;         // 押される部分のオブジェクト
    [SerializeField] private Material pressedMaterial;     // 押されたときのマテリアル
    [SerializeField] private float pressOffset = 0.01f;      // ボタンが下がる距離

    [Header("Dependencies")]
    [SerializeField] private MM_PlayerSpownTest spownTest;   // リスポーンテスト用スクリプト
    [SerializeField] private MM_ObserverBool observer;       // 状態監視用オブジェクト（必要ならインスペクターでアサイン）

    // 内部状態の保持
    private Vector3 initialWaterPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector3 initialButtonPosition;
    private Material buttonOriginalMaterial;

    // キャッシュしたコンポーネント
    private Collider ownCollider;

    private void Awake()
    {
        // 各参照が正しく設定されているか確認
        if (water == null)
            Debug.LogError("Waterオブジェクトがアサインされていません！");
        if (buttonTop == null)
            Debug.LogError("ButtonTopオブジェクトがアサインされていません！");
        if (spownTest == null)
            Debug.LogError("SpownTestがアサインされていません！");
        if (observer == null)
        {
            // Observerが未アサインなら新たに生成（必要に応じてDIなども検討）
            observer = new MM_ObserverBool();
        }

        // 初期位置のキャッシュ
        initialWaterPosition = water.transform.position;
        initialButtonPosition = buttonTop.transform.localPosition;
        buttonOriginalMaterial = buttonTop.GetComponent<Renderer>().material;

        // このオブジェクトのColliderをキャッシュ
        ownCollider = GetComponent<Collider>();
        if (ownCollider == null)
            Debug.LogWarning("WaterControlにColliderコンポーネントがありません！");
    }

    private void Update()
    {
        HandleWaterMovement();

        // Observerを用いた状態監視によるリセット判定
        if (observer.OnBoolTrueChange(spownTest.GetIsRespown()))
        {
            ResetWater();
        }
    }

    /// <summary>
    /// 水の移動処理。目標位置に向かって移動し、近づいたら移動完了とする。
    /// </summary>
    private void HandleWaterMovement()
    {
        if (isMoving)
        {
            water.transform.position = Vector3.MoveTowards(
                water.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(water.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Playerタグを持つオブジェクトのみ処理し、既に移動中でない場合に実行
        if (other.CompareTag("Player") && !isMoving)
        {
            // プレイヤーの状態チェック
            var playerPhaseState = other.GetComponent<MM_PlayerPhaseState>();
            if (playerPhaseState == null || playerPhaseState.GetState() != MM_PlayerPhaseState.State.Solid)
            {
                Debug.Log($"{gameObject.name}: プレイヤーはSolid状態ではありません");
                return;
            }

            // 移動目標位置の設定
            float direction = isAscending ? ascendAmount : -descendAmount;
            targetPosition = water.transform.position + new Vector3(0, direction, 0);
            isMoving = true;

            // Colliderを無効化して二重トリガーを防止
            if (ownCollider != null)
                ownCollider.enabled = false;

            // ボタンの位置とマテリアルを変更
            buttonTop.transform.localPosition = initialButtonPosition - new Vector3(0, pressOffset, 0);
            buttonTop.GetComponent<Renderer>().material = pressedMaterial;

            // 効果音を再生
            MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.ButtonPush);
            MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.WaterUpDown);
        }
    }

    /// <summary>
    /// 水とボタンの状態を初期状態にリセットする
    /// </summary>
    public void ResetWater()
    {
        water.transform.position = initialWaterPosition;
        isMoving = false;

        if (ownCollider != null)
            ownCollider.enabled = true;

        buttonTop.transform.localPosition = initialButtonPosition;
        buttonTop.GetComponent<Renderer>().material = buttonOriginalMaterial;
    }
}
