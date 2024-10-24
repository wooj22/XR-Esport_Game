using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] int currenLevel;
    [SerializeField] float currentSpeed;

    [Header("MapData")]
    [SerializeField] float playTime;
    [SerializeField] List<float> levelSwitchTime;
    [SerializeField] List<float> speedList;
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
    /// �� �ʱ� ����
    private void RollerMapStartSetting()
    {
        _rollerSoundManager.PlayBGM();
        _rollerUIManager.FadeInImage();
    }

    /// ���� ���� �ڷ�ƾ
    IEnumerator RollerGame()
    {
        yield return new WaitForSeconds(3f);

        // ī��Ʈ�ٿ�
        _rollerUIManager.StartCountDown(5);
        yield return new WaitForSeconds(8f);
        _rollerUIManager.StartTimer(playTime);

        // ���� ����
        currenLevel = 1;
        startItems.gameObject.SetActive(true);
        LevelSetting(currenLevel);
        MoveSetting(true);
        yield return new WaitForSeconds(levelSwitchTime[0] + 2f);   // �������� 2��

        currenLevel = 2;
        LevelSetting(currenLevel);
        yield return new WaitForSeconds(levelSwitchTime[1] + 1f);  // �������� 1��

        currenLevel = 3;
        LevelSetting(currenLevel);
        yield return new WaitForSeconds(levelSwitchTime[1] - 10f + 1f); // �������� 1��

        // ���� 10����
        StartCoroutine(EndCountDownSound());
        yield return new WaitForSeconds(10f);

        // ��������
        MoveSetting(false);
        itemCtrl.GetComponent<ItemGenerator>().ItemSpeedSetting(0.1f);

        // ���� ��� Ȯ��
        yield return new WaitForSeconds(1f);
        CheckGameResult();
        yield return new WaitForSeconds(4f);

        // ���θ� ����
        StartCoroutine(ReturnMainMap());
    }

    /// ���� 10�� �� ī��Ʈ�ٿ�
    IEnumerator EndCountDownSound()
    {
        _rollerSoundManager.PlaySFX("SFX_10Count");
        yield return new WaitForSeconds(10f);
        _rollerSoundManager.StopFSX();
    }

    /// ���� ����
    private void LevelSetting(int level)
    {
        currentSpeed = speedList[level - 1];
        SpeedSetting(currentSpeed);
    }

    /// ������, ���� ���ǵ� ����
    private void SpeedSetting(float speed)
    {
        itemCtrl.GetComponent<ItemGenerator>().ItemSpeedSetting(speed);
        for (int i = 0; i < railCtrlList.Count; i++)
        {
            railCtrlList[i].GetComponent<RailController>().railMoveSpeed = speed;
        }
    }

    /// ������, ���� ������ ����
    private void MoveSetting(bool b)
    {
        itemCtrl.GetComponent<ItemGenerator>().isGaming = b;
        for (int i = 0; i < railCtrlList.Count; i++)
        {
            railCtrlList[i].GetComponent<RailController>().isGaming = b;
        }
    }

    /// ���� ��� Ȯ��
    private void CheckGameResult()
    {
        if (_rollerUIManager.GaugeValueCheck())
        {
            // ���Ӽ���
            _rollerUIManager.GameSuccessUI();
            _rollerSoundManager.PlaySFX("SFX_Roller_GameClear");
        }
        else
        {
            // ���ӽ���
            _rollerUIManager.GameOverUI();
            _rollerSoundManager.PlaySFX("SFX_Roller_GameOver");
        }
    }

    /// ���� �� ����
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
