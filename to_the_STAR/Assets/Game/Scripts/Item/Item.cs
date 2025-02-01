using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public int itemId;
    //public GameObject popupUI;
    private bool isPlayerNearby = false;

    void Start()
    {
        //popupUI.SetActive(false);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.DeleteKey("Item_" + itemId);

        if(PlayerPrefs.GetInt("Item_" + itemId, 0) == 1)
        {
            //Destroy(GameObject);
            gameObject.SetActive(false);
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
            //popupUI.SetActive(true);
            Debug.Log("Player is nearby Item #" + itemId);
            //popupUI.GetComponentInChildren<TextMeshProUGUI>().text = $"[E] #{itemID} 아이템";
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //popupUI.SetActive(false);            
            isPlayerNearby = false;
        }
    }

    void CollectItem()
    {
        PlayerPrefs.SetInt("Item_" + itemId, 1);
        //FindObjectOfType<ItemUI>().ShowCollectedMessage(itemID);
        Debug.Log("You Got Item #" + itemId);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }    
}
