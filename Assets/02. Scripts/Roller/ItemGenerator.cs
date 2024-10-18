using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header ("ItemData")]
    [SerializeField] float itemGenerateTime;
    [SerializeField] List<GameObject> itemList;
    [SerializeField] List<Transform> linePos;

    [Header("Pulling")]
    [SerializeField] int poolSize;
    private List<GameObject> pooledItems;
    private int currentIndex = 0;

    private void Start()
    {
        PullingItem();
        InvokeRepeating("CreateItem", 0, itemGenerateTime);
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

    /// 아이템 활성화
    private void CreateItem()
    {
        GameObject item = GetPooledItem();  // 아이템 가져오기

        if (item != null)
        {
            int posIndex = Random.Range(0, linePos.Count);        // 위치 지정
            item.transform.position = linePos[posIndex].position;
            item.SetActive(true);
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
}
