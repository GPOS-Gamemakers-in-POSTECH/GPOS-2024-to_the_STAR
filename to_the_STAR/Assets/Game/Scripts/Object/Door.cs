using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool inverse; // 레버의 껐다 킴과 반대로 동작할것인가
    public int keys; // 활성화하는데 몇 개의 열쇠가 필요한지 나타냄

    public void DoorChanged(bool flag) // 문의 상태가 변경된 경우 호출되는 함수, flag : true => 활성화
    {
        if (inverse)
        {
            flag = !flag;
        }
        gameObject.SetActive(!flag);
    }
}
