using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool activateShake;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shake(0.1f, 0.08f));
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector2 orignalPosition = transform.position;
        float elapsed = 0f;

        if (activateShake == true)
        {
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.position = new Vector2(orignalPosition.x + x, orignalPosition.y + y);
                elapsed += Time.deltaTime;
                yield return 0;
            }
            transform.position = orignalPosition;
        }
        
        
    }
}
