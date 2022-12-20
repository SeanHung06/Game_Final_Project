using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject boat;
    public float velocity;
    Vector3 targetPosition;
    public float walking_velocity;
    private bool gotDestition;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 0.0f;
        targetPosition = new Vector3(1.11f, 0f, -32.74f);
        walking_velocity = 1.5f;
        gotDestition = false;
    }

    // Update is called once per frame
    void Update()
    {
        // boat gets to destination
        if (Vector3.Distance(boat.transform.position, targetPosition) < Vector3.kEpsilon)
        {
            gotDestition = true;
        }
        // boat hasn't get to destination
        else
        {
            gotDestition = false;
            transform.position = boat.transform.position;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0.0f, -0.1f, 0.0f));
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0.0f, 0.1f, 0.0f));
            }
        }
        if (gotDestition)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                //Debug.Log("TEST1");
                if (velocity <= walking_velocity)
                {
                    velocity += 0.5f;
                }
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (velocity >= -walking_velocity)
                {
                    velocity -= 0.5f;
                }
            }
            else
            {
                velocity = 0.0f;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0.0f, -0.1f, 0.0f));
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0.0f, 0.1f, 0.0f));
            }
            float xdirection_1 = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
            float zdirection_1 = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

            transform.position = new Vector3(transform.position.x + velocity * xdirection_1 * Time.deltaTime, 0.0f, transform.position.z + velocity * zdirection_1 * Time.deltaTime);

        }
    }
}
