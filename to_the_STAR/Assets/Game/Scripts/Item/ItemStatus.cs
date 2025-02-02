using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemStatus : MonoBehaviour
{
    private static int itemCount;
    public GameObject itemStatusUI;
    private static TextMeshProUGUI itemStatusText;
    private static int totalItemCount = 3;

    void Start()
    {
        PlayerPrefs.DeleteAll(); // refresh PlayerPrefs
        itemCount = 0;
        itemStatusText = itemStatusUI.GetComponentInChildren<TextMeshProUGUI>();
        UpdateItemStatusText();
    }

    void Update()
    {
        
    }

    public static void IncrementItemCount()
    {
        itemCount++;
        UpdateItemStatusText();
    }

    private static void UpdateItemStatusText()
    {
        itemStatusText.text = $"{itemCount} / {totalItemCount}";
    }
}
