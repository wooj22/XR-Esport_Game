using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] GameObject spoutCamera;
    [SerializeField] MainSoundManager _mainSoundManager;

    /// æ¿ ¿Ãµø
    public void OnLoadSceneByName(string sceneName)
    {
        _mainSoundManager.PlaySFX("SFX_Main_openMap");
        StartCoroutine(SoundWaiting(sceneName));
    }

    IEnumerator SoundWaiting(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
}
