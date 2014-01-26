using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    bool loadingLevel = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!loadingLevel && Input.anyKeyDown)
        {
            loadingLevel = true;
            Application.LoadLevel("Prototype");
        }
	}
}
