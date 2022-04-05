using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Route currentRoute;

    [SerializeField]
    private TurnController turnController;

    [SerializeField]
    private float speed = 0.0f;

    public Animator animator;
    public int routePosition = 0;

    [HideInInspector]
    public bool isMoving = false;

    [SerializeField]
    private int health = 0;

    private string[] playerDamagedSounds = { "OnPlayerDamaged", "OnPlayerDamaged2", "OnPlayerDamaged3" };
    private string[] happyPlayerSounds = { "OnPlayerHappy", "OnPlayerHappy2", "OnPlayerHappy3" };

    public IEnumerator Move(int steps, bool isMainPlayer)
    {

        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        yield return new WaitForSeconds(1f);

        while (steps > 0)
        {
            animator.Play("Jump", -1, 2f);
            Vector3 nextPos = currentRoute.tiles[routePosition + 1].position;

            while (MoveToNextTile(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.4f);
            steps--;
            routePosition++;
        }

        isMoving = false;
        var currentPos = currentRoute.tiles[routePosition].tag;
        checkCurrentTile(currentPos);

        if(isMainPlayer)
        {
            turnController.EndTurnPlayer();
        }
        else
        {
            turnController.EndTurnEnemy();
        }

    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetCurrentRoutePosition()
    {
        return routePosition;
    }

    private bool MoveToNextTile(Vector3 nextTile)
    {

        return nextTile != (transform.position = Vector3.MoveTowards(transform.position, nextTile, speed * Time.deltaTime));
    }

    private void checkCurrentTile(string currentPos)
    {
        if (currentPos.Equals("DamageTile"))
        {
            loseHealth(5);
            FindObjectOfType<AudioManager>().Play("Punch");
            animator.Play("TakeDamage", -1, 0f);
            FindObjectOfType<AudioManager>().Play(playerDamagedSounds[Random.Range(0, 2)]);

        }
        else if(currentPos.Equals("HealthTile"))
        {
            gainHealth(5);
            animator.Play("GainHealth", -1, 0f);
            FindObjectOfType<AudioManager>().Play(happyPlayerSounds[Random.Range(0, 2)]);
        }

    }

    private void loseHealth(int damage)
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

    private void gainHealth(int healing)
    {
        if ((health + healing) > 100)
        {
            health = 100;
        }
        else
        {
            health += healing;
        }
    }

}
