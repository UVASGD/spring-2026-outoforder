using System;
using UnityEngine;

public class SaveDataScript : MonoBehaviour
{
    private DateTime startTime; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStartTime(DateTime newStartTime)
    {
        startTime = newStartTime; 
    }

    public void SaveData()
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
        GameData.escapeRoomNumber = 4;
        PlayerPrefs.SetInt("Escape Room Number", GameData.escapeRoomNumber);
        PlayerPrefs.SetString("Play Time", GameData.playTime.ToString());
        Debug.Log("Player Prefs:");
        Debug.Log(PlayerPrefs.GetInt("Escape Room Number"));
        Debug.Log(PlayerPrefs.GetString("Play Time"));
    }
}
