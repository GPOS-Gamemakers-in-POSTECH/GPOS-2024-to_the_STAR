using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDetector : MonoBehaviour
{
    bool dashEnable = true;

    Vector2 move;

    public void setMove(Vector2 v) { move = 2 * v; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Map" || other.gameObject.tag == "Door" || other.gameObject.tag == "TurningPoint")
        {
            dashEnable = false;
            transform.position = transform.position - new Vector3(move.x, move.y, 0) * 0.5f;
        }
    }

    private void FixedUpdate()
    {
        if (dashEnable)
        {
            transform.position = transform.position + new Vector3(move.x, move.y, 0);
        }
    }

    public bool isEnable()
    {
        return dashEnable;
    }
}
