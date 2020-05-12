using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Vector2 startPoint;

    public GameObject Player;

    public Transform[] startPoses;
    public GameObject[] rooms;

    public GameObject winObj;

    private int direction;
    private int oldRoomDis;
    public float moveAmount = 10;

    public float maxX;
    public float minX;
    public float maxY;
    private bool generationStop;

    public LayerMask room;

    private int upCounter;


    private void Start()
    {
        //finder det sted der skal startes fra, laver det første rum og rykker spilleren dertil.
        int randSTPos = Random.Range(0, startPoses.Length);
        transform.position = startPoses[randSTPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);
        oldRoomDis = rooms[0].GetComponent<RoomType>().roomDis;

        Player.transform.position = startPoses[randSTPos].position;

        direction = Random.Range(1, 6);

       Move();
    }

    private void placeRoom(bool venstre)
    {
        upCounter = 0;

        //beregner hvor langt der skal rykkes for at rumne ikke overlapper eller laver mellemrum
        int rand = Random.Range(0, 4);
        int moveDis = oldRoomDis + rooms[rand].GetComponent<RoomType>().roomDis;

        Vector2 newPos;

        //finder ud af hvad vej vi skal rykkes i og rykker
        if (venstre)
        {
            newPos = new Vector2(transform.position.x - moveDis, transform.position.y);
        }
        else
        {
            newPos = new Vector2(transform.position.x + moveDis, transform.position.y);
        }

        transform.position = newPos;

        //placere et rum
        Instantiate(rooms[rand], transform.position, Quaternion.identity);

        //ændre oldroomdis til det nuværende rums roomDis var.
        oldRoomDis = rooms[rand].GetComponent<RoomType>().roomDis;
    }


    private void Move()
    {

        if(direction == 1 || direction == 2)
        {// Move right
            if(transform.position.x < maxX)
            {//tjekker om den kan rykke sig den givne retning uden at overskride.

                placeRoom(false);
                
                //finder ny retning -venstre 
                direction = Random.Range(1, 6);
                
                if (direction == 3)
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
                //kan den ikke gå længere til højre, gå op.
                direction = 5;
            }
        }
        else if(direction == 3 || direction == 4)
        {//move left
            if (transform.position.x > minX)
            {
                placeRoom(true);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
            
        }
        else if (direction == 5)
        {//move up
            //læg en til upcount
            upCounter++;


            if(transform.position.y < maxY)
            {//hvis vi kan fortsætte op
                
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                roomDetection.GetComponent<RoomType>().DestroyObj();

                if (upCounter >= 2)
                {//er der blevet gået op flere gange i træk, erstatter vi det forige rum med et der har åbninger i alle retninger  
                    Instantiate(rooms[3], transform.position, Quaternion.identity);
                }
                else
                {//ellers erstater vi rummet med et andet der har hul i toppen, højre og til venstre.
                    int randTopRoom = Random.Range(1, 4);
                    if(randTopRoom == 2)
                    {
                        randTopRoom = 1;
                    }
                    Instantiate(rooms[randTopRoom], transform.position, Quaternion.identity);
                }
                
                //ryker op og sætter et rum med hul i bunden.
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                oldRoomDis = rooms[rand].GetComponent<RoomType>().roomDis;

                //finder ny retning
                direction = Random.Range(1, 6);
                
            }
            else
            {
                //kan vi ikke gå længere op så stopper vi, ryk mapgeneratoren til start og fyld ud.
                Instantiate(winObj, transform.position, Quaternion.identity);
                generationStop = true;
                transform.position = startPoint;
                fillUp();
            }
        }
        //tjekker om vi er færdige med at generere hoved ruten.
        if (!generationStop)
        {
            Move();
        }
    }

    public void fillUp()
    {
        //tjekker om der er et rum i forvejen.
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
        if(roomDetection == null)
        {//er der ikke sætter vi et tilfældigt rum.
            int rand = Random.Range(0, rooms.Length);
            Instantiate(rooms[rand], transform.position, Quaternion.identity);
        }

        if(transform.position.x < maxX)
        {//kan vi fortsætte mod højre, gør vi det.
            Vector2 newPos = new Vector2(transform.position.x + (moveAmount * 3f), transform.position.y);
            transform.position = newPos;
        }
        else
        {//ellers rykker vi helt til venstre og går x op.
            Vector2 newPos = new Vector2(startPoint.x, transform.position.y + moveAmount);
            transform.position = newPos;
        }

        if(transform.position.x <= maxX && transform.position.y <= maxY)
        {//tjekker om vi er færdige, med at fylde ud.
            fillUp();
        }
    }
}
