using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySand : Enemy
{
    // Different variations of the sprite when hit
    [SerializeField] private Sprite[] damage_sprites;

    public override void TakeDamage(GameObject sender, int damage){
        base.TakeDamage(sender, damage);
        // Appears damaged
        if (healthPoints == 4){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[0];
        }
        if (healthPoints == 2){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[1];
        }
    }
}
