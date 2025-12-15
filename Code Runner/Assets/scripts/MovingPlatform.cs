using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        // Move platform toward the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch target when reached
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (target == pointB.position)
                target = pointA.position;
            else
                target = pointB.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
