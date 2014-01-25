using UnityEngine;
using System.Collections;

public class HostileBehaviour : NeutralBehaviour
{

    void OnEnable()
    {
        AggroToPlayer(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponent<Controller>().getDashing())
            {
                other.gameObject.GetComponent<Player>().kill();
            }
        }
    }
}
