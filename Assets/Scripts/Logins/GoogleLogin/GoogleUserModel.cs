using UnityEngine;

public class GoogleUserModel
{
    public string UserId { get; private set; }
    public string ProfilePicUrl { get; private set; }

    public bool IsLoggedIn;

    private const string GoogleUserNameKey = "GoogleUserNameKey";
    private const string GoogleUserIdKey = "GoogleUserIdKey";
    private const string GoogleUserDpKey = "GoogleUserDpKey";
    private const string GoogleLoginBoolKey = "googleLoginbool";

    public GoogleUserModel()
    {
        IsLoggedIn = PlayerModular.Instance.userDetails.IsLoggedIn;
    }

    public void LoadUserData()
    {
        if (PlayerPrefs.HasKey(GoogleUserNameKey))
        {
            PlayerModular.Instance.userDetails.UserName = PlayerPrefs.GetString(GoogleUserNameKey);
        }
        if (PlayerPrefs.HasKey(GoogleUserDpKey))
        {
            ProfilePicUrl = PlayerPrefs.GetString(GoogleUserDpKey);
        }
        IsLoggedIn = PlayerPrefs.GetInt(GoogleLoginBoolKey, 0) == 1;
    }

    public void SaveUserData(string userName, string userId, string profilePicUrl)
    {
        PlayerModular.Instance.userDetails.UserName = userName;
        PlayerModular.Instance.userDetails.UserId = userId;
        ProfilePicUrl = profilePicUrl;
        IsLoggedIn = true;
        //PlayerModular.Instance.userDetails.IsLoggedIn = IsLoggedIn;
        PlayerPrefs.SetString(GoogleUserNameKey, userName);
        PlayerPrefs.SetString(GoogleUserIdKey, userId);
        if (!string.IsNullOrEmpty(profilePicUrl))
        {
            PlayerPrefs.SetString(GoogleUserDpKey, profilePicUrl);
        }
        PlayerPrefs.SetInt(GoogleLoginBoolKey, 1);
        PlayerPrefs.Save();
    }

    public void ClearUserData()
    {
        PlayerModular.Instance.userDetails.UserName = null;
        PlayerModular.Instance.userDetails.UserId = null;
        ProfilePicUrl = null;
        IsLoggedIn = false;

        PlayerPrefs.DeleteKey(GoogleUserNameKey);
        PlayerPrefs.DeleteKey(GoogleUserIdKey);
        PlayerPrefs.DeleteKey(GoogleUserDpKey);
        PlayerPrefs.DeleteKey(GoogleLoginBoolKey);
        PlayerPrefs.Save();
    }
}
