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
    None, Pre, Dashing
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
    private bool inCooldown = false;

	private float cooldownPeriod = 0.8f;
    private float dashPeriod = 0.3f;

    private int lives = 2;

    TrailRenderer trailRenderer;

    bool leftPressed = false;

	void Awake() {
		Instance = this;
        trailRenderer = this.GetComponent<TrailRenderer>();
	}

	void Start() {
		animator = gameObject.GetComponentInChildren<Animator>();
		UnitSpawnManager.Instance.UpdatePlayerState(state);
        StartCoroutine(CheckForLeftTrigger());
	}

	IEnumerator CheckForLeftTrigger() {
        while (true)
        {
            if (!leftPressed && Input.GetAxis("L Trigger") == 1)
            {
                leftPressed = true;
                ToggleState();
                UnitSpawnManager.Instance.UpdatePlayerState(state);
                this.gameObject.GetComponents<AudioSource>()[2].Play();
                if (chainState != ChainState.None)
                {
                    StartCoroutine(EndedChainStreak());
                }
            }
            else
            {
                leftPressed = false;
            }
            yield return new WaitForSeconds(0.2f);
        }
       
	}

	void ToggleState() {
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
    
	public void StartChain() {
        StartCoroutine(Chaining());

		gameObject.GetComponentInChildren<Animator>().SetBool("stoppedChaining", false);
	}

    IEnumerator Chaining()
    {
       // yield return new WaitForSeconds(0.1f);
        chainState = ChainState.Pre;
        chainCount++;

        float increaser = chainCount * 0.6f;

		int audioCounter = (int)chainCount % 5 + 3;
		if (chainCount >= 4){
			audioCounter = 7;
		}

		this.gameObject.GetComponents<AudioSource>()[audioCounter].Play();

        if (increaser > 6) increaser = 6f;
        List<NPC> unitsInChainRange = UnitSpawnManager.Instance.Units.FindAll(x => x.UnitType == this.state && Vector3.Distance(x.transform.position, playerContainerReference.transform.position) < (7f + increaser));

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
                StartCoroutine(EndedChainStreak(1f));
            }
            yield break;
        }
        trailRenderer.time = 100f;
        PlayerCamera.Instance.IncreaseSize();

        foreach (NPC unit in nearestUnits)
        {
            GameObject arrow = (GameObject)Instantiate(directionArrowPrefab);
            arrow.GetComponent<DirectionArrow>().Initialize(unit);
            arrow.transform.parent = this.playerContainerReference.transform;
            arrow.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    IEnumerator EndedChainStreak(float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        if (chainCount == 0)
        {
            yield break;
        }
        Debug.Log("Chain streak: " + chainCount);
		gameObject.GetComponentInChildren<Animator>().SetBool("stoppedChaining", true);
        chainCount = 0;
        PlayerCamera.Instance.ResetSize();
        chainState = ChainState.None;
        DirectionArrow[] arrows = FindObjectsOfType<DirectionArrow>();
        foreach (DirectionArrow arrow in arrows)
        {
            Destroy(arrow.gameObject);
        }
        trailRenderer.time = -1f;
    }

	// Controller

	void FixedUpdate() {

		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		gameObject.GetComponentInChildren<Animator>().SetBool("moving", false);
        GetChainInput();

        if (chainState == ChainState.Dashing)
        {
            if (currentlyDashingChainedUnit == null)
            {
                return;
            }
            Vector3 direction = currentlyDashingChainedUnit.transform.position - this.playerContainerReference.transform.position;
            Dash(3f, direction);
            this.transform.rotation = Quaternion.LookRotation(direction);
            return;
        }

        if (inCooldown)
        {
            dashPeriod -= Time.deltaTime;
            cooldownPeriod -= Time.deltaTime;
            if (dashPeriod > 0)
            {
                Dash(1f, this.transform.rotation * Vector3.forward);
            }
            else
            {
                isDashing = false;
            }
            if (cooldownPeriod < 0)
            {
                inCooldown = false;
                dashPeriod = 0.3f;
                cooldownPeriod = 0.8f;
            }
            if (dashPeriod < 0 && cooldownPeriod > 0)
            {
                movePlayer(moveHorizontal, moveVertical);
            }
        }
        else if (Input.GetAxis("R Trigger") == 1 || Input.GetKey(KeyCode.Space))
        {
            Dash(1f, this.transform.rotation * Vector3.forward);
            this.gameObject.GetComponents<AudioSource>()[0].Play();
            inCooldown = true;
		}
		else {
            movePlayer(moveHorizontal, moveVertical);
		}
	}

    void movePlayer(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        gameObject.GetComponentInChildren<Animator>().SetBool("dashing", false);
        if (movement.magnitude > 0.25f)
        {
            gameObject.GetComponentInChildren<Animator>().SetBool("moving", true);
            isDashing = false;
            playerContainerReference.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + movement, Time.deltaTime * speed);
            this.transform.rotation = Quaternion.LookRotation(movement);
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
    }

    void GetChainInput()
    {
        float lookHorizontal = Input.GetAxis("RStick X");
        float lookVertical = Input.GetAxis("RStick Y");

        Vector2 pos = new Vector2(lookVertical, lookHorizontal);
        if (chainState == ChainState.Pre)
        {
            if (FindObjectsOfType<DirectionArrow>().Length == 0)
            {
                chainState = ChainState.None;
                StartCoroutine(EndedChainStreak());
                return;
            }

            if (pos.magnitude >= 1f)
            {
				gameObject.GetComponentInChildren<Animator>().SetBool("chaining", true);
                pos *= 2f;
                Vector3 newPos = new Vector3();
                newPos.x = Player.Instance.transform.position.x + pos.x;
                newPos.z = Player.Instance.transform.position.z + -pos.y;

                DirectionArrow[] arrows = FindObjectsOfType<DirectionArrow>();
                bool foundHit = false;
                foreach (DirectionArrow arrow in arrows)
                {
                    if (Vector3.Distance(arrow.transform.GetChild(0).position, newPos) < 2f)
                    {
                        currentlyDashingChainedUnit = arrow.unit;
                        chainState = ChainState.Dashing;
                        foundHit = true;
                    }
                }
                if (foundHit)
                {
                    foreach (DirectionArrow arrow in arrows)
                    {
                        GameObject.Destroy(arrow.gameObject);
                    }
                }
            }
			else {
				gameObject.GetComponentInChildren<Animator>().SetBool("chaining", false);
			}
        }
		else {
			gameObject.GetComponentInChildren<Animator>().SetBool("chaining", false);
		}
    }

    void ChainToUnit(NPC unit)
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponents<AudioSource>()[1].Play();

        if (other.GetComponent<HostileBehaviour>().enabled)
        {
            if (!IsDashing())
            {
                if (!isHit)
                {
                    Damage();
                    other.GetComponent<NPC>().Kill();
                    PlayerCamera.Instance.ShakeCamera();
                }
            }
            else
            {
                if (!isHit)
                {
                    other.GetComponent<NPC>().Kill();
                    Player.Instance.StartChain();
                    PlayerCamera.Instance.ShakeCamera();
                    //  this.GetComponent<NPC>().
                }
            }
        }

        if (other.GetComponent<NeutralBehaviour>().enabled)
        {
            if (chainState == ChainState.Dashing)
            {
                if (!isHit)
                {
                    other.GetComponent<NPC>().Kill();
                    PlayerCamera.Instance.ShakeCamera();
                }
            }
            else if (chainState == ChainState.Pre)
            {

            }
            else
            {
                if (!isHit)
                {
                    Damage();
                    other.GetComponent<NPC>().Kill();
                    PlayerCamera.Instance.ShakeCamera();
                }
            }
        }
    }

	public bool IsDashing() {
		return isDashing;
	}

    public void Damage()
    {
        lives -= 1;
        if (lives <= 0)
        {
            Kill();
        }
        else
        {
            StartCoroutine(restoreLive());
        }
    }

    private float newLiveTime = 2;
    private bool isHit;

    IEnumerator restoreLive()
    {
        isHit = true;
        for (int i = 0; i < 8; i++)
        {
            foreach (SpriteRenderer renderer in this.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.enabled = false;
            }
            yield return new WaitForSeconds(newLiveTime / 16);
            foreach (SpriteRenderer renderer in this.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.enabled = true;
            }
            yield return new WaitForSeconds(newLiveTime / 16);
        }
        isHit = false;
    }

    public void Kill()
    {
        this.gameObject.GetComponents<AudioSource>()[3].Play();
        Application.LoadLevel("Restart");
    }
}
