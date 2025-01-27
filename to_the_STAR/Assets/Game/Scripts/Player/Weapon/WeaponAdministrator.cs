using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdministrator : MonoBehaviour
{

    GameObject weaponUIManager;

    enum Weapon
    {
        hammer,
        flameThrower
    }

    Weapon_Hammer hammer;
    Weapon_Flamethrower flamethrower;

    Weapon weapon = Weapon.flameThrower;

    void Start()
    {
        hammer = gameObject.GetComponent<Weapon_Hammer>();
        flamethrower = gameObject.GetComponent<Weapon_Flamethrower>();

        weaponUIManager = GameObject.Find("weapon_manager");
    }
    void Update()
    {
        if (Check())
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(weapon == Weapon.hammer)
                {
                    weapon = Weapon.flameThrower;
                    hammer.disable();
                    flamethrower.enable();
                }
                else if(weapon == Weapon.flameThrower)
                {
                    weapon = Weapon.hammer;
                    hammer.enable();
                    flamethrower.disable();
                }
                weaponUIManager.GetComponent<WeaponUIManager>().weaponChanged();
            }
        }
    }

    bool Check()
    {
        if (weapon == Weapon.hammer) return !hammer.isCharging();
        else if (weapon == Weapon.flameThrower) return !flamethrower.isTurnOn();
        else return true;
    }
}
