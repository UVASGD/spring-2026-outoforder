using UnityEngine;

public class StartNewGameScript : MonoBehaviour
{
    public static StartGameScript startGameScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startGameScript = GetComponent<StartGameScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        startGameScript.StartGame("EscapeRoom0");
    }
}
