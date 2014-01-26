using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	private float offsetX;

	// Use this for initialization
	void Start() {
		renderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
	}

	// Update is called once per frame
	void FixedUpdate() {
		Background bg = GameObject.FindObjectOfType<Background>();
		renderer.material.mainTextureOffset = new Vector2(Camera.main.transform.position.x / bg.transform.localScale.x, Camera.main.transform.position.z / bg.transform.localScale.y);
	}
}