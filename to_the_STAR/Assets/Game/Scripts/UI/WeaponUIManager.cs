using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] GameObject hammer_selected;
    [SerializeField] GameObject flamethrower_selected;

    int weapon = 1; // 0 : hammer, 1 : flamethrower
    const int n = 2; // number of weapon types

    GameObject[] weapons = new GameObject[n];

    void setWeapon()
    {
        Debug.Log(weapon);
        for (int i = 0; i < n; i++) weapons[i].SetActive(false);
        weapons[weapon].SetActive(true);
    }

    public void weaponChanged()
    {
        weapon++;
        weapon %= n;
        setWeapon();
    }

    void Start()
    {
        weapons[0] = Instantiate(hammer_selected, transform);
        weapons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(167, -92.5f, 0);
        weapons[1] = Instantiate(flamethrower_selected, transform);
        weapons[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(66.666f, -46.666f, 0);

        setWeapon();
    }

    void Update()
    {
        
    }
}
