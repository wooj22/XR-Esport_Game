using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostController : MonoBehaviour
{
    [SerializeField] Animator goshAnimator;
    private HorrorGameManager _gameManager;

    private void Start()
    {
        goshAnimator = GetComponent<Animator>();
        _gameManager = GameObject.Find("GameManager").GetComponent<HorrorGameManager>();
    }

    /// 플레이어와 충돌
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _gameManager.OnPlayerFindGhost();
            goshAnimator.Play("Attack", 0, 0);
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
