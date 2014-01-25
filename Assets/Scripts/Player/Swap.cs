using UnityEngine;
using System.Collections;

public enum State {
	Cube, Cylinder
};

public class Swap : MonoBehaviour {
	[SerializeField]GameObject cubeReference;
	[SerializeField]GameObject cylinderReference;

	GameObject currentStateObject;
	State state = State.Cube;

	void Start() {
		currentStateObject = cubeReference;
	}

	void Update() {
		switch(Input.inputString) {
			case "1":
				state = State.Cube;
				SetActiveObject(cubeReference);
				break;
			case "2":
				state = State.Cylinder;
				SetActiveObject(cylinderReference);
				break;
		}
	}

	void SetActiveObject(GameObject obj) {
		DeActivateCurrentObject();
		obj.SetActive(true);
		currentStateObject = obj;
	}

	void DeActivateCurrentObject() {
		if(currentStateObject != null) {
			currentStateObject.SetActive(false);
		}
	}

	public State getState() {
		return state;
	}
}
