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
    public Boundary boundary;

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (movement.magnitude > 0.25)
        {
            rigidbody.velocity = movement * speed;
            this.transform.rotation = Quaternion.LookRotation(movement);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }

        rigidbody.position = new Vector3(
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
        );
    }
}
