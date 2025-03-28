using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Slider playerHealthBar;

    [SerializeField]
    private Slider enemyHealthBar;

    [SerializeField]
    private TextMeshProUGUI playerHealthValue;

    [SerializeField]
    private TextMeshProUGUI enemyHealthValue;

    [SerializeField]
    private TextMeshProUGUI playerBattleScore;

    [SerializeField]
    private TextMeshProUGUI enemyBattleScore;

    [SerializeField]
    private TextMeshProUGUI infoText;

    [SerializeField]
    private TextMeshProUGUI gameText;

    private GameObject rollButtonObject;

    [SerializeField]
    private Button rollButton;

    [SerializeField]
    private TextMeshProUGUI rollValueText;

    private int startHealth = 100;

    public delegate void RollButtonClicked();
    public static event RollButtonClicked rolled;

    private void Awake()
    {
        rollButtonObject = GameObject.FindGameObjectWithTag("RollButton");
    }

    void Start()
    {   
        DisableGameText();
        DisableInfoText();
        DisableBattleScores();
        playerHealthBar.value = startHealth;
        enemyHealthBar.value = startHealth;

        playerHealthValue.text = startHealth.ToString();
        enemyHealthValue.text = startHealth.ToString();

        rollButton.onClick.AddListener(() => {
            if (rolled != null)
            {
                rolled();
                SetRollButton(false);
            }
        });
    }

    public void UpdateInfoText(int value, TileType type, bool isMainPlayer, int health)
    {
        UpdateHealthStats(isMainPlayer, health);
        infoText.enabled = true;
        if(isMainPlayer && type == TileType.RedTile)
        {
            infoText.text = "You took " + value + " amount of damage!";
            Invoke("DisableInfoText", 2.5f);
        }
        else if(!isMainPlayer && type == TileType.RedTile)
        {
            infoText.text = "The enemy took " + value + " amount of damage!";
            Invoke("DisableInfoText", 2.5f);
        }
        else if(isMainPlayer && type == TileType.GreenTile)
        {
            infoText.text = "You gained " + value + " amount of health!";
            Invoke("DisableInfoText", 2.5f);
        }
        else if(!isMainPlayer && type == TileType.GreenTile)
        {
            infoText.text = "The enemy gained " + value + " amount of health!";
            Invoke("DisableInfoText", 2.5f);
        }
        else if(type == TileType.BlueTile)
        {
            infoText.text = "Roll again!";
            Invoke("DisableInfoText", 2.5f);
        }
    }

    private void UpdateBattleText(BattleStage stage, WinnerTypes winnerTypes)
    {
        gameText.enabled = true;

        switch(stage)
        {
            case BattleStage.Start:
                gameText.text = "The battle begins!";
                Invoke("DisableGameText", 2.5f);
                break;
            case BattleStage.Winner:
                gameText.text = winnerTypes == WinnerTypes.MainPlayer ? "You are the winner!" : "The enmy wins!";
                Invoke("DisableGameText", 3f);
                break;
            case BattleStage.Tie:
                gameText.text = "It's a tie, no one is punished";
                Invoke("DisableGameText", 3f);
                break;
        }
    }

    private void UpdateHealthStats(bool isMainPlayer, int health)
    {
            if (isMainPlayer)
            {
                playerHealthBar.value = health;
                playerHealthValue.text = health.ToString();
            }
            else
            {
                enemyHealthBar.value = health;
                enemyHealthValue.text = health.ToString();
            }
    }

    private void DisplayWinner(bool isMainPlayer)
    {
        rollValueText.enabled = false;
        gameText.enabled = true;

        if (isMainPlayer)
        {
            gameText.text = "Game over! You lost";
            Invoke("QuitGame", 5.2f);
        }
        else
        {
            gameText.text = "You won!";
            Invoke("QuitGame", 10.5f);
        }
    }

    private void DisplayRolledValue(int rollValue, bool isMainPlayer)
    {
         if (isMainPlayer)
        {
            rollValueText.text = "You rolled: " + rollValue.ToString();
        }
        else
        {
            rollValueText.text = "Enemy player rolled: " + rollValue.ToString();
        }
    }

    private void DisplayScore(bool isMainPlayer, int score)
    {
        enemyBattleScore.enabled = true;
        playerBattleScore.enabled = true;
        if (isMainPlayer)
        {
            playerBattleScore.text = "Your score: " + score.ToString();

        }
        else
        {
            enemyBattleScore.text = "Enemy score: " + score.ToString();
        }
    }

    private void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    private void DisableInfoText()
    {
        infoText.enabled = false;
    }

    private void DisableGameText()
    {
        gameText.enabled = false;
    }

    private void DisableBattleScores()
    {
        enemyBattleScore.enabled = false;
        playerBattleScore.enabled = false;
    }

    public void SetRollButton(bool active)
    {
        rollButtonObject.SetActive(active);
    }

    private void OnEnable()
    {
        TurnController.setRollButton += SetRollButton;
        Player.updatePlayerInfo += UpdateInfoText;
        Player.displayRolledValue += DisplayRolledValue;
        TurnController.disableBattleScores += DisableBattleScores;
        Player.displayBattleScores += DisplayScore;
        Player.updateBattleText += UpdateBattleText;
        TurnController.updateBattleText += UpdateBattleText;
        TurnController.endGame += DisplayWinner;
    }

    private void OnDisable()
    {
        TurnController.setRollButton -= SetRollButton;
        Player.updatePlayerInfo -= UpdateInfoText;
        Player.displayRolledValue -= DisplayRolledValue;
        TurnController.disableBattleScores -= DisableBattleScores;
        Player.displayBattleScores -= DisplayScore;
        Player.updateBattleText -= UpdateBattleText;
        TurnController.updateBattleText -= UpdateBattleText;
        TurnController.endGame -= DisplayWinner;
    }
}
