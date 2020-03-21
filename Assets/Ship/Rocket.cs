using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;

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
        float thrustPerFrame = 3 * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * 3);
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
