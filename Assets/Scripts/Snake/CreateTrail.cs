using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrail : MonoBehaviour
{
    List<TrailDecay> trails;
    Team team;

    public TrailPooler pooler;

    Vector3 lastSpawnPosition;
    Vector3 nextSpawnPosition;

    float spawnDistance = 0.5f;
    float sqrSpawnDistance;

    bool nextPositionSet;

    void Start()
    {
        sqrSpawnDistance = spawnDistance * spawnDistance;

        lastSpawnPosition = transform.position;

        team = GetComponent<Obstacle>().team;
        pooler = GameObject.FindGameObjectWithTag("Pooler").GetComponent<TrailPooler>();


        trails = new List<TrailDecay>();
    }

    void SpawnTrail(Vector3 pos)
    {
        GameObject trail = pooler.GetTrail(team, pos);
        TrailDecay decay = trail.GetComponent<TrailDecay>();
        decay.liveTime = decay.maxLiveTime;
        decay.creator = this;
        trails.Add(decay);
    }

    void Update()
    {
        CheckSpawn();
        DecayTrails();
    }

    void CheckSpawn()
    {
        if (!nextPositionSet)
        {
            if ((transform.position - lastSpawnPosition).sqrMagnitude > sqrSpawnDistance)
            {

                nextSpawnPosition = transform.position;
                nextPositionSet = true;
            }
        }
        else
        {
            if ((transform.position - nextSpawnPosition).sqrMagnitude > sqrSpawnDistance)
            {
                SpawnTrail(nextSpawnPosition);
                lastSpawnPosition = nextSpawnPosition;
                nextPositionSet = false;
            }
        }
    }

    private void DecayTrails() // first save all trails to be despawned in one list!!
    {
        List<TrailDecay> toBeDespawned = new List<TrailDecay>();

        foreach (TrailDecay trail in trails)
        {
            if (trail.Decay(Time.deltaTime) <= 0f)
            {
                toBeDespawned.Add(trail);
            }
        }
        foreach (TrailDecay t in toBeDespawned)
        {
            trails.Remove(t);
            pooler.ReturnTrail(team, t.gameObject);
        }
    }

    public void RemoveTrail(GameObject trail)
    {
        trails.Remove(trail.GetComponent<TrailDecay>());
        pooler.ReturnTrail(team, trail);
    }
}
