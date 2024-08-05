using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Sprite[] sprites; // index 0 : ��Ȱ��ȭ, 1 : Ȱ��ȭ
    public GameObject doorAdminister;
    SpriteRenderer _sr;
    DoorAdminister _doorAdminister;
    PlayerMovementController player;
    PlayerData playerData;
    public int door; // ����Ǿ� �ִ� ���� ��ȣ
    public int key; // �� ��° ��������
    bool state;

    void Start()
    {
        state = false;
        _sr = GetComponent<SpriteRenderer>();
        doorAdminister = GameObject.Find("DoorAdminister");
    }

    private void OnTriggerEnter2D(Collider2D collision) // �÷��̾�� �浹�� ������ �÷��̾�� ����
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovementController>();
            playerData = collision.GetComponent<PlayerData>();

            if(player != null && compareRotation(playerData.RotateDir))
            {
                player._lever = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player._lever = null;
            player = null;
        }
    }

    public void Interaction() // �� ������ �۵� ������ ��
    {
        state = !state;
        _sr.flipX = !_sr.flipX;
        int index = state ? 1 : 0;
        _sr.sprite = sprites[index];

        _doorAdminister = doorAdminister.GetComponent<DoorAdminister>();
        _doorAdminister.keys[door][key] = state;
        _doorAdminister.StateChanged(door);
    }

    private bool compareRotation(PlayerRotateDirection _prd)
    {
        return (_prd == PlayerRotateDirection.Up && transform.rotation.z == 180) || (_prd == PlayerRotateDirection.Right && transform.rotation.z == 90)
            || (_prd == PlayerRotateDirection.Down && transform.rotation.z == 0) || (_prd == PlayerRotateDirection.Left && transform.rotation.z == 270);
    }
}
