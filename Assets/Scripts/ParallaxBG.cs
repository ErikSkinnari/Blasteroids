using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    [SerializeField]
    public Transform playerTransform;
    private Vector3 lastPlayerPosition;

    private void Start()
    {
        playerTransform = GetComponent<PlayerBehaviour>().transform;
        lastPlayerPosition = playerTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = playerTransform.position - lastPlayerPosition;
        float parallaxMultiplyer = .5f;
        transform.position += deltaMovement * parallaxMultiplyer;
        lastPlayerPosition = playerTransform.position;
    }

}
