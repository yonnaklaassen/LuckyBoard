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

    //Tutorial followed:
    //https://www.youtube.com/watch?v=d1oSQdydJsM&list=WL&index=1&t=2194s
    //private IEnumerator Move()
    //{
    //    if (isMoving)
    //    {
    //        yield break;
    //    }

    //    isMoving = true;

    //    yield return new WaitForSeconds(1.15f);


    //    while (steps > 0)
    //    {
    //        Vector3 nextPos = currentRoute.tiles[routePosition + 1].position;

    //        while (MoveToNextTile(nextPos))
    //        {
    //            yield return null;
    //        }

    //        yield return new WaitForSeconds(0.5f);
    //        steps--;
    //        routePosition++;

    //    }

    //    isMoving = false;
    //    var currentPos = currentRoute.tiles[routePosition].tag;
    //    checkCurrentTile(currentPos);

    //    turnController.EndTurnEnemy();
    //    animator.SetBool("TakeDamage", false);
    //}

    //private bool MoveToNextTile(Vector3 nextTile)
    //{

    //    return nextTile != (transform.position = Vector3.MoveTowards(transform.position, nextTile, speed * Time.deltaTime));
    //}

    //private void checkCurrentTile(string currentPos)
    //{
    //    if (currentPos.Equals("DamageTile"))
    //    {
    //        loseHealth(health);
    //        animator.Play("TakeDamage", -1, 0f);
    //        FindObjectOfType<AudioManager>().Play("OnMainPlayerDamaged");
    //    }

    //}

    //private void loseHealth(float damage)
    //{

    //    if ((health - damage) < 0)
    //    {
    //        health = 0;
    //    }
    //    else
    //    {
    //        health -= damage;
    //    }
    //}

    //private void gainHealth(float healing)
    //{
    //    if ((health + healing) > 100f)
    //    {
    //        health = 100f;
    //    }
    //    else
    //    {
    //        health += healing;
    //    }
    //}

}
