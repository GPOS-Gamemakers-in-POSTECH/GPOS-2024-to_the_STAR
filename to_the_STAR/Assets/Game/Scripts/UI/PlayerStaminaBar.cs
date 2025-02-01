using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaBar : MonoBehaviour
{
    GameObject Player;
    Image StaminaBar;

    float playerHp = 0.0f;

    void Start()
    {
        Player = GameObject.Find("Player");
        StaminaBar = GetComponent<Image>();
    }
    void Update()
    {
        playerHp = Player.GetComponent<PlayerData>().playerStamina();
        StaminaBar.fillAmount = playerHp;
    }
}
