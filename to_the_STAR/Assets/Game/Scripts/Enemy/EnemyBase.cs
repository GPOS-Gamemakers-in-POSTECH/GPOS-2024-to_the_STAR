using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyInterface
{
    public void getDamage(float damage);
    public float attack();
    public float hpRatio();

    public int getFloor();

}