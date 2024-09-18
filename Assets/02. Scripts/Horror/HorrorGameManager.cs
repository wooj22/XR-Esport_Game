using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGameManager : MonoBehaviour
{
    [Header("라운드")]
    [SerializeField] int currentRound;
    [SerializeField] public List<GameObject> ghostsByRoundList;
    [SerializeField] public List<GameObject> itemsByRoundList;
    [SerializeField] Transform ghostParent;
    [SerializeField] Transform itemParent;
    private List<GameObject> ghostsPullingList;
    private List<GameObject> itemsPullingList;
    private GameObject currentGhosts;
    private GameObject currentItems;

    void Start()
    {
        StartCoroutine(HorrorHousePulling());
    }


    /// 오브젝트 풀링
    IEnumerator HorrorHousePulling()
    {
        // 귀신
        ghostsPullingList.Add(Instantiate(ghostsByRoundList[0], ghostParent));
        ghostsPullingList[0].SetActive(false);
        ghostsPullingList.Add(Instantiate(ghostsByRoundList[1], ghostParent));
        ghostsPullingList[1].SetActive(false);

        // 아이템
        itemsPullingList.Add(Instantiate(itemsByRoundList[0], itemParent));
        itemsPullingList[0].SetActive(false);
        itemsPullingList.Add(Instantiate(itemsByRoundList[1], itemParent));
        itemsPullingList[0].SetActive(false);
        yield return (0f);

        //TODO : 풀링 하고싶었는데~ 안되네~ 나중에해야지
    }
}
