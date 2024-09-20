using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    public HorrorGameManager _gameManager;

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
            StartCoroutine(Wait1F());
            Destroy(this.gameObject);
        }
    }

    IEnumerator Wait1F()
    {
        yield return new WaitForSeconds(1f);
    }
}
