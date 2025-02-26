using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModular
{
    private static PlayerModular _instance;

    public UserDetails userDetails = new UserDetails();

    public PlayerModular()
    {
        CheckPlayFabId();
    }

    public static PlayerModular Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerModular();
            }
            return _instance;
        }
    }


    private int _characterSelectionIndex = 0;

    public int CharacterSelectionIndex
    {
        get => _characterSelectionIndex;
        set => _characterSelectionIndex = value;
    }

    private void CheckPlayFabId()
    {
        if (PlayerPrefs.HasKey("UserId"))
        {
            userDetails.UserId = PlayerPrefs.GetString("UserId");
        }
    }

    public void SetCurrentPlayerData(string userId)
    {
        Debug.Log($"user id : {userId}");
        userDetails.UserId = userId;
        PlayerPrefs.SetString("UserId", userDetails.UserId);
    }

    public void SelectCurrentPlayer(int index, string cName)
    {
        CharacterSelectionIndex = index;
        userDetails.CharacterName = cName;
        Debug.Log($"SetMaterial for Character {CharacterSelectionIndex} (Index: {cName})");
    }

    //public void TrackPowerUpUsage(string pName, int Collect = 0)
    //{
    //    Debug.Log($"pname : {pName}");
    //    PowerUpDetails powerUp = userDetails.PowerUps.Find(p => p.Name == pName);
    //    if (powerUp == null)
    //    {
    //        PowerUpDetails powerUpDetails = new PowerUpDetails()
    //        {
    //            Name = pName,
    //            Count = Collect > 0 ? 1 : 0,
    //            UsedCount = Collect <= 0 ? 1 : 0
    //        };

    //        userDetails.PowerUps.Add(powerUpDetails);
    //    }
    //    else
    //    {
    //        if (Collect > 0)
    //        {
    //            powerUp.Count++;
    //        }
    //        else
    //            powerUp.UsedCount++;

    //    }

    //    userDetails.PowerUps.ForEach((e) =>
    //    {
    //        Debug.Log($"name : {e.Name} : {e.Count} : {e.UsedCount}");
    //    });
    //}

    public void UpateGameScore(int score)
    {
        userDetails.UserScore += score;
        Debug.Log($"upate score : {score} : {userDetails.UserScore}");
    }
}

[System.Serializable]
public class UserDetails
{
    public string UserId;
    public string UserName;
    public string Gmail;
    public string CharacterName;
    public int UserScore;
    public bool IsLoggedIn;
    public float CatcherCaptureTime;
}

//[System.Serializable]
//public class PowerUpDetails
//{
//    public string Name;
//    public int Count;
//    public int UsedCount;
//}

