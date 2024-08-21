using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    bool collide = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collide = true;
    }

    public bool isCollide()
    {
        return collide;
    }

}
