using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float speed = 0.0f;


    [SerializeField]
    private Animator animator;

    private bool isTurn = false;
    private bool hasClickedTile = false;
    private Vector3 newPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isTurn = true;
        animator.SetBool("IsTurn", true);
    }

    //Some code used from here:
    //https://answers.unity.com/questions/373818/how-to-detect-mouse-click-on-a-gameobject.html
    void FixedUpdate()
    {
        Debug.Log("turn: " + isTurn);
        Debug.Log("HasClickedTile: " + hasClickedTile);

        if (isTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    CurrentClickedGameObject(hit.transform.gameObject, hit);
                }
            }
        }
    }

    public void CurrentClickedGameObject(GameObject gameObject, RaycastHit hit)
    {
        if (gameObject.tag == "Tile")
        {
            hasClickedTile = true;
            animator.SetBool("HasClickedTile", true);

            newPosition = hit.point;
            transform.position = newPosition;
        }
    }
}
