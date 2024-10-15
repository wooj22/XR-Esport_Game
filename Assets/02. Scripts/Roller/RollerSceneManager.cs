using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RollerSceneManager : MonoBehaviour
{
    public void LoadMainMenuMap()
    {
        SceneManager.LoadScene("MainMap_new");
    }
}
