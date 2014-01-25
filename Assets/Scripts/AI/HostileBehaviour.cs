using UnityEngine;
using System.Collections;

public class HostileBehaviour : NeutralBehaviour
{
    bool isAggroing = false;
    override public void Update()
    {
        base.Update();
        if (!isAggroing && Vector2.Distance(this.transform.position, Player.Instance.transform.position) < TweakableValues.MinimumUnitDistanceToPlayer/2f)
        {
            AggroToPlayer(false);
        }
        
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
