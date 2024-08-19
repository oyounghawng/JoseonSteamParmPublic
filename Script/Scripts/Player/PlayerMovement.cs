using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputController playerInputController;
    private Rigidbody2D rigidBody2D;
    
    private Vector2 dir;
    private Vector2 lookDir;
    public Vector2 LookDir { get { return lookDir; }}


    private float moveSpeed = 5f;
    private float runSpeed = 10f;
    private bool isRun;

    //public Tilemap tilemap;
    //public TileBase outlineTile;

    private Vector3Int previousTilePosition;


    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        //tilemap = GameObject.Find("OutLine").GetComponent<Tilemap>();
    }

    private void Start()
    {
        playerInputController.OnMoveEvent += Move;
    }

    private void FixedUpdate()
    {
        if(!playerInputController.isAction)
        {
            if (isRun)
                rigidBody2D.velocity = dir * runSpeed;
            else
                rigidBody2D.velocity = dir * moveSpeed;
        }
        else
            rigidBody2D.velocity = Vector2.zero;

        //Vector3Int currentTilePosition = tilemap.WorldToCell(transform.position);

        ////�÷��̾ ���ϴ� ���⿡ �ܰ��� Ÿ�� ��ġ
        //Vector3Int direction = new Vector3Int(Mathf.RoundToInt(lookDir.x), Mathf.RoundToInt(lookDir.y), 0);
        //Vector3Int targetTilePosition = currentTilePosition + direction;

        //if (targetTilePosition != previousTilePosition)
        //{
        //    lookDir.y = lookDir.y < 0 ? lookDir.y + 0.5f : lookDir.y;

        //    //���� �ܰ��� Ÿ�� ����
        //    tilemap.SetTile(previousTilePosition, null);
        //    //���ο� �ܰ��� Ÿ�� ��ġ
        //    tilemap.SetTile(targetTilePosition, outlineTile);

        //    previousTilePosition = targetTilePosition;
        //}
    }

    private void Move(Vector2 direction)
    {
        dir = direction;
        if(direction != Vector2.zero)
        {
            lookDir = direction.normalized;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Managers.Game.onChangedPlayerLocation?.Invoke(collision.tag);
    }
}
