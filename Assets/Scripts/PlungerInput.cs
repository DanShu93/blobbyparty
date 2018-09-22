using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerInput : MonoBehaviour
{
    PlungerController pC;

    bool jumpInput;
    bool flipInput;

    void Start()
    {
        pC = GetComponent<PlungerController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        pC.Move(h);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            flipInput = true;
        }
        else
        {
            flipInput = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }
        pC.SynchInput(jumpInput, flipInput);
    }
}
