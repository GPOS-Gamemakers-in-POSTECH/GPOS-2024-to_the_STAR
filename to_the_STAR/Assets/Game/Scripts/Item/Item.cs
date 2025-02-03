using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Player;

public class Item : MonoBehaviour
{
    public int itemId;
    public GameObject popupUI;
    private TextMeshPro popupText;
    private ItemStatus ItemStatus;
    private PlayerMovementController _pmc;

    void Start()
    {
        popupUI.SetActive(false);
        popupText = popupUI.GetComponent<TextMeshPro>();
        if(popupText == null)
            Debug.LogError("popupText is null");
        ItemStatus = GameObject.FindObjectOfType<ItemStatus>();

        PlayerPrefs.SetInt("Item_" + itemId, 0);
        gameObject.SetActive(true);
        
        /*
        if(PlayerPrefs.GetInt("Item_" + itemId, 0) == 1)
        {
            Destroy(gameObject);            
        }
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            popupUI.SetActive(true);
            Debug.Log("Player is nearby Item #" + itemId);
            popupText.text = $"[E] Item #{itemId}";
            _pmc = other.GetComponent<PlayerMovementController>();
            _pmc._item = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            popupUI.SetActive(false);            
            _pmc._item = null;
            _pmc = null;
        }
    }

    public void CollectItem()
    {
        PlayerPrefs.SetInt("Item_" + itemId, 1);
        ItemStatus.CollectUI(itemId);
        //Destroy(gameObject);        
        gameObject.SetActive(false);
        ItemStatus.IncrementItemCount();
    }    
}
