using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Image Weapon_Base_Prefab;
    [SerializeField] Image Weapon_Hammer_Prefab;
    [SerializeField] Image Weapon_Flamethrower_Prefab;
    [SerializeField] Image Weapon_Select_Prefab;

    [SerializeField] Image Healthbar_Prefab;
    [SerializeField] Image Weapon_Charge_Prefab;
    [SerializeField] Image Weapon_Cooldown_Prefab;
    [SerializeField] Image Weapon_Fever_Prefab;

    [SerializeField] Image Dash_Prefab;

    GameObject View;
    GameObject Player;

    Image Weapon_Hammer_Base;
    Image Weapon_Hammer;
    Image Weapon_Flamethrower_Base;
    Image Weapon_Flamethrower;
    Image Weapon_Select;

    Image Healthbar;
    Image Weapon_Hammer_Charge;
    Image Weapon_Hammer_Cooldown;
    Image Weapon_Flamethrower_Cooldown;
    Image Weapon_Flamethrower_Fever;

    Image[] Dash = new Image[3];
    string[] dashName = new string[3];

    const float WeaponUI_y = -108f;
    const float WeaponUI_x = 96f;
    const float WeaponUI_x_gap = 108f;
    const float WeaponValueAlpha = 0.75f;
    Vector2 WeaponUI_size = new Vector2(1.5f, 1.5f);
    Vector2 Healthbar_size = new Vector2(3.0f, 1.0f);
    Vector2 ScreenSize = new Vector2(-Screen.width / 2, Screen.height / 2);

    float playerHp = 0.0f;
    float hammerCharge = 0.0f;
    float hammerCooldown = 0.0f;
    float flamethrowerCooldown = 0.0f;
    float flamethrowerFever = 0.0f;
    int whatWeapon = 0;

    float dashPoint = 3.0f;

    void Start()
    {
        dashName[0] = "1"; dashName[1] = "2"; dashName[2] = "3";

        View = GameObject.Find("Main Camera");
        Player = GameObject.Find("Player");

        Weapon_Select = new GameObject("UI_WeaponSelection").AddComponent<Image>();
        Weapon_Select.transform.SetParent(transform);
        Weapon_Select.sprite = Weapon_Select_Prefab.sprite;
        Weapon_Select.material = Weapon_Select_Prefab.material;
        Weapon_Select.GetComponent<RectTransform>().sizeDelta = WeaponUI_size * 1.2f;

        Weapon_Hammer_Base = new GameObject("UI_Hammer_Base").AddComponent<Image>();
        Weapon_Hammer_Base.transform.SetParent(transform);
        Weapon_Hammer_Base.sprite = Weapon_Base_Prefab.sprite;
        Weapon_Hammer_Base.material = Weapon_Base_Prefab.material;
        Weapon_Hammer_Base.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x, WeaponUI_y);
        Weapon_Hammer_Base.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;

        Weapon_Hammer = new GameObject("UI_Hammer").AddComponent<Image>();
        Weapon_Hammer.transform.SetParent(transform);
        Weapon_Hammer.sprite = Weapon_Hammer_Prefab.sprite;
        Weapon_Hammer.material = Weapon_Hammer_Prefab.material;
        Weapon_Hammer.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x, WeaponUI_y);
        Weapon_Hammer.GetComponent<RectTransform>().sizeDelta = WeaponUI_size * 0.7f;

        Weapon_Flamethrower_Base = new GameObject("UI_Flamethrower_Base").AddComponent<Image>();
        Weapon_Flamethrower_Base.transform.SetParent(transform);
        Weapon_Flamethrower_Base.sprite = Weapon_Base_Prefab.sprite;
        Weapon_Flamethrower_Base.material = Weapon_Base_Prefab.material;
        Weapon_Flamethrower_Base.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x * 2 + WeaponUI_x_gap, WeaponUI_y);
        Weapon_Flamethrower_Base.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;

        Weapon_Flamethrower = new GameObject("UI_Flamethrower").AddComponent<Image>();
        Weapon_Flamethrower.transform.SetParent(transform);
        Weapon_Flamethrower.sprite = Weapon_Flamethrower_Prefab.sprite;
        Weapon_Flamethrower.material = Weapon_Flamethrower_Prefab.material;
        Weapon_Flamethrower.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x * 2 + WeaponUI_x_gap, WeaponUI_y);
        Weapon_Flamethrower.GetComponent<RectTransform>().sizeDelta = WeaponUI_size * 0.7f;

        Healthbar = new GameObject("UI_Healthbar").AddComponent<Image>();
        Healthbar.transform.SetParent(transform);
        Healthbar.sprite = Healthbar_Prefab.sprite;
        Healthbar.material = Healthbar_Prefab.material;
        Healthbar.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x * 1.7f, WeaponUI_y * 2.2f);
        Healthbar.GetComponent<RectTransform>().sizeDelta = Healthbar_size;
        Healthbar.type = Image.Type.Filled;
        Healthbar.fillMethod = Image.FillMethod.Horizontal;
        Healthbar.fillOrigin = (int)Image.OriginHorizontal.Left;

        Weapon_Hammer_Charge = new GameObject("UI_Hammer_Charge").AddComponent<Image>();
        Weapon_Hammer_Charge.transform.SetParent(transform);
        Weapon_Hammer_Charge.sprite = Weapon_Charge_Prefab.sprite;
        Weapon_Hammer_Charge.material = Weapon_Charge_Prefab.material;
        Weapon_Hammer_Charge.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x, WeaponUI_y);
        Weapon_Hammer_Charge.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;
        Weapon_Hammer_Charge.type = Image.Type.Filled;
        Color hcc = Weapon_Hammer_Charge.GetComponent<Image>().color;
        Weapon_Hammer_Charge.GetComponent<Image>().color = new Color(hcc.r, hcc.g, hcc.b, WeaponValueAlpha);
        Weapon_Hammer_Charge.fillMethod = Image.FillMethod.Vertical;
        Weapon_Hammer_Charge.fillOrigin = (int)Image.OriginVertical.Bottom;

        Weapon_Hammer_Cooldown = new GameObject("UI_Hammer_Cooldown").AddComponent<Image>();
        Weapon_Hammer_Cooldown.transform.SetParent(transform);
        Weapon_Hammer_Cooldown.sprite = Weapon_Cooldown_Prefab.sprite;
        Weapon_Hammer_Cooldown.material = Weapon_Cooldown_Prefab.material;
        Weapon_Hammer_Cooldown.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x, WeaponUI_y);
        Weapon_Hammer_Cooldown.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;
        Weapon_Hammer_Cooldown.type = Image.Type.Filled;
        Color fcd = Weapon_Hammer_Cooldown.GetComponent<Image>().color;
        Weapon_Hammer_Cooldown.GetComponent<Image>().color = new Color(fcd.r, fcd.g, fcd.b, WeaponValueAlpha);

        Weapon_Flamethrower_Cooldown = new GameObject("UI_Flamethrower_Cooldown").AddComponent<Image>();
        Weapon_Flamethrower_Cooldown.transform.SetParent(transform);
        Weapon_Flamethrower_Cooldown.sprite = Weapon_Cooldown_Prefab.sprite;
        Weapon_Flamethrower_Cooldown.material = Weapon_Cooldown_Prefab.material;
        Weapon_Flamethrower_Cooldown.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x * 2 + WeaponUI_x_gap, WeaponUI_y);
        Weapon_Flamethrower_Cooldown.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;
        Weapon_Flamethrower_Cooldown.type = Image.Type.Filled;
        Color fcc = Weapon_Flamethrower_Cooldown.GetComponent<Image>().color;
        Weapon_Flamethrower_Cooldown.GetComponent<Image>().color = new Color(fcc.r, fcc.g, fcc.b, WeaponValueAlpha * 0.5f);

        Weapon_Flamethrower_Cooldown.enabled = false;

        Weapon_Flamethrower_Fever = new GameObject("UI_Flamethrower_Fever").AddComponent<Image>();
        Weapon_Flamethrower_Fever.transform.SetParent(transform);
        Weapon_Flamethrower_Fever.sprite = Weapon_Fever_Prefab.sprite;
        Weapon_Flamethrower_Fever.material = Weapon_Fever_Prefab.material;
        Weapon_Flamethrower_Fever.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x * 2 + WeaponUI_x_gap, WeaponUI_y);
        Weapon_Flamethrower_Fever.GetComponent<RectTransform>().sizeDelta = WeaponUI_size;
        Weapon_Flamethrower_Fever.type = Image.Type.Filled;
        Color ffc = Weapon_Flamethrower_Fever.GetComponent<Image>().color;
        Weapon_Flamethrower_Fever.GetComponent<Image>().color = new Color(ffc.r, ffc.g, ffc.b, WeaponValueAlpha);

        for(int i = 0; i < 3; i++)
        {
            Dash[i] = new GameObject("UI_Dash" + dashName[i]).AddComponent<Image>();
            Dash[i].transform.SetParent(transform);
            Dash[i].sprite = Dash_Prefab.sprite;
            Dash[i].material = Dash_Prefab.material;
            Dash[i].GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x + WeaponUI_x_gap * 0.66f * i, WeaponUI_y * 3.5f + WeaponUI_x_gap);
            Dash[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0.5f, 0.25f);
            Dash[i].type = Image.Type.Filled;
        }
    }

    float max(float a, float b) { return a > b ? a : b; }


    void Update()
    {
        transform.position = View.transform.position;

        whatWeapon = Player.GetComponent<PlayerData>().weaponSelection();
        Weapon_Select.GetComponent<RectTransform>().anchoredPosition = ScreenSize + new Vector2(WeaponUI_x * (whatWeapon + 1) + WeaponUI_x_gap * whatWeapon, WeaponUI_y);

        playerHp = Player.GetComponent<PlayerData>().playerHp();
        Healthbar.fillAmount = playerHp;

        if(whatWeapon == 0)
        {
            Weapon_Hammer_Charge.enabled = true;
            Weapon_Hammer_Cooldown.enabled = true;
            Weapon_Flamethrower_Fever.enabled = false;
        }

        hammerCharge = max(0,Player.GetComponent<PlayerData>().hammerCharge());
        Weapon_Hammer_Charge.fillAmount = hammerCharge;

        hammerCooldown = max(0,Player.GetComponent<PlayerData>().hammerCooldown());
        Weapon_Hammer_Cooldown.fillAmount = hammerCooldown;

        if(whatWeapon == 1)
        {
            Weapon_Hammer_Charge.enabled = false;
            Weapon_Hammer_Cooldown.enabled = false;
            Weapon_Flamethrower_Fever.enabled = true;
        }

        //flamethrowerCooldown = max(0, Player.GetComponent<PlayerData>().flamethrowerCooldown());
        //Weapon_Flamethrower_Cooldown.fillAmount = flamethrowerCooldown;

        flamethrowerFever = max(0, Player.GetComponent<PlayerData>().flamethrowerFever());
        Weapon_Flamethrower_Fever.fillAmount = flamethrowerFever;

        dashPoint = Player.GetComponent<PlayerMovementController>().getDash();
        for(int i = 0; i < 3; i++)
        {
            Dash[i].fillAmount = dashPoint - i;
        }
    }
}
