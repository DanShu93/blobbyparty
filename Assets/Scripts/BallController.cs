using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState 
{
	Floating,
	Active
}

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour {

	public bool IsActive {get; private set;}
	public float Scale = 1f;
	public float Speed = 1f;
	private float radius;
	private BallState state;
	private EnvironmentController environment;
	private Rigidbody2D rigid;

	public void Start()
	{
		// SET UP INITIAL SIZE OF BALL
		transform.localScale = Vector3.one * Scale;

		radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
		rigid = GetComponent<Rigidbody2D>();

		environment = Transform.FindObjectOfType<EnvironmentController>();
	}

	public void Update()
	{

		if(Input.GetKeyDown(KeyCode.H)) 
		{
			HitBall( Random.Range(0f, 1f), new Vector2(Random.Range(-1f, 1f), 0));		
		}

		if(environment.IsOnGround(transform.position, radius))
		{
			// TODO: ADD POINT FOR TEAM LATER

			switch(environment.GetWinningTeam(transform.position))
			{
				case 1:
						// RIGHT TEAM WINS
						Debug.Log("[Ball] Point for right Team.");
					break;
				case -1:
						Debug.Log("[Ball] Point for left Team.");
				 		// LEFT TEAM WINS
					break;
				default:
						Debug.Log("[Ball] Draw!");
						// DRAW
					break;
			
			}
			environment.RespawnBall();
		}
	}

	public void HitBall(float force, Vector2 direction) 
	{
		if(this.state == BallState.Floating)
		{
			Debug.Log("[Ball] Ball hit (force: " + force + ", direction: " + direction + ")");
			rigid.bodyType = RigidbodyType2D.Dynamic;

			rigid.velocity = force * direction;
		}
	}

	public void SetState(BallState ballState)
	{
		if(ballState == BallState.Floating)
		{
			rigid.bodyType = RigidbodyType2D.Kinematic;
			rigid.velocity = Vector2.zero;
		}
	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}




}
