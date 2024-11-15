using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroDropSceneManager : MonoBehaviour
{
    GameObject Front,Down;


    // 각 카메라의 원래 설정값을 저장할 딕셔너리
    Dictionary<GameObject, CameraSettings> originalCameraSettings = new Dictionary<GameObject, CameraSettings>();


    void Start()
    {
        Front = GameObject.Find("SpoutCamera").transform.Find("Front").gameObject;
        Down = GameObject.Find("SpoutCamera").transform.Find("Down").gameObject;

        // 초기 설정 저장
        SaveCameraSettings(Front);;
        SaveCameraSettings(Down);

        // 각 카메라 설정 변경
        ModifyCameraSettings(Front, 37f, 98.7f, 1781f);


        // Down 카메라는 Perspective 모드로 변경
        Camera downCam = Down.GetComponent<Camera>();
        downCam.orthographic = false;
        ModifyCameraSettings(Down, 130f, 33.05f, 596.1f);
    }


    // [ 카메라 설정 저장 ]
    void SaveCameraSettings(GameObject cameraObj)
    {
        Camera cam = cameraObj.GetComponent<Camera>();
        if (cam != null)
        {
            originalCameraSettings[cameraObj] = new CameraSettings(cam.fieldOfView, cam.nearClipPlane, cam.farClipPlane, cam.orthographic);
        }
    }


    // [ 카메라 설정 복원 ]
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


    // [ 카메라 설정 변경 ]
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
            Debug.LogWarning($"{cameraObj.name}에는 Camera 컴포넌트가 없습니다.");
        }
    }


    // [ 메인 메뉴로 이동하기 전 카메라 설정 복원하고 씬 이동 ]
    public void LoadMainMenuMap()
    {
        RestoreCameraSettings(Front);
        RestoreCameraSettings(Down);

        print("카메라 세팅을 복원하고 돌아갑니다.");

        SceneManager.LoadScene("MainMap_new");
    }


    // [ 카메라 설정 저장하는 구조체 ]
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
