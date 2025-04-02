using UnityEngine;
using System;

public class ModeManager : MonoBehaviour
{
    [Header("Player Models")]
    [SerializeField] private GameObject player1Model;
    [SerializeField] private GameObject player2Model;

    [HideInInspector] public KK_PlayerModelSwitcher player1Mode;
    [HideInInspector] public KK_PlayerModelSwitcher player2Mode;

    // 内部で保持するタグ
    private string _player1ModelTag;
    private string _player2ModelTag;

    // 外部から参照できるプロパティを追加
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
                Debug.LogError("Player1のKK_PlayerModelSwitcherコンポーネントが見つかりません！");
        }
        else
        {
            Debug.LogError("Player1モデルがアサインされていません！");
        }

        if (player2Model != null)
        {
            player2Mode = player2Model.GetComponent<KK_PlayerModelSwitcher>();
            if (player2Mode == null)
                Debug.LogError("Player2のKK_PlayerModelSwitcherコンポーネントが見つかりません！");
        }
        else
        {
            Debug.LogError("Player2モデルがアサインされていません！");
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
            Debug.LogWarning($"{playerName}のモデルまたはその currentModel が不足しています！");
        }
    }
}
