using UnityEngine;

public class FacebookModel
{
    private const string FBUserNameKey = "FBUserName";
    private const string FBUserIdKey = "FBUserId";
    private const string FBUserDpKey = "FBUserDp";
    private const string FBLoginboolKey = "FBLoginbool";

    public string UserName
    {
        get => PlayerPrefs.GetString(FBUserNameKey, "");
        set { PlayerPrefs.SetString(FBUserNameKey, value); PlayerPrefs.Save(); }
    }

    public string UserId
    {
        get => PlayerPrefs.GetString(FBUserIdKey, "");
        set { PlayerPrefs.SetString(FBUserIdKey, value); PlayerPrefs.Save(); }
    }

    public string UserDp
    {
        get => PlayerPrefs.GetString(FBUserDpKey, "");
        set { PlayerPrefs.SetString(FBUserDpKey, value); PlayerPrefs.Save(); }
    }

    public bool IsLoggedIn
    {
        get => PlayerPrefs.GetInt(FBLoginboolKey, 0) == 1;
        set { PlayerPrefs.SetInt(FBLoginboolKey, value ? 1 : 0); PlayerPrefs.Save(); }
    }

    public void ClearUserData()
    {
        PlayerPrefs.DeleteKey(FBUserNameKey);
        PlayerPrefs.DeleteKey(FBUserIdKey);
        PlayerPrefs.DeleteKey(FBUserDpKey);
        PlayerPrefs.DeleteKey(FBLoginboolKey);
        PlayerPrefs.Save();
    }
}
