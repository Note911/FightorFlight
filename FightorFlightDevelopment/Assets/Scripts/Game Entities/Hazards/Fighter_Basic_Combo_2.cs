using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Basic_Combo_2 : Hazard {

    float force = 1000;

	public override void OnHit(Collider2D other) {
       BaseGameEntity otherObj = other.gameObject.GetComponent<BaseGameEntity>();
       if(!_audioSource.isPlaying) { 
           _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumKick");
           _audioSource.Play();
           TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(0.7f * Mathf.Sign(player.transform.localScale.x), 0.7f), "Foreground2", 6);
       }
       otherObj.DealTempDamage(damage * damageMod);
       if(Mathf.Sign(transform.localScale.x) == 1.0f)
            otherObj.Knockback(new Vector2(2.0f,1.0f).normalized, force, 0.2f);
       else
            otherObj.Knockback(new Vector2(-2.0f,1.0f).normalized, force, 0.2f);
    }
}
