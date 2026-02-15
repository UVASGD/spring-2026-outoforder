using System;
using UnityEngine;

// TODO: Move this into GameProgression as it is a persisting object throughout the scenes
// if (something) saveDataScript.SetStartTime(DateTime.Now); do something like this for when the button clicked goes to game
public class SaveData : MonoBehaviour
{
    private DateTime startTime; 

    public void SetStartTime(DateTime newStartTime)
    {
        startTime = newStartTime; 
    }

    public void Save()
    {
        SetPlayTime();
        StoreGameDataInPlayerPrefs();
    }

    private void SetPlayTime()
    {
        GameData.playTime = DateTime.Now - startTime;
        startTime = DateTime.Now;
    }

    private void StoreGameDataInPlayerPrefs()
    {
        GameData.routeFlags.Add("Flag 1", true);
        PlayerPrefs.SetInt("Escape Room Number", GameData.escapeRoomNumber);
        PlayerPrefs.SetString("Play Time", GameData.playTime.ToString());
        PlayerPrefs.SetString("Route Flags", JsonUtility.ToJson(GameData.routeFlags));
        Debug.Log($"Player Prefs: Escape Room Number: {PlayerPrefs.GetInt("Escape Room Number")}, Play Time: {PlayerPrefs.GetInt("Play Time")}, Route Flags: {PlayerPrefs.GetInt("Route Flags")}");
    }
}
