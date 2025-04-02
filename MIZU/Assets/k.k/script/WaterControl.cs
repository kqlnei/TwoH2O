using UnityEngine;

public class WaterControl : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private GameObject water;           // �㉺���鐅�I�u�W�F�N�g
    [SerializeField] private float ascendAmount = 1.0f;     // �㏸���̈ړ���
    [SerializeField] private float descendAmount = 1.0f;    // ���~���̈ړ���
    [SerializeField] private float moveSpeed = 2.0f;        // �ړ����x
    [SerializeField] private bool isAscending = true;       // true�Ȃ�㏸�Afalse�Ȃ牺�~

    [Header("Button Settings")]
    [SerializeField] private GameObject buttonTop;         // ������镔���̃I�u�W�F�N�g
    [SerializeField] private Material pressedMaterial;     // �����ꂽ�Ƃ��̃}�e���A��
    [SerializeField] private float pressOffset = 0.01f;      // �{�^���������鋗��

    [Header("Dependencies")]
    [SerializeField] private MM_PlayerSpownTest spownTest;   // ���X�|�[���e�X�g�p�X�N���v�g
    [SerializeField] private MM_ObserverBool observer;       // ��ԊĎ��p�I�u�W�F�N�g�i�K�v�Ȃ�C���X�y�N�^�[�ŃA�T�C���j

    // ������Ԃ̕ێ�
    private Vector3 initialWaterPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector3 initialButtonPosition;
    private Material buttonOriginalMaterial;

    // �L���b�V�������R���|�[�l���g
    private Collider ownCollider;

    private void Awake()
    {
        // �e�Q�Ƃ��������ݒ肳��Ă��邩�m�F
        if (water == null)
            Debug.LogError("Water�I�u�W�F�N�g���A�T�C������Ă��܂���I");
        if (buttonTop == null)
            Debug.LogError("ButtonTop�I�u�W�F�N�g���A�T�C������Ă��܂���I");
        if (spownTest == null)
            Debug.LogError("SpownTest���A�T�C������Ă��܂���I");
        if (observer == null)
        {
            // Observer�����A�T�C���Ȃ�V���ɐ����i�K�v�ɉ�����DI�Ȃǂ������j
            observer = new MM_ObserverBool();
        }

        // �����ʒu�̃L���b�V��
        initialWaterPosition = water.transform.position;
        initialButtonPosition = buttonTop.transform.localPosition;
        buttonOriginalMaterial = buttonTop.GetComponent<Renderer>().material;

        // ���̃I�u�W�F�N�g��Collider���L���b�V��
        ownCollider = GetComponent<Collider>();
        if (ownCollider == null)
            Debug.LogWarning("WaterControl��Collider�R���|�[�l���g������܂���I");
    }

    private void Update()
    {
        HandleWaterMovement();

        // Observer��p������ԊĎ��ɂ�郊�Z�b�g����
        if (observer.OnBoolTrueChange(spownTest.GetIsRespown()))
        {
            ResetWater();
        }
    }

    /// <summary>
    /// ���̈ړ������B�ڕW�ʒu�Ɍ������Ĉړ����A�߂Â�����ړ������Ƃ���B
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
        // Player�^�O�����I�u�W�F�N�g�̂ݏ������A���Ɉړ����łȂ��ꍇ�Ɏ��s
        if (other.CompareTag("Player") && !isMoving)
        {
            // �v���C���[�̏�ԃ`�F�b�N
            var playerPhaseState = other.GetComponent<MM_PlayerPhaseState>();
            if (playerPhaseState == null || playerPhaseState.GetState() != MM_PlayerPhaseState.State.Solid)
            {
                Debug.Log($"{gameObject.name}: �v���C���[��Solid��Ԃł͂���܂���");
                return;
            }

            // �ړ��ڕW�ʒu�̐ݒ�
            float direction = isAscending ? ascendAmount : -descendAmount;
            targetPosition = water.transform.position + new Vector3(0, direction, 0);
            isMoving = true;

            // Collider�𖳌������ē�d�g���K�[��h�~
            if (ownCollider != null)
                ownCollider.enabled = false;

            // �{�^���̈ʒu�ƃ}�e���A����ύX
            buttonTop.transform.localPosition = initialButtonPosition - new Vector3(0, pressOffset, 0);
            buttonTop.GetComponent<Renderer>().material = pressedMaterial;

            // ���ʉ����Đ�
            MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.ButtonPush);
            MM_SoundManager.Instance.PlaySE(MM_SoundManager.SoundType.WaterUpDown);
        }
    }

    /// <summary>
    /// ���ƃ{�^���̏�Ԃ�������ԂɃ��Z�b�g����
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
