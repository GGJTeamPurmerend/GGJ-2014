using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

    Transform lookAt = null;
	// Use this for initialization
	public void Initialize(Transform lookAt){
        this.lookAt = lookAt;
    }
	
	// Update is called once per frame
	void Update () {
        if (lookAt != null)
        {
            this.transform.LookAt(lookAt);
        }
	}
}
