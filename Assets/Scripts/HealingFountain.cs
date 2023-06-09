using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable
{
    // Start is called before the first frame update
    public int healingAmount = 1;

    private float healCooldown = 1.0f;
    private float lastHeal;

    protected override void OnCollide(Collider2D collider2D)
    {
        if (collider2D.name != "Player")
        {
            return;
        }

        if (Time.time - lastHeal > healCooldown)
        {
            lastHeal = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }
    }
}
