using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class GoliathLeg : MonoBehaviour, EnemyInterface
{
    bool p = false;
    private Goliath goliath;
    SpriteRenderer _sr;

    public void init(Goliath tmp)
    {
        goliath = tmp;
    }

    public int getFloor()
    {
        return goliath.getFloor();
    }

    public void getDamage(float damage, float stunCooldownSet)
    {
        goliath.getDamage(damage, stunCooldownSet);
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