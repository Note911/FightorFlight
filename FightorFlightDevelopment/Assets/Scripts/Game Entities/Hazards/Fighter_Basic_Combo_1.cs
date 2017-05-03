using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Basic_Combo_1 : Hazard {

    float force = 250;

	public override void OnHit(Collider2D other) {
       BaseGameEntity otherObj = other.gameObject.GetComponent<BaseGameEntity>();
        if(!_audioSource.isPlaying) { 
            _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("WeakKick");
            _audioSource.Play();
            int i = Random.Range(1, 4);
            string smallHit = "Small_Hit_0";
            smallHit = smallHit.Insert(smallHit.Length, i.ToString());
            TempSpriteManager.GetInstance().PlayAnimation(smallHit, other.transform.position + new Vector3(1 * Mathf.Sign(player.transform.localScale.x), 0 , 0), new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), "Foreground2", 6);
        }
       otherObj.DealTempDamage(damage * damageMod);
        Vector3 direction = Vector3.right;
        if (Mathf.Sign(transform.localScale.x) == -1)
            direction = Vector3.left;
       otherObj.Knockback(direction * 2 + Vector3.up, force, 0.1f);
    }
}
