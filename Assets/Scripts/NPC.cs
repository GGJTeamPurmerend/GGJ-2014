using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    public UnitType UnitType;
    public bool IsSpotted = false;
    public string closest = "";

    public Sprite hostile, defaultSprite;
    public RuntimeAnimatorController red, black;
    // Sprite hostileBody, defaultBody;

    MonoBehaviour realBehaviour;
    Sprite realSprite;

    public GameObject DeadHostileParticleEmitterPrefab;
    public GameObject DeadParticleEmitterForNeutralPrefab;
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(this.transform.position, Player.Instance.transform.position) >= TweakableValues.NPCMaxDistance)
        {
            this.Kill();
        }

    }

    public void Kill()
    {
        if (this.GetComponent<HostileBehaviour>().enabled)
        {
            GameObject obj = (GameObject) Instantiate(DeadHostileParticleEmitterPrefab, this.transform.position, this.transform.rotation);
            
        }
        else if (this.GetComponent<NeutralBehaviour>().enabled)
        {
            GameObject obj = (GameObject)Instantiate(DeadParticleEmitterForNeutralPrefab, this.transform.position, this.transform.rotation);
 
        }



        UnitSpawnManager.Instance.RemoveUnit(this);
        GameObject.Destroy(this.gameObject);
    }
    public void InitializeUnitType(UnitType unitType)
    {
        this.UnitType = unitType;
    }

    public void Initialize(UnitType type)
    {
        UnitType = type;
        realBehaviour = null;
        realBehaviour = this.gameObject.AddComponent<HostileBehaviour>();

        realBehaviour.enabled = false;
        if (Player.Instance.state == UnitType)
        {
            ToggleSpotted(true);
        }

    }

    public void ToggleSpotted(bool isSpotted)
    {
        IsSpotted = isSpotted;
        this.GetComponent<NeutralBehaviour>().enabled = !isSpotted;
        this.GetComponent<HostileBehaviour>().enabled = isSpotted;
        if (isSpotted)
        {
            transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = hostile;
            transform.FindChild("Sprite").GetComponent<Animator>().runtimeAnimatorController = red;
        }
        else
        {
            transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = defaultSprite;
            transform.FindChild("Sprite").GetComponent<Animator>().runtimeAnimatorController = black;
        }
    }

    public void AggroToPlayer()
    {
        if (IsSpotted)
        {
            Debug.LogWarning("AttractToPlayer can't be called when unit is spotted!!");
            return;
        }
        this.GetComponent<NeutralBehaviour>().AggroToPlayer(false);
    }
}
