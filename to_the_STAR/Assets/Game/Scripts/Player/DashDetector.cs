using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDetector : MonoBehaviour
{
    bool dashEnable = true;

    Vector2 move;

    public void setMove(Vector2 v) { move = v; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Map" || other.gameObject.tag == "Door")
        {
            dashEnable = false;
        }
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(move.x, move.y, 0);
    }
    public bool isEnable()
    {
        return dashEnable;
    }
}
