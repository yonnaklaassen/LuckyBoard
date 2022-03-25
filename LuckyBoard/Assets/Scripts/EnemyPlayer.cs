using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;

    [HideInInspector]
    public bool isTurn = false;

    public Animator animator;

    [SerializeField]
    private Route currentRoute;

    [SerializeField]
    private TurnController turnController;

    [HideInInspector]
    public int steps;

    private bool rolledOne = false;
    private bool isMoving = false;
    private int routePosition;
    private float health = 100f;

    // Update is called once per frame
   private void Update()
    {
        if (!isMoving && isTurn)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Rolled: " + steps);

            if (routePosition + steps < currentRoute.tiles.Count)
            {
                if (steps == 1)
                {
                    rolledOne = true;
                }

                animator.SetBool("Jump", true);
                StartCoroutine(Move());
            }
            else
            {
                Debug.Log("Rolled number too high");
            }
        }
    }

    //Tutorial followed:
    //https://www.youtube.com/watch?v=d1oSQdydJsM&list=WL&index=1&t=2194s
    private IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        yield return new WaitForSeconds(1.15f);


        while (steps > 0)
        {
            //Prevent double jump
            if (rolledOne)
            {
                animator.SetBool("Jump", false);
            }

            Debug.Log("steps" + steps);

            Vector3 nextPos = currentRoute.tiles[routePosition + 1].position;

            while (MoveToNextTile(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            steps--;
            routePosition++;

            if (steps <= 1)
            {
                animator.SetBool("Jump", false);
            }

        }

        animator.SetBool("Jump", false);
        isMoving = false;

        turnController.EndTurnEnemy();
    }

    private bool MoveToNextTile(Vector3 nextTile)
    {

        return nextTile != (transform.position = Vector3.MoveTowards(transform.position, nextTile, speed * Time.deltaTime));
    }

    public void loseHealth(float damage)
    {

        if ((health - damage) < 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
    }

    public void gainHealth(float healing)
    {
        if ((health + healing) > 100f)
        {
            health = 100f;
        }
        else
        {
            health += healing;
        }
    }

}
