using UnityEngine;
using System.Collections;

public class ArmSpringController : MonoBehaviour
{
    public GameObject springJointObject; // SpringJointを持つオブジェクト

    private SpringJoint springJoint;

    void Start()
    {
        // springJointObjectが設定されているか確認
        if (springJointObject != null)
        {
            // SpringJointコンポーネントを取得
            springJoint = springJointObject.GetComponent<SpringJoint>();
            if (springJoint == null)
            {
                Debug.LogError("指定されたオブジェクトにSpringJointコンポーネントが見つかりません。");
            }
        }
        else
        {
            Debug.LogError("SpringJointを持つオブジェクトが指定されていません。");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Playerタグのついたオブジェクトと接触した時
        if (collision.gameObject.CompareTag("Player"))
        {
            if (springJoint != null)
            {
                StartCoroutine(ChangeSpringValue());
            }
        }
    }

    private IEnumerator ChangeSpringValue()
    {
        float startValue = springJoint.spring;
        float targetValue = 100f;
        float elapsedTime = 0f;
        float duration = 1.5f; // 2秒かけて変化

        // 2秒かけて徐々に300に変化
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            springJoint.spring = Mathf.Lerp(startValue, targetValue, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       
        Debug.Log("SpringJoint の Spring 値が 300 に設定されました。");

        // 2秒待機
        //yield return new WaitForSeconds(1000f);

        // 0に戻す
        springJoint.spring = 0f;
        Debug.Log("SpringJoint の Spring 値が 0 に戻されました。");
    }
}
