using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageSaveMe : MonoBehaviour {

    public Button button;
    public Image iconCooldown;
    public Text textKey, totalTextKey, warn;
    public int timeCooldown = 3;//tinh bang giay
    private bool mesShow = false, updateTime = true;
    private int totalTime = 0;
    private int tunTime = 0;
    public GameObject butShareVideo;
    public GameObject button1;
    public GameObject button2;
    //xu ly ngon ngu
    public Text textSaveMe, textButUpVideo;

    public void StartShowMessage()
    {
        Modules.SetStatusButShareVideo(butShareVideo);
        int iLang = Modules.indexLanguage;
        textSaveMe.font = AllLanguages.listFontLangA[iLang];
        textSaveMe.text = AllLanguages.playSaveMe[iLang];
        textButUpVideo.font = AllLanguages.listFontLangA[iLang];
        textButUpVideo.text = AllLanguages.menuUpVideo[iLang];
        totalTextKey.font = AllLanguages.listFontLangA[iLang];
        totalTextKey.text = Modules.totalKey.ToString();
        textKey.text = Mathf.Pow(2, Modules.timeSaveMe).ToString();
        iconCooldown.fillAmount = 1f;
        totalTime = Modules.SecondsToTimePerFrame(timeCooldown);
        tunTime = 0;
        mesShow = true;
        updateTime = true;


        totalTextKey.color = Color.white;
        textKey.color = Color.white;



    }

    public void ButtonSaveMeClick(GameObject myButton)
    {

        button1 = myButton;


        if (tunTime >= totalTime) return;
        Modules.PlayAudioClipFree(Modules.audioButton);
        //xu ly kiem tra du key chua, neu du thi thuc thi hoi sinh, khong thi hien bang mua

        if (Modules.chances < 2)
        {

            if (!Modules.HandleReborn())//neu khong du thi hien thi bang mua key
            {

                totalTextKey.color = Color.red;
                textKey.color = Color.red;

                // Modules.mesNotEnoughKey.SetActive(true);
                // Modules.mesNotEnoughKey.GetComponent<MessageBuyKeys>().StartShowMessage();
                // Modules.HandleGameOver();
            }
            else
            {
                transform.gameObject.GetComponent<Animator>().SetTrigger("TriClose");
                tunTime = 0;
                mesShow = false;

                if (Modules.chance != 3) {

                    EnableButtonStatus(button2);
                    ResetText(button2);

                }

                if (Modules.chances == 2)
                {
                    myButton.GetComponent<ButtonStatus>().Disable();
                    return;
                }

            }
        }

        /* else {
           
            textKey.color = Color.black;

        } */
    }


    public void WatchToSave(GameObject myButton)
    {

        button2 = myButton;

        if (tunTime >= totalTime) return;
        Modules.PlayAudioClipFree(Modules.audioButton);


        // transform.gameObject.GetComponent<Animator>().SetTrigger("TriClose");
        updateTime = false;

            myButton.GetComponent<ButtonStatus>().myText.text = "Loading";

            myButton.GetComponent<ButtonStatus>().Disable();

            ADSController.Instance.RequestRewardBasedVideo(true, WatchAd);
        
    }

    public void WatchAd(bool obj) {

        if (obj)
        {

            if (Modules.RewardReborn())
            {

                transform.gameObject.GetComponent<Animator>().SetTrigger("TriClose");
                tunTime = 0;
                mesShow = false;
                EnableButtonStatus(button2);
                ResetText(button2);


                if (Modules.chance == 4)
                {

                    DisableButtonStatus(button2);
                    // myButton.GetComponent<ButtonStatus>().Disable();
                    // return;

                }
            }
        }
    }

    public void failed() {

        updateTime = true;

    }

        public void PanelOutsideClick()
    {
        if (Modules.bonusFirst > 0) Modules.HandleGameOver();
        else Modules.ShowBonusFirst();
        transform.gameObject.SetActive(false);
        Modules.chance = 0;
        Modules.chances = 0;
        EnableButtonStatus(button1);
        EnableButtonStatus(button2);
        ResetText(button2);
        updateTime = true;
    }

    void FixedUpdate()
    {
        if (mesShow)
        {
            if (tunTime >= totalTime)
            {
                mesShow = false;
                if (Modules.bonusFirst > 0) Modules.HandleGameOver();
                else Modules.ShowBonusFirst();
                transform.gameObject.SetActive(false);
                Modules.chance = 0;
                Modules.chances = 0;
                EnableButtonStatus(button1);
                EnableButtonStatus(button2);
                ResetText(button2);
                updateTime = true;
            }
            else
            {
                if (updateTime){ 
                tunTime++;
                iconCooldown.fillAmount = 1f - (float)tunTime / (float)totalTime;
            }
            }
        }
    }

    void EnableButtonStatus(GameObject button)
    {
        if (button != null && button.GetComponent<ButtonStatus>() != null)
        {
            button.GetComponent<ButtonStatus>().Enable();
        }
    }

    void DisableButtonStatus(GameObject button)
    {
        if (button != null && button.GetComponent<ButtonStatus>() != null)
        {
            button.GetComponent<ButtonStatus>().Disable();
        }
    }

    void ResetText(GameObject button)
    {
        if (button != null && button.GetComponent<ButtonStatus>() != null)
        {
            button.GetComponent<ButtonStatus>().myText.text = "Watch AD";
        }
    }
}
