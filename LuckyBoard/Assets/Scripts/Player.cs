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
    private float health = 0f;

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
            animator.Play("Jump", -1, 0f);
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

    public float GetCurrentHealth()
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
            loseHealth(health);
            FindObjectOfType<AudioManager>().Play("Punch");
            animator.Play("TakeDamage", -1, 0f);
            FindObjectOfType<AudioManager>().Play(playerDamagedSounds[Random.Range(0, 2)]);

        }
        else if(currentPos.Equals("HealthTile"))
        {
            gainHealth(health);
            animator.Play("GainHealth", -1, 0f);
            FindObjectOfType<AudioManager>().Play(happyPlayerSounds[Random.Range(0, 2)]);
        }

    }

    private void loseHealth(float damage)
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

    private void gainHealth(float healing)
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
