using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Sprite[] sprites; // index 0 : ��Ȱ��ȭ, 1 : Ȱ��ȭ
    public GameObject doorAdminister;
    SpriteRenderer spriteRenderer;
    DoorAdminister _doorAdminister;
    PlayerMovementController player;
    public int door; // ����Ǿ� �ִ� ���� ��ȣ
    public int key; // �� ��° ��������
    bool state;

    void Start()
    {
        state = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorAdminister = GameObject.Find("DoorAdminister");
    }

    private void OnTriggerEnter2D(Collider2D collision) // �÷��̾�� �浹�� ������ �÷��̾�� ����
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

    public void Interaction() // �� ������ �۵� ������ ��
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
