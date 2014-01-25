using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class Controller : MonoBehaviour
{

    public float speed;
    public int dashDistance;
    public Boundary boundary;

    private bool isDashingPressed = false;

    private float deltaTime = 0;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float lookHorizontal = Input.GetAxis("RStick X");
        float lookVertical = Input.GetAxis("RStick Y");

        Vector3 lookDirection = new Vector3(lookHorizontal, 0.0f, lookVertical);

        rigidbody.velocity = Vector3.zero;

        if (lookDirection.magnitude > 0.9 || isDashingPressed == true && deltaTime > 0)
        {
            if (isDashingPressed == false)
            {
                deltaTime += 0.25f;
            }
            isDashingPressed = true;
            //Vector3 rotatedVector = this.transform.rotation * Vector3.forward;
            lookDirection.Normalize();

            rigidbody.velocity = lookDirection * dashDistance;

            deltaTime -= Time.deltaTime;
        }
        else
        {
            isDashingPressed = false;
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            if (movement.magnitude > 0.25)
            {
                rigidbody.velocity += movement * speed;
                this.transform.rotation = Quaternion.LookRotation(movement);
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }
        }

        rigidbody.position = new Vector3(
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
        );
    }
}
