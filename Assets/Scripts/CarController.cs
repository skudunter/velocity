using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public TMP_Text speedometer;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float motorStrength;
    public float brakeStrength;
    public float tiltAngle;
    public float tiltBias;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float motorStrengthMultiplier = motorStrength;
        float tiltMultiplier = Mathf.Clamp01(1 - Mathf.Abs(transform.rotation.x / tiltAngle));

        if (motor > 0)
        {
            rb.AddForce(transform.forward.normalized * motor * motorStrengthMultiplier * Time.fixedDeltaTime * tiltMultiplier, ForceMode.Impulse);
        }
        else if (motor < 0)
        {
            float forwardSpeed = rb.velocity.magnitude;
            if (forwardSpeed > 0)
            {
                float brakeForce = brakeStrength * forwardSpeed + 1;
                rb.AddForce(-transform.forward.normalized * brakeForce * Time.fixedDeltaTime, ForceMode.Acceleration);
                motorStrengthMultiplier = 0;
            }
        }

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor * motorStrengthMultiplier;
                axleInfo.rightWheel.motorTorque = motor * motorStrengthMultiplier;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        speedometer.text = string.Format("motor {0:N0}\n{1:N0} kph\n{2:N0} tilt", motor, rb.velocity.magnitude, transform.rotation.x);
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
