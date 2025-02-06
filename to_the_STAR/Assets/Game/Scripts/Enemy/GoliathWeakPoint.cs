using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathWeakPoint : MonoBehaviour, EnemyInterface
{
    bool p = false;
    Goliath goliath;
    SpriteRenderer _sr;

    public int getFloor()
    {
        return goliath.getFloor();
    }

    public void getDamage(float damage, float stunCooldownSet)
    {
        goliath.getDamage(damage * 20, stunCooldownSet);
    }

    public float hpRatio()
    {
        return goliath.hpRatio();
    }

    public void Dead()
    {
        p = true;
    }

    void Start()
    {
        goliath = transform.parent.GetComponent<Goliath>();
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
