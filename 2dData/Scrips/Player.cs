using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D mageRigidbody;

    private Animator mageAnimation;

    [SerializeField]
    private float mageSpeed;

    private bool attack;

    private bool facingRight=true;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;



    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;




	void Start () {

        mageRigidbody = GetComponent<Rigidbody2D>();  // reference to the given gravity to the player.
        mageAnimation = GetComponent<Animator>();  // reference to the player animation.
	}

     void Update()
    {

        HandelInput();

      
    }

    // Update is called once per frame
    void FixedUpdate () {

        float horizontal = Input.GetAxis("Horizontal"); //movement on a horizontal plane.

        isGrounded = IsGrounded();

        MageMovement(horizontal);   // calling the movement of the player either to left or right.

        Flip(horizontal);

        MageAttacks();

        HandleLayers();

        Reset();
	}
    private void MageMovement(float horizontal)
    {
        if (mageRigidbody.velocity.y < 0)
        {
            mageAnimation.SetBool("land", true);
        }
        if (!mageAnimation.GetCurrentAnimatorStateInfo(0).IsTag("attack") && (isGrounded || airControl))
        {
           mageRigidbody.velocity = new Vector2(horizontal * mageSpeed, mageRigidbody.velocity.y); // movement for the mage on x = -1, y = 0
        }
        if (isGrounded && jump)
        {
           isGrounded = false;
           mageRigidbody.AddForce(new Vector2(0, jumpForce));
            mageAnimation.SetTrigger("jump");
        }
       

        mageAnimation.SetFloat("speed", Mathf.Abs (horizontal));  // calling the action of running for the mage animation.
    }

    private void MageAttacks()
    {
        if (attack && !this.mageAnimation.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            mageAnimation.SetTrigger("attack");  // calling the mage attack animation sequence

            mageRigidbody.velocity = Vector2.zero; // resets velocity when it attaks.
        }
    }

    private void HandelInput()         // input calling the players movementes, jump and attack by using the keyboard.
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            attack = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            jump = true;
        }
    }

    private void Flip(float horizontal)  //reference to where the player will face when is walking.
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void Reset()  //resets the values from one animation to other 
    {
        attack = false;
        jump = false;
        
    
    }

    private bool IsGrounded() {


        if (mageRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                
                for (int i = 0; i< colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        mageAnimation.ResetTrigger("jump");
                        mageAnimation.SetBool("land", false);
                        return true;
                    }
                }
               
            }
        }
        return false;    
   }

    private void HandleLayers()
    {
        if (isGrounded)
        {
            mageAnimation.SetLayerWeight(1, 1);
        }
        else
        {
            mageAnimation.SetLayerWeight(1, 0);
        }
    }

}
