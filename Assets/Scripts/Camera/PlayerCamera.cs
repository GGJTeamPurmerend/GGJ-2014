using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public static PlayerCamera Instance;

    public GameObject CameraHolder;

    void Awake()
    {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 pos = this.transform.position;

        pos.x = playerPos.x;
        pos.z = playerPos.z;

        this.transform.position = pos;
	}

    public void ShakeCamera()
    {
        iTween.ShakePosition(CameraHolder, new Vector3(0.5f, 0.5f, 0.5f), 0.2f);
     //   iTween.ShakePosition(CameraHolder, iTween.Hash("z", -1f, "time", 1.0f));
    }

}
