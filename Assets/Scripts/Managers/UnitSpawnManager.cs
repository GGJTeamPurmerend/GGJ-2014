﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSpawnManager : MonoBehaviour {

    public static UnitSpawnManager Instance;

    public List<NPC> Units = new List<NPC>();

    public NPC CubeUnitPrefab;
    public NPC CircleUnitPrefab;

    public void UpdatePlayerState(UnitType playerState)
    {
        foreach (NPC npc in Units)
        {
            npc.ToggleSpotted(npc.UnitType == playerState);
        }
    }

    public void RemoveUnit(NPC unit)
    {
        Units.Remove(unit);
    }


    void Awake()
    {
        Instance = this;
    }

	void Start () {
        StartCoroutine(SpawnUnits());
        StartCoroutine(AttractUnitToPlayer());
	}
	
    IEnumerator SpawnUnits()
    {
        while (true)
        {
            if (Units.Count < TweakableValues.MinimumUnits)
            {
                SpawnUnit();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AttractUnitToPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(TweakableValues.PlayerAttractsUnitRatio);
            List<NPC> unspottedUnits = Units.FindAll(x => !x.IsSpotted && Vector2.Distance(x.transform.position, Player.Instance.transform.position) < TweakableValues.MinimumUnitDistanceToPlayer);
            if (unspottedUnits.Count > 0)
            {
                unspottedUnits[Random.Range(0, unspottedUnits.Count - 1)].AggroToPlayer();
            }
        }
    }

    void SpawnUnit()
    {
        float range = TweakableValues.MinimumUnitDistanceToPlayer;

        // get a random direction (360°) in radians
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        // create a vector with length 1.0
        Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        // scale it to the desired length
        v *= (range + Random.Range(1f, 30f));

        v += Player.Instance.transform.position;

        NPC unit = null;
        if (Random.value > 0.5f)
        {
            unit = (NPC)Instantiate(CubeUnitPrefab, v, Quaternion.identity);
            unit.gameObject.AddComponent<NeutralBehaviour>();
            unit.Initialize(UnitType.Cube);
        }
        else
        {
            unit = (NPC)Instantiate(CircleUnitPrefab, v, Quaternion.identity);
            unit.gameObject.AddComponent<NeutralBehaviour>();
            unit.Initialize(UnitType.Circle);
        }
        Units.Add(unit);
    }
}
