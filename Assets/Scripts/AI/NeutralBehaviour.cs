using UnityEngine;
using System.Collections;

public class NeutralBehaviour : MonoBehaviour {
    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;


    Transform target;
    float heading;
    Vector3 targetRotation;

	void Start () {
        target = Player.Instance.transform;
	}

    void Awake()
    {
        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        this.rigidbody.velocity = forward * speed;
    }

    /// <summary>
    /// Repeatedly calculates a new direction to move towards.
    /// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
    /// </summary>
    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /// <summary>
    /// Calculates a new direction to move towards.
    /// </summary>
    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }

    /*// Update is called once per frame
void Update () {
    transform.rotation = Quaternion.Slerp(transform.rotation,
    Quaternion.LookRotation(target.position - transform.position), rotationSpeed * Time.deltaTime);

    this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
}*/
}
