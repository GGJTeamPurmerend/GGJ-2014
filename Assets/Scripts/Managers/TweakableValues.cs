using UnityEngine;
using System.Collections;

public class TweakableValues : MonoBehaviour {
    // Units
    public static int MinimumUnits = 50;
    public static int MinimumUnitDistanceToPlayer = 15;
    public static float UnitHostileRatio = 0.5f; // 50%
    public static float PlayerAttractsUnitRatio = 2f;
    public static float BluffAggroPlayerDistance = 3f;
    public static float NPCMaxDistance = MinimumUnitDistanceToPlayer * 2;
    public static float NeutralNPCSpeed = 2f;

	// Unit separation
	public static float separationRange = 15;
	public static float separationWeight = 0.6f;
	public static float targetWeight = 0.4f;
}
