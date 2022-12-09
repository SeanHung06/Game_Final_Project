using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueShow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private Animator animation_controller;

    public float speed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2f)
        {
            DialoguePanel.SetActive(true);
            // Determine which direction to rotate towards
            Vector3 targetDirection = player.transform.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

        }
        else
        {
            DialoguePanel.SetActive(false);
        }
    }
}
