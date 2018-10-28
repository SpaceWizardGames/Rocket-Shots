using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
   [SerializeField] float sideThrusters = 200f;
   [SerializeField] float mainThruster = 50f;
   [SerializeField] AudioClip levelComplete;
   [SerializeField] AudioClip death;
   [SerializeField] AudioClip mainEngine;


    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Advancing }
    State state = State.Alive;

    // initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
	}

     void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) // ignore collisions after death
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly": // safe collision
                break;
            case "Finish": // advance to next level
                state = State.Advancing;
                audioSource.Stop();
                audioSource.PlayOneShot(levelComplete);
                Invoke("LoadNextScene", 3f); // wait period before next level
                break;
            default: // kill player
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                Invoke("RestartGame", 2f); // wait period before restart
                break;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThruster);
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
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

