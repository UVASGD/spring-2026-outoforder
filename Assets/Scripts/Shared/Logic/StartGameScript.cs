using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public static SaveDataScript saveDataScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame(string sceneName)
    {
        saveDataScript.setStartTime(DateTime.Now);
        changeScene(sceneName);
    }

    private void changeScene(string name) 
    {
        SceneManager.LoadScene(name);
    }
}
