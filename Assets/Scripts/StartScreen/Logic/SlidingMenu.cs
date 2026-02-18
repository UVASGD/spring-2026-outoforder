using System.Collections;
using UnityEngine;

public class SlidingMenu : MonoBehaviour
{
    public void MoveLeft()
    {
        StartCoroutine(Move(-400f));
    }

    public void MoveRight()
    {
        StartCoroutine(Move(0f));
    }

   public IEnumerator Move(float targetXPosition)
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(targetXPosition, startPosition.y, startPosition.z);
        float timeElapsed = 0;

        while (timeElapsed < 0.25f)
        {
            float t = timeElapsed / 0.25f;
            
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
    }
}
