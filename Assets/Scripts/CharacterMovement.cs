using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
   
    public float animSpeed = 1.5f;              // a public setting for overall animator animation speed
    public float lookSmoother = 3f;             // a smoothing setting for camera motion

    private CapsuleCollider col;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int runState = Animator.StringToHash("Base Layer.Run");

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");              // setup h variable as our horizontal input axis
        float v = Input.GetAxis("Vertical");                // setup v variables as our vertical input axis
        anim.SetFloat("Speed", v);                          // set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("Direction", h);                      // set our animator's float parameter 'Direction' equal to the horizontal input axis		

        anim.speed = animSpeed;
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

       
    }
}
