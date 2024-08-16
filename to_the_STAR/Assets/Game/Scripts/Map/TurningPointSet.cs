using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPointSet : MonoBehaviour
{
    [SerializeField] GameObject Checker;
    int type = 0;
    GameObject[] checks = new GameObject[8];
    void Start()
    {
        for(int i = 0; i < 8; i++)
        {
            checks[i] = Instantiate(Checker, transform.position + new Vector3(Mathf.Cos(45*i*Mathf.Deg2Rad), Mathf.Sin(45*i*Mathf.Deg2Rad), 0), Quaternion.identity);
        }
    }


    /* getType에서 반환하는 값
     * 바깥쪽 코너      안쪽코너
     * -------------   -------------
     * | 1       2 |   | 5       6 |  
     * | 3       4 |   | 7       8 | 
     * -------------   -------------
     *                 ////////// 
     *    ------       ///|------
     *    |/////       ///|
     *    |/////       ///|
     * */
    public int getType()
    {
        switch (type)
        {
            case 128: return 1;
            case 32: return 2;
            case 2: return 3;
            case 8: return 4;
            case 62: return 5;
            case 143: return 6;
            case 248: return 7;
            case 227: return 8;
            default: return 0;
        }
    }

    void Update()
    {
        if (type == 0)
        {
            for(int i = 0; i < 8; i++)
            {
                if (checks[i].GetComponent<Checker>().isCollide())
                {
                    type += (1 << i);
                }
                Destroy(checks[i]);
            }
            if (type == 0) type = -1;
        }
    }
}
