using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    [Header ("SensorManager")]
    [SerializeField] OSCManager _oscManager;
    [SerializeField] SensorEnum sensorEnum;                              // 센서 Enum
    [SerializeField] RectTransform sensorGround;                         // 센서 그라운드

    private List<Vector3> sensorPositionList = new List<Vector3>();      // 포지션 리스트
    private SensorDataFormat SensorData;                                 // 센서 데이터 포멧


    void Start()
    {
        _oscManager = GameObject.Find("OSCManager").GetComponent<OSCManager>();
        StartCoroutine(GetSendsorData());
    }

    /// 센서가 인식된 위치를 업데이트(반복)
    IEnumerator GetSendsorData()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            SensorData = _oscManager.sensorData[((int)sensorEnum)];
            sensorPositionList.Clear();

            // 현재 인식되어있는 센서들의 포지션값 업데이트
            for (int i = 0; i < SensorData.positionList.Count; i++)
            {
                sensorPositionList.Add(new Vector3(Scale(-SensorData.rectSize.x / 2, SensorData.rectSize.x / 2, sensorGround.position.x - sensorGround.rect.width / 2, sensorGround.position.x + sensorGround.rect.width / 2, SensorData.positionList[i].x),
                                        Scale(-SensorData.rectSize.y / 2, SensorData.rectSize.y / 2, sensorGround.position.y - sensorGround.rect.height / 2, sensorGround.position.y + sensorGround.rect.height / 2, SensorData.positionList[i].y),
                                        0));
            }
        }
    }

    /// 포지션 리스트 getter 제공
    public List<Vector3> GetSensorVector()
    {
        return sensorPositionList;
    }


    /// 센서 그라운드 맵핑
    private float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
