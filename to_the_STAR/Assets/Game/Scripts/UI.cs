using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Image Weapon_Base_Prefab;
    [SerializeField] Image Weapon_Hammer_Prefab;
    [SerializeField] Image Weapon_Flamethrower_Prefab;

    GameObject View;

    Image Weapon_Hammer_Base;
    Image Weapon_Hammer;
    Image Weapon_Flamethrower_Base;
    Image Weapon_Flamethrower;

    float WeaponUI_y = 0.25f;
    float WeaponUI_x_gap = 1.25f;
    Vector2 WeaponUI_size = new Vector2(1f, 1f);

    void Start()
    {
        View = GameObject.Find("Main Camera");

        Weapon_Hammer_Base = new GameObject("UI_Hammer_Base").AddComponent<Image>();
        Weapon_Hammer_Base.transform.SetParent(transform);
        Weapon_Hammer_Base.sprite = Weapon_Base_Prefab.sprite;
        Weapon_Hammer_Base.material = Weapon_Base_Prefab.material;
        Weapon_Hammer_Base.GetComponent<RectTransform>().anchoredPosition = new Vector2(WeaponUI_x_gap, WeaponUI_y);
        Weapon_Hammer_Base.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;

        Weapon_Hammer = new GameObject("UI_Hammer").AddComponent<Image>();
        Weapon_Hammer.transform.SetParent(transform);
        Weapon_Hammer.sprite = Weapon_Hammer_Prefab.sprite;
        Weapon_Hammer.material = Weapon_Hammer_Prefab.material;
        Weapon_Hammer.GetComponent<RectTransform>().anchoredPosition = new Vector2(WeaponUI_x_gap, WeaponUI_y);
        Weapon_Hammer.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;

        Weapon_Flamethrower_Base = new GameObject("UI_Flamethrower_Base").AddComponent<Image>();
        Weapon_Flamethrower_Base.transform.SetParent(transform);
        Weapon_Flamethrower_Base.sprite = Weapon_Base_Prefab.sprite;
        Weapon_Flamethrower_Base.material = Weapon_Base_Prefab.material;
        Weapon_Flamethrower_Base.GetComponent<RectTransform>().anchoredPosition = new Vector2(WeaponUI_x_gap * 2, WeaponUI_y);
        Weapon_Flamethrower_Base.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;

        Weapon_Flamethrower = new GameObject("UI_Flamethrower").AddComponent<Image>();
        Weapon_Flamethrower.transform.SetParent(transform);
        Weapon_Flamethrower.sprite = Weapon_Flamethrower_Prefab.sprite;
        Weapon_Flamethrower.material = Weapon_Flamethrower_Prefab.material;
        Weapon_Flamethrower.GetComponent<RectTransform>().anchoredPosition = new Vector2(WeaponUI_x_gap * 2, WeaponUI_y);
        Weapon_Flamethrower.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;
    }


    void Update()
    {
        transform.position = View.transform.position;
    }
}