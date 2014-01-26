using UnityEngine;
using System.Collections;

public class DeadParticleEmitter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterTime());
	}

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
