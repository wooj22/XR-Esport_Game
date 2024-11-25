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

    /*------ new 이스터에그 ------*/
    [SerializeField] public float easterEggTime;
    private float currentTime;
    private Coroutine easterEggTimeCheak;


    private void Start()
    {
        /*
        for(int i =0; i< isLocked.Length; i++)
        {
            isLocked[i] = false;
        }
        */

        easterEggTimeCheak = StartCoroutine(EasterEggTimeCheak());
    } 

    // 이스터에그 타임 체크
    private IEnumerator EasterEggTimeCheak()
    {
        currentTime = 0f;

        while (easterEggTime > currentTime)
        {
            currentTime += 1f;
            yield return new WaitForSeconds(1f);
            Debug.Log(currentTime);
        }

        StartCoroutine(EndingCreadit());
        yield return null;
    }

    // 이스터에그 시간체크 초기화, 재시작
    public void TimeInitialization()
    {
        StopCoroutine(easterEggTimeCheak);
        easterEggTimeCheak = StartCoroutine(EasterEggTimeCheak());
    }

    // 엔딩크레딧
    IEnumerator EndingCreadit()
    {
        while (screetMessage.rectTransform.anchoredPosition.y < endPosY)
        {
            screetMessage.rectTransform.Translate(transform.up * speed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }

        screetMessage.rectTransform.anchoredPosition = new Vector2(0, startPosY);

        /*
        // 초기화
        isUnLock = false;
        nameList.Clear();
        for (int k = 0; k < isLocked.Length; k++)
        {
            isLocked[k] = false;
        }
        
        StopAllCoroutines();
        */
    }


    /*------ before 이스터에그 ------*/

    /*
    private void Update()
    {
        Cheaking();
    }
    */

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
}
