using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private GameObject mainPlayer;

    private GameObject enemyPlayer;

    private bool followMainPlayer;

    private AudioManager audioManager;

    private float cameraDistance = 4f;

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
            transform.position = mainPlayer.transform.position - mainPlayer.transform.forward * cameraDistance;
            transform.LookAt(mainPlayer.transform.position);
            transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        }
        else
        {
            transform.position = enemyPlayer.transform.position - enemyPlayer.transform.forward * cameraDistance;
            transform.LookAt(enemyPlayer.transform.position);
            transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
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
