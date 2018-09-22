using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPooler : MonoBehaviour
{
    public GameObject firePrefab;
    public GameObject icePrefab;

    public Stack<GameObject> firePool;
    public Stack<GameObject> icePool;

    int stackSize = 20;


    void Awake()
    {
        firePrefab = Resources.Load("Fire_Trail") as GameObject;
        icePrefab = Resources.Load("Ice_Trail") as GameObject;

        firePool = new Stack<GameObject>(stackSize);
        icePool = new Stack<GameObject>(stackSize);

        FillPool(firePool, firePrefab);
    }

    void FillPool(Stack<GameObject> stack, GameObject prefab)
    {
        for (int i = 0; i < stackSize; i++)
        {
            GameObject gO = Instantiate(prefab, transform);
            gO.SetActive(false);
            stack.Push(gO);
        }
    }

    public GameObject GetTrail(Team team, Vector3 position)
    {
        GameObject prefab = null;
        Stack<GameObject> stack = null;
        if(team == Team.Fire)
        {
            prefab = firePrefab;
            stack = firePool;
        }
        else
        {
            prefab = icePrefab;
            stack = icePool;
        }

        GameObject gO;
        if(stack.Count != 0)
        {
            gO = stack.Pop();

        }
        else
        {
            gO = Instantiate(prefab, transform);
        }

        gO.transform.position = position;
        gO.SetActive(true);

        return gO;
    }

    public void ReturnTrail(Team team, GameObject trail)
    {
        Stack<GameObject> stack = null;
        if (team == Team.Fire)
        {
            stack = firePool;
        }
        else
        {
            stack = icePool;
        }

        stack.Push(trail);
        trail.SetActive(false);
    }
}