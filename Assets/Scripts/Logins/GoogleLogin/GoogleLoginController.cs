using UnityEngine;
using Google;
using System.Threading.Tasks;

public class GoogleLoginController : MonoBehaviour
{
    private GoogleUserModel userModel;
    private GoogleLoginView userView;
    private GoogleSignInConfiguration configuration;

    //Todo:"YOUR_GOOGLE_CLIENT_ID_HERE"
    private string _webClientId = "374526353654-it6n5lot2fopi2sdstcqiqbtr8oqhunt.apps.googleusercontent.com";

    private void Awake()
    {
        userModel = new GoogleUserModel();
        userView = FindObjectOfType<GoogleLoginView>();

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = _webClientId,
            RequestIdToken = true,
            RequestEmail = true
        };

        userModel.LoadUserData();
        userView.UpdateUI(userModel);
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.Default);
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Google Sign-In failed.");
            return;
        }

        string name = task.Result.DisplayName;
        string idToken = task.Result.IdToken;
        string profilePicUrl = task.Result.ImageUrl != null ? task.Result.ImageUrl.ToString() : null;

        userModel.SaveUserData(name, idToken, profilePicUrl);
        userView.UpdateUI(userModel);
    }

    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        userModel.ClearUserData();
        userView.UpdateUI(userModel);
    }
}
