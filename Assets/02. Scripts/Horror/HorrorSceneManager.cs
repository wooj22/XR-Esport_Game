using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorSceneManager : MonoBehaviour
{
    public void LoadMainMenuMap()
    {
        SceneManager.LoadScene("MainMap");
    }
}
