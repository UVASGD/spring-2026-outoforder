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
    }

    private void setPlayTime()
    {
        GameData.playTime = DateTime.Now - startTime;
        startTime = DateTime.Now;
    }
}
