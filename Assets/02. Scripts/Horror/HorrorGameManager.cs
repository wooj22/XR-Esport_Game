using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGameManager : MonoBehaviour
{
    [Header("Current Round")]
    [SerializeField] int curRound;                           // 현재라운드
    [SerializeField] int curRoundItemMaxCount;               // 현재라운드에서 먹어야할 아이템 개수

    [Header("Round Data")]
    [SerializeField] List<int> itemMaxCountByRoundList;      // 각 라운드별 먹어야할 아이템 개수
    [SerializeField] List<GameObject> mapByRoundList;        // 각 라운드별 맵 데이터
    [SerializeField] List<GameObject> ghostsByRoundList;     // 각 라운드별 귀신 데이터
    [SerializeField] List<GameObject> itemsByRoundList;      // 각 라운드별 아이템 데이터

    [Header("Setting")]
    [SerializeField] Transform mapParent;
    [SerializeField] Transform ghostParent;
    [SerializeField] Transform itemParent;

    [Header("Managers")]
    [SerializeField] HorrorUIManager _horrorUIManager;
    [SerializeField] HorrorSoundManager _horrorSoundManager;


    private int eatItemCount;       // 현재 라운드에서 플레이어가 먹은 아이템 개수
    private GameObject curMap;      // 현재 라운드에 생성된 맵 (관리용)
    private GameObject curGhosts;   // 현재 라운드에 생성된 귀신들 (관리용)
    private GameObject curItems;    // 현재 라운드에 생성된 아이템들 (관리용)


    void Start()
    {
        StartCoroutine(HorrorHouseStart());
    }


    /*-------------------------- Corutines -----------------------------*/
    IEnumerator HorrorInstructionStart()
    {
        curMap = Instantiate(mapByRoundList[0], mapParent);
        _horrorSoundManager.PlayBGM(0);
        yield return new WaitForSeconds(5f);
        _horrorSoundManager.bgmSource.volume = 0.1f;
        _horrorSoundManager.PlaySFX("SFX_Horror_announcement1");
        yield return new WaitForSeconds(48f);
        _horrorSoundManager.bgmSource.volume = 1f;
        yield return new WaitForSeconds(10f);
        _horrorSoundManager.StopBGM();
        Destroy(curMap);
    }

    IEnumerator HorrorHouseStart()
    {
        // 안내음성
        StartCoroutine(HorrorInstructionStart());
        yield return new WaitForSeconds(64f);

        // 1라운드
        Debug.Log("1Round Start");
        curRound = 1;
        RoundDataSetting(curRound);
        RoundUISetting();
        RoundSoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        RoundCleaning(curRound);

        // 1라운드 실패
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("1라운드에서 실패했습니다. 메인광장으로 이동합니다.");
            // 실패 UI 띄우고 코루틴 멈추고 메인광장으로 이동
        }

        // 공간이동
        Debug.Log("1라운드 성공. 공간이동. 5초 뒤 2라운드를 시작합니다.");
        yield return new WaitForSeconds(5f);

        // 2라운드
        Debug.Log("2Round Start");
        curRound = 2;
        RoundDataSetting(curRound);
        RoundUISetting();
        RoundSoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        RoundCleaning(curRound);

        // 2라운드 실패
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("2라운드에서 실패했습니다. 메인 광장으로 이동합니다.");
            // 실패 UI 띄우고 코루틴 멈추고 메인화면 이동
        }

        // 게임성공 UI 띄우고(+성공 효과) 코루틴 멈추고 메인화면 이동
        Debug.Log("2라운드에서 성공했습니다. 메인 광장으로 이동합니다"); 
    }



    /*-------------------------- Round Setting -----------------------------*/

    private void RoundDataSetting(int round)
    {
        curMap = Instantiate(mapByRoundList[round - 1], mapParent);
        curGhosts = Instantiate(ghostsByRoundList[round-1], ghostParent);
        curItems = Instantiate(itemsByRoundList[round-1], itemParent);

        curRoundItemMaxCount = itemMaxCountByRoundList[curRound - 1];
        eatItemCount = 0;
    }

    private void RoundUISetting()
    {
        StartCoroutine(_horrorUIManager.StartRoundTimer());
        _horrorUIManager.ItemGaugeActive(curRoundItemMaxCount);
    }
    private void RoundSoundSetting(int round)
    {
        _horrorSoundManager.PlayBGM(round - 1);
    }

    private void RoundCleaning(int round)
    {
        Destroy(curMap);
        Destroy(curGhosts);
        Destroy(curItems);
        _horrorUIManager.ItemGaugeInactive();
        _horrorSoundManager.StopBGM();
    }

    


    /*--------------------------- Event ----------------------------*/

    public void OnPlayerEatItem()
    {
        eatItemCount++;
        _horrorUIManager.ItemGaugeUp();
        _horrorSoundManager.PlaySFX("SFX_Horror_item");
    }

    public void OnPlayerFindGhost()
    {
        _horrorSoundManager.PlaySFX("SFX_Horror_ghost");
    }
}
