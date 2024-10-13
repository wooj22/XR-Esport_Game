using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    /// æ¿ ¿Ãµø
    public void OnLoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
