using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] int currentLevel;
    [SerializeField] float currentSpeed;
    [SerializeField] float currentGeneTime;

    [Header("MapData")]
    [SerializeField] float playTime;
    [SerializeField] List<float> levelSwitchTime;
    [SerializeField] List<float> speedList;
    [SerializeField] List<float> itemGenerateTimeList;
    [SerializeField] GameObject itemCtrl;
    [SerializeField] GameObject startItems;
    [SerializeField] List<GameObject> railCtrlList;

    [Header("Managers")]
    [SerializeField] RollerUIManager _rollerUIManager;
    [SerializeField] RollerSoundManager _rollerSoundManager;
    [SerializeField] RollerSceneManager _rollerSceneManager;

    private void Start()
    {
        RollerMapStartSetting();
        StartCoroutine(RollerGame());
        startItems.gameObject.SetActive(false);
    }

    /*-------------- Game -------------------*/
    /// 맵 초기 셋팅
    private void RollerMapStartSetting()
    {
        _rollerSoundManager.PlayBGM();
        _rollerUIManager.FadeInImage();
    }

    /// 게임 진행 코루틴
    IEnumerator RollerGame()
    {
        yield return new WaitForSeconds(3f);

        // 시작 전 설명
        _rollerUIManager.StartCountDown(5);
        yield return new WaitForSeconds(12f);
        _rollerUIManager.StartTimer(playTime);

        // 게임 진행
        currentLevel = 1;
        startItems.gameObject.SetActive(true);
        LevelSetting(currentLevel);
        MoveSetting(true);
        yield return new WaitForSeconds(levelSwitchTime[0]); 

        currentLevel = 2;
        LevelSetting(currentLevel);
        yield return new WaitForSeconds(levelSwitchTime[1]);

        currentLevel = 3;
        LevelSetting(currentLevel);
        yield return new WaitForSeconds(levelSwitchTime[1] - 10f);

        // 종료 10초전
        StartCoroutine(EndCountDownSound());
        yield return new WaitForSeconds(10f);

        // 게임종료
        MoveSetting(false);
        itemCtrl.GetComponent<ItemGenerator>().ItemSpeedSetting(0.1f);

        // 게임 결과 확인
        yield return new WaitForSeconds(1f);
        CheckGameResult();
        yield return new WaitForSeconds(4f);

        // 메인맵 복귀
        StartCoroutine(ReturnMainMap());
    }

    /// 종료 10초 전 카운트다운
    IEnumerator EndCountDownSound()
    {
        _rollerSoundManager.PlaySFX("SFX_10Count");
        yield return new WaitForSeconds(10f);
        _rollerSoundManager.StopFSX();
    }

    /// 레벨 셋팅
    private void LevelSetting(int level)
    {
        currentSpeed = speedList[level - 1];
        currentGeneTime = itemGenerateTimeList[currentLevel - 1];
        ItemRailLevelSetting(currentSpeed);

        if(level == 1)
        {
            return;
        }
        else
        {
            _rollerUIManager.LevelUpUI();
            _rollerSoundManager.PlaySFX("SFX_LevelUP");
        }
    }

    /// 아이템, 레일 레벨 제어
    private void ItemRailLevelSetting(float speed)
    {
        // 아이템 생성 주기
        itemCtrl.GetComponent<ItemGenerator>().SetItemGanerateItme(currentGeneTime);

        //아이템, 레일 스피드
        itemCtrl.GetComponent<ItemGenerator>().ItemSpeedSetting(speed);
        for (int i = 0; i < railCtrlList.Count; i++)
        {
            railCtrlList[i].GetComponent<RailController>().railMoveSpeed = speed;
        }
    }

    /// 아이템, 레일 움직임 제어
    private void MoveSetting(bool b)
    {
        itemCtrl.GetComponent<ItemGenerator>().isGaming = b;
        for (int i = 0; i < railCtrlList.Count; i++)
        {
            railCtrlList[i].GetComponent<RailController>().isGaming = b;
        }
    }

    /// 게임 결과 확인
    private void CheckGameResult()
    {
        if (_rollerUIManager.GaugeValueCheck())
        {
            // 게임성공
            _rollerUIManager.GameSuccessUI();
            _rollerSoundManager.PlaySFX("SFX_GameSuccess");
        }
        else
        {
            // 게임실패
            _rollerUIManager.GameOverUI();
            _rollerSoundManager.PlaySFX("SFX_GameOver");
        }
    }

    /// 메인 맵 복귀
    IEnumerator ReturnMainMap()
    {
        _rollerUIManager.FadeOutImage();
        _rollerSoundManager.StopBGM();

        yield return new WaitForSeconds(5f);
        _rollerSceneManager.LoadMainMenuMap();
    }

    /*-------------- Event -------------------*/
    public void HitItem()
    {
        _rollerUIManager.GaugeUp();
        _rollerSoundManager.PlaySFX("SFX_Roller_Hit");
        Debug.Log("hit");
    }

    public void LoseItem()
    {
        Debug.Log("lose");
    }
}
