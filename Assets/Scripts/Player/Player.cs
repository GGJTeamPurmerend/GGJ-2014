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

public enum ChainState
{
    None, Pre, Dashing, In
}

public class Player : MonoBehaviour {

	public static Player Instance;

	public UnitType state = UnitType.Cube;
    public ChainState chainState = ChainState.None;
    NPC currentlyDashingChainedUnit;

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
	public float dashSpeed;
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
            this.gameObject.GetComponents<AudioSource>()[2].Play();
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
        this.gameObject.GetComponents<AudioSource>()[3].Play();
		Application.LoadLevel(0);
	}

	public void StartChain() {
        chainState = ChainState.Pre;

		StartCoroutine(Chaining());
	}

	IEnumerator Chaining() {
		while(true) {
            
            List<NPC> unitsInChainRange = UnitSpawnManager.Instance.Units.FindAll(x => x.UnitType == this.state && x.AIType == AIType.Hostile && Vector3.Distance(x.transform.position, playerContainerReference.transform.position) < (13f + chainCount));
  
            List<NPC> nearestUnits = new List<NPC>();
            if (unitsInChainRange.Count > 0)
            {
                for (int j = 0; j < 3; j++)
                {
                    float closestDistance = 10000f;
                    NPC nearestUnit = unitsInChainRange[0];
                    for (int i = 0; i < unitsInChainRange.Count; i++)
                    {
                        NPC unit = unitsInChainRange[i];
                        float distance = Vector3.Distance(this.playerContainerReference.transform.position, unit.transform.position);
                        if (distance > closestDistance) continue;
                        if (nearestUnits.Contains(unit)) continue;
                        closestDistance = distance;
                        nearestUnit = unit;
                    }

                    nearestUnits.Add(nearestUnit);
                }
            }

            if (nearestUnits.Count == 0)
            {
                if (chainCount != 0)
                {
                    // TODO: chainstreak has been ended
                }
				break;
			}

            foreach (NPC unit in nearestUnits)
            {
				GameObject arrow = (GameObject)Instantiate(directionArrowPrefab);
				arrow.GetComponent<DirectionArrow>().Initialize(unit);
				arrow.transform.parent = this.playerContainerReference.transform;
				arrow.transform.localPosition = new Vector3(0, 0, 0);
			}

			yield return new WaitForSeconds(20f);

			// chainCount++;

		}

        chainState = ChainState.None;
		chainCount = 0;
	}


	// Controller

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

        GetChainInput();

        if (chainState == ChainState.Dashing)
        {
            Vector3 direction = currentlyDashingChainedUnit.transform.position - this.playerContainerReference.transform.position;
            Dash(4f, direction);
            this.transform.rotation = Quaternion.LookRotation(currentlyDashingChainedUnit.transform.position);
            return;
        }

        if (Input.GetAxis("R Trigger") == 1 || Input.GetKey(KeyCode.Space))
        {
            Dash(1f, this.transform.rotation * Vector3.forward);
		}
		else {
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
			gameObject.GetComponentInChildren<Animator>().SetBool("dashing", false);
			if(movement.magnitude > 0.25f) {
				gameObject.GetComponentInChildren<Animator>().SetBool("moving", true);
				isDashing = false;
				playerContainerReference.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movement, Time.deltaTime * speed);
				this.transform.rotation = Quaternion.LookRotation(movement);
			}
			else {
				gameObject.GetComponentInChildren<Animator>().SetBool("moving", false);
			}
		}
	}

    void Dash(float extraPower, Vector3 direction)
    {
        Vector3 dashDirection;
        gameObject.GetComponentInChildren<Animator>().SetBool("dashing", true);
        isDashing = true;
        dashDirection = direction;
        dashDirection.Normalize();
        playerContainerReference.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + dashDirection, Time.deltaTime * dashSpeed * extraPower);
        this.gameObject.GetComponents<AudioSource>()[0].Play();
    }

    void GetChainInput()
    {
        float lookHorizontal = Input.GetAxis("RStick X");
        float lookVertical = Input.GetAxis("RStick Y");

        Vector2 pos = new Vector2(lookVertical, lookHorizontal);

        if (chainState == ChainState.Pre)
        {
        
            if (pos.magnitude >= 1f)
            {
                pos *= 2f;
                Vector3 newPos = new Vector3();
                newPos.x = Player.Instance.transform.position.x + pos.x;
                newPos.z = Player.Instance.transform.position.z + -pos.y;

                DirectionArrow[] arrows = FindObjectsOfType<DirectionArrow>();
                bool removeArrows = false;
                foreach (DirectionArrow arrow in arrows)
                {
                    if (Vector3.Distance(arrow.transform.GetChild(0).position, newPos) < 1.7f)
                    {
                        currentlyDashingChainedUnit = arrow.unit;
                        chainState = ChainState.Dashing;
                        removeArrows = true;
                    }
                }
                if (removeArrows)
                {
                    foreach (DirectionArrow arrow in arrows)
                    {
                        GameObject.Destroy(arrow.gameObject);
                    }
                }
            }
        }
    }

    void ChainToUnit(NPC unit)
    {
        
    }

	void OnTriggerEnter(Collider other) {
		if(isDashing) {
            this.gameObject.GetComponents<AudioSource>()[1].Play();
		}
	}

	public bool IsDashing() {
		return isDashing;
	}
}
