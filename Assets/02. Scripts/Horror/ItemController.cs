using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
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
        Debug.Log("아이템 습득");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
