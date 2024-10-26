using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header ("ItemData")]
    [SerializeField] float itemGenerateTime;
    [SerializeField] List<GameObject> itemList;
    [SerializeField] List<Transform> linePos;
    public bool isGaming;

    [Header("Pulling")]
    [SerializeField] int poolSize;
    private List<GameObject> pooledItems;
    private int currentIndex = 0;

    private void Start()
    {
        isGaming = false;
        PullingItem();
        StartCoroutine(CreateItem2());
        //InvokeRepeating("CreateItem", 0, itemGenerateTime);
    }

    /// 오브젝트 풀링
    private void PullingItem()
    {
        pooledItems = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            int randomItemIndex = Random.Range(0, itemList.Count);
            GameObject it = Instantiate(itemList[randomItemIndex], this.transform);
            it.SetActive(false);
            pooledItems.Add(it);
        }
    }

    /// 아이템 활성화 코루틴
    IEnumerator CreateItem2()
    {
        while (true)
        {
            if (isGaming)
            {
                GameObject item = GetPooledItem();  // 아이템 가져오기

                if (item != null)
                {
                    int posIndex = Random.Range(0, linePos.Count);        // 위치 지정
                    item.transform.position = linePos[posIndex].position;
                    item.SetActive(true);
                }
            }
            yield return new WaitForSeconds(itemGenerateTime);
        }
    }

    /// 아이템 활성화 - 인보크가 1초 이하는 잘 못따라오는 것 같아서 코루틴으로 수정
    private void CreateItem()
    {
        if(isGaming)
        {
            GameObject item = GetPooledItem();  // 아이템 가져오기

            if (item != null)
            {
                int posIndex = Random.Range(0, linePos.Count);        // 위치 지정
                item.transform.position = linePos[posIndex].position;
                item.SetActive(true);
            }
        }
    }

    /// 풀링 비활성 아이템 추적
    private GameObject GetPooledItem()
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            currentIndex = (currentIndex + 1) % pooledItems.Count;
            if (!pooledItems[currentIndex].activeInHierarchy)
            {
                return pooledItems[currentIndex];
            }
        }

        return null;  // 만약 사용할 수 있는 객체가 없으면 null 반환
    }

    /// 아이템 스피드 셋팅
    public void ItemSpeedSetting(float speed)
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            pooledItems[i].GetComponent<TwingController>().itemMoveSpeed = speed;
        }
    }

    /// 아이템 생성 시간 셋팅
    public void SetItemGanerateItme(float time)
    {
        this.itemGenerateTime = time;
    }
}
