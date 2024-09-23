using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;

    void Start()
    {
        // 시그라프용 사운드 들려주고 나가기
        StartCoroutine(CircusSample_sigraf());
    }


    /*-------------------------- Corutines -----------------------------*/
    IEnumerator CircusSample_sigraf()
    {
        _circusSoundManager.PlayBGM();
        yield return new WaitForSeconds(5f);
        _circusSoundManager.bgmSource.volume = 0.3f;
        // 안내음성 재생
        _circusSoundManager.PlaySFX("SFX_Horror_announcement");
        yield return new WaitForSeconds(60f);
        _circusSoundManager.bgmSource.volume = 1f;
        yield return new WaitForSeconds(10f);
        _circusSceneManager.LoadMainMenuMap();
    }
}
