using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerInputMobile : MonoBehaviour
{
    PlungerController pC;

    Vector2 directionInput;
    bool jumpInput;
    bool flipInput;

    void Start()
    {
        pC = GetComponent<PlungerController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        pC.Move(directionInput.x);

    }

   public void UpdateInput(Vector2 direction, bool flip, bool jump)
   {
       directionInput = direction;
       jumpInput = jump;
       flipInput = flip;
        pC.SynchInput(jumpInput, flipInput);
   }
}
