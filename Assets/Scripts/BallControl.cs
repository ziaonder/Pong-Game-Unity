using System;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private float ballVelocityX = 5;
    private float camSizeY, camSizeX;
    public static event Action<string> OnScore;
    private string lastScoredPlayer;
    public static StateMachine stateMachine;
    [SerializeField] private AudioClip goalSound, hitSound;
    private AudioSource audioSource;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        camSizeY = Camera.main.orthographicSize;

        // If the aspect ratio is 16:9 and if we know the cameraSize vertically we can calculate the cameraSize horizontally.
        camSizeX = 16.0f * 5.0f / 9.0f;
    }
    
    void Start()
    {
        stateMachine.ChangeState(StateMachine.states.SET);
        lastScoredPlayer = UnityEngine.Random.Range(0, 2) == 0 ? "left" : "right";
    }

    private void Update()
    {
        // invert the y velocity if the ball is off screen vertically
        if (transform.position.y + transform.localScale.y / 2 > camSizeY && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);

        if (transform.position.y - transform.localScale.y / 2 < -camSizeY && rb.velocity.y < 0)
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        //

        // score if the ball is off screen horizontally
        if (transform.position.x + transform.localScale.x / 2 < -camSizeX)
        {
            SetScoreArrangements("right");
            lastScoredPlayer = "right";
            audioSource.clip = goalSound;
            audioSource.Play();
        }

        if (transform.position.x - transform.localScale.x / 2 > camSizeX)
        {
            SetScoreArrangements("left");
            lastScoredPlayer = "left";
            audioSource.clip = goalSound;
            audioSource.Play();
        }

        if (stateMachine.GetCurrentState() == StateMachine.states.SET && Input.GetKeyDown(KeyCode.Return))
        {
            stateMachine.ChangeState(StateMachine.states.STARTED);
        }

        if(stateMachine.GetCurrentState() == StateMachine.states.STARTED && rb.velocity.x == 0)
        {
            rb.velocity = new Vector2(lastScoredPlayer == "left" ? -ballVelocityX : ballVelocityX, UnityEngine.Random.Range(-1.5f, 1.5f));
        }

        if(stateMachine.GetCurrentState() == StateMachine.states.ENDED && Input.GetKeyDown(KeyCode.Return))
        {
            stateMachine.ChangeState(StateMachine.states.SET);
            ScoreManager.ResetScore();
            OnScore?.Invoke("reset");
        }
    }
     
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Right"))
        {
            if(transform.position.y > collision.gameObject.transform.position.y)
                rb.velocity = new Vector2(-ballVelocityX, 1.5f * Vector3.Distance(transform.position, collision.gameObject.transform.position));
            else
                rb.velocity = new Vector2(-ballVelocityX, 1.5f * -Vector3.Distance(transform.position, collision.gameObject.transform.position));

            audioSource.clip = hitSound;
            audioSource.Play();
        }
        else
        {
            if (transform.position.y > collision.gameObject.transform.position.y)
                rb.velocity = new Vector2(ballVelocityX, 1.5f * Vector3.Distance(transform.position, collision.gameObject.transform.position));
            else
                rb.velocity = new Vector2(ballVelocityX, 1.5f * -Vector3.Distance(transform.position, collision.gameObject.transform.position));

            audioSource.clip = hitSound;
            audioSource.Play();
        }
    }

    private void SetScoreArrangements(string scoredPlayer)
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        OnScore?.Invoke(scoredPlayer);
        if (ScoreManager.GetScore(scoredPlayer) >= 5)
            stateMachine.ChangeState(StateMachine.states.ENDED);
    }
}
