using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 센서 데이터 포멧
/// (1)SensorGround, (2)Position List
[SerializeField]
public class SensorDataFormat
{
    public Vector2 rectSize;
    public List<Vector3> positionList = new List<Vector3>();
}


/// 센서 Enum
public enum SensorEnum
{
    Front,
    Right,
    Left,
    Back,
    Down
}
