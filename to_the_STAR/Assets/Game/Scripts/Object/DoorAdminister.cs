using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorAdminister : MonoBehaviour
{
    public GameObject[] doors;
    public bool[][] keys;
    public GameObject[][] doorToTurningPoints;
    [SerializeField] AudioClip doorOpenSound;
    [SerializeField] int doorOpenedCount = 1; // ó���� �����ִ� ���� ���� (1�� Ʃ�丮�� �� ����)
    private void Start()
    {
        keys = new bool[doors.Length][];
        int[] count = new int[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            int l = doors[i].GetComponent<Door>().keys;
            count[i] = 0;
            keys[i] = new bool[l];
            for(int j = 0; j < l; j++)
            {
                keys[i][j] = false;
            }
        }
        doorToTurningPoints = new GameObject[doors.Length][];
        int[] table = GetComponent<TurningPointDoorTable>().DoorTable;
        for (int i = 0; i < table.Length; i++)
        {
            count[table[i]]++;
        }
        for(int i = 0; i < doors.Length; i++)
        {
            doorToTurningPoints[i] = new GameObject[count[i]];

        }
    }

    public void StateChanged(int i) // i��° ���� ����� ���� ���� ����� => ���� ���°� �ٲ���� Ȯ��
    {
        Door _door = doors[i].GetComponent<Door>();
        bool before = doors[i].activeSelf;
        bool flag = !_door.isOr;
        for(int j = 0; j < _door.keys; j++)
        {
            if (_door.isOr)
            {
                flag |= keys[i][j];
            }
            else
            {
                flag &= keys[i][j];
            }
        }
        _door.DoorChanged(flag);
        bool after = doors[i].activeSelf;
        if (before != after)
        {
            if(doorOpenedCount > 0) doorOpenedCount--;
            else GetComponent<AudioSource>().PlayOneShot(doorOpenSound, 1.0f);
        }
        if (doorToTurningPoints[i] != null)
        {
            for (int j = 0; j < doorToTurningPoints[i].Length; j++)
            {
                doorToTurningPoints[i][j].GetComponent<TurningPointSet>().turningPointChanged(flag);
            }
        }
    }
}