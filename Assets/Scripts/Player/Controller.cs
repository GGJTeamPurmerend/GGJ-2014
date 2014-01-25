using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class Controller : MonoBehaviour {

	public float speed;
	public int dashSpeed;
	public Boundary boundary;

	private bool isDashing = false;

	private float deltaTime = 0;

	void Update() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		float lookHorizontal = Input.GetAxis("RStick X");
		float lookVertical = Input.GetAxis("RStick Y");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 lookDirection = new Vector3(lookVertical, 0.0f, -lookHorizontal);

		rigidbody.velocity = Vector3.zero;

		if(lookDirection.magnitude > 0.75f || isDashing == true && deltaTime > 0) {
			if(isDashing == false) {
				deltaTime += 0.25f;
			}
			isDashing = true;

			lookDirection.Normalize();

			rigidbody.velocity = lookDirection * dashSpeed;
			this.transform.rotation = Quaternion.LookRotation(lookDirection);

			deltaTime -= Time.deltaTime;
		}
		else {
			isDashing = false;

			if(movement.magnitude > 0.25f) {
				rigidbody.velocity += movement * speed;
				this.transform.rotation = Quaternion.LookRotation(movement);
			}
			else {
				rigidbody.velocity = Vector3.zero;
			}
		}

		rigidbody.position = new Vector3(
			Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
	}
}
