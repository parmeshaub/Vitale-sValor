using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    [SerializeField] private float timeToDisappear;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterWait());
    }

    private IEnumerator DestroyAfterWait() {
        yield return new WaitForSeconds(timeToDisappear);
        Destroy(gameObject);
    }
}
