using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingItemTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ItemStatus.itemCount + " / " + ItemStatus.totalItemCount + " items collected!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("EndingScene");
        }
    }
}
