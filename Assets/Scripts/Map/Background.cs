using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] private Vector2 parallaxEffectMultiplier;
    //[SerializeField] private bool infinytiX;
    //[SerializeField] private bool infinitiY;

    private Transform cameraTrans;
    private Vector3 lastCamPos;
    private float textureSizeX;
    private float textureSizeY;


    void Start()
    {
        cameraTrans = Camera.main.transform;
        lastCamPos = cameraTrans.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureSizeX = texture.width / sprite.pixelsPerUnit;
        textureSizeY = texture.width / sprite.pixelsPerUnit;
    }


    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTrans.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPos = cameraTrans.position;

        /*
        if (infinytiX) { 
            if (Mathf.Abs(cameraTrans.position.x - transform.position.x) >= textureSizeX)
            {
                float offsetPositionX = (cameraTrans.position.x - transform.position.x) % textureSizeX;
                transform.position = new Vector3(cameraTrans.position.x + offsetPositionX, transform.position.y);
            }
        }
        if (infinitiY)
        {
            if (Mathf.Abs(cameraTrans.position.y - transform.position.y) >= textureSizeX)
            {
                float offsetPositionY = (cameraTrans.position.y - transform.position.y) % textureSizeY;
                transform.position = new Vector3(cameraTrans.position.x, transform.position.y + offsetPositionY);
            }
        }
        */
    }
}
