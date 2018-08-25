using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitManager : MonoBehaviour
{
    public abstract void Attack(int damage, Vector2 position, GameObject damageBox);
    public abstract void Interact(Vector2 position, GameObject genericInteractionBox);
    public abstract int[] Move(float xMove, float yMove, int recentDirectionChange, int facing, Rigidbody2D rb2D, Transform transform, Animator animator, UnitStateMono unit);
}