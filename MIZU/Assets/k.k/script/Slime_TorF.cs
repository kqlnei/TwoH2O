using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_TorF : MonoBehaviour
{
    public GameObject childObject; // 状態を監視する子オブジェクト
    public SlimeCamera slimeCamera; // スクリプト2を参照

    private bool lastState; // 最後に記録された状態

    void Start()
    {
        if (childObject != null)
        {
            lastState = childObject.activeSelf; // 初期状態を記録
        }
    }

    void Update()
    {
        if (childObject != null)
        {
            bool currentState = childObject.activeSelf;

            // 状態が変わった場合のみ通知
            if (currentState != lastState)
            {
                lastState = currentState;

                // スクリプト2に通知
                if (slimeCamera != null)
                {
                    slimeCamera.UpdateChildState(currentState);
                }
            }
        }
    }
}
