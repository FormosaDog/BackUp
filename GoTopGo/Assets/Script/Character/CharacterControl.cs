using UnityEngine;
using System.Collections;

namespace GoTop
{
    public class CharacterControl : MonoBehaviour
    {
        //前置設定
        private Transform trans;
        private Rigidbody2D playerRige;
        private Animator anim;

        //地面偵測
        public LayerMask groundLayer;

        internal bool grounded;

        //移動
        float horizontal;
        public float walkSpeed;
        bool closeWallRight;
        bool closeWallLeft;
        internal bool faceRight = true;
        internal float wait;
        //跳躍
        public float jumpForce;
        float jumpBuffer;
        bool inputtingJumpingButton;

        //拿方塊
        internal float TakeDist=100;
        internal bool hode=false;

        //暈眩
        internal bool Giddy = false;

        void Awake()
        {
            // 前置設定 -----------------------------
            playerRige = GetComponent<Rigidbody2D>();
            trans = transform;
            anim = GetComponent<Animator>();
        }

        void Start()
        {
        }

        void Update()
        {
            Move();
        }
        void Move()
        {
            //拿方塊動畫
            anim.SetBool("hode", hode);

            //移動------------------------------------
            Vector3 rightGroundCheck = trans.position + trans.right * 0.1f - trans.up * 0.08f;
            Vector3 leftGroundCheck = trans.position - trans.right * 0.1f - trans.up * 0.08f;

            Vector3 rightGroundCheck2 = trans.position + trans.right * 0.1f - trans.up * 0.18f;
            Vector3 leftGroundCheck2 = trans.position - trans.right * 0.1f - trans.up * 0.18f;

            float horizontal = Input.GetAxis("Horizontal");

            if (wait < 0)
            {
                //右側有牆壁時停止向右移動
                if (horizontal <= 0)
                {
                    if (!closeWallRight)
                    {
                        anim.SetFloat("speed",-horizontal);
                        trans.position += new Vector3(horizontal * walkSpeed * Time.deltaTime, 0.0f, 0.0f);
                    }
                    else
                        anim.SetFloat("speed", 0);
                }
                //左側有牆壁時停止向左移動
                if (horizontal >= 0)
                {
                    if (!closeWallLeft)
                    {
                        anim.SetFloat("speed", horizontal);
                        trans.position += new Vector3(horizontal * walkSpeed * Time.deltaTime, 0.0f, 0.0f);
                    }
                    else
                        anim.SetFloat("speed", 0);
                }

                // 轉向 -----------------------------
                if (horizontal > 0 && !faceRight) // 向右轉
                {
                    faceRight = true;
                    trans.eulerAngles = new Vector3(trans.eulerAngles.x, 0.0f, trans.eulerAngles.z);
                }
                if (horizontal < 0 && faceRight)    // 向左轉
                {
                    faceRight = false;
                    trans.eulerAngles = new Vector3(trans.eulerAngles.x, 180.0f, trans.eulerAngles.z);
                }


                //跳躍-----------------------------------------------------------------
                if (grounded)
                {
                    // 跳躍緩衝, 防止一次加太多力道
                    jumpBuffer += Time.deltaTime;
                    if (jumpBuffer > 0.25f)
                    {
                        if (inputtingJumpingButton)
                        {
                            playerRige.velocity += new Vector2(0.0f, jumpForce * 0.6f);
                            jumpBuffer = 0.0f;
                        }
                    }
                }
                if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.W))
                {
                    inputtingJumpingButton = true;
                }
                else
                {
                    inputtingJumpingButton = false;
                }

                //踢人
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.B))
                {
                    if (wait < 0)
                    {
                        anim.SetTrigger("Kick");///踢的動作
                        wait = 0.3f;///踢的時候不能動
                    }
                }

                if (Physics2D.Raycast(leftGroundCheck, -trans.up, 0.4f, groundLayer)
                    || Physics2D.Raycast(rightGroundCheck, -trans.up, 0.4f, groundLayer))
                {
                    grounded = true;
                }
                else grounded = false;
            }
            else wait -= Time.deltaTime;


            if (faceRight)
            {
                if (Physics2D.Raycast(leftGroundCheck, -trans.right, 0.2f, groundLayer)
                    || Physics2D.Raycast(leftGroundCheck2, -trans.right, 0.2f, groundLayer))
                {
                    closeWallRight = true;
                }
                else { closeWallRight = false; }

                if (Physics2D.Raycast(rightGroundCheck, trans.right, 0.2f, groundLayer)||
                    Physics2D.Raycast(rightGroundCheck2, trans.right, 0.2f, groundLayer))
                {
                    closeWallLeft = true;
                }
                else { closeWallLeft = false; }
            }
            else
            {
                if (Physics2D.Raycast(leftGroundCheck, -trans.right, 0.2f, groundLayer)
                    || Physics2D.Raycast(leftGroundCheck2, -trans.right, 0.2f, groundLayer))
                {
                    closeWallLeft = true;
                }
                else { closeWallLeft = false; }

                if (Physics2D.Raycast(rightGroundCheck, trans.right, 0.2f, groundLayer)
                    || Physics2D.Raycast(rightGroundCheck2, trans.right, 0.2f, groundLayer))
                {
                    closeWallRight = true;
                }
                else { closeWallRight = false; }
            }
           

            //Debug.DrawRay(rightGroundCheck, trans.right * 0.5f, Color.yellow);
            //Debug.DrawRay(leftGroundCheck, -trans.right * 0.5f, Color.yellow);
            //Debug.DrawRay(rightGroundCheck2, trans.right * 0.5f, Color.yellow);
            //Debug.DrawRay(leftGroundCheck2, -trans.right * 0.5f, Color.yellow);
            //Debug.DrawRay(rightGroundCheck, -trans.up * 0.3f, Color.yellow);
            // Debug.DrawRay(leftGroundCheck, -trans.up * 0.3f, Color.yellow);
            
           

            //暈眩--------------------------------------------------------------------------------------//
            if (Giddy)
            {
                wait = 1;
                anim.SetTrigger("Dizzy");
                Giddy = false;
            } 

        
        }
    
    }
}