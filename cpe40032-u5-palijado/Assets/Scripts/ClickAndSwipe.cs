using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;
    private bool swiping = false;

    void Awake()
    {
        // Initializes the components
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        // If the game is not yet over
        if (gameManager.isGameActive)
        {
            // Set swiping to true when the left mouse button is held down
            if (Input.GetMouseButtonDown(0))
            {
                swiping = true;
                UpdateComponents();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                swiping = false;
                UpdateComponents();
            }

            // If swiping is true, we will update the mouse position
            if (swiping)
            {
                UpdateMousePosition();
            }
        }
    }

    // Set up the GameObject to move with the mouse position
    void UpdateMousePosition()
    {
        // ScreenToWorld will convert the screen position of the mouse to a world position
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    // Set the enabled state to whatever the swiping boolean is
    void UpdateComponents()
    {
        trail.enabled = swiping;
        col.enabled = swiping;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When we collide with something, we will check if it’s a target
        if (collision.gameObject.GetComponent<Target>())
        {
            // Destroy the target
            collision.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }
}
