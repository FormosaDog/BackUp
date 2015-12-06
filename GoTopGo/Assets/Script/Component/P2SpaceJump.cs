using UnityEngine;
using System.Collections;
namespace GoTop
{
    public class P2SpaceJump : MonoBehaviour
    {

        Transform player2;

        //逃脫方塊堆判定
        bool canSpaceJump;

        bool moveTOTop;

        float checkWayTime;//移動一段時間沒碰到方塊傳送主角

        CharacterControl2 characterControl2;

        void Awake()
        {
            player2 = GameObject.Find("Player2").transform;
            characterControl2 = GameObject.Find("Player2").GetComponent<CharacterControl2>();
        }

        void Update()
        {
            //拿著方塊不能使用
            if (Input.GetKey(KeyCode.Joystick2Button5) || Input.GetKey(KeyCode.N))
            {
                canSpaceJump = false;
            }
            if (characterControl2.wait < 0)
            {
                if (canSpaceJump && !moveTOTop && characterControl2.grounded)
                {
                    if (Input.GetKey(KeyCode.Joystick2Button1) || Input.GetKey(KeyCode.UpArrow))
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
                    player2.position = transform.position;

                    moveTOTop = false;
                    transform.position = new Vector2(player2.position.x, player2.position.y + 0.7f);
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
