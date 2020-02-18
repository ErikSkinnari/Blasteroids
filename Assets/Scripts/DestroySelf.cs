using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    float offset = 30f;

    void Update()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height + offset 
            || screenPosition.y < 0 - offset 
            || screenPosition.x > Screen.width + offset 
            || screenPosition.x < 0 - offset)
        {
            Debug.Log(this.gameObject.name + " removed, because it was out of bounds");
            Destroy(this.gameObject);
        }
    }
}
