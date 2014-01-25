using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

    Transform lookAt = null;
    public NPC unit;
    int alpha = 255;
    bool hadLookAt = false;
	// Use this for initialization
	public void Initialize(NPC unit ){
        this.lookAt = unit.transform;
        this.unit = unit;
        StartCoroutine(RemoveAfterTime());
    }

    IEnumerator RemoveAfterTime()
    {
        while(true){
            alpha -= 2;
            this.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
            if (alpha <= 20)
            {
                GameObject.Destroy(this.gameObject);
            }
            yield return new WaitForEndOfFrame();
         }
    }
	
	// Update is called once per frame
	void Update () {
        if (lookAt != null)
        {
            hadLookAt = true;
            this.transform.LookAt(lookAt);
        }
        if (hadLookAt && lookAt == null)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
