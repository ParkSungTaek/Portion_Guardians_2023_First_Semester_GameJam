using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBound : MonoBehaviour
{
    Monster_Controller monster;
    void Start()
    {
        monster = transform.parent.GetComponent<Monster_Controller>();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {

            Debug.Log("Enter");

            collision.GetComponent<T_Controller>().AddMonster(monster.gameObject);
            monster.inRangeTower.AddLast(collision.gameObject);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {
            Debug.Log("Exit");

            collision.GetComponent<T_Controller>().RemoveMonster(monster.gameObject);
            monster.RemoveNode(collision.gameObject);

        }


    }

}