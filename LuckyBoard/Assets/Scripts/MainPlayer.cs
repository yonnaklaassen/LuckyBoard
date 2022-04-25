using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayer : Player
{
    [HideInInspector]
    public int steps;

    [HideInInspector]
    public bool isTurn = false;

    [SerializeField]
    private Button rollButton;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("OnMainPlayerTurn");
        rollButton.onClick.AddListener(RollButtonClicked);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isTurn && !isMoving)
        {
            RollButtonClicked();
        }
    }

    private void RollButtonClicked()
    {
        if(isTurn && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Main player Rolled: " + steps);

            if(routePosition + steps < currentRoute.tiles.Count)
            {
                StartCoroutine(Move(steps, true));
            } else
            {
                Debug.Log("Rolled number too high");
            }
        }
    }
}
