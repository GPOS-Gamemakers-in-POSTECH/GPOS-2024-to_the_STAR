using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] GameObject healthbarObj;
    GameObject healthbar;
    SpriteRenderer barSprite;
    LineRenderer barLine;
    float hp;
    float printHp = 1;
    float healthbarCooldown = 0;
    const float healthbarCooldownMax = 1;
    Vector3 barPrintPos = new Vector3(0, -0.5f);
    Vector3 barShowPos = new Vector3(-0.5f, 0, -1);
    Vector3 barMaxPos = new Vector3(1, 0, 0);

    bool flag = false;

    void Start()
    {
        float angle = (180 - GetComponent<EnemyInterface>().getFloor() * 90) * Mathf.Deg2Rad;
        barPrintPos = rotateVector(barPrintPos, angle);
        barShowPos = rotateVector(barShowPos, angle);
        barMaxPos = rotateVector(barMaxPos, angle);
        healthbar = Instantiate(healthbarObj, transform.position + barPrintPos, transform.rotation);
        barSprite = healthbar.GetComponent<SpriteRenderer>();
        barSprite.enabled = false;
        barLine = healthbar.GetComponent<LineRenderer>();
        barLine.enabled = false;
    }

    Vector3 rotateVector(Vector3 v, float rot)
    {
        return new Vector3(v.x * Mathf.Cos(rot) - v.y * Mathf.Sin(rot), v.x * Mathf.Sin(rot) + v.y * Mathf.Cos(rot), 0);
    }
    void setBarColor(Color c)
    {
        barLine.startColor = c;
        barLine.endColor = c;
    }
    void Update()
    {
        hp = GetComponent<EnemyInterface>().hpRatio();
        if (hp <= 0 && flag) Destroy(healthbar);
        else
        {
            healthbar.transform.position = transform.position + barPrintPos;
            barLine.SetPosition(0, healthbar.transform.position + barShowPos);
            barLine.SetPosition(1, healthbar.transform.position + barShowPos + barMaxPos * printHp);
            if (Mathf.Abs(printHp - hp) > 0.01)
            {
                barSprite.enabled = true;
                barLine.enabled = true;
                if (printHp > 0.33) setBarColor(Color.green);
                else if (printHp > 0.1) setBarColor(Color.yellow);
                else setBarColor(Color.red);
                healthbarCooldown = healthbarCooldownMax;
            }
            else if (healthbarCooldown < 0)
            {
                barSprite.enabled = false;
                barLine.enabled = false;
                if (hp <= 0) flag = true;
            }
            healthbarCooldown -= Time.deltaTime;
            printHp -= (printHp - hp) / 2;
        }
    }
}
