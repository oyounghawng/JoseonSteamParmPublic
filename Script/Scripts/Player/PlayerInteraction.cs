using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInputController playerInputController;
    private PlayerMovement playerMovement;

    private float rayDistance = 1.5f;
    private bool isInteraction = false;
    public LayerMask layerMask;
    public IInteractable curInteractable;
    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        playerInputController.OnInteractionEvent += Interaction;
    }

    private void Interaction()
    {
        if (isInteraction)
            return;

        isInteraction = true;
        Vector2 dir = playerMovement.LookDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, layerMask);
        Debug.DrawRay(transform.position, dir * rayDistance, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent(out curInteractable))
            {
                curInteractable?.Interact();
            }
        }
        StartCoroutine(OnInteraction());
    }

    IEnumerator OnInteraction()
    {
        yield return new WaitForSeconds(0.5f);
        isInteraction = false;
    }

}
