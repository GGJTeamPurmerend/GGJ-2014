﻿using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    public UnitType UnitType;
    public bool IsSpotted = false;
    public string closest = "";

    public Sprite hostile;
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

    public void Initialize(UnitType type)
    {
        UnitType = type;
        realBehaviour = null;
        realBehaviour = this.gameObject.AddComponent<HostileBehaviour>();
        realSprite = hostile;
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
        this.GetComponent<NeutralBehaviour>().enabled = !isSpotted;
        this.GetComponent<HostileBehaviour>().enabled = isSpotted;
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
        this.GetComponent<NeutralBehaviour>().AggroToPlayer(false);
    }

    public void MakeMaterialVisible()
    {
        this.GetComponentInChildren<SpriteRenderer>().sprite = realSprite;
    }
}
