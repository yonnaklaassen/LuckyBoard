using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : Player
{
    [HideInInspector]
    public int steps;

    [HideInInspector]
    public bool isTurn = false;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("OnMainPlayerTurn");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isTurn && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Main player Rolled: " + steps);

            if(routePosition + steps < currentRoute.tiles.Count)
            {
                StartCoroutine(Move(steps, true));
            } else
            {
                Debug.Log("Rolled number too high");
            }
        }
    }
}
