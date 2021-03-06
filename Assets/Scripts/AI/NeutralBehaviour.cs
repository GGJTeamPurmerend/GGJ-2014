﻿using UnityEngine;
using System.Collections;

public class NeutralBehaviour : MonoBehaviour {
	public float rotationSpeed = 5f;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 30;

	public int enemyDashSpeed = 4;

	Transform target;
	float heading;
	Vector3 targetRotation;
	bool aggroIsFake;

	Vector3 separationPosition;
	Vector3 direction = Vector3.zero;
	int separationCount = 0;

	public void AggroToPlayer(bool aggroIsFake) {
		this.aggroIsFake = aggroIsFake;
		target = Player.Instance.transform;
	}
	void Awake() {
		// Set random initial rotation
		heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, heading, 0);

		StartCoroutine(NewHeading());
	}

	public virtual void Update() {

		float speed = TweakableValues.NeutralNPCSpeed;
		if(target != null) {
			gameObject.GetComponentInChildren<Animator>().SetBool("moving", true);
			Vector3 eulerAngles = transform.eulerAngles;
			transform.LookAt(target.position);
			transform.eulerAngles = Vector3.Slerp(eulerAngles, transform.eulerAngles, Time.deltaTime * 1);

			if(Vector2.Distance(transform.position, target.position) <= TweakableValues.BluffAggroPlayerDistance) {
				if(aggroIsFake) {
					target = null;
				}
				else {
					gameObject.GetComponentInChildren<Animator>().SetBool("dashing", true);
					Vector3 dashDirection = Vector3.zero;
					dashDirection = this.transform.rotation * Vector3.forward;
					dashDirection.Normalize();
					this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + dashDirection, Time.deltaTime * enemyDashSpeed);
				}
			}
			else {
				gameObject.GetComponentInChildren<Animator>().SetBool("dashing", false);
			}
			speed *= 1.5f;
		}
		else {
			gameObject.GetComponentInChildren<Animator>().SetBool("moving", false);
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
		}

		var forward = transform.TransformDirection(Vector3.forward);

		this.rigidbody.velocity = forward * speed;
	}

	/// <summary>
	/// Repeatedly calculates a new direction to move towards.
	/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
	/// </summary>
	IEnumerator NewHeading() {
		while(true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	/// <summary>
	/// Calculates a new direction to move towards.
	/// </summary>
	void NewHeadingRoutine() {
		if(target != null) {
			return;
		}
		var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
		var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
		heading = Random.Range(floor, ceil);
		targetRotation = new Vector3(0, heading, 0);
	}

}
