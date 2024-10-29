using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasterEgg : MonoBehaviour
{
    [Header ("Cheak")]
    [SerializeField] private bool[] isLocked = new bool[7];
    [SerializeField] private string[] lockedValue = new string[7];
    public List<string> nameList;

    [Header("EndingCredit")]
    [SerializeField] Text screetMessage;
    [SerializeField] float speed;
    [SerializeField] private float startPosY;
    [SerializeField] private float endPosY = 2000f;

    private bool isUnLock;

    private void Start()
    {
        for(int i =0; i< isLocked.Length; i++)
        {
            isLocked[i] = false;
        }
    }

    private void Update()
    {
        Cheaking();
    }

    // 플레이어가 밟은 곳 받아오기
    public void AddNameList(string name)
    {
        nameList.Add(name);
    }


    /// 퍼즐 잠금상태 확인
    private void Cheaking()
    {
        // 정답 할당
        for(int i=0; i< nameList.Count; i++)
        {
            if(nameList[i] == lockedValue[i])
            {
                isLocked[i] = true;
            }
            else
            {
                // 틀리면 초기화
                nameList.Clear();
                for (int k = 0; k < isLocked.Length; k++)
                {
                    isLocked[k] = false;
                }
            }
            
        }

        // 정답 체크
        if(nameList.Count >= 7)
        {
            for (int i = 0; i < isLocked.Length; i++)
            {
                if (isLocked[i] == true)
                {
                    isUnLock = true;
                }
                else
                {
                    isUnLock = false;
                }
            }

            // 7칸이 모두 true일때 ㄱㄱ
            if (isUnLock == true)
            {
                StartCoroutine(EndingCreadit());
            }
        }
    }
    
    // 엔딩크레딧
    IEnumerator EndingCreadit()
    {
        while(screetMessage.rectTransform.anchoredPosition.y < endPosY)
        {
            screetMessage.rectTransform.Translate(transform.up * speed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }

        screetMessage.rectTransform.anchoredPosition = new Vector2(0, startPosY);

        // 초기화
        isUnLock = false;
        nameList.Clear();
        for (int k = 0; k < isLocked.Length; k++)
        {
            isLocked[k] = false;
        }
        StopAllCoroutines();
    }
}
