using UnityEngine;
using System;

public class ModeManager : MonoBehaviour
{
    [Header("Player Models")]
    [SerializeField] private GameObject player1Model;
    [SerializeField] private GameObject player2Model;

    [HideInInspector] public KK_PlayerModelSwitcher player1Mode;
    [HideInInspector] public KK_PlayerModelSwitcher player2Mode;

    // �����ŕێ�����^�O
    private string _player1ModelTag;
    private string _player2ModelTag;

    // �O������Q�Ƃł���v���p�e�B��ǉ�
    public string player1ModelTag => _player1ModelTag;
    public string player2ModelTag => _player2ModelTag;

    public GameObject Player1 => player1Model;
    public GameObject Player2 => player2Model;

    public event Action<string> OnPlayer1ModelTagChanged;
    public event Action<string> OnPlayer2ModelTagChanged;

    private void Start()
    {
        if (player1Model != null)
        {
            player1Mode = player1Model.GetComponent<KK_PlayerModelSwitcher>();
            if (player1Mode == null)
                Debug.LogError("Player1��KK_PlayerModelSwitcher�R���|�[�l���g��������܂���I");
        }
        else
        {
            Debug.LogError("Player1���f�����A�T�C������Ă��܂���I");
        }

        if (player2Model != null)
        {
            player2Mode = player2Model.GetComponent<KK_PlayerModelSwitcher>();
            if (player2Mode == null)
                Debug.LogError("Player2��KK_PlayerModelSwitcher�R���|�[�l���g��������܂���I");
        }
        else
        {
            Debug.LogError("Player2���f�����A�T�C������Ă��܂���I");
        }
    }

    private void Update()
    {
        UpdatePlayerModelTag(ref _player1ModelTag, player1Mode, "Player1");
        UpdatePlayerModelTag(ref _player2ModelTag, player2Mode, "Player2");
    }

    private void UpdatePlayerModelTag(ref string currentTag, KK_PlayerModelSwitcher playerMode, string playerName)
    {
        if (playerMode != null && playerMode.currentModel != null)
        {
            string newTag = playerMode.currentModel.tag;
            if (currentTag != newTag)
            {
                currentTag = newTag;
                Debug.Log($"{playerName} model tag updated: {currentTag}");
                if (playerName == "Player1")
                {
                    OnPlayer1ModelTagChanged?.Invoke(currentTag);
                }
                else if (playerName == "Player2")
                {
                    OnPlayer2ModelTagChanged?.Invoke(currentTag);
                }
            }
        }
        else
        {
            Debug.LogWarning($"{playerName}�̃��f���܂��͂��� currentModel ���s�����Ă��܂��I");
        }
    }
}
