using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g�ݒ�")]
    [SerializeField] private GameObject player1Model;
    [SerializeField] private GameObject player2Model;

    [Header("�Փːݒ�")]
    [SerializeField] private LayerMask collisionLayer; // �ՓˑΏۂ̃��C���[
    [SerializeField] private float collisionRadiusMultiplier = 1.0f;

    public event Action<GameObject, Collider> OnPlayerCollision; // �Փˎ��̃C�x���g�ʒm

    private Collider player1Collider;
    private Collider player2Collider;

    private Collider[] collisionResults = new Collider[10]; // NonAlloc�p�z��

    // �v���C���[1�ƃv���C���[2�̏Փ˂����R���C�_�[��ێ����郊�X�g
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

    // �v���C���[1���Փ˂����I�u�W�F�N�g���擾
    public List<Collider> GetPlayer1HitColliders()
    {
        return player1HitColliders;
    }

    // �v���C���[2���Փ˂����I�u�W�F�N�g���擾
    public List<Collider> GetPlayer2HitColliders()
    {
        return player2HitColliders;
    }

    private void CheckCollisions(Collider playerCollider, List<Collider> hitCollidersList)
    {
        // �Փ˔��a���v���C���[�̃o�E���f�B���O�G�N�X�e���g�Ɋ�Â��v�Z
        float radius = playerCollider.bounds.extents.magnitude * collisionRadiusMultiplier;
        int numHits = Physics.OverlapSphereNonAlloc(playerCollider.transform.position, radius, collisionResults, collisionLayer);

        // �Փ˂����R���C�_�[���X�g���N���A
        hitCollidersList.Clear();

        for (int i = 0; i < numHits; i++)
        {
            Collider hitCollider = collisionResults[i];
            if (hitCollider != playerCollider)
            {
                // �Փ˂����I�u�W�F�N�g�����X�g�ɒǉ�
                hitCollidersList.Add(hitCollider);

                // ���O�o�͂ƃC�x���g����
                Debug.Log($"�Փ˂����I�u�W�F�N�g: {hitCollider.gameObject.name}, �^�O: {hitCollider.tag}");
                OnPlayerCollision?.Invoke(playerCollider.gameObject, hitCollider);
            }
        }
    }

    // �V�[���r���[��ɏՓ˔͈͂�`��i�f�o�b�O�p�j
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
