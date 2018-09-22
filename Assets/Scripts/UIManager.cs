using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BlobbyTeam {
	Left,
	Right
}

public class UIManager : MonoBehaviour {


	public Text leftScoreText;
	public Text rightScoreText;

	public Text timerText;

	private float timerCounter;
	private bool timerEnabled = false;

	private int leftTeamScore = 0;
	private int rightTeamScore = 0;

	// Use this for initialization
	void Start () {
		timerText.enabled = false;
		timerEnabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(timerEnabled)
		{
			if(timerCounter > Time.time) {
				timerText.text = "" + ((int)(timerCounter - Time.time) + 1);
			}
			else {
				timerText.enabled = false;
				timerEnabled = false;

			}
		}
	}

	public void StartTimer(float time)
	{
		this.timerEnabled = true;
		this.timerCounter = Time.time + time;

		timerText.text = (time + 1).ToString();
		timerText.enabled = true;
	}

	public void IncrementScore(BlobbyTeam team)
	{
		if(team == BlobbyTeam.Left)
		{
			this.leftTeamScore++;
			this.leftScoreText.text = leftTeamScore.ToString();
		} else {
			this.rightTeamScore++;
			this.rightScoreText.text = rightTeamScore.ToString();
		}
	}
}
