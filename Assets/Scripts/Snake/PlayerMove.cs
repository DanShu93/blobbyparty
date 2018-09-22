using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Team team;

    SpriteRenderer rend;

    bool inGame = false;

    Vector3 inputVector;
    Vector3 targetMove;
    Vector3 moveVector;

    float inputSqrMagnitude;

    float inputDeadzone = 0.1f;
    float sqrInputDeadzone;


    float minInput = 0.5f;
    float sqrMinSpeed;

    float steerSpeed = 5f;

    float speed = 0.05f;

    //The minimum result of DotProduct between current moveDir and desired MoveDir. 
    //Makes it impossible to immediately turn around
    float dotLimit = -0.5f;

    public float dot;

    Vector3 ScreenSize;

    Rigidbody2D rb;

    float blinkTimer;
    float blinkInterval = 1f;
    bool blinked = false;

    public float debugColor;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        team = GetComponent<Obstacle>().team;
        rend = GetComponent<SpriteRenderer>();

        sqrMinSpeed = minInput * minInput;
        sqrInputDeadzone = inputDeadzone * inputDeadzone;

        ScreenSize = new Vector3(Screen.width, Screen.height, 0f);

        SetRandomColor();
    }

    private void SetRandomColor()
    {
        float r = GetRandomColorValue();
        float g = GetRandomColorValue();
        float b = GetRandomColorValue();
        Color c = new Color(r, g, b, 1f);
        rend.color = c;
    }

    float GetRandomColorValue()
    {
        return UnityEngine.Random.Range(0.4f, 1f);
    }

    void Spawn() //Add a timer so player doesn't accidentally immediately cancel invulnerability
    {
        Color c = rend.color;
        c.a = 0.5f;
        rend.color = c;

        inGame = false;

        transform.position = Vector3.zero;

        moveVector = Vector3.zero;
        inputVector = Vector3.zero;

    }

    public void ServerUpdate(Vector2 input)
    {
        if (!inGame)
        {
            if (GameManager.instance.running)
            {
                CheckJoin(input);
            }

            //Blink();
            return;
        }

        ProcessInput(input);
    }

    //private void Blink()
    //{
    //    blinkTimer += Time.deltaTime;
    //    float target;

    //    if (blinked)
    //    {
    //        target = 0f;
    //    }
    //    else
    //    {
    //        target = 1f;
    //    }

    //    Color c = rend.color;


    //    if(blinkTimer >= blinkInterval)
    //    {
    //        blinked = !blinked;
    //        blinkTimer = 0f;
    //    }
    //    else
    //    {
    //        c.a = (Mathf.Lerp(1 - target, target, blinkTimer / blinkInterval));
    //    }

    //    debugColor = blinkTimer / blinkInterval;
    //}

    private void Update()
    {
        if (!inGame || !GameManager.instance.running) return;

        Move();

        CheckStageLimits();
    }

    private void CheckStageLimits()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (playerScreenPos.x > ScreenSize.x || playerScreenPos.x < 0f)
        {
            Die("Left horizontal bounds");
        }
        if (playerScreenPos.y > ScreenSize.y || playerScreenPos.y < 0f)
        {
            Die("Left vertical bounds");
        }
    }

    void Move()
    {
        moveVector = Vector3.Lerp(moveVector, targetMove, Time.deltaTime * steerSpeed);

        //transform.position += moveVector * speed;
        rb.MovePosition(transform.position + (moveVector * speed));
    }

    private void CheckJoin(Vector2 input)
    {
        inputVector = input;

        inputSqrMagnitude = inputVector.sqrMagnitude;

        if (inputVector.sqrMagnitude > inputDeadzone)
        {
            JoinGame();
        }
        else
        {
            return;
        }
    }

    private void JoinGame()
    {
        Color c = rend.color;
        c.a = 1f;
        rend.color = c;
        inGame = true;
    }

    private void ProcessInput(Vector2 input)
    {
        inputVector = input;

        inputSqrMagnitude = inputVector.sqrMagnitude;


        dot = Vector3.Dot(inputVector.normalized, moveVector.normalized);
        if (dot < dotLimit)
        {

            return;
        }

        if (inputSqrMagnitude > sqrInputDeadzone)
        {
            if (inputSqrMagnitude < sqrMinSpeed)
            {
                targetMove = inputVector.normalized * minInput;
            }
            else
            {
                targetMove = inputVector;
                targetMove = Vector3.ClampMagnitude(targetMove, 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inGame)
        {
            return;
        }

        if (other.GetComponent<Obstacle>().team == team)
        {
            if (other.CompareTag("Player"))
            {
                Merge();
            }
            else
            {
                Die("collided with " + team.ToString() + "trail");
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMove>().Die("Collided with player of other team");
                Die("Collided with player of other team");
            }
            else
            {
                other.GetComponent<TrailDecay>().BeDestroyed();
            }
        }
    }

    public void Die(string reason)
    {
        Debug.Log(transform.name + "died. Reason: " + reason);
        Spawn();
    }

    private void Merge()
    {
        throw new NotImplementedException();
    }
}
