using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MM_Player_Y_Rotation : MonoBehaviour
{
    [SerializeField]
    private MM_Test_Player playerTest;
    [SerializeField, Header("‚Ç‚ê‚®‚ç‚¢Šç‚ªŒ©‚¦‚È‚¢‚æ‚¤‚É‰ñ‚·‚©")]
    private int playerModelYRotation;

    void Update()
    {
        int dir = 0;
        dir = playerTest.GetPlayerOrientation() * playerModelYRotation;

        this.gameObject.transform.localEulerAngles = new Vector3(0, 180 - dir, 0);
    }
}
