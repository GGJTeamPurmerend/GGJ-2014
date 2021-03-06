﻿using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public static PlayerCamera Instance;

    public GameObject CameraHolder;

    public float size;

    float defaultSize;

    void Awake()
    {
        size = defaultSize = Camera.main.orthographicSize;
        Instance = this;
    }
	// Use this for initialization
	void Start () {
       // iTween.ValueTo(this.gameObject, iTween.Hash("from", 9f, "to", 10f, "easetype", "easeOutElastic", "time", 0.8f, "onupdate", "OnUpdate"));
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 pos = this.transform.position;

        pos.x = playerPos.x;
        pos.z = playerPos.z;

        this.transform.position = pos;

        
	}

    void OnUpdate(float value)
    {
        Camera.main.orthographicSize = value;
    }

    bool IsBusy = false;

    public void ShakeCamera()
    {
        iTween.ShakePosition(CameraHolder, new Vector3(0.5f, 0.5f, 0.5f), 0.2f);
    }

    public void IncreaseSize()
    {
        if (IsBusy) return;
        float prevSize = size;
        size += 0.9f;
        if(size >= 9f)
            size = 9f;
        iTween.ValueTo(this.gameObject, iTween.Hash("from", prevSize, "to", size, "easetype", "easeOutElastic", "time", 0.8f, "onupdate", "OnUpdate"));
    }

    public void ResetSize()
    {
        iTween.ValueTo(this.gameObject, iTween.Hash("from", size, "to", defaultSize, "easetype", "easeOutSine", "time", 0.8f, "onupdate", "OnUpdate"));
        size = defaultSize;
        IsBusy = true;
        StartCoroutine(EnableAgain());
    }

    IEnumerator EnableAgain()
    {
        yield return new WaitForSeconds(0.5f);
        IsBusy = false;
    }

}
