using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public static SaveDataScript saveDataScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDataScript = GetComponent<SaveDataScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(string sceneName)
    {
        saveDataScript.SetStartTime(DateTime.Now);
        ChangeScene(sceneName);
    }

    private void ChangeScene(string name) 
    {
        SceneManager.LoadScene(name);
    }
}
