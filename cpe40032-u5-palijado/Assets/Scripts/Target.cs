using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;

    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -3;

    private GameManager gameManager;
    public int pointValue;
    public bool isBad = false;
    public bool hitBadTarget = false;
    public ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        // Adds force and torque upong spawning
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        // Spawns at a random position
        transform.position = RandomSpawnPos();
        // Finds game manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    Vector3 RandomForce()
    {
        // Generate random upward force
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        // Generate random torque value
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        // Generate a random spawn position
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Destroys the game object if it is not on the view anymore
        Destroy(gameObject);

        // Reduces the current lives by 1 if the player was not able to slice the target
        if (!gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            gameManager.ReduceLives();
        }
    }

    public void DestroyTarget()
    {
        // Only allows the player to click the targets if the game is still active
        if (gameManager.isGameActive)
        {
            // Destroys the target gameobject
            Destroy(gameObject);
            // Displays a particle effect upon clicking the target
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

            // If the target is bad and the player hit it
            if (isBad)
            {
                gameManager.HitBadTarget();
            }
            else
            {
                // Updates the current score depending the target's point value
                gameManager.UpdateScore(pointValue);
            }
        }
    }
}
