using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    
   // Start() and Update() methods deleted - we don't need them right now

    public static MainManager inst;

    private void Awake()
    {
        Debug.Log("MainManager Awake called");
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
            credits = 10;
        }
        else
        {
            Debug.Log("MainManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
        }
    }


    public int credits;
    
}
