using UnityEngine;
using System.Collections;

public enum UnitType
{
    Cube, Circle
};

public class Player : MonoBehaviour {

    public static Player Instance;

    [SerializeField]
    GameObject cubeReference;
    [SerializeField]
    GameObject cylinderReference;

    GameObject currentStateObject;
    UnitType state = UnitType.Cube;

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
        if ((Input.inputString != ""))
        {
            switch (Input.inputString)
            {
                case "1":
                    state = UnitType.Cube;
                    SetActiveObject(cubeReference);
                    break;
                case "2":
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
