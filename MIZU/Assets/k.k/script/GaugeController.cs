using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private GameObject gauge;
    [SerializeField] private int maxHP = 100;
    private float hpUnit; // 1HP������̃Q�[�W�T�C�Y

    [Header("�Q�ƃI�u�W�F�N�g")]
    [SerializeField] private GameObject managerObject;  // ModeManager�����Ă���I�u�W�F�N�g
    private ModeManager modeManager;
    private CollisionManager collisionManager;
    private GameObject player1Object;
    private GameObject player2Object;

    public enum Player { Player1, Player2 }
    public Player player;

    [Header("�����_���[�W�{��")]
    public float water = 1;
    public float ice = 1;
    public float cloud = 1;
    public float slime = 1;

    private bool isOnGround = false;
    private bool isOnWater = false;

    private List<string> allowedTags = new List<string> { "HealSpot", "Ground" };

    [SerializeField] private MM_GroundCheck groundCheck;

    // RectTransform�̃L���b�V��
    private RectTransform gaugeRect;

    private void Start()
    {
        // �e�R���|�[�l���g�̃L���b�V��
        if (gauge != null)
            gaugeRect = gauge.GetComponent<RectTransform>();
        else
            Debug.LogError("Gauge�I�u�W�F�N�g���ݒ肳��Ă��܂���");

        modeManager = managerObject.GetComponent<ModeManager>();
        collisionManager = managerObject.GetComponent<CollisionManager>();

        hpUnit = gaugeRect.sizeDelta.x / maxHP;

        player1Object = modeManager.Player1;
        player2Object = modeManager.Player2;

        if (groundCheck == null)
            Debug.LogWarning($"{nameof(groundCheck)}���A�^�b�`����Ă��܂���");
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Update()
    {
        // ���f���^�O�̎擾
        string currentModelTag = (player == Player.Player1) ? modeManager.player1ModelTag : modeManager.player2ModelTag;

        // �_���[�W�����i�U���͂͒萔���j
        const float attackPower = 0.1f;
        BeInjured(attackPower, currentModelTag);

        // �q�[���X�|�b�g�Ƃ̏Փ˃`�F�b�N
        if (player == Player.Player1)
        {
            foreach (Collider col in collisionManager.GetPlayer1HitColliders())
            {
                if (isOnGround || isOnWater || allowedTags.Contains(col.gameObject.tag))
                {
                    Heal(100f);  // �v���C���[1�̉񕜗�
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
                    Heal(100f);  // �v���C���[2�̉񕜗�
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

        // �_���[�W�������R���[�`���Ŏ��s�i�K�v�ɉ����ă��[�v�����ɕύX�\�j
        StartCoroutine(DamageCoroutine(damage));
    }

    private IEnumerator DamageCoroutine(float damage)
    {
        // �P��t���[���ōX�V����ꍇ�i�K�v�Ȃ玞�Ԃ��������A�������ɕύX�j
        float damagePerFrame = damage; // �K�v�ɉ����ĕ���
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
