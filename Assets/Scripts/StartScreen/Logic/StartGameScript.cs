using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private DateTime startTime;

    public TimeSpan playTime; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(string name) 
    {
        SceneManager.LoadScene(name);
    }

    public void setPlayTime()
    {
        playTime = DateTime.Now - startTime;
    }
}
