using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int keys; // Ȱ��ȭ�ϴµ� �� ���� ���谡 �ʿ����� ��Ÿ��

    public void DoorChanged(bool flag) // ���� ���°� ����� ��� ȣ��Ǵ� �Լ�, flag : true => Ȱ��ȭ
    {
        gameObject.SetActive(!flag);
    }
}
