using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] GameObject hammer_selected;
    [SerializeField] GameObject flamethrower_selected;
    [SerializeField] GameObject cooldown;

    int weapon = 2; // 0 : hammer, 1 : flamethrower, n + 1: none
    const int n = 2; // number of weapon types

    float hammerCooldown = 0.0f;
    float flamethrowerFever = 0.0f;

    GameObject[] weapons = new GameObject[n + 1];

    GameObject Player;

    void disableWeaponUI()
    {
        for (int i = 0; i < n; i++) weapons[i].SetActive(false);
    }

    void setWeapon()
    {
        for (int i = 0; i < n; i++) weapons[i].SetActive(false);
        weapons[weapon]?.SetActive(true);
    }

    public void weaponChanged()
    {
        WeaponSelected((weapon + 1) % n);
    }

    public void WeaponSelected(int n)
    {
        if (IsSelectedWeaponEnabled(n))
        {
            weapon = n;
            setWeapon();
        }
    }

    private bool IsSelectedWeaponEnabled(int n)
    {
        switch (n)
        {
            case 0: return Player.GetComponent<WeaponAdministrator>().isHammerUnlocked;
            case 1: return Player.GetComponent<WeaponAdministrator>().isFlamethrowerUnlocked;
            default: return true;
        }
    }

    void Start()
    {
        Player = GameObject.Find("Player");

        weapons[0] = Instantiate(hammer_selected, transform);
        weapons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(163, -96.666f, 0);
        weapons[1] = Instantiate(flamethrower_selected, transform);
        weapons[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(66.666f, -46.666f, 0);

        cooldown = Instantiate(cooldown, transform);
        cooldown.GetComponent<RectTransform>().anchoredPosition = new Vector3(112.5f, -42.5f, 0);

        disableWeaponUI();
    }

    float max(float a, float b) { return a > b ? a : b; }

    void Update()
    {
        switch (weapon)
        {
            case 0:
                hammerCooldown = max(0, Player.GetComponent<PlayerData>().hammerCooldown());
                cooldown.GetComponent<Image>().fillAmount = hammerCooldown;
                break;
            case 1:
                flamethrowerFever = max(0, Player.GetComponent<PlayerData>().flamethrowerFever());
                cooldown.GetComponent<Image>().fillAmount = flamethrowerFever;
                break;
            case 2:
                break;
        }
    }
}
