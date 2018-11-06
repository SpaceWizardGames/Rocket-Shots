using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
   [SerializeField] float sideThrusters = 200f;
   [SerializeField] float mainThruster = 50f;
   [SerializeField]float SceneLoadDelay = 3f;

   [SerializeField] AudioClip levelComplete;
   [SerializeField] AudioClip death;
   [SerializeField] AudioClip mainEngine;

   [SerializeField] ParticleSystem levelCompleteParticles;
   [SerializeField] ParticleSystem deathParticles;
   [SerializeField] ParticleSystem mainEngineParticles;

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
        // todo: allow to work only in debug mode
        RespondToDebugKeys();
	}

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown
        {
            LoadNextScene();
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
                BeginAdvancing();
                break;
            default: // kill player
                BeginDeath();
                break;
        }
    }
    
    private void BeginAdvancing()
    {
        state = State.Advancing;
        audioSource.Stop();
        audioSource.PlayOneShot(levelComplete, 0.4f);
        levelCompleteParticles.Play();
        mainEngineParticles.Stop();
        Invoke("LoadNextScene", SceneLoadDelay); // wait period before next level

    }

    private void BeginDeath()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        mainEngineParticles.Stop();
        Invoke("RestartGame", SceneLoadDelay); // wait period before restart
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
            mainEngineParticles.Stop();
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

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThruster * Time.deltaTime);
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}

