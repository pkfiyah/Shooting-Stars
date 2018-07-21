using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class player : MonoBehaviour {

    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] public float m_RunSpeed = 10f;                      // Force to apply when running on the ground
    [SerializeField] public float m_Thrust = 4f;                        // Force to apply via in free space thrusters
    [SerializeField] public float m_Max_Launch_Force = 1000.0f;
    private float m_Launch_Cooldown = 0f;
    private float m_Launch_Force = 0f;                  // Amount of force added when the player jumps.
    const float k_GroundedRadius = 0.2f; // Radius of the overlap circle to determine if grounded
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_Transform;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    private bool m_FacingRight = true;                                  // For determining which way the player is currently facing.
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    private CircleCollider2D m_GroundCheckCollider;
    private LaunchSystem m_launchBar;

    // Use this for initialization
    void Start () {
       
    }

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_GroundCheckCollider = GetComponent<CircleCollider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Transform = GetComponent<Transform>();
        m_launchBar = gameObject.transform.Find("LaunchBar").gameObject.GetComponent<LaunchSystem>();
    }

    // Update is called once per frame
    void Update () { 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        if (m_Launch_Cooldown <= Time.time)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    if (colliders[i].isTrigger) continue;
                    m_Grounded = true;
                }
            }
        }

        handleMovement();
    }

    private void FixedUpdate()
    {
        // If the player should jump...
        // if (m_Grounded && m_Launch)
        // {
        //    Debug.Log("Jump");
        //    // Add a vertical force to the player.
        //    m_Grounded = false;
        //    m_Rigidbody2D.AddForce(new Vector2(m_JumpForce * m_Transform.up.x, m_JumpForce * m_Transform.up.y));
        // }
      
      
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.GetContact(0);
        m_Transform.up = contactPoint.normal;
    }

    private void handleMovement ()
    {
        // Check controller input
        float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        float inputY = CrossPlatformInputManager.GetAxis("Vertical");
        if (!m_Grounded)
        {
            m_Rigidbody2D.AddForce(new Vector2(inputX * m_Thrust * Vector2.right.x, inputY * m_Thrust * Vector2.up.y));
        }
        else
        {
            // Imitate Friction (Stop Free Floating movement when on planet surface
            m_Rigidbody2D.drag = 2;

            if (CrossPlatformInputManager.GetButton("Jump"))
            {
                m_Launch_Force += Time.deltaTime * m_Max_Launch_Force;
                m_launchBar.displayCharge(Mathf.Clamp(m_Launch_Force, 0, m_Max_Launch_Force), m_Max_Launch_Force);
            }

            if (CrossPlatformInputManager.GetButtonUp("Jump"))
            {
                Debug.Log("Release Charge");
                m_launchBar.endCharge();
                m_Rigidbody2D.AddForce(new Vector2(m_Launch_Force * m_Transform.up.x, m_Launch_Force * m_Transform.up.y));
                m_Launch_Force = 0f;
                m_Grounded = false;
                m_Rigidbody2D.drag = 0;
                m_Launch_Cooldown = Time.time + 0.2f;
            }
        }
        
        // If the input is moving the player right and the player is facing left...
        if (inputX > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (inputX < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
}
