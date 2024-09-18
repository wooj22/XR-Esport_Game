using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostController : MonoBehaviour
{
    [SerializeField] Animator goshAnimator;

    private void Start()
    {
        goshAnimator = GetComponent<Animator>();
    }

    /// 플레이어와 충돌
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(FoundPlayer());
        }
    }

    IEnumerator FoundPlayer()
    {
        Debug.Log("귀신 발견");
        goshAnimator.Play("Attack", 0, 0);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
