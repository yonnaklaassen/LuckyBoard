using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

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

    [SerializeField]
    private UIController uiController;

    public AudioManager audioManager;

    private string[] playerDamagedSounds = { "OnPlayerDamaged", "OnPlayerDamaged2", "OnPlayerDamaged3" };
    private string[] happyPlayerSounds = { "OnPlayerHappy", "OnPlayerHappy2", "OnPlayerHappy3" };
    private int[] turnLeftPositions = { 19, 36, 46 };

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void Roll(bool isMainPlayer)
    {
        audioManager.Play("DiceRoll");
        int steps = Random.Range(1, 7);

        if (routePosition + steps < currentRoute.tiles.Count)
         {
            StartCoroutine(Move(steps, isMainPlayer));
         }
    }

    public IEnumerator Move(int steps, bool isMainPlayer)
    {
        if (isMoving)
        {
            yield break;
        }

       uiController.DisplayRolledValue(steps, isMainPlayer);

        isMoving = true;

        yield return new WaitForSeconds(1f);

        while (steps > 0)
        {
            animator.Play("Jump", -1, 0f);

            RotatePlayer(routePosition);
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
        checkCurrentTile(currentPos, routePosition, isMainPlayer);

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

    private void checkCurrentTile(string currentPos, int routePosition, bool isMainPlayer)
    {
        if (currentPos.Equals("DamageTile"))
        {
            LoseHealth(isMainPlayer);
        }
        else if(currentPos.Equals("HealthTile"))
        {
            GainHealth(isMainPlayer);

        }
        else if(currentPos.Equals("TeleportTile"))
        {
            audioManager.Play("Teleport");
            TeleportPlayer(routePosition);
        }

    }

    private void TeleportPlayer(int routePosition)
    {
        var nextTeleportTile = currentRoute.tiles.GetRange(routePosition + 1, currentRoute.tiles.Count - routePosition - 1).Find(t => t.tag.Equals("TeleportTile"));
        var tilesBeforeCurrentPos = currentRoute.tiles.GetRange(0, routePosition);

        tilesBeforeCurrentPos.Reverse();
        var lastTeleportTile = tilesBeforeCurrentPos.Find(t => t.tag.Equals("TeleportTile"));

        Transform teleportTo = transform;
        if (lastTeleportTile != null && nextTeleportTile != null)
        {
            Transform[] options = { lastTeleportTile, nextTeleportTile };
            teleportTo = options[Random.Range(0, 2)];

            Debug.Log("Teleport to tile: " + teleportTo);
        }
        else if (lastTeleportTile != null && nextTeleportTile == null)
        {
            teleportTo = lastTeleportTile;

            Debug.Log("Teleport to lastTile: " + teleportTo);
        }
        else if (lastTeleportTile == null && nextTeleportTile != null)
        {
            teleportTo = nextTeleportTile;

            Debug.Log("Teleport to nextTile: " + teleportTo);
        }

        var newPosition = currentRoute.tiles.FindIndex(t => t == teleportTo);
        this.routePosition = newPosition;
        transform.position = teleportTo.position;
    }


    private void RotatePlayer(int routePosition)
    {
        if(turnLeftPositions.Contains(routePosition))
        {
            transform.Rotate(0f, -90.0f, 0f);
        }
    }

    private void LoseHealth(bool isMainPlayer)
    {
        int damage = Random.Range(0, 11);
        audioManager.Play("Punch");
        if ((health - damage) < 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }

        uiController.UpdateInfoText(damage, TileTypes.RedTile, isMainPlayer);
        animator.Play("TakeDamage");
        audioManager.Play(playerDamagedSounds[Random.Range(0, 3)]);
    }

    private void GainHealth(bool isMainPlayer)
    {
        int healing = Random.Range(0, 11);
        audioManager.Play("Heal");
        if ((health + healing) > 100)
        {
            health = 100;
        }
        else
        {
            health += healing;
        }
        uiController.UpdateInfoText(healing, TileTypes.GreenTile, isMainPlayer);
        animator.Play("GainHealth");
        audioManager.Play(happyPlayerSounds[Random.Range(0, 3)]);
    }

}
