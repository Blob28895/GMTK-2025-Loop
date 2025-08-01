using System;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    private int howMany = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        howMany++;
        CheckForFull();
    }

    private void CheckForFull()
    {
        if(howMany == 2 || howMany == 3)
        {
            howMany = 0;
            Debug.Log("Capture");
            GameObject[] ropePoints = GameObject.FindGameObjectsWithTag("Rope");
            Array.ForEach(ropePoints, ropePoint => { Destroy(ropePoint); });
        }
    }
}
