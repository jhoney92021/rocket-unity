using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 15f;
    [SerializeField] float loadDelay = 2f;

    [SerializeField] int health = 100;
    [SerializeField] int fuel = 1000;
    [SerializeField] int fuelCost = 1;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip win;

    [SerializeField] ParticleSystem deathParticleSystem;
    [SerializeField] ParticleSystem winParticleSystem;

    private int currentLevelIdx = 0;

    enum KeyAction
    {
        Thrust = KeyCode.Space,
        RotateLeft = KeyCode.A,
        RotateRight = KeyCode.D
    }

    enum PlayerState
    {
        Alive,
        Dying,
        Trancending
    }

    PlayerState playerState = PlayerState.Alive;

    KeyAction action;
    
    Rigidbody rigidBody;
    AudioSource audioSource;
    ParticleSystem thrustParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        thrustParticleSystem = GetComponentInChildren<ParticleSystem>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState == PlayerState.Alive)
        {
            ProcessInput();
        }
        print($"Level:  {currentLevelIdx}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerState != PlayerState.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "LandingPad":
                StartSuccessSequence();
                break;
            case "Friendly":
                print("FRIENDS");
                break;
            case "Fuel":
                print("Fuel Added");
                fuel += 10;
                break;
            default:
                TakeDamage();
                break;
        }

        CheckPlayerHealth();
    }

    private void TakeDamage()
    {
        health /= 2;
        print($"Damage Taken {health} remaining");
    }

    private void CheckPlayerHealth()
    {
        if (health < 1 || fuel < 1)
        {
            playerState = PlayerState.Dying;
            currentLevelIdx = 0;
            audioSource.PlayOneShot(death);
            deathParticleSystem.Play();
            print($"YOU DIED HEALTH {health} FUEL {fuel}");
            Invoke("LoadNextScene", loadDelay);//TODO: parameterize time
        }
    }

    private void StartSuccessSequence()
    {
        playerState = PlayerState.Trancending;
        currentLevelIdx += 1;
        audioSource.PlayOneShot(win);
        winParticleSystem.Play();
        Invoke("LoadNextScene", loadDelay);//TODO: parameterize time
        print("WIN!!!");
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(currentLevelIdx);
    }

    private void ProcessInput()
    {
        OnInputShipThrust();
        OnInputRotateShip();
    }
    private void OnInputShipThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            action = (KeyAction)KeyCode.Space;
            print($"{action}");
            Thrust();
        }
        else
        {
            StopThrust();
        }

    }
    private void Thrust()
    {
        if (fuel < 1) { print("Out Of Fuel"); return; }
        float thrustPerFrame = 3 * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        fuel -= fuelCost;
        //print($"Fuel:{fuel}");
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!thrustParticleSystem.isPlaying)
        {
            thrustParticleSystem.Play();
        }
    }
    private void StopThrust()
    {
        audioSource.Stop();
        thrustParticleSystem.Stop();
    }
    private void OnInputRotateShip()
    {
        rigidBody.freezeRotation = true;//manual rotation

        float roationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            action = (KeyAction)KeyCode.A;
            print($"{action}");
            transform.Rotate(Vector3.left * roationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            action = (KeyAction)KeyCode.D;
            print($"{action}");
            transform.Rotate(Vector3.right * roationThisFrame);
        }
        else
        {

        }
    }
}
