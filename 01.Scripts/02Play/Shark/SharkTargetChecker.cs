using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTargetChecker : MonoBehaviour
{

    public SharkMovement sharkMovement;
    public bool move;
    private void Update()
    {
        sharkMovement.move = move;
    }


    public void CloseEndPoint()
    {
        sharkMovement.move = false;
    }
    public void OnMove()
    {
        sharkMovement.move = true;
    }
}
