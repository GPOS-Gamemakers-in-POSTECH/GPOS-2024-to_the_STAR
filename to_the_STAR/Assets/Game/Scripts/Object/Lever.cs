using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Sprite[] sprites; // index 0 : 비활성화, 1 : 활성화
    GameObject doorAdminister;
    SpriteRenderer _sr;
    DoorAdminister _doorAdminister;
    PlayerMovementController player;
    PlayerData playerData;
    public int[] door; // 연결되어 있는 문의 번호
    public int key; // 몇 번째 레버인지
    public bool isTimer; // 뒤주용 타이머 레버인가?
    public bool initial; // 최초 활성화 상태 지정
    float timer;
    bool state;
    bool playerUsed;

    public GameObject popUpUI;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        doorAdminister = GameObject.Find("DoorAdminister");
        popUpUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) // 플레이어와 충돌한 레버를 플레이어와 연결
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovementController>();
            playerData = collision.GetComponent<PlayerData>();

            if(player != null && compareRotation(playerData.RotateDir))
            {
                player._lever = this;
            }
            popUpUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player._lever = null;
            player = null;
            popUpUI.SetActive(false);
        }
    }

    private void works()
    {
        state = !state;
        int index = state ? 1 : 0;
        _sr.sprite = sprites[index];
        _doorAdminister = doorAdminister.GetComponent<DoorAdminister>();
        for (int i = 0; i < door.Length; i++)
        {
            _doorAdminister.keys[door[i]][key] = state;
            _doorAdminister.StateChanged(door[i]);
        }
    }
    public void Interaction() // 이 레버를 작동 시켰을 때
    {
        works();
        playerUsed = !playerUsed;
        timer = 3;
    }

    private bool compareRotation(PlayerRotateDirection _prd)
    {
        return (_prd == PlayerRotateDirection.Up && transform.rotation.eulerAngles.z == 180) || (_prd == PlayerRotateDirection.Right && transform.rotation.eulerAngles.z == 90)
            || (_prd == PlayerRotateDirection.Down && transform.rotation.eulerAngles.z == 0) || (_prd == PlayerRotateDirection.Left && transform.rotation.eulerAngles.z == 270);
    }

    private void Update()
    {
        if (initial)
        {
            initial = false;
            works();
        }
        if (isTimer && timer < 0 && playerUsed)
        {
            playerUsed = false;
            works();
        }
        timer -= Time.deltaTime;
    }
}
