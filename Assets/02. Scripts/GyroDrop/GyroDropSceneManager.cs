using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroDropSceneManager : MonoBehaviour
{
    GameObject Front, Down;

    // �� ī�޶��� ���� �������� ������ ��ųʸ�
    Dictionary<GameObject, CameraSettings> originalCameraSettings = new Dictionary<GameObject, CameraSettings>();


    void Start()
    {
        Front = GameObject.Find("SpoutCamera").transform.Find("Front").gameObject;
        Down = GameObject.Find("SpoutCamera").transform.Find("Down").gameObject;

        // �ʱ� ���� ����
        SaveCameraSettings(Front);
        SaveCameraSettings(Down);

        // Down ī�޶�� Perspective ���� ����
        Camera downCam = Down.GetComponent<Camera>();
        downCam.orthographic = false;

        // �� ī�޶� ���� ���� : fov, near, far
        ModifyCameraSettings(Front, 90f, 47f, 800f);
        ModifyCameraSettings(Down, 90f, 47f, 800f);

        // ī�޶� ��ġ ���� 
        SettingCameraPositions();
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

    // [ Ư�� ��ġ�� ī�޶� ���� ]
    void SettingCameraPositions()
    {
        if (Front != null)
        {
            Front.transform.position = new Vector3(0, 13.8f, 52f);
        }

        if (Down != null)
        {
            Down.transform.position = new Vector3(0, 13.2f, 52.7f); 
        }
    }

    // [ Ư�� ��ġ�� ī�޶� ���� ���� ]
    void RestoreCameraPositions()
    {
        if (Front != null)
        {
            Front.transform.position = new Vector3(0, 13.58f, -40.5f); 
        }

        if (Down != null)
        {
            Down.transform.position = new Vector3(0, 0f, 52.7f); 
        }
    }

    // [ ���� �޴��� �̵��ϱ� �� ī�޶� ���� �����ϰ� �� �̵� ]
    public void LoadMainMenuMap()
    {
        RestoreCameraSettings(Front);
        RestoreCameraSettings(Down);

        // ī�޶� ��ġ ����
        // RestoreCameraPositions();

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
