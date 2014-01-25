using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public enum UnitType {
	Cube, Circle
};

public class Player : MonoBehaviour {

	public static Player Instance;

	public UnitType state = UnitType.Cube;
	public bool IsChaining = false;

	[SerializeField]
	GameObject playerContainerReference;

	private Animator animator;
	[SerializeField]
	GameObject directionArrowPrefab;

	float chainCount = 0;

	// Controller
	public float speed;
	[SerializeField]
	RuntimeAnimatorController controller1, controller2;
	public int dashSpeed;
	public Boundary boundary;

	private bool isDashing = false;

	private float deltaTime = 0;


	void Awake() {
		Instance = this;
	}

	void Start() {
		animator = gameObject.GetComponentInChildren<Animator>();

		UnitSpawnManager.Instance.UpdatePlayerState(state);
	}

	void Update() {
		if(Input.GetButtonDown("X Button")) {
			toggleState();
			UnitSpawnManager.Instance.UpdatePlayerState(state);
		}
	}

	void toggleState() {
		if(this.state == UnitType.Cube) {
			animator.runtimeAnimatorController = controller2;
			this.state = UnitType.Circle;
		}
		else {
			animator.runtimeAnimatorController = controller1;
			this.state = UnitType.Cube;
		}
	}

	public UnitType getState() {
		return state;
	}

	public void kill() {
		Application.LoadLevel(0);
	}

	public void StartChain() {
		IsChaining = true;

		StartCoroutine(Chaining());
	}

	IEnumerator Chaining() {
		while(true) {
			List<NPC> unitsInChainRange = UnitSpawnManager.Instance.Units.FindAll(x => x.UnitType == this.state && x.AIType == AIType.Hostile && Vector2.Distance(x.transform.position, this.transform.position) < (2f + chainCount * 1));
			if(unitsInChainRange.Count == 0) {
				break;
			}

			foreach(NPC unit in unitsInChainRange) {
				GameObject arrow = (GameObject)Instantiate(directionArrowPrefab);
				arrow.GetComponent<DirectionArrow>().Initialize(unit.transform);
				arrow.transform.parent = this.playerContainerReference.transform;
				arrow.transform.localPosition = new Vector3(0, 0, 0);
				/* // get a random direction (360°) in radians
				 float angle = Vector2.Angle(this.transform.position, unit.transform.position);
				 Debug.Log("angle: " + angle);
				 // create a vector with length 1.0
				 Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
				 // scale it to the desired length
				 v *= 1.5f;
				 arrow.transform.localPosition = v;*/
			}

			yield return new WaitForSeconds(20f);

			// chainCount++;

		}

		IsChaining = false;
		chainCount = 0;
	}


	// Controller

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		float lookHorizontal = Input.GetAxis("RStick X");
		float lookVertical = Input.GetAxis("RStick Y");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 dashDirection = Vector3.zero;

        if (Input.GetAxis("R Trigger") == 1 || Input.GetKey(KeyCode.Space))
        {
			gameObject.GetComponentInChildren<Animator>().SetBool("dashing", true);
			isDashing = true;
			dashDirection = this.transform.rotation * Vector3.forward;
			dashDirection.Normalize();
			playerContainerReference.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + dashDirection, Time.deltaTime * dashSpeed);
		}
		else {
			gameObject.GetComponentInChildren<Animator>().SetBool("dashing", false);
			
			if(movement.magnitude > 0.25f) {
				isDashing = false;
				playerContainerReference.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movement, Time.deltaTime * speed);
				this.transform.rotation = Quaternion.LookRotation(movement);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(isDashing) {
			UnitSpawnManager.Instance.killUnit(other.GetComponent<NPC>());
		}
	}

	public bool IsDashing() {
		return isDashing;
	}
}
