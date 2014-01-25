using UnityEngine;
using System.Collections;

public enum UnitType
{
    Cube, Circle
};

public class Player : MonoBehaviour {

    public static Player Instance;

    public UnitType state = UnitType.Cube;

    [SerializeField]
    GameObject cubeReference;
    [SerializeField]
    GameObject cylinderReference;

    GameObject currentStateObject;
   

    void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        currentStateObject = cubeReference;
        UnitSpawnManager.Instance.UpdatePlayerState(state);
    }

    void Update()
    {
        if (Input.GetButtonDown("X Button"))
        {
            switch (state) {
                case UnitType.Circle:
                    state = UnitType.Cube;
                    SetActiveObject(cubeReference);
                    break;
                case UnitType.Cube:
                    state = UnitType.Circle;
                    SetActiveObject(cylinderReference);
                    break;
            }
            UnitSpawnManager.Instance.UpdatePlayerState(state);
        }
    }

    void SetActiveObject(GameObject obj)
    {
        DeActivateCurrentObject();
        obj.SetActive(true);
        currentStateObject = obj;
    }

    void DeActivateCurrentObject()
    {
        if (currentStateObject != null)
        {
            currentStateObject.SetActive(false);
        }
    }

    public UnitType getState()
    {
        return state;
    }
	
}
