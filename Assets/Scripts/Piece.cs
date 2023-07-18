using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Piece : MonoBehaviour
{
    public int Value;
    public bool isWhite;
    public string pieceName;
    public NavMeshAgent navmesh;
    public bool hasMoved = false;
    public Animator animator;
    public bool wantsToAttack = false;
    public Vector3 target;
    public Piece pieceToAttack;
    public Piece pieceBeingAttackedBy;
    public bool isReadyToDie = false;
    public Quaternion originalRotation;

    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip getHitSound;
    public Camera mainCamera;

    public int getValue()
    {
        return Value;
    }

    public Vector3 cameraPosition = new Vector3(-10.2f, 79.7f, 31.2f);

    public void move(GameObject Tile)
    {
        rotateToTarget(Tile);
       // Debug.Log(ChessRules.current.pieceIsMoving);
        animator.SetBool("isWalking", true);
        target = Tile.transform.position;
        navmesh.SetDestination(Tile.transform.position);
        ChessRules.current.setPieceIsMovingTrue();
        hasMoved = true;

    }
    //called by the StopAttack function to return to center of tile after killing enemy.
    public void move(Vector3 target)
    {
        rotateToTarget();
        hasMoved = true;
        animator.SetBool("isWalking", true);
        navmesh.SetDestination(target);
        wantsToAttack = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Outline1>().enabled = false;
        navmesh = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        navmesh.autoBraking = false;
        navmesh.speed = 5;
        navmesh.radius = 0.01f;
        navmesh.stoppingDistance = 0.5f;
        navmesh.avoidancePriority = 0;
        navmesh.angularSpeed = 999999999999999999;
        navmesh.acceleration = 999999999999999999;
        originalRotation = this.transform.rotation;
        //animator.applyRootMotion = false;
    }



    public void rotateBackToOriginalPosition()
    {
        if (this.transform.rotation != originalRotation)
        {
            Quaternion current = this.transform.rotation;
            StartCoroutine(Lerp(current, originalRotation));
        }
    }

    public void rotateToTarget()
    {
        float dot = Vector3.Dot(transform.forward, (pieceToAttack.transform.position - transform.position).normalized);

        Quaternion current = this.transform.rotation;
        Vector3 relativePos = pieceToAttack.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion.Slerp(current, rotation, Time.deltaTime);

    }
    public void rotateToTarget(GameObject Tile)
    {
        float dot = Vector3.Dot(transform.forward, (Tile.transform.position - transform.position).normalized);
        Quaternion current = this.transform.rotation;
        Vector3 relativePos = Tile.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        /* if (dot < 0.9f)
         {
             float lerpTime = 0f;
             while (lerpTime < 5)
             {
                 lerpTime += Time.deltaTime;
                 transform.rotation = Quaternion.Slerp(current, rotation, lerpTime / 5);
             }
         } */
        Quaternion.Slerp(current, rotation, Time.deltaTime);
    }
    IEnumerator checkMove()
    {
        while (ChessRules.current.pieceIsMoving)
        {
            Debug.Log("waiting");
            yield return null; // wait until next frame
        }
    }

    IEnumerator Lerp(Quaternion startValue, Quaternion endValue)
    {
       // Debug.Log("lerpinggg");
        float timeElapsed = 0;
        while (timeElapsed < 2)
        {
           // Debug.Log("lerping");
            transform.rotation = Quaternion.Slerp(startValue, endValue, timeElapsed / 2);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void killPiece()
    {
        Destroy(this.animator);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isReadyToDie)
        {

            //this.GetComponent<NavMeshAgent>().enabled = false;
            Destroy(this.animator);
            this.gameObject.SetActive(false);
        }
        if (hasMoved && (this.transform.position - navmesh.destination).magnitude < 0.5f && !wantsToAttack)
        {
            navmesh.velocity = Vector3.zero;
            navmesh.SetDestination(this.transform.position);
            animator.SetBool("isWalking", false);
            rotateBackToOriginalPosition();
            ChessRules.current.setPieceIsMovingFalse();
            hasMoved = false;
            if(ChessRules.current.win_state == true)
            {
                ChessRules.current.checkmateMenu.SetActive(true);
                Time.timeScale = 0;
            }

        }
        else if (hasMoved && (this.transform.position - navmesh.destination).magnitude < 6f && wantsToAttack)
        {

            navmesh.velocity = Vector3.zero;
            navmesh.SetDestination(this.transform.position);
            hasMoved = false;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }

    }
}
