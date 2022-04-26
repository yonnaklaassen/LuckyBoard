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
    private MainPlayer mainPlayer;

    [SerializeField]
    private EnemyPlayer enemyPlayer;

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

    public delegate void RollButtonClicked();
    public static event RollButtonClicked rolled;


    void Start()
    {
        rollButtonObject = GameObject.FindGameObjectWithTag("RollButton");
        playerHealthBar.value = mainPlayer.GetCurrentHealth();
        enemyHealthBar.value = enemyPlayer.GetCurrentHealth();

        playerHealthValue.text = mainPlayer.GetCurrentHealth().ToString();
        enemyHealthValue.text = enemyPlayer.GetCurrentHealth().ToString();
    }

    void Update()
    { 
        rollButton.onClick.AddListener(() => rolled());

        playerHealthBar.value = mainPlayer.GetCurrentHealth();
        enemyHealthBar.value = enemyPlayer.GetCurrentHealth();

        playerHealthValue.text = mainPlayer.GetCurrentHealth().ToString();
        enemyHealthValue.text = enemyPlayer.GetCurrentHealth().ToString();
    }

    public void UpdateInfoText(int value, TileTypes type, bool isMainPlayer)
    {
        infoText.enabled = true;
        if(isMainPlayer && type == TileTypes.RedTile)
        {
            infoText.text = "You took " + value + " amount of damage!";
            Invoke("DisableInfoText", 3);
        }
        else if(!isMainPlayer && type == TileTypes.RedTile)
        {
            infoText.text = "The enemy took " + value + " amount of damage!";
            Invoke("DisableInfoText", 3);
        }
        else if(isMainPlayer && type == TileTypes.GreenTile)
        {
            infoText.text = "You gained " + value + " amount of health!";
            Invoke("DisableInfoText", 3);
        }
        else if(!isMainPlayer && type == TileTypes.GreenTile)
        {
            infoText.text = "The enemy gained " + value + " amount of health!";
            Invoke("DisableInfoText", 3);
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
}
