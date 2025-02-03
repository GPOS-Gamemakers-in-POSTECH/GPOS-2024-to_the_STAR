using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemStatus : MonoBehaviour
{
    private int itemCount; // Count how many items player collected
    
    public GameObject itemStatusUI;
    private TextMeshProUGUI itemStatusText;

    public GameObject itemCollectedUI;
    private TextMeshProUGUI itemCollectedText;

    private int totalItemCount = 3; // Number of total items

    void Start()
    {    
        itemCount = 0;
        itemStatusText = itemStatusUI.GetComponentInChildren<TextMeshProUGUI>();
        itemCollectedText = itemCollectedUI.GetComponentInChildren<TextMeshProUGUI>();
        itemCollectedUI.SetActive(false);

        UpdateItemStatusText();
    }

    public void IncrementItemCount()
    {
        itemCount++;
        UpdateItemStatusText();
    }

    private void UpdateItemStatusText()
    {
        itemStatusText.text = $"{itemCount} / {totalItemCount}";
    }

    public void CollectUI(int itemId)
    {
        itemCollectedText.text = $"Item #{itemId} collected!";
        itemCollectedUI.SetActive(true);
        StartCoroutine(HideItemCollectedUI());
    }

    private IEnumerator HideItemCollectedUI()
    {
        yield return new WaitForSeconds(1f);
        itemCollectedUI.SetActive(false);
    }
}
