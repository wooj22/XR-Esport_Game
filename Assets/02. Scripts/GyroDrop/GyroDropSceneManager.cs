using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroDropSceneManager : MonoBehaviour
{
    GameObject Front, Left, Right, Down;


    // �� ī�޶��� ���� �������� ������ ��ųʸ�
    Dictionary<GameObject, CameraSettings> originalCameraSettings = new Dictionary<GameObject, CameraSettings>();


    void Start()
    {
        Front = GameObject.Find("SpoutCamera").transform.Find("Front").gameObject;
        Right = GameObject.Find("SpoutCamera").transform.Find("Right").gameObject;
        Left = GameObject.Find("SpoutCamera").transform.Find("Left").gameObject;
        Down = GameObject.Find("SpoutCamera").transform.Find("Down").gameObject;

        // �ʱ� ���� ����
        SaveCameraSettings(Front);
        SaveCameraSettings(Right);
        SaveCameraSettings(Left);
        SaveCameraSettings(Down);

        // �� ī�޶� ���� ����
        ModifyCameraSettings(Front, 37.3f, 98.7f, 1781f);
        ModifyCameraSettings(Right, 48.454f, 74f, 1337f);
        ModifyCameraSettings(Left, 48.454f, 74f, 1325f);

        // Down ī�޶�� Perspective ���� ����
        Camera downCam = Down.GetComponent<Camera>();
        downCam.orthographic = false;
        ModifyCameraSettings(Down, 142.7f, 33.3f, 596.1f);
    }


    // [ ī�޶� ���� ���� ]
    void SaveCameraSettings(GameObject cameraObj)
    {
        Camera cam = cameraObj.GetComponent<Camera>();
        if (cam != null)
        {
            originalCameraSettings[cameraObj] = new CameraSettings(cam.fieldOfView, cam.nearClipPlane, cam.farClipPlane, cam.orthographic);
        }
    }


    // [ ī�޶� ���� ���� ]
    void RestoreCameraSettings(GameObject cameraObj)
    {
        if (originalCameraSettings.TryGetValue(cameraObj, out CameraSettings settings))
        {
            Camera cam = cameraObj.GetComponent<Camera>();
            if (cam != null)
            {
                cam.fieldOfView = settings.FieldOfView;
                cam.nearClipPlane = settings.NearClipPlane;
                cam.farClipPlane = settings.FarClipPlane;
                cam.orthographic = settings.IsOrthographic;
            }
        }
    }


    // [ ī�޶� ���� ���� ]
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


    // [ ���� �޴��� �̵��ϱ� �� ī�޶� ���� �����ϰ� �� �̵� ]
    public void LoadMainMenuMap()
    {
        RestoreCameraSettings(Front);
        RestoreCameraSettings(Right);
        RestoreCameraSettings(Left);
        RestoreCameraSettings(Down);

        SceneManager.LoadScene("MainMap_new");
    }


    // [ ī�޶� ���� �����ϴ� ����ü ]
    struct CameraSettings
    {
        public float FieldOfView;
        public float NearClipPlane;
        public float FarClipPlane;
        public bool IsOrthographic;

        public CameraSettings(float fov, float near, float far, bool isOrthographic)
        {
            FieldOfView = fov;
            NearClipPlane = near;
            FarClipPlane = far;
            IsOrthographic = isOrthographic;
        }
    }
}
