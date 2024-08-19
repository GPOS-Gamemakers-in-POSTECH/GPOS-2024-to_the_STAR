using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathWeakPoint : MonoBehaviour, EnemyInterface
{
    Goliath goliath;
    private void Start()
    {
        goliath = transform.parent.GetComponent<Goliath>();
    }

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
}
