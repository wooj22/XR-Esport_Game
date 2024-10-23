using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroManager : MonoBehaviour
{
    [Header("SpoutCamera")]
    [SerializeField] GameObject xRoom;
    [SerializeField] GameObject spoutCamera;
    [SerializeField] float spoutMoveSpeed;

    [Header ("Front")]
    [SerializeField] Animator frontDoor;

    [Header("FadeOutImage")]
    [SerializeField] Image fade_front;
    [SerializeField] Image fade_right;
    [SerializeField] Image fade_left;
    [SerializeField] Image fade_down;

    [Header("Managers")]
    [SerializeField] IntroSoundManager _introSoundManager;
    [SerializeField] IntroSceneManager _introSceneManager;

    private Coroutine sp;

    private void Start()
    {
        _introSoundManager.PlayBGM();
    }

    public void StartMidnightCarnival()
    {
        Debug.Log("인트로 시작");
        StartCoroutine(MovingSpoutCamera());
        StartCoroutine(FadeOut());
        frontDoor.SetBool("IntroStart", true);
    }

    /// 카메라 앞으로 이동
    IEnumerator MovingSpoutCamera()
    {
        while (true)
        {
            xRoom.transform.Translate(Vector3.forward * spoutMoveSpeed);
            spoutCamera.transform.Translate(Vector3.forward * spoutMoveSpeed);
            yield return new WaitForSeconds(0.05f);
            if(spoutCamera.transform.position.z >= -1f)
            {
                break;
            }
        }
        xRoom.transform.position = Vector3.zero;
        spoutCamera.transform.position = Vector3.zero;
    }

    /// 페이드아웃 연출
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        _introSoundManager.StopBGM();

        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.005f);

            fade_front.color = new Color(0, 0, 0, fadeCount);
            fade_right.color = new Color(0, 0, 0, fadeCount);
            fade_left.color = new Color(0, 0, 0, fadeCount);
            fade_down.color = new Color(0, 0, 0, fadeCount);
        }

        spoutCamera.transform.position = Vector3.zero;
        yield return new WaitForSeconds(3f);
        _introSceneManager.LoadMainMenuMap();
    }
}
