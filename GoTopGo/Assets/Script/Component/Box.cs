using UnityEngine;
using System.Collections;

namespace GoTop
{
    public class Box : MonoBehaviour
    {
        //前置---------
        Transform trans;
        Rigidbody2D rige;


        //------------------------------//
        //           Player1            //
        //------------------------------//
        CharacterControl characterControl;
        Transform p1Take;//判定抓取位置
        Transform p1HeadTake;//抓取位置
        bool p1canTake;
        bool p1hode;//被抓者
        


        //------------------------------//
        //           Player2            //
        //------------------------------//
        CharacterControl2 characterControl2;
        Transform p2Take;//判定抓取位置
        Transform p2HeadTake;//抓取位置
        bool p2canTake;
        bool p2hode;//被抓者

        ///方塊
        float boxDes;//消除時間
        float useBoxDes;//使用消除方塊時間
        float throwForce = 100f;//與主角距離
        bool boxAttack=true;
        string faction="Null";//方快派別

        //戰時分方塊顏色
        SpriteRenderer sprite;
        

        void Awake()
        {
            trans = transform;
            rige = GetComponent<Rigidbody2D>();
            
            //P1------------------------
            characterControl = GameObject.Find("Player1").GetComponent<CharacterControl>();
            p1Take = GameObject.Find("P1Take").GetComponent<Transform>();
            p1HeadTake = GameObject.Find("P1HeadTake").GetComponent<Transform>();

            //P2------------------------
            characterControl2 = GameObject.Find("Player2").GetComponent<CharacterControl2>();
            p2Take = GameObject.Find("P2Take").GetComponent<Transform>();
            p2HeadTake = GameObject.Find("P2HeadTake").GetComponent<Transform>();

            //戰時分方塊顏色
            sprite = GetComponent<SpriteRenderer>();

        }
        void Start()
        {

        }


        void Update()
        {
            if (faction == "Null" || faction == "P1")
            {
                UseBoxP1();
            }

            if (faction == "Null" || faction == "P2")
            {
                UseBoxP2();
            }

            if(faction == "P2")
                sprite.color = new Color(0, 0, 1, 1);

        }

        void UseBoxP1()
        {
            //在P1拿取範圍中
            if (p1canTake)
            {
                //跟主角拿取位置距離
                float toTake = Vector2.Distance(trans.position, p1Take.position);
                //  print(toTake);
                // print(characterControl.TakeDist  + "f");

                //距離最小時
                if (characterControl.TakeDist > toTake)
                {
                    characterControl.TakeDist = toTake + 0.01f;
                    if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.V))
                        p1hode = true;
                }
                else characterControl.TakeDist += 0.01F;
            }

            //P1拿取中--------
            if (p1hode)
            {
                float horizontal = Input.GetAxis("Horizontal");

                characterControl.hode = true;

                trans.position = p1HeadTake.position;
                trans.Rotate(0, 0, Time.deltaTime * 100);

                rige.isKinematic = true;
                rige.gravityScale = 0.3f;

                if (faction == "Null")//分方塊
                    faction = "P1";

                if (Input.GetKeyUp(KeyCode.Joystick1Button5) || Input.GetKeyUp(KeyCode.V))
                {
                    rige.isKinematic = false;

                    characterControl.hode = false;

                    if (characterControl.faceRight)
                    {
                        if (Mathf.Abs(horizontal) > 0.5f)
                            rige.AddForce(Vector2.right * throwForce * 1.6F);
                        else rige.AddForce(Vector2.right * throwForce);
                    }
                    else
                    {
                        if (Mathf.Abs(horizontal) > 0.5f)
                            rige.AddForce(Vector2.left * throwForce * 1.6F);
                        else rige.AddForce(Vector2.left * throwForce);
                    }
                    boxAttack = true;
                    useBoxDes = 7;//看有沒有交疊方塊
                    p1hode = false;
                }
            }
        }

        void UseBoxP2()
        {
            //在P2拿取範圍中
            if (p2canTake)
            {
                //跟主角拿取位置距離
                float toTake = Vector2.Distance(trans.position, p2Take.position);
                //  print(toTake);
                // print(characterControl.TakeDist  + "f");

                //距離最小時
                if (characterControl2.TakeDist > toTake)
                {
                    characterControl2.TakeDist = toTake + 0.01f;
                    if (Input.GetKeyDown(KeyCode.Joystick2Button5) || Input.GetKeyDown(KeyCode.N))
                        p2hode = true;
                }
                else characterControl2.TakeDist += 0.01F;
            }

            //P1拿取中--------
            if (p2hode)
            {
                float horizontal = Input.GetAxis("Horizontal2");

                characterControl2.hode = true;

                trans.position = p2HeadTake.position;
                trans.Rotate(0, 0, Time.deltaTime * 100);

                rige.isKinematic = true;
                rige.gravityScale = 0.3f;

                if (faction == "Null")//分方塊
                    faction = "P2";

                if (Input.GetKeyUp(KeyCode.Joystick2Button5) || Input.GetKeyUp(KeyCode.N))
                {
                    rige.isKinematic = false;

                    characterControl2.hode = false;

                    if (characterControl2.faceRight)
                    {
                        if (Mathf.Abs(horizontal) > 0.5f)
                            rige.AddForce(Vector2.right * throwForce * 1.6F);
                        else rige.AddForce(Vector2.right * throwForce);
                    }
                    else
                    {
                        if (Mathf.Abs(horizontal) > 0.5f)
                            rige.AddForce(Vector2.left * throwForce * 1.6F);
                        else rige.AddForce(Vector2.left * throwForce);
                    }
                    boxAttack = true;
                    useBoxDes = 7;//看有沒有交疊方塊
                    p2hode = false;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D enterer)
        {    
             //player1   
            if (enterer.name=="P1Take")
            {
                p1canTake = true;
            }
            //player2   
            if (enterer.name == "P2Take")
            {
                p2canTake = true;
            }
        }
        void OnTriggerStay2D(Collider2D enterer)
        {
            //讓交錯的方塊消失------------拿起時不會消失// 
            if (useBoxDes > 0)
            {
                if (!p1hode || !p2hode)
                {
                    useBoxDes -= Time.deltaTime;

                    if (enterer.tag == "Box" && !p1hode)
                    {
                        boxDes += Time.deltaTime;
                        if (boxDes > 4)
                        {
                            Destroy(enterer.gameObject);
                            boxDes = 0;
                        }
                    }
                }
            }
        }
        void OnTriggerExit2D(Collider2D enterer) 
        {      
            if (enterer.name == "P1Take")
            { 
                characterControl.TakeDist = 100;
                p1canTake = false;
            }
            if (enterer.name == "P2Take")
            {
                characterControl2.TakeDist = 100;
                p2canTake = false;
            }
            if (enterer.tag == "Box")
            {
                boxDes = 0;//消除方塊時間
            }
        }
        void OnCollisionEnter2D(Collision2D coll)
        {
            if (boxAttack)
            {
                if (coll.gameObject.tag == "Player")
                {
                    characterControl.Giddy = true;
                }
                if (coll.gameObject.tag == "Player2")
                {
                    characterControl2.Giddy = true;
                }
                boxAttack = false;
            }
        }

    }   
}