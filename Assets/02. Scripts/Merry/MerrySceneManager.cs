using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MerrySceneManager : MonoBehaviour
{
    public void LoadMainMenuMap()
    {
        SceneManager.LoadScene("MainMap_new");
    }
}
