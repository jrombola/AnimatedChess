using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAttack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*float time = Time.time;
        float targetTime = time + 0.1f;
        while (time <= targetTime)
            time += Time.deltaTime;*/
        // Debug.Log(animator.GetComponent<Piece>().pieceToAttack.GetComponent<Animator>().ToString());
        animator.GetComponent<Piece>().pieceToAttack.pieceBeingAttackedBy = animator.GetComponent<Piece>();
         animator.GetComponent<Piece>().pieceToAttack.GetComponent<Animator>().SetBool("isUnderAttack", true);
        if(animator.GetComponent<Piece>().attackSound != null)
             AudioSource.PlayClipAtPoint(animator.GetComponent<Piece>().attackSound, animator.GetComponent<Piece>().mainCamera.transform.position);
        
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
