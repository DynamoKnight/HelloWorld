using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGiant : Enemy
{
    // Different variations of the sprite when hit
    [SerializeField] private Sprite[] damage_sprites;

    protected override void Start(){
        // Pity drop
        dropChance = 10;
        base.Start();
    }

    public override void TakeDamage(GameObject sender, int damage){
        base.TakeDamage(sender, damage);
        // Appears damaged
        if (healthPoints == 9){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[0];
        }
        if (healthPoints == 7){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[1];
        }
        if (healthPoints == 5){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[2];
        }
        if (healthPoints == 2){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[3];
        }
    }
}
