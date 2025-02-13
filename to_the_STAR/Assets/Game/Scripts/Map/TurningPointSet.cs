using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurningPointSet : MonoBehaviour
{
    [SerializeField] GameObject Checker;
    int type = 0;
    GameObject[] checks = new GameObject[8];
    GameObject doorCheck;
    void Start()
    {
        doorCheck = GameObject.Find("DoorAdminister");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < 8; i++)
        {
            checks[i] = Instantiate(Checker, transform.position + new Vector3(Mathf.Cos(45 * i * Mathf.Deg2Rad), Mathf.Sin(45 * i * Mathf.Deg2Rad), 0), Quaternion.identity);
        }
        type = 0;
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

    public void turningPointChanged(bool flag)
    {
        if(type == 128 || type == 32 || type == 2 || type == 8)
        {
            flag = !flag;
        }
        gameObject.SetActive(!flag);
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
            for(int i = 0; i < doorCheck.GetComponent<TurningPointDoorTable>().DoorTable.Length; i++)
            {
                if ((doorCheck.GetComponent<TurningPointDoorTable>().LocTable[i] - new Vector2(transform.position.x, transform.position.y)).magnitude < 0.5)
                {
                    int loc = doorCheck.GetComponent<TurningPointDoorTable>().DoorTable[i];
                    int pos = doorCheck.GetComponent<DoorAdminister>().doorToTurningPoints[loc][0] == null ? 0 : 1;
                    doorCheck.GetComponent<DoorAdminister>().doorToTurningPoints[loc][pos] = gameObject;
                    type = doorCheck.GetComponent<TurningPointDoorTable>().ChangedType[i];
                    break;
                }
            }
        }
    }
}
