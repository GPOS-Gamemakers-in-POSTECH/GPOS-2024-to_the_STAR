using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public int itemId;
    public GameObject popupUI;
    private bool isPlayerNearby = false;
    private TextMeshPro popupText;
    private ItemStatus ItemStatus;

    void Start()
    {
        popupUI.SetActive(false);
        popupText = popupUI.GetComponent<TextMeshPro>();
        if(popupText == null)
            Debug.LogError("popupText is null");
        ItemStatus = GameObject.FindObjectOfType<ItemStatus>();

        if(PlayerPrefs.GetInt("Item_" + itemId, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E))
            CollectItem();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            popupUI.SetActive(true);
            Debug.Log("Player is nearby Item #" + itemId);
            popupText.text = $"[E] Item #{itemId}";
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            popupUI.SetActive(false);            
            isPlayerNearby = false;
        }
    }

    void CollectItem()
    {
        PlayerPrefs.SetInt("Item_" + itemId, 1);
        ItemStatus.CollectUI(itemId);
        Destroy(gameObject);        
        ItemStatus.IncrementItemCount();
    }    
}
