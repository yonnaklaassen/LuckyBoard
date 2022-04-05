using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Player
{
    [HideInInspector]
    public bool isTurn = false;

    [HideInInspector]
    public int steps;

    // Update is called once per frame
   private void Update()
    {
        if (isTurn && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Enemy Rolled: " + steps);

            if (routePosition + steps < currentRoute.tiles.Count)
            {
                StartCoroutine(Move(steps, false));
            }
            else
            {
                Debug.Log("Rolled number too high");
            }
        }
    }
}
