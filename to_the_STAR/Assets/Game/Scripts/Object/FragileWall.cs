using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileWall : MonoBehaviour
{
    bool p = false;
    SpriteRenderer _sr;
    public void BreakDown()
    {
        p = true;
    }

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (p)
        {
            if (_sr.color.a == 0) gameObject.SetActive(false);
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, (_sr.color.a - 0.005f) > 0 ? (_sr.color.a - 0.005f) : 0);
        }
    }
}
