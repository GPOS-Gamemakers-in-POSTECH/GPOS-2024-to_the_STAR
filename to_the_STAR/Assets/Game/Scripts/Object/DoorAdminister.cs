using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorAdminister : MonoBehaviour
{
    public GameObject[] doors;
    public bool[][] keys;
    public GameObject[] doorToTurningPoints;
    private void Start()
    {
        keys = new bool[doors.Length][];
        for(int i = 0; i < doors.Length; i++)
        {
            int l = doors[i].GetComponent<Door>().keys;
            keys[i] = new bool[l];
            for(int j = 0; j < l; j++)
            {
                keys[i][j] = false;
            }
        }
    }

    public void StateChanged(int i) // i번째 문과 연결된 레버 상태 변경됨 => 문의 상태가 바뀌는지 확인
    {
        Door _door = doors[i].GetComponent<Door>();
        bool flag = true;
        for(int j = 0; j < _door.keys; j++)
        {
            flag &= keys[i][j];
        }
        _door.DoorChanged(flag);
        if (doorToTurningPoints[i] != null)
        {
            doorToTurningPoints[i].GetComponent<TurningPointSet>().turningPointChanged(flag);
        }
    }
}