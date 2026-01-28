using System.Linq;
using TMPro;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    protected char[] answer;
    protected char[] guess;

    public void AttemptSolve()
    {
        // print($"the answer is {new string(answer)} and the guess was {new string(guess)}");
        // TODO: VERY TEMPORARY
        if (answer.SequenceEqual(guess)) GameObject.Find("DemoMessage").GetComponent<TextMeshProUGUI>().text = "DEMO MESSAGE: SUCCESS";
    }
}
