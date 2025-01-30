using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    GameObject Player;

    [SerializeField] GameObject Dash_prefab;

    float dashPoint = 3.0f;

    GameObject[] Dash = new GameObject[3];

    void Start()
    {
        Player = GameObject.Find("Player");

        for (int i = 0; i < 3; i++)
        {
            Dash[i] = Instantiate(Dash_prefab, transform);
            Dash[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-166.666f + 83.333f * (float)i, -30, 0);
        }
    }

    void Update()
    {
        dashPoint = Player.GetComponent<PlayerMovementController>().GetStamina();
        
        for (int i = 0; i < 3; i++)
        {
            Dash[i].GetComponent<Image>().fillAmount = dashPoint - i;
        }
    }
}
