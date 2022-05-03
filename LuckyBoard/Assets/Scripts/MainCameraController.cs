using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private GameObject mainPlayer;

    private GameObject enemyPlayer;

    private bool followMainPlayer;

    private AudioManager audioManager;

    private void Awake()
    {
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
        enemyPlayer = GameObject.FindGameObjectWithTag("Enemy");
        audioManager = FindObjectOfType<AudioManager>();
    }
    void Start()
    {
        followMainPlayer = true;
        audioManager.Play("Background");
    }

    void Update()
    {
        if(followMainPlayer)
        {
            transform.position = mainPlayer.transform.position + new Vector3(4, 2.5f, 0);
        }
        else
        {
            transform.position = enemyPlayer.transform.position + new Vector3(4, 2.5f, 0);
        }

    }

    private void FollowPlayer(bool isMainPlayer)
    {
        followMainPlayer = isMainPlayer;
    }


    private void OnEnable()
    {
        TurnController.setMainCamera += FollowPlayer;

    }

    private void OnDisable()
    {
        TurnController.setMainCamera -= FollowPlayer;

    }
}
