using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Vector2 startPoint;

    public GameObject Player;

    public Transform[] startPoses;
    public GameObject[] rooms;

    private int direction;
    public float moveAmount = 10;

    public float maxX;
    public float minX;
    public float maxY;
    private bool generationStop;

    public LayerMask room;

    private int upCounter;

    private void Start()
    {
        int randSTPos = Random.Range(0, startPoses.Length);
        transform.position = startPoses[randSTPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        Player.transform.position = startPoses[randSTPos].position;

        direction = Random.Range(1, 6);

       Move();
    }


    private void Move()
    {

        if(direction == 1 || direction == 2)
        {// Move right
            if(transform.position.x < maxX)
            {
                upCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if(direction == 3)
                {
                    direction = 2;
                }
                else if(direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        }
        else if(direction == 3 || direction == 4)
        {//move left
            if (transform.position.x > minX)
            {
                upCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
            
        }
        else if (direction == 5)
        {//move up
            upCounter++;

            if(transform.position.y < maxY)
            {
                
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomType>().type != 2 || roomDetection.GetComponent<RoomType>().type != 3)
                {
                    roomDetection.GetComponent<RoomType>().DestroyObj();
                    if (upCounter >= 2)
                    {
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        int randTopRoom = Random.Range(1, 4);
                        if(randTopRoom == 2)
                        {
                            randTopRoom = 1;
                        }
                        Instantiate(rooms[randTopRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                
            }
            else
            {
                generationStop = true;
                transform.position = startPoint;
                fillUp();
            }
        }
        if (!generationStop)
        {
            Move();
        }
    }

    public void fillUp()
    {

        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
        if(roomDetection == null)
        {
            int rand = Random.Range(0, rooms.Length);
            Instantiate(rooms[rand], transform.position, Quaternion.identity);
        }

        if(transform.position.x < maxX)
        {
            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
            transform.position = newPos;
        }
        else
        {
            Vector2 newPos = new Vector2(startPoint.x, transform.position.y + moveAmount);
            transform.position = newPos;
        }

        if(transform.position.x <= maxX && transform.position.y <= maxY)
        {
            fillUp();
        }

    }
}
