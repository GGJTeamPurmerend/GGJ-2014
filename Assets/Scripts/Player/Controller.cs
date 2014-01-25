﻿using UnityEngine;
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

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		float lookHorizontal = Input.GetAxis("RStick X");
		float lookVertical = Input.GetAxis("RStick Y");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 dashDirection = Vector3.zero;

		if(Input.GetAxis("R Trigger") == 1) {
            isDashing = true;
			dashDirection = this.transform.rotation * Vector3.forward;
			dashDirection.Normalize();
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + dashDirection, Time.deltaTime * dashSpeed);
		}
		else if(movement.magnitude > 0.25f) {
            isDashing = false;
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movement, Time.deltaTime * speed);
			this.transform.rotation = Quaternion.LookRotation(movement);
			Vector3 rotation = this.transform.rotation.eulerAngles;
			rotation.x = 90;
			this.transform.rotation = Quaternion.Euler(rotation);
		}

	}

    void OnTriggerEnter(Collider other)
    {
        if (isDashing)
        {
            UnitSpawnManager.Instance.killUnit(other.GetComponent<NPC>());
        }
    }

    public bool getDashing()
    {
        return isDashing;
    }
}
