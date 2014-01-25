using UnityEngine;
using System.Collections;

public class NeutralBehaviour : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        this.speed = 5f;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.MoveTowards(this.transform.position, Player.Instance.transform.position, speed * Time.deltaTime);

	}
}
