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

    public void setStartTime(DateTime newStartTime)
    {
        startTime = newStartTime; 
    }

    public void saveData()
    {
        setPlayTime();
        storeGameDataInPlayerPrefs();
    }

    private void setPlayTime()
    {
        GameData.playTime = DateTime.Now - startTime;
        startTime = DateTime.Now;
    }

    private void storeGameDataInPlayerPrefs()
    {
        PlayerPrefs.SetInt("Escape Room Number", GameData.escapeRoomNumber);
        PlayerPrefs.SetString("Play Time", GameData.playTime.ToString());
        Debug.Log("Player Prefs:");
        Debug.Log(PlayerPrefs.GetString("Escape Room Number"));
        Debug.Log(PlayerPrefs.GetString("Play Time"));
    }
}
