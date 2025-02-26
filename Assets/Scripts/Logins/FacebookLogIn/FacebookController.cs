using UnityEngine;
using Facebook.Unity;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FacebookController : MonoBehaviour
{
    public static FacebookController Instance;

    private FacebookModel model;
    private FacebookView view;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        model = new FacebookModel();
        view = FindObjectOfType<FacebookView>();

        if (!FB.IsInitialized)
        {
            FB.Init(OnInitComplete);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void Start()
    {
        if (model.IsLoggedIn)
        {
            LoadUserData();
        }
    }

    private void OnInitComplete()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to initialize Facebook SDK");
        }
    }

    public void Login()
    {
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(new List<string> { "public_profile", "email" }, OnLoginCallback);
        }
        else
        {
            Debug.Log("Already logged in");
        }
    }

    public void Logout()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }

        model.ClearUserData();
        view.ResetUI();
    }

    private void OnLoginCallback(ILoginResult result)
    {
        if (result.Cancelled)
        {
            Debug.Log("Facebook login cancelled");
        }
        else if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Facebook login error: " + result.Error);
        }
        else if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook login successful");
            FB.API("/me?fields=id,first_name,last_name,email", HttpMethod.GET, OnUserDataReceived);
            model.IsLoggedIn = true;
            view.TogglePanel(true);
        }
    }

    private void OnUserDataReceived(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Error retrieving user data: " + result.Error);
        }
        else
        {
            var userData = result.ResultDictionary;
            string userId = userData["id"].ToString();
            string firstName = userData["first_name"].ToString();

            model.UserName = firstName;
            model.UserId = userId;

            FB.API("/me/picture?redirect=false&type=large", HttpMethod.GET, OnProfilePictureReceived);
        }
    }

    private void OnProfilePictureReceived(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Error retrieving profile picture: " + result.Error);
        }
        else
        {
            var pictureData = result.ResultDictionary["data"] as Dictionary<string, object>;
            string pictureURL = pictureData["url"].ToString();

            model.UserDp = pictureURL;
            StartCoroutine(LoadProfilePicture(pictureURL));
        }
    }

    private IEnumerator LoadProfilePicture(string pictureURL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(pictureURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite profilePic = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            view.UpdateUI(model.UserName, profilePic);
        }
        else
        {
            Debug.Log("Error fetching profile picture: " + www.error);
            view.ResetUI();
        }
    }

    private void LoadUserData()
    {
        string savedName = model.UserName;
        string savedProfilePicUrl = model.UserDp;

        if (!string.IsNullOrEmpty(savedName) && !string.IsNullOrEmpty(savedProfilePicUrl))
        {
            StartCoroutine(LoadProfilePicture(savedProfilePicUrl));
        }
    }
}
