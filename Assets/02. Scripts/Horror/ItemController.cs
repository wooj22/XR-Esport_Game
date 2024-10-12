using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    private HorrorGameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<HorrorGameManager>();
    }

    /// 플레이어와 충돌
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _gameManager.OnPlayerEatItem();
            StartCoroutine(Eat());
            
        }
    }

    IEnumerator Eat()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
