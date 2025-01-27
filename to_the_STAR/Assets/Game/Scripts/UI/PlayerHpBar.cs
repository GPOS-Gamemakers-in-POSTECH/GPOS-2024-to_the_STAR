using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    GameObject Player;
    Image HpBar;

    float playerHp = 0.0f;

    void Start()
    {
        Player = GameObject.Find("Player");
        HpBar = GetComponent<Image>();
    }
    void Update()
    {
        playerHp = Player.GetComponent<PlayerData>().playerHp();
        HpBar.fillAmount = playerHp;
    }
}
