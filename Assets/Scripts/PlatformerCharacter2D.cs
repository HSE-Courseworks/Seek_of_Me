using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_JumpForce = 400f;
        [SerializeField] private bool m_AirControl = false;
        [SerializeField] private LayerMask m_WhatIsGround;

        [SerializeField] string landingSoundName = "LandingFootsteps";

        private Transform m_GroundCheck;
        const float k_GroundedRadius = .2f;
        private bool m_Grounded;
        private Animator m_Anim;
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;

        Transform playerGraphics;

        private bool inAir = false;

        private void Awake()
        {
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            playerGraphics = transform.Find("Graphics");
            if (playerGraphics == null)
            {
                Debug.LogError("Error: Platformer Character 2D: no Graphics object referenced on the player");
            }    
        }

        private void FixedUpdate()
        {
            m_Grounded = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }

            if (!m_Grounded && !inAir)
            {
                inAir = true;
            }

            m_Anim.SetBool("Ground", m_Grounded);

            if (m_Grounded && inAir)
            {
                inAir = false;
                GetComponent<AudioSource>().Play();
            }

            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }

        public void Move(float move, bool jump)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                m_Anim.SetFloat("Speed", Mathf.Abs(move));
                m_Rigidbody2D.velocity = new Vector2(move * PlayerStats.instance.movementSpeed, m_Rigidbody2D.velocity.y);

                if (move > 0 && !m_FacingRight)
                {
                    Flip();
                }
                else if (move < 0 && m_FacingRight)
                {
                    Flip();
                }
            }

            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }

        private void Flip()
        {
            m_FacingRight = !m_FacingRight;
            Vector3 theScale = playerGraphics.localScale;
            theScale.x *= -1;
            playerGraphics.localScale = theScale;
        }
    }
}
