using UnityEngine;
using System.Collections;

public class Visibility : MonoBehaviour {
	GameObject player;
	Material material;
	
	private Material defaultMaterial;
	public Material friendly, hostile;

	private bool isHostile;

	// Use this for initialization
	void Start () {
		defaultMaterial = this.GetComponent<MeshRenderer>().material;
		player = GameObject.FindGameObjectWithTag("Player");

		if(Random.value > 0.5f) {
			isHostile = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(player.GetComponent<Swap>().getState() == State.Cube) {
			if(this.tag == "CubeEnemy") {
				this.GetComponent<MeshRenderer>().material = isHostile ? hostile : friendly;
			}
			else if(this.tag == "CylinderEnemy") {
				this.GetComponent<MeshRenderer>().material = defaultMaterial;
			}
		}
		else {
			if(this.tag == "CubeEnemy") {
				this.GetComponent<MeshRenderer>().material = defaultMaterial;
			}
			else if(this.tag == "CylinderEnemy") {
				this.GetComponent<MeshRenderer>().material = isHostile ? hostile : friendly;
			}
		}
	}
}
