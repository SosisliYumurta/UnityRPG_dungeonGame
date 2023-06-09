using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int damage = 1;
    public float pushForce = 5.0f;

    protected override void OnCollide(Collider2D collider2D)
    {
        if (collider2D.tag == "Fighter" && collider2D.name == "Player")
        {
            //Create a new damage object, before sending it to the player
            Damage damage = new Damage
            {
                damageAmount = this.damage,
                origin = transform.position,
                pushForce = pushForce
            };

            collider2D.SendMessage("ReceiveDamage", damage);
        }
    }
}
