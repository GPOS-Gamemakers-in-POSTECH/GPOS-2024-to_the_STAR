using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathLeg : MonoBehaviour, EnemyInterface
{
    private Goliath goliath;

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
}