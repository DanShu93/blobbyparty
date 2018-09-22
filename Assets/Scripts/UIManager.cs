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
	public Text statusText;

	private float timerCounter;
	private bool timerEnabled = false;

	private int leftTeamScore = 0;
	private int rightTeamScore = 0;

	// Use this for initialization
	void Start () 
	{
		timerText.enabled = false;
		timerEnabled = false;

		SetMessage("Open up your Browser and connect to bla bla.");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(timerEnabled)
		{
			if(timerCounter > Time.time) {
				int remaining = ((int)(timerCounter - Time.time) + 1);
				if(remaining > 3)
					this.timerText.text = remaining.ToString();
				else 
				{
					if(remaining == 3) {
						this.timerText.text = "  Ready..";
					} else if(remaining == 2) {
						this.timerText.text = "  Set..";
					} else {
						this.timerText.text = "Go!";
					}
				}


			}
			else {
				timerText.enabled = false;
				timerEnabled = false;

			}
		}
	}

	public void StartTimer(float time)
	{
		HideStatusMessage();
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

	public void SetMessage(string text)
	{
		this.statusText.enabled = true;
		this.statusText.text = text;
	}

	public void HideStatusMessage()
	{
		this.statusText.enabled = false;
	}
}
