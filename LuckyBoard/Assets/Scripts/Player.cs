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
    private TextMeshProUGUI rollValueText;

    [SerializeField]
    private UIController uiController;

    private string[] playerDamagedSounds = { "OnPlayerDamaged", "OnPlayerDamaged2", "OnPlayerDamaged3" };
    private string[] happyPlayerSounds = { "OnPlayerHappy", "OnPlayerHappy2", "OnPlayerHappy3" };
    private int[] turnLeftPositions = { 19, 36, 46 };

    public IEnumerator Move(int steps, bool isMainPlayer)
    {
        if (isMoving)
        {
            yield break;
        }

        DisplayRolledValue(steps, isMainPlayer);

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
        checkCurrentTile(currentPos, routePosition);

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

    private void checkCurrentTile(string currentPos, int routePosition)
    {
        if (currentPos.Equals("DamageTile"))
        {
            LoseHealth();
            FindObjectOfType<AudioManager>().Play("Punch");
            animator.Play("TakeDamage", -1, 0f);
            FindObjectOfType<AudioManager>().Play(playerDamagedSounds[Random.Range(0, 3)]);

        }
        else if(currentPos.Equals("HealthTile"))
        {
            GainHealth();
            animator.Play("GainHealth", -1, 0f);
            FindObjectOfType<AudioManager>().Play(happyPlayerSounds[Random.Range(0, 3)]);
        }
        else if(currentPos.Equals("TeleportTile"))
        {
            TeleportPlayer(routePosition);
            FindObjectOfType<AudioManager>().Play("Teleport");
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

    private void DisplayRolledValue(int rollValue, bool isMainPlayer)
    {
        if(isMainPlayer)
        {
            rollValueText.text = "You rolled: " + rollValue.ToString();
        }
        else
        {
            rollValueText.text = "Enemy player rolled: " + rollValue.ToString();
        }
    }

    private void RotatePlayer(int routePosition)
    {
        if(turnLeftPositions.Contains(routePosition))
        {
            transform.Rotate(0f, -90.0f, 0f);
        }
    }

    private void LoseHealth()
    {
        int damage = Random.Range(0, 11);
        uiController.UpdateInfoText(damage, true);
        if ((health - damage) < 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
    }

    private void GainHealth()
    {
        int healing = Random.Range(0, 11);
        uiController.UpdateInfoText(healing, false);
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
