using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutWall : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameLogic gameLogic;
    public Vector3 startPosition = new Vector3(-10, 13.5f, 10);

    private Rigidbody RB;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "OutWall")
        {
            if (other.name == "8")
            {
                gameLogic.Loss();
            }
            else
            {
                gameLogic.NextTurn();
                if(other.tag == "Player")
                {
                    gameLogic.NextTurn();
                    other.gameObject.transform.position = startPosition;
                    RB = other.gameObject.GetComponent<Rigidbody>();
                    RB.velocity = new Vector3(0, 0, 0);
                }
                Destroy(other.gameObject);
            }
        }
    }
}