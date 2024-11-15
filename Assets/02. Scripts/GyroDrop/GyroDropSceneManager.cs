using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroDropSceneManager : MonoBehaviour
{
    GameObject Front,Down;


    // �� ī�޶��� ���� �������� ������ ��ųʸ�
    Dictionary<GameObject, CameraSettings> originalCameraSettings = new Dictionary<GameObject, CameraSettings>();


    void Start()
    {
        Front = GameObject.Find("SpoutCamera").transform.Find("Front").gameObject;
        Down = GameObject.Find("SpoutCamera").transform.Find("Down").gameObject;

        // �ʱ� ���� ����
        SaveCameraSettings(Front);;
        SaveCameraSettings(Down);

        // �� ī�޶� ���� ����
        ModifyCameraSettings(Front, 37f, 98.7f, 1781f);


        // Down ī�޶�� Perspective ���� ����
        Camera downCam = Down.GetComponent<Camera>();
        downCam.orthographic = false;
        ModifyCameraSettings(Down, 130f, 33.05f, 596.1f);
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
        RestoreCameraSettings(Down);

        print("ī�޶� ������ �����ϰ� ���ư��ϴ�.");

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
