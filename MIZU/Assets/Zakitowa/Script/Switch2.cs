using UnityEngine;
using System.Collections;

public class SwitchPlatform : MonoBehaviour
{
    public GameObject platform; // ?????????????????????????
    private Vector3 originalScale;
    public Vector3 enlargedScale = new Vector3(2, 2, 2); // ???????????????
    public float shrinkDelay = 2.0f; // ??????????????????

    private Coroutine shrinkCoroutine;

    void Start()
    {
        if (platform != null)
        {
            originalScale = platform.transform.localScale;
        }
    }

    // 3D??OnTriggerEnter????
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (shrinkCoroutine != null)
            {
                StopCoroutine(shrinkCoroutine);
            }
            platform.transform.localScale = enlargedScale;
        }
    }

    // 3D??OnTriggerExit????
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shrinkCoroutine = StartCoroutine(ShrinkPlatformAfterDelay());
        }
    }

    IEnumerator ShrinkPlatformAfterDelay()
    {
        yield return new WaitForSeconds(shrinkDelay);
        platform.transform.localScale = originalScale;
    }
}
