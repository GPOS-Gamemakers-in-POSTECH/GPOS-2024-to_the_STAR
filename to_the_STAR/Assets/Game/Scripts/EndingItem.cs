using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingItemTest : MonoBehaviour
{
    public TextMeshProUGUI text;// 텍스트를 받아올 부분

    // Start is called before the first frame update
    void Start()
    {
        text.text = ItemStatus.itemCount + " / " + ItemStatus.totalItemCount + " items collected!";
    }
}
