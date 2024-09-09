using System.Collections.Generic;
using UnityEngine;

public class OSCManager : MonoBehaviour
{
    [Header ("OSC")]
    [SerializeField] public OSC sensorOSC;
    public SensorDataFormat[] sensorData;

    private void Start()
    {
        SetOSC_Event();
        sensorData = new SensorDataFormat[System.Enum.GetValues(typeof(SensorEnum)).Length];

        //5 size �迭
        for (int i = 0; i < sensorData.Length; i++)
            sensorData[i] = new SensorDataFormat();
    }

    /// ���� �ڵ鷯 - Start, Update, Stop, Quit
    #region Front Sensor Handler
    public void getFrontStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Front)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Front)].position.Clear();
        Debug.Log("Front ���� ����");
    }

    public void getFrontSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Front)].position.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void getFrontStopMessage(OscMessage message)
    {
        Debug.Log("Front ���� ����");
    }

    public void FrontSensorQuit(OscMessage message)
    {
        Debug.Log("Front ���� ����");
    }
    #endregion

    #region Right Sensor Handler
    public void getRightStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Right)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Right)].position.Clear();
        Debug.Log("Right ���� ����");
    }

    public void getRightSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Right)].position.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void getRightStopMessage(OscMessage message)
    {
        Debug.Log("Right ���� ����");
    }

    public void RightSensorQuit(OscMessage message)
    {
        Debug.Log("Right ���� ����");
    }
    #endregion

    #region Back Sensor Handler
    public void getBackStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Back)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Back)].position.Clear();
        Debug.Log("Back ���� ����");
    }

    public void getBackSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Back)].position.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void getBackStopMessage(OscMessage message)
    {
        Debug.Log("Back ���� ����");
    }

    public void BackSensorQuit(OscMessage message)
    {
        Debug.Log("Back ���� ����");
    }
    #endregion

    #region Left Sensor Handler
    public void getLeftStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Left)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Left)].position.Clear();
        Debug.Log("Left ���� ����");
    }

    public void getLeftSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Left)].position.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void getLeftStopMessage(OscMessage message)
    {
        Debug.Log("Left ���� ����");
    }

    public void LeftSensorQuit(OscMessage message)
    {
        Debug.Log("Left ���� ����");
    }
    #endregion

    #region Down Sensor Handler
    public void getDownStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Down)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Down)].position.Clear();
        Debug.Log("Down ���� ����");
    }
    public void getDownSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Down)].position.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }
    public void getDownStopMessage(OscMessage message)
    {
        Debug.Log("Down ���� ����");
    }

    public void DownSensorQuit(OscMessage message)
    {
        Debug.Log("Down ���� ����");
    }
    #endregion

    
    
    /// �ڵ鷯 ���

    void SetOSC_Event()
    {
        // Front
        sensorOSC.SetAddressHandler("/Front/Start", getFrontStartMessage);
        sensorOSC.SetAddressHandler("/Front/Data", getFrontSensorMessage);
        sensorOSC.SetAddressHandler("/Front/End", getFrontStopMessage);
        sensorOSC.SetAddressHandler("/Front/Quit", FrontSensorQuit);

        // Right
        sensorOSC.SetAddressHandler("/Right/Start", getRightStartMessage);
        sensorOSC.SetAddressHandler("/Right/Data", getRightSensorMessage);
        sensorOSC.SetAddressHandler("/Right/End", getRightStopMessage);
        sensorOSC.SetAddressHandler("/Right/Quit", RightSensorQuit);

        // Back
        sensorOSC.SetAddressHandler("/Back/Start", getBackStartMessage);
        sensorOSC.SetAddressHandler("/Back/Data", getBackSensorMessage);
        sensorOSC.SetAddressHandler("/Back/End", getBackStopMessage);
        sensorOSC.SetAddressHandler("/Back/Quit", BackSensorQuit);


        // Left
        sensorOSC.SetAddressHandler("/Left/Start", getLeftStartMessage);
        sensorOSC.SetAddressHandler("/Left/Data", getLeftSensorMessage);
        sensorOSC.SetAddressHandler("/Left/End", getLeftStopMessage);
        sensorOSC.SetAddressHandler("/Left/Quit", LeftSensorQuit);

        // Down
        sensorOSC.SetAddressHandler("/Down/Start", getDownStartMessage);
        sensorOSC.SetAddressHandler("/Down/Data", getDownSensorMessage);
        sensorOSC.SetAddressHandler("/Down/End", getDownStopMessage);
        sensorOSC.SetAddressHandler("/Down/Quit", DownSensorQuit);
    }
}