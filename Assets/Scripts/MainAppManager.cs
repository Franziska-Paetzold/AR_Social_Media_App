using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAppManager : MonoBehaviour {

    public static MainAppManager mainAppManager;


	// Use this for initialization
	void Awake () {
		if(mainAppManager == null)
        {
            mainAppManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
