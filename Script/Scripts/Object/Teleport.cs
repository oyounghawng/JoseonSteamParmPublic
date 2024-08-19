using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform targetPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = targetPosition.position;
        }
    }
}
