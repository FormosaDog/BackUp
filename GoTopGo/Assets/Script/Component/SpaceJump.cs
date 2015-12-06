using UnityEngine;
using System.Collections;
namespace GoTop
{
    public class SpaceJump : MonoBehaviour
    {

        Transform player1;

        //逃脫方塊堆判定
        bool canSpaceJump;

        bool moveTOTop;
        
        float checkWayTime;//移動一段時間沒碰到方塊傳送主角

        CharacterControl characterControl;

        void Awake()
        {
            player1 = GameObject.Find("Player1").transform;
            characterControl = GameObject.Find("Player1").GetComponent<CharacterControl>();
        }

        void Update()
        {
            //拿著方塊不能使用
            if (Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.V))
            {
                canSpaceJump = false;
            }
            if (characterControl.wait < 0)
            {
                if (canSpaceJump && !moveTOTop && characterControl.grounded)
                {
                    if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.W))
                    {
                        //防止平常亂用
                        moveTOTop = true;
                        checkWayTime = 0.03f;
                    }
                }
            }

        }
        void FixedUpdate()
        {
            //往上 用fixedupdate防止飛太高
            if (moveTOTop)
            {
                transform.Translate(0, 0.3f, 0);
                checkWayTime -= Time.deltaTime;
                if (checkWayTime < 0)
                {
                    player1.position = transform.position;

                    moveTOTop = false;
                    transform.position = new Vector2(player1.position.x, player1.position.y + 0.7f);
                }
            }
        }
        void OnTriggerStay2D(Collider2D enterer)
        {
            if (enterer.tag == "Box")
            {
                canSpaceJump = true;
                checkWayTime = 0.03f;
            }
        }
        void OnTriggerExit2D(Collider2D enterer)
        {
            if (enterer.tag == "Box")
            {
                canSpaceJump = false;
            }
        }
    }
}
