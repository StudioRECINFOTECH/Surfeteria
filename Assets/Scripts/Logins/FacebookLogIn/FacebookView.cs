using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FacebookView : MonoBehaviour
{
    public TextMeshProUGUI FB_userName;
    //public Image defaultAvatar;
    public Image FB_userDp;
    public GameObject panel;
    //public GameObject GuestBtn;
    public Button LoginBtn;

    public void UpdateUI(string userName, Sprite profilePic)
    {
        FB_userName.text = userName;
        FB_userDp.sprite = profilePic;
        //FB_userDp.gameObject.SetActive(true);
        //defaultAvatar.gameObject.SetActive(false);
        panel.SetActive(false);
        //GuestBtn.SetActive(false);
        LoginBtn.interactable = false;
    }

    public void ResetUI()
    {
        FB_userName.text = "Player";
        FB_userDp.sprite = null;
        //FB_userDp.gameObject.SetActive(false);
        //defaultAvatar.gameObject.SetActive(true);
        panel.SetActive(true);
        //GuestBtn.SetActive(true);
        LoginBtn.interactable = true;
    }

    public void TogglePanel(bool show)
    {
        StartCoroutine(ShowPanelCoroutine(show));
    }

    private IEnumerator ShowPanelCoroutine(bool show)
    {
        yield return new WaitForSeconds(0.5f);
        panel.SetActive(!show);
        //defaultAvatar.enabled = !show;
    }
}
