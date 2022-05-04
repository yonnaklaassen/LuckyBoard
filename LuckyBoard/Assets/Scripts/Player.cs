using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Route currentRoute;

    [SerializeField]
    private float speed = 0.0f;

    [HideInInspector]
    public int routePosition = 0;

    private bool isMoving = false;
    private bool isMainPlayer = false;

    [SerializeField]
    private int health = 0;

    public delegate void UpdatePlayerInfoText(int value, TileTypes type, bool isMainPlayer, int health);
    public static event UpdatePlayerInfoText updatePlayerInfo;

    public delegate void DisplayRolledValue(int steps, bool isMainPlayer);
    public static event DisplayRolledValue displayRolledValue;

    public delegate void EndPlayerTurn(bool isMainPlayer);
    public static event EndPlayerTurn endPlayerTurn;

    public AudioManager audioManager;
    public Animator animator;

    private string[] playerDamagedSounds = { "OnPlayerDamaged", "OnPlayerDamaged2", "OnPlayerDamaged3" };
    private string[] happyPlayerSounds = { "OnPlayerHappy", "OnPlayerHappy2", "OnPlayerHappy3" };
    private int[] turnLeftPositions = { 19, 36};

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        isMainPlayer = true;
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
        this.isMainPlayer = isMainPlayer;
        if (isMoving)
        {
            yield break;
        }

        if(displayRolledValue != null)
        {
            displayRolledValue(steps, isMainPlayer);
        }

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
        var wait = checkCurrentTile(currentPos, routePosition, isMainPlayer);

        if(endPlayerTurn != null)
        {
          if(wait)
            {
                Invoke("EndTurn", 2f);
            }
          else
            {
                EndTurn();
            }
        }
    }

    private void EndTurn()
    {
        endPlayerTurn(this.isMainPlayer);
    }

    private bool MoveToNextTile(Vector3 nextTile)
    {

        return nextTile != (transform.position = Vector3.MoveTowards(transform.position, nextTile, speed * Time.deltaTime));
    }

    private bool checkCurrentTile(string currentPos, int routePosition, bool isMainPlayer)
    {
        var wait = false;
        if (currentPos.Equals("DamageTile"))
        {
            LoseHealth(isMainPlayer);
            wait = true;
        }
        else if(currentPos.Equals("HealthTile"))
        {
            GainHealth(isMainPlayer);
            wait = true;

        }
        else if(currentPos.Equals("TeleportTile"))
        {
            audioManager.Play("Teleport");
            TeleportPlayer(routePosition);
            wait = true;
        }
        return wait;
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
        if(routePosition < turnLeftPositions[0])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
        }
        else if(routePosition >= turnLeftPositions[0] && routePosition < turnLeftPositions[1])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -180, transform.rotation.z));
        }
        else if(routePosition >= turnLeftPositions[1])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -270, transform.rotation.z));
        }
    }

    private void LoseHealth(bool isMainPlayer)
    {
        int damage = Random.Range(1, 11);
        audioManager.Play("Punch");
        if ((health - damage) < 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }

        animator.Play("TakeDamage");
        audioManager.Play(playerDamagedSounds[Random.Range(0, 3)]);
        if (updatePlayerInfo != null)
        {
            updatePlayerInfo(damage, TileTypes.RedTile, isMainPlayer, health);
        }
    }

    private void GainHealth(bool isMainPlayer)
    {
        int healing = Random.Range(1, 11);
        audioManager.Play("Heal");
        if ((health + healing) > 100)
        {
            health = 100;
        }
        else
        {
            health += healing;
        }

        animator.Play("GainHealth");
        audioManager.Play(happyPlayerSounds[Random.Range(0, 3)]);

        if(updatePlayerInfo != null)
        {
            updatePlayerInfo(healing, TileTypes.GreenTile, isMainPlayer, health);
        }
    }

}
