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

    public void SelectHammer()
    {
        if (Check())
        {
            weapon = Weapon.hammer;
            hammer.enable();
            flamethrower.disable();
            weaponUIManager.GetComponent<WeaponUIManager>().WeaponSelected(0);
        }
    }

    public void SelectFlamethrower()
    {
        if (Check())
        {
            weapon = Weapon.flameThrower;
            hammer.disable();
            flamethrower.enable();
            weaponUIManager.GetComponent<WeaponUIManager>().WeaponSelected(1);
        }
    }

    public void WeaponChange()
    {
        if (Check())
        {
            if (weapon == Weapon.hammer)
            {
                weapon = Weapon.flameThrower;
                hammer.disable();
                flamethrower.enable();
            }
            else if (weapon == Weapon.flameThrower)
            {
                weapon = Weapon.hammer;
                hammer.enable();
                flamethrower.disable();
            }
            weaponUIManager.GetComponent<WeaponUIManager>().weaponChanged();
        }
    }

    public bool Check()
    {
        if (weapon == Weapon.hammer) return !hammer.isCharging();
        else if (weapon == Weapon.flameThrower) return !flamethrower.isTurnOn();
        else return true;
    }
}
