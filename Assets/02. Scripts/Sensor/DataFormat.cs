using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ���� ������ ����
/// (1)SensorGround, (2)Position List
[SerializeField]
public class SensorDataFormat
{
    public Vector2 rectSize;
    public List<Vector3> position = new List<Vector3>();
}


/// ���� Enum
public enum SensorEnum
{
    Front,
    Right,
    Back,
    Left,
    Down
}