using static UnityEngine.Mathf;
using UnityEngine;

public class UsingStaticExample : MonoBehaviour
{
	private void Start ()
    {        
        Debug.Log(RoundToInt(PI));
        // Output:
        // 3
    }
}
