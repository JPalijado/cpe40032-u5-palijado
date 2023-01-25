using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    public GameObject pauseScreen;

    private float spawnRate = 1.0f;
    private int score;
    private int lives;
    private bool paused;
    private bool gameOverPlayed = false;

    public bool isGameActive;

    public AudioClip swooshSound;
    public AudioClip gameOverSound;
    public AudioClip slashSound;
    public AudioClip explosionSound;
    public AudioClip deductSound;
    private AudioSource gameAudio;

    void Start()
    {
        // Gets the audio source
        gameAudio = GetComponent<AudioSource>();
    }

    public void StartGame(int difficulty)
    {
        // Disables the title screen
        titleScreen.gameObject.SetActive(false);

        // Set the game as active at start
        isGameActive = true;

        // Reduces the spawn interval depending on the difficulty
        spawnRate /= difficulty;

        // Spawns the targets
        StartCoroutine(SpawnTarget());

        // Set the inital score to 0 and lives to 3
        UpdateScore(0);
        UpdateLives(3);
    }

    void ChangePaused()
    {
        if (!paused)
        {
            paused = true;
            // Enables the pause screen
            pauseScreen.SetActive(true);
            // Sets the time scale to 0 to make physics calculations paused
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            // Disables the pause screen
            pauseScreen.SetActive(false);
            // Continue physics calculations 
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Set the game as over if the score is negative
        if (score < 0 && !gameOverPlayed)
        {
            // Runs the gameover method
            GameOver();
            gameOverPlayed = true;
        }

        //Check if the user has pressed the P key
        if (Input.GetKeyDown(KeyCode.P) && isGameActive)
        {
            ChangePaused();
            // Pause the audio source
            gameAudio.Pause();
        }
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            // Waits for a couple of seconds before executing the command
            yield return new WaitForSeconds(spawnRate);
            // Spawns the random target
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
            // Plays the swoosh sound 
            gameAudio.PlayOneShot(swooshSound, 0.5f);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        // Adds score to the current score
        score += scoreToAdd;
        // Displays the current score   
        scoreText.text = "Score: " + score;
        gameAudio.PlayOneShot(slashSound, 3.0f);
    }

    public void GameOver()
    {
        // Displays the gameover text and restart button
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        // Set the gameactive as false
        isGameActive = false;
        // Plays the gameover sound
        gameAudio.PlayOneShot(gameOverSound, 5.0f);
    }

    public void RestartGame()
    {
        // Plays the game from the beginning
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateLives(int livesToChange)
    {
        // Increments the current lives
        lives += livesToChange;

        // Displays the current lives
        livesText.text = "Lives: " + lives;

        // Runs the gameover method if the current life is less than or equal to 0
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void HitBadTarget()
    {
        // Reduce the remaining lives
        UpdateLives(-1);
        // Play explosion sound
        gameAudio.PlayOneShot(explosionSound, 5.0f);
    }

    public void ReduceLives()
    {
        // Reduce the remaining lives
        UpdateLives(-1);
        // Play explosion sound
        gameAudio.PlayOneShot(deductSound, 0.5f);
    }
}
