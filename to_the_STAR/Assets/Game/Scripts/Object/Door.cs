using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool inverse; // ������ ���� Ŵ�� �ݴ�� �����Ұ��ΰ�
    public int keys; // Ȱ��ȭ�ϴµ� �� ���� ���谡 �ʿ����� ��Ÿ��

    public void DoorChanged(bool flag) // ���� ���°� ����� ��� ȣ��Ǵ� �Լ�, flag : true => Ȱ��ȭ
    {
        if (inverse)
        {
            flag = !flag;
        }
        gameObject.SetActive(!flag);
    }
}
