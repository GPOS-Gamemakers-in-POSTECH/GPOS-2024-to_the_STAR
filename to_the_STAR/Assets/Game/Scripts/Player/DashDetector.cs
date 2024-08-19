using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDetector : MonoBehaviour
{
    bool dashEnable = true;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Map")
        {
            dashEnable = false;
        }
    }

    public bool isEnable()
    {
        return dashEnable;
    }
}
