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
        if (!this.enabled) return;
        if (other.tag == "Player")
        {
            if (!other.GetComponent<Player>().IsDashing())
            {
                other.gameObject.GetComponent<Player>().Kill();
            }
            else
            {
                this.GetComponent<NPC>().Kill();
                Player.Instance.StartChain();
                PlayerCamera.Instance.ShakeCamera();
              //  this.GetComponent<NPC>().
            }
        }
    }
}
