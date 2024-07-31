using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Sprite[] sprites; // index 0 : 비활성화, 1 : 활성화
    public GameObject doorAdminister;
    SpriteRenderer spriteRenderer;
    DoorAdminister _doorAdminister;
    PlayerMovementController player;
    public int door; // 연결되어 있는 문의 번호
    public int key; // 몇 번째 레버인지
    bool state;

    void Start()
    {
        state = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorAdminister = GameObject.Find("DoorAdminister");
    }

    private void OnTriggerEnter2D(Collider2D collision) // 플레이어와 충돌한 레버를 플레이어와 연결
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovementController>();

            if(player != null)
            {
                player.lever = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.lever = null;
            player = null;
        }
    }

    public void Interaction() // 이 레버를 작동 시켰을 때
    {
        state = !state;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        int index = state ? 1 : 0;
        spriteRenderer.sprite = sprites[index];

        _doorAdminister = doorAdminister.GetComponent<DoorAdminister>();
        _doorAdminister.keys[door][key] = state;
        _doorAdminister.StateChanged(door);
    }
}
