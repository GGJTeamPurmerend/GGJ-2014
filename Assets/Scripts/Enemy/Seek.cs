using UnityEngine;
using System.Collections;

public class Seek : MonoBehaviour {
	GameObject player;
	public float speed;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
	}
}
