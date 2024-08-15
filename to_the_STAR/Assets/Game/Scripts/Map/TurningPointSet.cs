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

    public int getType()
    {
        return type;
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
