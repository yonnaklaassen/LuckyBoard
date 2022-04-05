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


    void Start()
    {
        playerHealthBar.value = mainPlayer.GetCurrentHealth();
        enemyHealthBar.value = enemyPlayer.GetCurrentHealth();

        playerHealthValue.text = mainPlayer.GetCurrentHealth().ToString();
        enemyHealthValue.text = enemyPlayer.GetCurrentHealth().ToString();
    }

    void Update()
    {
        playerHealthBar.value = mainPlayer.GetCurrentHealth();
        enemyHealthBar.value = enemyPlayer.GetCurrentHealth();

        playerHealthValue.text = mainPlayer.GetCurrentHealth().ToString();
        enemyHealthValue.text = enemyPlayer.GetCurrentHealth().ToString();
    }
}
