using UnityEngine;
using System.Collections;

public class HostileBehaviour : NeutralBehaviour
{


    void OnEnable()
    {
        AggroToPlayer(false);
    }
}
