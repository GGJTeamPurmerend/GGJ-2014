using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
	
	float transition = 3;
	float deltaTime = 0;
	
	// Update is called once per frame
	void Update () {
		deltaTime += Time.deltaTime;
		if(deltaTime >= transition) {
            Application.LoadLevel("Start");
		}  
	}
}
