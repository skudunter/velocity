using UnityEngine;

public class Despawnable : MonoBehaviour
{
    private bool hasCollision;
    private float timer;
    [SerializeField]
    private float timeToDespawn = 20f;

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

            if (timer >= timeToDespawn)
            {
                Destroy(gameObject);
            }
        }
    }
}
