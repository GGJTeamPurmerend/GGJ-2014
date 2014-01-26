using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	private static Music instance = null;

	// Use this for initialization
	void Awake() {
		if(instance != null) {
			Destroy(this.gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
