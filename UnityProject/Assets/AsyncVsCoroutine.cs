using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncVsCoroutine : MonoBehaviour
{
    private async void Start()
    {
        var waitTime = 5;
        StartCoroutine(WaitSeconds(waitTime));
        await DoTapExampleAsync(waitTime);
    }
    
    private async Task DoTapExampleAsync(int waitTime)
    {
        var startTime = Time.time;
        Debug.Log($"Task started at {startTime}");
        Debug.Log("Wait.");
        await WaitSecondsAsync(waitTime);
        var endTime = Time.time - startTime;
        Debug.Log($"Completed Task at {endTime}");
        Debug.Log($"Task duration: {endTime - startTime}");
    }

    private async Task WaitSecondsAsync(int waitTime)
    {
        Debug.Log($"Thread: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(TimeSpan.FromSeconds(waitTime));
        Debug.Log("Finished waiting.");
    }

    private IEnumerator WaitSeconds(float x)
    {
        var startTime = Time.time;
        Debug.Log($"Coroutine started at {startTime}");
        Debug.Log("Wait.");
        yield return new WaitForSeconds(x);
        var endTime = Time.time - startTime;
        Debug.Log($"Completed Coroutine at {endTime}");
        Debug.Log($"Coroutine duration: {endTime - startTime}");
    }
}
