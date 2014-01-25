using UnityEngine;
using System.Collections;

public enum AIType { Friendly, Hostile };

public class NPC : MonoBehaviour {

    public AIType AIType;
    public UnitType UnitType;
    public bool IsSpotted = false;

    public Material friendly, hostile;
    Material defaultMaterial;

    bool hasBeenSpottedBefore = false;
    MonoBehaviour realBehaviour;
    Material realMaterial;
	
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
                realMaterial = friendly;
                break;
            case AIType.Hostile:
                realBehaviour = this.gameObject.AddComponent<HostileBehaviour>();
                realMaterial = hostile;
                break;
        }

        defaultMaterial = this.GetComponent<MeshRenderer>().material;

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
            this.GetComponent<MeshRenderer>().material = realMaterial;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

    public void AggroToPlayer()
    {
        Debug.Log("ISAGGROING");
        if (IsSpotted)
        {
            Debug.LogWarning("AttractToPlayer can't be called when unit is spotted!!");
            return;
        }
        bool aggroIsFake = AIType == AIType.Friendly;
        this.GetComponent<NeutralBehaviour>().AggroToPlayer(aggroIsFake);
    }
}
