using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonSceneManager : MonoBehaviour
{
    public void LoadMainMenuMap()
    {
        SceneManager.LoadScene("MainMap");
    }
}
