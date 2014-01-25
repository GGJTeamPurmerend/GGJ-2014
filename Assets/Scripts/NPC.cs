using UnityEngine;
using System.Collections;

public enum AIType { Friendly, Hostile };

public class NPC : MonoBehaviour {

    public AIType AIType;
    public UnitType UnitType;
    public bool IsSpotted = false;
    public string closest = "";

    public Sprite friendly, hostile;
    Sprite defaultSprite;

    bool hasBeenSpottedBefore = false;
    MonoBehaviour realBehaviour;
    Sprite realSprite;
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(this.transform.position, Player.Instance.transform.position) >= TweakableValues.NPCMaxDistance)
        {
            this.Kill();
        }
    }

    public void Kill()
    {
        UnitSpawnManager.Instance.RemoveUnit(this);
        GameObject.Destroy(this.gameObject);
    }
    public void InitializeUnitType(UnitType unitType)
    {
        this.UnitType = unitType;
    }

    public void InitializeAIType(AIType aiType)
    {
        this.AIType = aiType;
        realBehaviour = null;
        switch (aiType)
        {
            case AIType.Friendly:
                realBehaviour = this.gameObject.AddComponent<FriendlyBehaviour>();
                realSprite = friendly;
                break;
            case AIType.Hostile:
                realBehaviour = this.gameObject.AddComponent<HostileBehaviour>();
                realSprite = hostile;
                break;
        }

        defaultSprite = this.GetComponentInChildren<SpriteRenderer>().sprite;

        realBehaviour.enabled = false;
        if (Player.Instance.state == UnitType)
        {
            ToggleSpotted(true);
        }

    }

    public void ToggleSpotted(bool isSpotted)
    {
        IsSpotted = isSpotted;
        if (isSpotted && !hasBeenSpottedBefore)
        {
            hasBeenSpottedBefore = true;
            realBehaviour.enabled = true;
            this.GetComponent<NeutralBehaviour>().enabled = false;
            realBehaviour.enabled = true;
        }
        if (isSpotted)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = realSprite;
        }
        else
        {
			this.GetComponentInChildren<SpriteRenderer>().sprite = defaultSprite;
        }
    }

    public void AggroToPlayer()
    {
        if (IsSpotted)
        {
            Debug.LogWarning("AttractToPlayer can't be called when unit is spotted!!");
            return;
        }
        bool aggroIsFake = AIType == AIType.Friendly;
        this.GetComponent<NeutralBehaviour>().AggroToPlayer(aggroIsFake);
    }

    public void MakeMaterialVisible()
    {
        this.GetComponentInChildren<SpriteRenderer>().sprite = realSprite;
    }
}
