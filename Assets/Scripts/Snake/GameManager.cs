using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private Dictionary<String, PlayerMove> playerMap;
    private int numberOfFire = 0;
    private int numberOfIce = 0;
    public Boolean running = false;

    private GameObject firePrefab;
    private GameObject icePrefab;

    public static GameManager instance = null;

    public int timer = 30;
    public Canvas canvas;
    public Text timerText;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
	
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerStart();
        }


        //if((numberOfFire + numberOfIce) < 2)
        //{
        //    StopGame();
        //}
    }

    //private void StopGame()
    //{
    //    running = false;
    //}

    private void TriggerStart()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public IEnumerator CountdownRoutine()
    {
        int i = 1;
        while (timer > i)
        {
            timer -= 1;
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
        }
        SwitchCanvas();
        setRunning(true);
        timer = 30;
    }

    public void SwitchCanvas()
    {
        canvas.gameObject.SetActive(!canvas.gameObject.active);
    }

    void Start()
    {
        playerMap = new Dictionary<string, PlayerMove>();
        firePrefab = Resources.Load("Fire_Player") as GameObject;
        icePrefab = Resources.Load("Ice_Player") as GameObject;
        StartCoroutine(CountdownRoutine());
    }

    public void spawnPlayer(string id)
    {
        Transform player;

        Vector2 position = createRandomPosition(-10, 10, -4, 4);

        if (numberOfIce > numberOfFire)
        {
            player = Instantiate(firePrefab, position, Quaternion.identity).transform;
            numberOfFire++;
        }
        else
        {
            player = Instantiate(icePrefab, position, Quaternion.identity).transform;
            numberOfIce++;
        }

        Debug.Log("Got Player " + id);
        
        playerMap.Add(id, player.GetComponent<PlayerMove>());
    }

    public void deSpawnPlayer(string id)
    {
        //   playerMap.Remove(name);
        Destroy(playerMap[id].gameObject);
    }

    private Vector2 createRandomPosition(int minX, int maxX, int minY, int maxY)
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public void movePlayer(string id, Vector2 direction)
    {
        if (running)
        {
            PlayerMove player = playerMap[id];
            player.ServerUpdate(direction);
            //player.GetComponent<Rigidbody2D>().AddForce(direction * Time.deltaTime * 1000, ForceMode2D.Force);
        }
    }

    public void setRunning(Boolean running)
    {
        this.running = running;
    }
}