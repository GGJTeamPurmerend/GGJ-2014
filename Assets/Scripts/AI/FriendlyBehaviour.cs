using UnityEngine;
using System.Collections;

public class FriendlyBehaviour : NeutralBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
          //  PlayerCamera.Instance.ShakeCamera();
        }

    }
}
