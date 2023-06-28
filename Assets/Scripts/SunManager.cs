using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    public float distanceFromPlayer;

    void LateUpdate()
    {
        Vector3 sunPosition =
            playerTransform.position + (playerTransform.forward * -distanceFromPlayer);
        transform.position = sunPosition;
    }
}
