using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum	SpawnMode {
	Random,
	WinningTeam,
	LosingTeam,
	Center
}

public class EnvironmentController : MonoBehaviour {


	// TRANSFORMS
	public Transform FloorTransform;
	public Transform NetTransform;
	public Transform BackgroundTransform;

	public float floorLevel {get; private set;}
	public float VerticalNetPosition = 0;

	public float NetHeight = 1;

	public bool gameStarted = false;

	public BallController ball;
	private Vector2 screenBounds;
	public float startPosition;

	// Use this for initialization
	void Start () 
	{
		if(NetTransform == null
			|| FloorTransform == null
			|| BackgroundTransform == null)
		{
			Debug.LogError("[Environment] You are missing a reference! One of the Transforms is not connected.");

			this.enabled = false;
		}

		this.floorLevel = FloorTransform.position.y + FloorTransform.GetComponent<SpriteRenderer>().bounds.size.y;

		this.ball = Transform.FindObjectOfType<BallController>();
		if(this.ball == null) 
		{
			Debug.LogError("[Environemnt] There is no ball in this scene!");
			this.enabled = false;
		}

		float height = Camera.main.orthographicSize * 2.0f;
 		float width = height * Screen.width / Screen.height;

		 this.screenBounds = new Vector2(width, height);

		// DEBUGGING TESTS
		RespawnBall();
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			RespawnBall();
		}
	}

	public bool IsOnGround(Vector2 ballPosition, float ballSize)
	{
		Debug.Log("Ball Height: " + ((ballPosition.y - ballSize) - floorLevel));
		if((ballPosition.y - ballSize) - floorLevel <= 0 ) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}


	// RETURN 0 IF DRAW; RETURN 1 FOR RIGHT AND -1 FOR LEFT
	public int GetWinningTeam(Vector2 ballPosition)
	{
		if(ballPosition.x > VerticalNetPosition)
		{
			return -1;
		} else if(ballPosition.x < VerticalNetPosition)
		{
			return 1;
		}

		return 0;
	}

	public Vector2 GetBallSpawnPosition(SpawnMode mode = SpawnMode.Center, int startingSide = 0)
	{	
		ball.SetState(BallState.Floating);

		switch(mode)
		{
			case SpawnMode.Center:
				return Vector2.zero;
			case SpawnMode.LosingTeam:
				return Vector2.zero;
			case SpawnMode.WinningTeam:
				return Vector2.zero;
			case SpawnMode.Random:
				startingSide = Random.Range(0, 2) * 2 - 1;
				return new Vector2((screenBounds.x / 4) * startingSide, startPosition);
			default:
				return Vector2.zero;
		}
	}

	public void RespawnBall() 
	{
		Debug.Log("[Enironemnt] Respawned the ball.");

		this.ball.SetState(BallState.Floating);
		this.ball.SetPosition(GetBallSpawnPosition(SpawnMode.Random));
	}
}
