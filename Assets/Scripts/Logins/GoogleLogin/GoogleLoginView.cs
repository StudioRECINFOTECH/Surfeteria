using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class GoogleLoginView : MonoBehaviour
{
    public TextMeshProUGUI gname;
    public Image Google_userDp;
    public GameObject panel;
    //public GameObject GuestBtn;
    public Button LoginBtn;
    //public Image defaultAvatar;

    public void UpdateUI(GoogleUserModel user)
    {
        if (user.IsLoggedIn)
        {
            gname.text = PlayerModular.Instance.userDetails.UserName;
            if (!string.IsNullOrEmpty(user.ProfilePicUrl))
            {
                StartCoroutine(LoadProfilePic(user.ProfilePicUrl));
            }

            //Todo:enable login
            ShowLoggedInState();
        }
        else
        {
            //Todo:enable login
            ShowLoggedOutState();
        }
    }

    private void ShowLoggedInState()
    {
        panel?.SetActive(false);
        //GuestBtn?.SetActive(false);
        LoginBtn.interactable = false;
        //defaultAvatar?.gameObject.SetActive(true);
    }

    private void ShowLoggedOutState()
    {
        panel?.SetActive(true);
        //GuestBtn?.SetActive(true);
        LoginBtn.interactable = true;
        gname.text = "Player";
        Google_userDp.sprite = null;
    }

    private IEnumerator LoadProfilePic(string imageUrl)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Google_userDp.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
        else
        {
            Debug.LogError("Failed to load profile picture: " + www.error);
        }
    }
}
