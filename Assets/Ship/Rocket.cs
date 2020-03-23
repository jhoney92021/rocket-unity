using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 15f;
    [SerializeField] int health = 100;
    [SerializeField] int fuel = 1000;
    [SerializeField] int fuelCost = 1;
    private int levelStart = 0;

    enum KeyAction
    {
        Thrust = KeyCode.Space,
        RotateLeft = KeyCode.A,
        RotateRight = KeyCode.D

    }

    KeyAction action;
    ParticleSystem particleSystem;
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {        
        particleSystem = GetComponent<ParticleSystem>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "LandingPad":
                levelStart++;
                SceneManager.LoadScene(levelStart);
                print("WIN!!!");
                break;
            case "Friendly":
                print("FRIENDS");
                break;
            case "Fuel":
                print("Fuel Added");
                fuel += 10;
                break;
            default:
                health = 0;
                levelStart = 0;
                print($"Damage Taken {health} remaining");
                break;
        }

        if (health < 1)
        {            
            print("YOU DIED");
            SceneManager.LoadScene(levelStart);
        }
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
        print($"Fuel:{fuel}");
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }
    private void StopThrust()
    {
        audioSource.Stop();
        particleSystem.Stop();
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
