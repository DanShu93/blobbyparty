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

	public Animator scoreAnimator;
	public Text timerText;
	public Text statusText;

	public Transform SplashScreen;

	private float timerCounter;
	private bool timerEnabled = false;

	private int leftTeamScore = 0;
	private int rightTeamScore = 0;

	private bool splashVisible = false;
	// Use this for initialization
	void Start () 
	{
		timerText.enabled = false;
		timerEnabled = false;

		SetMessage("OPEN UP YOUR BROWSER AND JOIN IN!");

		ShowSplashScreen(true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(Input.GetKeyDown(KeyCode.M))
		{
			Debug.Log("Change Splash Screen.");
			ShowSplashScreen(!splashVisible);
			return;
		}

		if(timerEnabled)
		{
			if(timerCounter > Time.time) {
				int remaining = ((int)(timerCounter - Time.time) + 1);
				if(remaining > 3)
					this.timerText.text = remaining.ToString();
				else 
				{
					if(remaining == 3) {
						this.timerText.text = "  READY";
					} else if(remaining == 2) {
						this.timerText.text = "  SET";
					} else {
						this.timerText.text = "GO!";
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
		Text text = scoreAnimator.transform.GetChild(0).GetComponent<Text>();

		if(team == BlobbyTeam.Left)
		{

			this.leftTeamScore++;
			this.leftScoreText.text = leftTeamScore.ToString();

			text.text = "1 POINT BLUE";
			text.color = Color.blue;
		} else {
			this.rightTeamScore++;
			this.rightScoreText.text = rightTeamScore.ToString();

			text.text = "1 POINT RED";
			text.color = Color.red;
		}

		scoreAnimator.Play("PopUp");
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

	public void ShowSplashScreen(bool b)
	{
		splashVisible = b;
		this.SplashScreen.gameObject.SetActive(b);//.SetActive(b);
	}
}
