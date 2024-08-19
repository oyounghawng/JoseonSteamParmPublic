using UnityEngine;

public class preBuild : MonoBehaviour
{
    private bool isBuildable;
    private SpriteRenderer spriteRenderer;
    private GridManager gridManager;
    private Camera _camera;
    private Vector3Int gridPos;
    void Start()
    {
        isBuildable = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = GridManager.Instance;
        _camera = Camera.main;
    }

    //TODO : 건축가능한 타일 정리하기
    void Update()
    {
        Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        gridPos = gridManager.interactableMap.WorldToCell(position);
        gridPos.z = 0;
        transform.position = gridPos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            isBuildable = false;
            spriteRenderer.color = Color.red;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isBuildable = true;
        spriteRenderer.color = Color.white;
    }

    public bool CanBuild()
    {
        return isBuildable;
    }

    public Vector3Int GetBuildPosition()
    {
        return gridPos;
    }
}
