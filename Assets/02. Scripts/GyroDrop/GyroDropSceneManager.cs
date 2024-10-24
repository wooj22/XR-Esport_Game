using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroDropSceneManager : MonoBehaviour
{
    GameObject Front, Left, Right, Down;


    // [ ���Ŀ�Ʈ ī�޶� ���� ]
    // 
    void Start()
    {
        Front = GameObject.Find("SpoutCamera").transform.Find("Front").gameObject;
        Right = GameObject.Find("SpoutCamera").transform.Find("Right").gameObject;
        Left = GameObject.Find("SpoutCamera").transform.Find("Left").gameObject;
        Down = GameObject.Find("SpoutCamera").transform.Find("Down").gameObject;

        // Front : Field of View : 37.3
        //         Clipping Planes : Near 98.7 / Far 1781 
        // Right : Field of View : 48.454
        //         Clipping Planes : Near 74 / Far 1337
        // Left : Field of View : 48.454
        //         Clipping Planes : Near 74 / Far 1325
        // Down : Field of View : 142.7  => (progection�� perspective�� ����)
        //         Clipping Planes : Near 33.3 / Far 596.1 


        // �� ī�޶��� ���� ����
        ModifyCameraSettings(Front, 37.3f, 98.7f, 1781f);
        ModifyCameraSettings(Right, 48.454f, 74f, 1337f);
        ModifyCameraSettings(Left, 48.454f, 74f, 1325f);

        // Down ī�޶�� Perspective ���� ����
        Camera downCam = Down.GetComponent<Camera>();
        downCam.orthographic = false;
        ModifyCameraSettings(Down, 142.7f, 33.3f, 596.1f);
    }


    // ī�޶� ������ �����ϴ� �޼���
    void ModifyCameraSettings(GameObject cameraObj, float fov, float near, float far)
    {
        Camera cam = cameraObj.GetComponent<Camera>();
        if (cam != null)
        {
            cam.fieldOfView = fov;
            cam.nearClipPlane = near;
            cam.farClipPlane = far;
        }
        else
        {
            Debug.LogWarning($"{cameraObj.name}���� Camera ������Ʈ�� �����ϴ�.");
        }
    }


    public void LoadMainMenuMap()
    {
        SceneManager.LoadScene("MainMap_new");
    }
}
