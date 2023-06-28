using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject player;
    public float movementForce = 10f;
    public float rotationSpeed = 5f;
    public float slipperiness = 0.5f;
    private Rigidbody rb;

    private void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            Vector3 movementForceVector = playerDirection * movementForce;
            Vector3 relativeVelocity = transform.InverseTransformDirection(
                GetComponent<Rigidbody>().velocity
            );
            Vector3 slipForce = -relativeVelocity * slipperiness;

            rb.AddForce(movementForceVector + slipForce, ForceMode.Force);

            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}
