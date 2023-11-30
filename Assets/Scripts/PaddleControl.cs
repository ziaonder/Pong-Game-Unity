using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControl : MonoBehaviour
{
    private string paddlePosition;
    private int moveValue = 5;
    private float cameraSize;
    private float paddleScaleY;

    private void Awake()
    {
        cameraSize = Camera.main.orthographicSize;
        paddleScaleY = transform.localScale.y;
        
        if (gameObject.name.Contains("Left"))
            paddlePosition = "left";
        else 
            paddlePosition = "right";
    }

    void Update()
    {
        if(BallControl.stateMachine.GetCurrentState() == StateMachine.states.STARTED)
        {
            // The last expression in these if blocks is to check whether the paddle off screen.
            if (paddlePosition == "left" && Input.GetKey(KeyCode.W) && transform.position.y < cameraSize - paddleScaleY / 2)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveValue * Time.deltaTime);

            if (paddlePosition == "left" && Input.GetKey(KeyCode.S) && transform.position.y > -cameraSize + paddleScaleY / 2)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - moveValue * Time.deltaTime);

            if (paddlePosition == "right" && Input.GetKey(KeyCode.UpArrow) && transform.position.y < cameraSize - paddleScaleY / 2)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveValue * Time.deltaTime);

            if (paddlePosition == "right" && Input.GetKey(KeyCode.DownArrow) && transform.position.y > -cameraSize + paddleScaleY / 2)
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - moveValue * Time.deltaTime);
        }

        if (BallControl.stateMachine.GetCurrentState() == StateMachine.states.SET && transform.position.y != 0)
            transform.position = new Vector2(transform.position.x, 0);
    }
}
