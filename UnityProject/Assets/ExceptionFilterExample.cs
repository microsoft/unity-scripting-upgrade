using System;
using UnityEngine;

public class ExceptionFilterExample : MonoBehaviour
{
    private void ExceptionFilterTest()
    {
        bool testExceptionFilter = true;
        try
        {

            throw new Exception("Error!");
        }
        catch (Exception) when (testExceptionFilter)
        {
            Debug.Log("In filter");
        }
    }
}
