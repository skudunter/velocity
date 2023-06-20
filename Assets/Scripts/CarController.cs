using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
    public TMPro.TMP_Text speedometer;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private Rigidbody rb;

    [SerializeField]
    private float motorStrength;

    [SerializeField]
    private float brakeStrength;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float motorStrengthMultiplier = motorStrength;

        if (motor < 0)
        {
            float forwardSpeed = rb.velocity.magnitude;
            if (forwardSpeed > 0)
            {
                float brakeForce = brakeStrength * forwardSpeed + 1;
                rb.AddForce(-rb.velocity.normalized * brakeForce, ForceMode.Acceleration);
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
            WheelHit hit;
            bool isWheelSlippingLeft =
                axleInfo.leftWheel.GetGroundHit(out hit)
                && (Mathf.Abs(hit.sidewaysSlip) > 0.2f || Mathf.Abs(hit.forwardSlip) > 0.2f);
            bool isWheelSlippingRight =
                axleInfo.leftWheel.GetGroundHit(out hit)
                && (Mathf.Abs(hit.sidewaysSlip) > 0.2f || Mathf.Abs(hit.forwardSlip) > 0.2f);
            Debug.Log("isWheelSlippingLeft: " + isWheelSlippingLeft);
            Debug.Log("isWheelSlippingRight: " + isWheelSlippingRight);
        }
        speedometer.text = string.Format("motor {0:N0}\n{1:N0} kph", motor, rb.velocity.magnitude);
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
