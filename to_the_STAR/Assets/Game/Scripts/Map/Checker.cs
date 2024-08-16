using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    bool collide = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        collide = true;
    }

    public bool isCollide()
    {
        return collide;
    }

}
