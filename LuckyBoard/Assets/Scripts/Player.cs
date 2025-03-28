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

    [HideInInspector]
    public bool battleMode = false;

    public int playerScore = 0;
    public int rollCount = 3;

    [SerializeField]
    private int health = 100;

    public delegate void UpdatePlayerInfoText(int value, TileType type, bool isMainPlayer, int health);
    public static event UpdatePlayerInfoText updatePlayerInfo;

    public delegate void DisplayRolledValue(int steps, bool isMainPlayer);
    public static event DisplayRolledValue displayRolledValue;

    public delegate void EndPlayerTurn(bool isMainPlayer);
    public static event EndPlayerTurn endPlayerTurn;

    public delegate void SetIsBattling(bool isBattling);
    public static event SetIsBattling setIsBattling;

    public delegate TileType GetTileType(string currentTile);
    public static event GetTileType getTileType;

    public delegate void DisplayBattleScores(bool isMainPlayer, int score);
    public static event DisplayBattleScores displayBattleScores;

    public delegate void UpdateBattleText(BattleStage stage, WinnerTypes winnerTypes);
    public static event UpdateBattleText updateBattleText;

    public delegate void EndGame();
    public static event EndGame endGame;

    [HideInInspector]
    public AudioManager audioManager;

    [HideInInspector]
    public Animator animator;

    private string[] playerDamagedSounds = { "OnPlayerDamaged", "OnPlayerDamaged2", "OnPlayerDamaged3" };
    private string[] happyPlayerSounds = { "OnPlayerHappy", "OnPlayerHappy2", "OnPlayerHappy3" };
    private int[] turnPositions = { 19, 36, 46, 60, 65, 78, 81, 0};

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
        this.isMainPlayer = isMainPlayer;
        audioManager.Play("DiceRoll");
        int steps = Random.Range(1, 7);
            if (displayRolledValue != null)
            {
                displayRolledValue(steps, isMainPlayer);
            }

            if(!battleMode)
            {
                StartCoroutine(Move(steps, isMainPlayer));

            }
            else
            {
                playerScore += steps;
                rollCount--;

                if(displayBattleScores != null)
                {
                    displayBattleScores(isMainPlayer, playerScore);
                }

                Invoke("EndTurn", 2.5f);
            }
    }

    private IEnumerator Move(int steps, bool isMainPlayer)
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

            RotatePlayer(routePosition);

            Vector3 nextPos;
            if(routePosition == 97)
            {
                nextPos = currentRoute.tiles[0].position;
                routePosition = -1;
            }else
            {
                nextPos = currentRoute.tiles[routePosition + 1].position;
            }
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
        var currentTile = getTileType != null ? getTileType(currentPos) : TileType.YellowTile;
        var wait = checkCurrentTile(currentTile, routePosition, isMainPlayer);

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

    private bool checkCurrentTile(TileType currentTile, int routePosition, bool isMainPlayer)
    {
        var wait = false;

        switch (currentTile)
        {
            case TileType.RedTile:
                LoseHealth(isMainPlayer, false);
                wait = true;
                break;
            case TileType.GreenTile:
                GainHealth(isMainPlayer);
                wait = true;
                break;
            case TileType.PurpleTile:
                audioManager.Play("Teleport");
                TeleportPlayer(routePosition);
                wait = true;
                break;
            case TileType.BlueTile:
                if (updatePlayerInfo != null)
                {
                    updatePlayerInfo(0, TileType.BlueTile, isMainPlayer, health);
                }
                this.isMainPlayer = !isMainPlayer;
                break;
            case TileType.BlackTile:
                StartBattle();
                break;
        } 
        return wait;
    }

    private void TeleportPlayer(int routePosition)
    {
        var nextTeleportTile = currentRoute.tiles.GetRange(routePosition + 1, currentRoute.tiles.Count - routePosition - 1)
                                                    .Find(t => t.tag.Equals("TeleportTile"));
        var tilesBeforeCurrentPos = currentRoute.tiles.GetRange(0, routePosition);

        tilesBeforeCurrentPos.Reverse();
        var lastTeleportTile = tilesBeforeCurrentPos.Find(t => t.tag.Equals("TeleportTile"));

        Transform teleportTo = transform;
        if (lastTeleportTile != null && nextTeleportTile != null)
        {
            Transform[] options = { lastTeleportTile, nextTeleportTile };
            teleportTo = options[Random.Range(0, 2)];
        }
        else if (lastTeleportTile != null && nextTeleportTile == null)
        {
            teleportTo = lastTeleportTile;
        }
        else if (lastTeleportTile == null && nextTeleportTile != null)
        {
            teleportTo = nextTeleportTile;
        }

        var newPosition = currentRoute.tiles.FindIndex(t => t == teleportTo);
        this.routePosition = newPosition;
        transform.position = teleportTo.position;
    }

    private void StartBattle()
    {
        if (updateBattleText != null)
        {
            updateBattleText(BattleStage.Start, WinnerTypes.None);
        }

        this.isMainPlayer = !isMainPlayer;

        if (setIsBattling != null)
        {
            setIsBattling(true);
        }
    }

    private void RotatePlayer(int routePosition)
    {
        if(routePosition >= turnPositions[0] && routePosition < turnPositions[1])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -180, transform.rotation.z));
        }
        else if(routePosition >= turnPositions[1] && routePosition < turnPositions[2])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -270, transform.rotation.z));
        }
        else if(routePosition >= turnPositions[2] && routePosition < turnPositions[3])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -360, transform.rotation.z));
        }
        else if(routePosition >= turnPositions[3] && routePosition < turnPositions[4])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -270, transform.rotation.z));
        }
        else if(routePosition >= turnPositions[4] && routePosition < turnPositions[5])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -180, transform.rotation.z));
        }
        else if(routePosition >= turnPositions[5] && routePosition < turnPositions[6])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -270, transform.rotation.z));
        }
        else if(routePosition >=turnPositions[6])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -360, transform.rotation.z));
        }
        else if(routePosition >= turnPositions[7] && routePosition < turnPositions[0])
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
        }
    }

    public void LoseHealth(bool isMainPlayer, bool isBattle)
    {
        int damage = isBattle ? Random.Range(20, 35) : Random.Range(5, 15);
        audioManager.Play("Punch");
        if ((health - damage) < 0)
        {
            health = 0;

            if(endGame != null)
            {
                endGame();
            }
        }
        else
        {
            health -= damage;
        }

        animator.Play("TakeDamage");
        audioManager.Play(playerDamagedSounds[Random.Range(0, 3)]);
        if (updatePlayerInfo != null)
        {
            updatePlayerInfo(damage, TileType.RedTile, isMainPlayer, health);
        }
    }

    private void GainHealth(bool isMainPlayer)
    {
        int healing = Random.Range(5, 15);
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
            updatePlayerInfo(healing, TileType.GreenTile, isMainPlayer, health);
        }
    }

    public void SetBattleMode(bool active)
    {
        battleMode = active;
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetPlayerHealth()
    {
        return health;
    }

    public void ResetScore()
    {
        playerScore = 0;
        rollCount = 3;
    }

}
