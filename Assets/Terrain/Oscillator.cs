using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    const float tau = Mathf.PI * 2;//about 6.28

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    float movementFactor;

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveObstacles();
    }

    private void MoveObstacles()
    {
        if (period > 0)
        {

            float cycles = Time.time / period;//grows from zero

            float rawSinWave = Mathf.Sin(cycles * tau);
            movementFactor = rawSinWave / 2f + 0.5f;
            print(rawSinWave);

            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPosition + offset;
        }
    }
}
