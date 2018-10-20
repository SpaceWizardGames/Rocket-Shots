using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

   [SerializeField] float sideThrusters = 200f;
   [SerializeField] float mainThruster = 50f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
		}

    // once per frame
    void Update() {
        Thrust();
        Rotate();	
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly": // safe collision
                break;
            case "Fuel": // collect fuel
                break;
            default: // kill player
                break;
        }
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThruster);
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // manually control rotation

        float rotationThisFrame = sideThrusters * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)== true)
            if (Input.GetKey(KeyCode.A))
            {
            transform.Rotate(Vector3.forward * rotationThisFrame);
            }
             else if (Input.GetKey(KeyCode.D))
             {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
             }

        rigidBody.freezeRotation = false; // physics resume
    }
}

