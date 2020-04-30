using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int type;
    public int roomDis;

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
