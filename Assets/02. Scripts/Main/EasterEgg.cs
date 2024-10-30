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

    // �÷��̾ ���� �� �޾ƿ���
    public void AddNameList(string name)
    {
        nameList.Add(name);
    }


    /// ���� ��ݻ��� Ȯ��
    private void Cheaking()
    {
        // ���� �Ҵ�
        for(int i=0; i< nameList.Count; i++)
        {
            if(nameList[i] == lockedValue[i])
            {
                isLocked[i] = true;
            }
            else
            {
                // Ʋ���� �ʱ�ȭ
                nameList.Clear();
                for (int k = 0; k < isLocked.Length; k++)
                {
                    isLocked[k] = false;
                }
            }
            
        }

        // ���� üũ
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

            // 7ĭ�� ��� true�϶� ����
            if (isUnLock == true)
            {
                StartCoroutine(EndingCreadit());
            }
        }
    }
    
    // ����ũ����
    IEnumerator EndingCreadit()
    {
        while(screetMessage.rectTransform.anchoredPosition.y < endPosY)
        {
            screetMessage.rectTransform.Translate(transform.up * speed * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }

        screetMessage.rectTransform.anchoredPosition = new Vector2(0, startPosY);

        // �ʱ�ȭ
        isUnLock = false;
        nameList.Clear();
        for (int k = 0; k < isLocked.Length; k++)
        {
            isLocked[k] = false;
        }
        StopAllCoroutines();
    }
}