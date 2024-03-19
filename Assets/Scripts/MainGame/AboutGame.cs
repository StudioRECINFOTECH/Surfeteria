using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutGame : MonoBehaviour
{

    public GameObject settingBox;

    void Start()
    {
        // Ensure that the settingBox is not null
        if (settingBox != null)
        {
            // Show the settingBox on startup
            settingBox.SetActive(true);
          //  settingBox.GetComponent<MessageSetting>().StartShowMessage();
            settingBox.GetComponent<Animator>().SetTrigger("TriOpen");

            // Invoke a method to hide the settingBox after 3 seconds
            // Invoke("HideSettingBox", 3f);
        }
    }

    void HideSettingBox()
    {
        // Hide the settingBox
        settingBox.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
