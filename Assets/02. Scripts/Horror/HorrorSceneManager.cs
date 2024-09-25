using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorSceneManager : MonoBehaviour
{
    [SerializeField] GameObject spoutCamera;

    private void Awake()
    {
        spoutCamera.gameObject.SetActive(true);
    }

    public void LoadMainMenuMap()
    {
        spoutCamera.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMap");
    }
}
