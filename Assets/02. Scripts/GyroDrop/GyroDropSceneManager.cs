using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroDropSceneManager : MonoBehaviour
{
    GameObject Front, Left, Right, Down;


    // [ 스파우트 카메라 조정 ]
    // 
    void Start()
    {
        Front = GameObject.Find("SpoutCamera").transform.Find("Front").gameObject;
        Right = GameObject.Find("SpoutCamera").transform.Find("Right").gameObject;
        Left = GameObject.Find("SpoutCamera").transform.Find("Left").gameObject;
        Down = GameObject.Find("SpoutCamera").transform.Find("Down").gameObject;

        // 스파우트 카메라 : 0,0,0

        // Front : Field of View : 37.3
        //         Clipping Planes : Near 98.7 / Far 1781 
        // Right : Field of View : 48.454
        //         Clipping Planes : Near 74 / Far 1337
        // Left : Field of View : 48.454
        //         Clipping Planes : Near 74 / Far 1325
        // Down : Field of View : 142.7  => (progection을 perspective로 변경)
        //         Clipping Planes : Near 33.3 / Far 596.1 



    }


    public void LoadMainMenuMap()
    {
        SceneManager.LoadScene("MainMap_new");
    }
}
