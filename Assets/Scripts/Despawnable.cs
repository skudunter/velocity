using UnityEngine;

public class Despawnable : MonoBehaviour
{
    private bool hasCollision;
    private float timer;

    private void Start()
    {
        hasCollision = false;
        timer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasCollision = true;
        timer = 0f;
    }

    private void Update()
    {
        if (!hasCollision)
        {
            timer += Time.deltaTime;

            if (timer >= 20f)
            {
                Destroy(gameObject);
            }
        }
    }
}
