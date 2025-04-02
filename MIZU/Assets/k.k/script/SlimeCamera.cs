using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCamera : MonoBehaviour
{
    public GameObject[] childObjects; // 更新する子オブジェクトたち

    public void UpdateChildState(bool newState)
    {
        foreach (GameObject child in childObjects)
        {
            if (child != null)
            {
                child.SetActive(newState);
            }
        }
    }
}
