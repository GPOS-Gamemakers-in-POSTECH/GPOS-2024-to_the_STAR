using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdministrator : MonoBehaviour
{

    GameObject weaponUIManager;

    public bool isHammerUnlocked { get; private set; } = false;
    public bool isFlamethrowerUnlocked { get; private set; } = false;

    private bool debugCheat = false;

    enum Weapon
    {
        hammer,
        flameThrower,
        none
    }

    Weapon_Hammer hammer;
    Weapon_Flamethrower flamethrower;

    Weapon weapon = Weapon.hammer;

    void Start()
    {
        hammer = gameObject.GetComponent<Weapon_Hammer>();
        flamethrower = gameObject.GetComponent<Weapon_Flamethrower>();

        weaponUIManager = GameObject.Find("weapon_manager");
        disableWeapon();
    }

    public void disableWeapon()
    {
        weapon = Weapon.none;
        hammer.disable();
        flamethrower.disable();
        weaponUIManager.GetComponent<WeaponUIManager>().WeaponSelected(2);
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
        if (Check() && isFlamethrowerUnlocked)
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
            if (weapon == Weapon.hammer && isFlamethrowerUnlocked)
            {
                weapon = Weapon.flameThrower;
                hammer.disable();
                flamethrower.enable();
                weaponUIManager.GetComponent<WeaponUIManager>().weaponChanged();
            }
            else if (weapon == Weapon.flameThrower)
            {
                weapon = Weapon.hammer;
                hammer.enable();
                flamethrower.disable();
                weaponUIManager.GetComponent<WeaponUIManager>().weaponChanged();
            }
            
        }
    }

    public bool Check()
    {
        if (weapon == Weapon.none) return isHammerUnlocked;
        else if (weapon == Weapon.hammer) return !hammer.isCharging();
        else if (weapon == Weapon.flameThrower) return !flamethrower.isTurnOn();
        else return true;
    }

    public void UnlockHammer()
    {
        isHammerUnlocked = true;
        SelectHammer();
    }

    public void UnlockFlamethrower()
    {
        isFlamethrowerUnlocked = true;
        SelectFlamethrower();
    }

    private void Update()
    {
        if (debugCheat)
        {
            bool flameCheat = UnityEngine.Input.GetKey("i");
            bool hammerCheat = UnityEngine.Input.GetKey("o");

            if (flameCheat) isFlamethrowerUnlocked = true;
            if (hammerCheat) isHammerUnlocked = true;
        }
    }
}
