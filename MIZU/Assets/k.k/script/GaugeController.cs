using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private GameObject gauge;
    [SerializeField] private int maxHP = 100;
    private float hpUnit; // 1HPあたりのゲージサイズ

    [Header("参照オブジェクト")]
    [SerializeField] private GameObject managerObject;  // ModeManagerがついているオブジェクト
    private ModeManager modeManager;
    private CollisionManager collisionManager;
    private GameObject player1Object;
    private GameObject player2Object;

    public enum Player { Player1, Player2 }
    public Player player;

    [Header("属性ダメージ倍率")]
    public float water = 1;
    public float ice = 1;
    public float cloud = 1;
    public float slime = 1;

    private bool isOnGround = false;
    private bool isOnWater = false;

    private List<string> allowedTags = new List<string> { "HealSpot", "Ground" };

    [SerializeField] private MM_GroundCheck groundCheck;

    // RectTransformのキャッシュ
    private RectTransform gaugeRect;

    private void Start()
    {
        // 各コンポーネントのキャッシュ
        if (gauge != null)
            gaugeRect = gauge.GetComponent<RectTransform>();
        else
            Debug.LogError("Gaugeオブジェクトが設定されていません");

        modeManager = managerObject.GetComponent<ModeManager>();
        collisionManager = managerObject.GetComponent<CollisionManager>();

        hpUnit = gaugeRect.sizeDelta.x / maxHP;

        player1Object = modeManager.Player1;
        player2Object = modeManager.Player2;

        if (groundCheck == null)
            Debug.LogWarning($"{nameof(groundCheck)}がアタッチされていません");
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Update()
    {
        // モデルタグの取得
        string currentModelTag = (player == Player.Player1) ? modeManager.player1ModelTag : modeManager.player2ModelTag;

        // ダメージ処理（攻撃力は定数化）
        const float attackPower = 0.1f;
        BeInjured(attackPower, currentModelTag);

        // ヒールスポットとの衝突チェック
        if (player == Player.Player1)
        {
            foreach (Collider col in collisionManager.GetPlayer1HitColliders())
            {
                if (isOnGround || isOnWater || allowedTags.Contains(col.gameObject.tag))
                {
                    Heal(100f);  // プレイヤー1の回復量
                    break;
                }
            }
        }
        else if (player == Player.Player2)
        {
            foreach (Collider col in collisionManager.GetPlayer2HitColliders())
            {
                if (isOnGround || isOnWater || allowedTags.Contains(col.gameObject.tag))
                {
                    Heal(100f);  // プレイヤー2の回復量
                    break;
                }
            }
        }
    }

    private void GroundCheck()
    {
        isOnGround = groundCheck.IsGround();
        isOnWater = groundCheck.IsPuddle();
    }

    public void BeInjured(float attack, string modelTag)
    {
        float damage = 0f;

        switch (modelTag)
        {
            case "Water":
                damage = hpUnit * attack * water;
                break;
            case "Ice":
                damage = hpUnit * attack * ice;
                break;
            case "Cloud":
                damage = hpUnit * attack * cloud;
                break;
            case "Slime":
                damage = hpUnit * attack * slime;
                break;
        }

        // ダメージ処理をコルーチンで実行（必要に応じてループ処理に変更可能）
        StartCoroutine(DamageCoroutine(damage));
    }

    private IEnumerator DamageCoroutine(float damage)
    {
        // 単一フレームで更新する場合（必要なら時間をかけた連続処理に変更）
        float damagePerFrame = damage; // 必要に応じて分割
        Vector2 currentSize = gaugeRect.sizeDelta;
        currentSize.x -= damagePerFrame * Time.deltaTime;

        if (currentSize.x <= 0)
        {
            currentSize.x = 0;
            Debug.Log($"{player} is dead!");

            MM_Test_Player playerScript = null;
            if (player == Player.Player1 && player1Object != null)
                playerScript = player1Object.GetComponent<MM_Test_Player>();
            else if (player == Player.Player2 && player2Object != null)
                playerScript = player2Object.GetComponent<MM_Test_Player>();

            if (playerScript != null)
                playerScript.OnStateChangeLiquid();
        }

        gaugeRect.sizeDelta = currentSize;
        yield return null;
    }

    public void Heal(float healAmount)
    {
        float healValue = hpUnit * healAmount;
        StartCoroutine(HealCoroutine(healValue));
    }

    private IEnumerator HealCoroutine(float heal)
    {
        Vector2 currentSize = gaugeRect.sizeDelta;
        currentSize.x += heal;

        float maxWidth = hpUnit * maxHP;
        if (currentSize.x > maxWidth)
            currentSize.x = maxWidth;

        gaugeRect.sizeDelta = currentSize;
        yield return null;
    }
}
