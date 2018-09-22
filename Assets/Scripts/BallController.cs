using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState 
{
	Floating,
	Active,
	OnGround
}

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour {

	public bool IsActive {get; private set;}
	public float Scale = 1f;
	public float Speed = 1f;
	private float radius;
	public BallState state;
	private EnvironmentController environment;
	private UIManager uIManager;
	private Rigidbody2D rigid;

	public void Awake()
	{
		// SET UP INITIAL SIZE OF BALL
		transform.localScale = Vector3.one * Scale;

		radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
		rigid = GetComponent<Rigidbody2D>();

		environment = Transform.FindObjectOfType<EnvironmentController>();
		uIManager = Transform.FindObjectOfType<UIManager>();
	}

	public void Update()
	{

		if(Input.GetKeyDown(KeyCode.H)) 
		{
			HitBall( Random.Range(0f, 1f) * 10, new Vector2(Random.Range(-1f, 1f), 1));		
		}

		if(this.state == BallState.Active && environment.IsOnGround(transform.position, radius))
		{
			// TODO: ADD POINT FOR TEAM LATER

			switch(environment.GetWinningTeam(transform.position))
			{
				case 1:
						// RIGHT TEAM WINS
						Debug.Log("[Ball] Point for right Team.");

						uIManager.IncrementScore(BlobbyTeam.Right);
					break;
				case -1:
						Debug.Log("[Ball] Point for left Team.");
						uIManager.IncrementScore(BlobbyTeam.Left);
				 		// LEFT TEAM WINS
					break;
				default:
						Debug.Log("[Ball] Draw!");
						// DRAW
					break;
			
			}
			state = BallState.OnGround;
			StartCoroutine("TriggerRestart");
		}
	}

	private IEnumerator ExecuteAfterTime(float time, System.Action action)
	{
		yield return new WaitForSeconds(time);

		action();
	}

	public void HitBall(float force, Vector2 direction) 
	{
		if(this.state == BallState.Floating)
		{
			Debug.Log("[Ball] Ball hit (force: " + force + ", direction: " + direction + ")");
			rigid.bodyType = RigidbodyType2D.Dynamic;

			SetState(BallState.Active);
		}

		rigid.velocity = force * direction;
	}

	public void SetState(BallState ballState)
	{
		if(ballState == BallState.Floating)
		{
			rigid.bodyType = RigidbodyType2D.Kinematic;
			rigid.velocity = Vector2.zero;
		}

		this.state = ballState;
	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

	public IEnumerator TriggerRestart()
	{
		Debug.Log("[Ball] Triggered Restart!");

		float restartDelay = 5f;
		uIManager.StartTimer(restartDelay);
		yield return ExecuteAfterTime(restartDelay, environment.RespawnBall);
	}

}
