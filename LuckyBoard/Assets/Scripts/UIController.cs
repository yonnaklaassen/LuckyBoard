using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private TextMeshProUGUI infoText;

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
        playerHealthBar.value = startHealth;
        enemyHealthBar.value = startHealth;

        playerHealthValue.text = startHealth.ToString();
        enemyHealthValue.text = startHealth.ToString();

        rollButton.onClick.AddListener(() => {
            if (rolled != null)
            {
                rolled();
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
        }else if(type == TileType.BlackTile)
        {
            infoText.text = "Time to battle!";
            Invoke("DisableInfoText", 2.5f);
        }
    }

    private void UpdateHealthStats(bool isMainPlayer, int health)
    {
        if(isMainPlayer)
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

    public void DisplayRolledValue(int rollValue, bool isMainPlayer)
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

    private void DisableInfoText()
    {
        infoText.enabled = false;
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
    }

    private void OnDisable()
    {
        TurnController.setRollButton -= SetRollButton;
        Player.updatePlayerInfo -= UpdateInfoText;
        Player.displayRolledValue -= DisplayRolledValue;
    }
}
