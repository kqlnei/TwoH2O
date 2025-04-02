using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{
    public float b_force = 1000f;
    public float b_torque = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        CharacterJoint joint = GetComponent<CharacterJoint>();
        joint.breakForce = b_force;   // ç≈ëÂ1000ÇÃóÕÇ≈âÛÇÍÇÈ
        joint.breakTorque = b_torque;   // ç≈ëÂ500ÇÃÉgÉãÉNÇ≈âÛÇÍÇÈ
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnJointBreak(float breakForce)
    {
        Debug.Log("Joint broke with force: " + breakForce);
    }
}
