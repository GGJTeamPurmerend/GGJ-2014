using UnityEngine;
using System.Collections;

public class TweakableValues : MonoBehaviour {
    // Units
    public static int MinimumUnits = 50;
    public static int MinimumUnitDistanceToPlayer = 15;
    public static float UnitHostileRatio = 0.5f;
    public static float PlayerAttractsUnitRatio = 1f;
    public static float BluffAggroPlayerDistance = 2f;
    public static float NPCMaxDistance = MinimumUnitDistanceToPlayer * 2;
    public static float NeutralNPCSpeed = 5f;
}
