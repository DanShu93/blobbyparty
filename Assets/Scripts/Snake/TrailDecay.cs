using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDecay : MonoBehaviour
{
    public float maxLiveTime = 3f;
    public float liveTime;

    public CreateTrail creator;

    public float Decay(float deltaTime)
    {
        liveTime -= deltaTime;

        return liveTime;
    }

    public void BeDestroyed()
    {
        creator.RemoveTrail(gameObject);
    }
}
