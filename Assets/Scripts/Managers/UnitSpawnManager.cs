using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSpawnManager : MonoBehaviour {

    public static UnitSpawnManager Instance;

    public int mininumUnits = 50;
    public float hostileRatio = 0.5f;

    List<NPC> units = new List<NPC>();

    public NPC CubeUnitPrefab;
    public NPC CircleUnitPrefab;

    public void UpdatePlayerState(UnitType playerState)
    {
        foreach (NPC npc in units)
        {
            npc.ToggleSpotted(npc.UnitType == playerState);
        }
    }


    void Awake()
    {
        Instance = this;
    }

	void Start () {
        StartCoroutine(SpawnUnits());
	}
	
    IEnumerator SpawnUnits()
    {
        while (true)
        {
            if (units.Count < mininumUnits)
            {
                SpawnUnit();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void SpawnUnit()
    {
        float range = 15f;

        // get a random direction (360°) in radians
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        // create a vector with length 1.0
        Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        // scale it to the desired length
        v *= (range + Random.Range(1f, 10f));

        v += Player.Instance.transform.position;

        NPC unit = null;
        if (Random.value > 0.5f)
        {
            unit = (NPC)Instantiate(CubeUnitPrefab, v, Quaternion.identity);
            unit.InitializeUnitType(UnitType.Cube);
        }
        else
        {
            unit = (NPC)Instantiate(CircleUnitPrefab, v, Quaternion.identity);
            unit.InitializeUnitType(UnitType.Circle);
        }
        units.Add(unit);
        Debug.Log(unit);
        if (Random.value > hostileRatio)
        {
            unit.InitializeAIType(AIType.Friendly);
        }
        else
        {
            unit.InitializeAIType(AIType.Hostile);
        }
        unit.gameObject.AddComponent<NeutralBehaviour>();
    }
}
