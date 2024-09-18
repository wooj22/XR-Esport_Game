using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGameManager : MonoBehaviour
{
    [Header("Round Setting")]
    [SerializeField] int curRound;                           // 현재라운드
    [SerializeField] int curRoundItemMaxCount;               // 현재라운드에서 먹어야할 아이템 개수
    [SerializeField] List<int> itemMaxCountByRoundList;      // 가 라운드별 먹어야할 아이템 개수
    [SerializeField] List<GameObject> ghostsByRoundList;     // 각 라운드별 귀신리스트
    [SerializeField] List<GameObject> itemsByRoundList;      // 각 라운드별 아이템리스트

    [Header("Else Setting")]
    [SerializeField] Transform ghostParent;
    [SerializeField] Transform itemParent;

    [Header("Sharing")]
    public int eatItemCount;        // 현재 라운드에서 플레이어가 먹은 아이템 개수(아이템,UI)

    private GameObject curGhosts;   // 현재 라운드에 생성된 귀신들 (관리용)
    private GameObject curItems;    // 현재 라운드에 생성된 아이템들 (관리용)


    void Start()
    {
        StartCoroutine(HorrorHouseStart());
    }


    /// 귀신의 집 게임 진행 코루틴
    IEnumerator HorrorHouseStart()
    {
        // 1라운드
        Debug.Log("1Round Start");
        curRound = 1;
        RoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        Roundup(curRound);

        // 1라운드 실패
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("1라운드에서 실패했습니다. 메인광장으로 이동합니다.");
            // 실패 UI 띄우고 코루틴 멈추고 메인광장으로 이동
        }

        // 1라운드 성공, 공간이동
        Debug.Log("1라운드 성공. 공간 이동을 시작합니다.");
        yield return new WaitForSeconds(5f);
        Debug.Log("5초뒤 2라운드를 시작합니다.");
        yield return new WaitForSeconds(5f);

        // 2라운드
        Debug.Log("2Round Start");
        curRound = 2;
        RoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        Roundup(curRound);

        // 2라운드 실패
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("2라운드에서 실패했습니다. 메인 광장으로 이동합니다.");
            // 실패 UI 띄우고 코루틴 멈추고 메인화면 이동
        }

        // 게임 성공
        Debug.Log("2라운드에서 성공했습니다. 메인 광장으로 이동합니다");
        // 최종 성공 UI 띄우고(+성공 효과) 코루틴 멈추고 메인화면 이동
    }

    /*----------------------------------------------------------------*/
    /// 라운드 셋팅
    private void RoundSetting(int round)
    {
        // 오브젝트 생성
        curGhosts = Instantiate(ghostsByRoundList[round-1], ghostParent);
        curItems = Instantiate(itemsByRoundList[round-1], itemParent);

        // 먹어야할 아이템 개수 셋팅
        curRoundItemMaxCount = itemMaxCountByRoundList[curRound - 1];
    }

    /// 라운드 정리
    private void Roundup(int round)
    {
        // 오브젝트 삭제
        Destroy(curGhosts);
        Destroy(curItems);
    }
}
