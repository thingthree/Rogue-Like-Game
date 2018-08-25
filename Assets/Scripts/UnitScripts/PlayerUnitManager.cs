using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitManager : UnitManager
{
    public override void Attack(int damage, Vector2 position, GameObject damageBox)
    {
        GameObject toInstantiate = damageBox;
        GameObject instance = Instantiate(toInstantiate, new Vector2(position.x + 1, position.y + 1),
        Quaternion.identity) as GameObject;
        Destroy(instance, .1f);
        return;
    }

    public override void Interact(Vector2 position, GameObject genericInteractionBox)
    {
        GameObject toInstantiate = genericInteractionBox;
        GameObject instance = Instantiate(toInstantiate, new Vector2(position.x, position.y),
        Quaternion.identity) as GameObject;
        return;
    }

    public override int[] Move(float xMove, float yMove, int recentDirectionChange, int facing, Rigidbody2D rb2D, Transform transform, Animator animator, UnitStateMono unit)
    {
        if (xMove == 0 && yMove == 0)                       // not trying to move
        {
            recentDirectionChange = 0;
        }
        if (xMove == 0 && yMove != 0)                       // trying to move in y only
        {
            animator.SetBool("PlayerMove", true);
            recentDirectionChange = 0;
            rb2D.AddForce(transform.up * yMove);
            if (yMove > 0)
                facing = 0;
           
            if (yMove < 0)
                facing = 1;
        }

        if (xMove != 0 && yMove == 0)                       // trying to move in x only
        {
            animator.SetBool("PlayerMove", true);
            recentDirectionChange = 0;
            rb2D.AddForce(transform.right * xMove);
            if (xMove > 0)
                facing = 3;
            if (xMove < 0)
                facing = 2;
        }
        if (xMove != 0 && yMove != 0)                       // trying to move in y and x
        {
            animator.SetBool("PlayerMove", true);
            if (recentDirectionChange == 1)                 // and we just swapped directions
            {
                if (facing == 0)                                    // and are facing up
                {
                    rb2D.AddForce(transform.up * yMove);
                    if (yMove > 0)
                    {
                        facing = 0;
                    }
                }
                if (facing == 1)                                    // and are facing down
                {
                    rb2D.AddForce(transform.up * yMove);
                    if (yMove < 0)
                    {
                        facing = 1;
                    }
                }
                if (facing == 2)                                    // and are facing left
                {
                    rb2D.AddForce(transform.right * xMove);
                    if (xMove > 0)
                    {
                        facing = 3;
                    }
                }
                if (facing == 3)                                    // and are facing right
                {
                    rb2D.AddForce(transform.right * xMove);
                    if (xMove < 0)
                    {
                        facing = 2;
                    }
                }
            }
                                                        // and we haven't swapped directions

            else if (facing == 0)                                    // and are facing up
            {
                recentDirectionChange = 1;
                rb2D.AddForce(transform.right * xMove);
                if (xMove > 0)
                {
                    facing = 3;
                }
                if (xMove < 0)
                {
                    facing = 2;
                }
            }                                
            
                
            else  if (facing == 1)                                    // and are facing down
            {
                recentDirectionChange = 1;
                rb2D.AddForce(transform.right * xMove);
                if (xMove > 0)
                {
                    facing = 3;
                }
                if (xMove < 0)
                {
                    facing = 2;
                }
            }
            else if (facing == 2)                                    // and are facing left
            {
                recentDirectionChange = 1;
                rb2D.AddForce(transform.up * yMove);
                if (yMove > 0)
                {
                    facing = 0;
                }
                rb2D.AddForce(transform.up * yMove);
                if (yMove < 0)
                {
                    facing = 1;
                }
            }
            else if (facing == 3)                                    // and are facing right
            {
                recentDirectionChange = 1;
                rb2D.AddForce(transform.up * yMove);
                if (yMove > 0)
                {
                    facing = 0;
                }
                rb2D.AddForce(transform.up * yMove);
                if (yMove < 0)
                {
                    facing = 1;
                }
            }
        }
      
        int[] returnedValue = new int[] { recentDirectionChange, facing };
        return returnedValue;
    }

    public void Lift(bool strong, bool weak, bool heavy)
    {

        if (heavy == true)
        {
            if (strong == true)
            {
                Hodl();
            }

            else
            {
                Struggle();
            }
        }



        if (heavy == false)
        {
            if (weak == true)
            {
                Struggle();
            }
            else
            {
                Hodl();
            }
        }
    }

    private void Struggle()
    {
        throw new NotImplementedException();
    }

    private void Hodl()
    {
        throw new NotImplementedException();
    }
}
