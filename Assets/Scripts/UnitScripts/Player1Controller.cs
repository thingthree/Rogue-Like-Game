using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Random = UnityEngine.Random;
using System.Linq;

public class Player1Controller : MonoBehaviour

{
    // components
    private Animator animator;
    private Rigidbody2D rb2D;
    private Vector2 position;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    // components, scripts
    public static UnitStateSerial playerStateSerial;
    public static UnitStateMono playerStateMono;

    // variables used to determine how player moves when given input
    private int facingHorizontal = 0;
    private int moveCount = 0;
    private int recentDirectionChange = 0;

    // start
    void Start()
    {
        // component references
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStateMono = GetComponent<UnitStateMono>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        // sets player position from boardScript.player,
        // sets player facing and active state to 0(up) and true
        playerStateSerial = new UnitStateSerial 
            (GameManager.boardScript.player.transform, 0, true);

        // use playerstatemono's memento function to revert from 
        playerStateMono.Memento = playerStateSerial;
        playerStateMono = playerStateMono.Revert(playerStateMono);
    }

    private void Update()
    {
        // if we can't move, don't
        if (GameManager.pause == false)
            return;
        // if we can move, update our position, ensure playerstatemono is set
        // to active and call the function which moves the player based on 
        else
        {
            playerStateMono.Transform = gameObject.transform;
            playerStateMono.Active = true;
            PlayerManual();
            animator.SetInteger("Direction", playerStateMono.Facing);
            if (playerStateMono.Facing == 3)
                spriteRenderer.flipX = true;
            else
            {
                spriteRenderer.flipX = false;
            } 
        }
    }

    private void Player1Auto()
    {
    }

    private void PlayerManual()
    {
        animator.SetBool("PlayerMove", false);
        float xMove = 0;
        float yMove = 0;
        xMove = (int)(Input.GetAxisRaw("Horizontal"));
        yMove = (int)(Input.GetAxisRaw("Vertical"));
        int[] moveFeedBack = GameManager.playerUnitScript.Move(xMove, yMove, recentDirectionChange, playerStateMono.Facing, rb2D, transform, animator, playerStateMono);
        recentDirectionChange = moveFeedBack[0];
        playerStateMono.Facing = moveFeedBack[1];
        GameManager.boardScript.playerPositionIndicator.transform.position = new Vector3(Camera.main.transform.position.x + (transform.position.x/(GameManager.boardScript.roomSpacingX*2)) + 9,
                Camera.main.transform.position.y + (transform.position.y/(GameManager.boardScript.roomSpacingY*2)) + 4, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GenericInteraction")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}