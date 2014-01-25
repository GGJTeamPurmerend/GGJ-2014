using UnityEngine;
using System.Collections;

public enum AIType { Friendly, Hostile };

public class NPC : MonoBehaviour {

    public AIType AIType;
    public UnitType UnitType;

    public Material friendly, hostile;
    Material defaultMaterial;

    bool hasBeenSpottedBefore = false;
    MonoBehaviour realBehaviour;
    Material realMaterial;

	// Use this for initialization
	void Start () {
        defaultMaterial = this.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
       
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
        realBehaviour.enabled = false;
    }

    public void ToggleSpotted(bool isSpotted)
    {
        if (isSpotted && !hasBeenSpottedBefore)
        {
            hasBeenSpottedBefore = true;
            realBehaviour.enabled = true;
            this.GetComponent<NeutralBehaviour>().enabled = false;
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

    public void BlufPlayer()
    {

    }
}
