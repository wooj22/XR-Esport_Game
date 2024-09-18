using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

    // [ 풍선 - 플레이어 콜라이더 충돌 ]
    //
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PopBalloon());
        }
    }


    // [ 풍선 터뜨리는 애니메이션 재생 -> 일정 시간 후 풍선 삭제 ]
    //
    //  Animator에 'Pop' 트리거를 걸어 풍선 터뜨리는 애니메이션 재생
    //  
    IEnumerator PopBalloon()
    {
        GetComponent<Animator>().SetTrigger("Pop");
        print("터지는 애니메이션이 실행됩니다.");

        yield return new WaitForSeconds(0.5f); 
        Destroy(gameObject);
    }
}
