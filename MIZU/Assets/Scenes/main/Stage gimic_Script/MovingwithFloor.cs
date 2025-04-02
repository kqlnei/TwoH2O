using UnityEngine;

public class MovingwithFloor : MonoBehaviour
{
    private Transform originalParent; // 元の親オブジェクト

    void Start()
    {
        // プレイヤーの元の親オブジェクトを記録しておく
        originalParent = transform.parent;
    }

    // 他のコライダーと接触した時に呼ばれるメソッド
    void OnCollisionEnter(Collision collision)
    {
        // "MovingFloor"というタグを持つオブジェクトに接触したとき
        if (collision.gameObject.CompareTag("MoveGround"))
        {
            // プレイヤーを床の子オブジェクトに設定
            transform.parent = collision.transform;
        }
    }

    // 接触が解除されたときに呼ばれるメソッド
    void OnCollisionExit(Collision collision)
    {
        // "MovingFloor"というタグを持つオブジェクトから離れたとき
        if (collision.gameObject.CompareTag("MoveGround"))
        {
            // プレイヤーを元の親オブジェクトに戻す
            transform.parent = originalParent;
        }
    }
}
