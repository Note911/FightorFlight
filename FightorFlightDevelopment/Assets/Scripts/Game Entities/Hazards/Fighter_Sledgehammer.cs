using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Sledgehammer : Hazard {

    public float force = 3000;

    private bool crit = false;

	public override void OnHit(Collider2D other) {
       BaseGameEntity otherObj = other.gameObject.GetComponent<BaseGameEntity>();
       Vector2 directionVector = (otherObj.transform.position - transform.position).normalized;
       Vector2 horizontalVector = Vector2.right;
       Vector2 knockbackVector = Quaternion.AngleAxis(-90, new Vector3(0.0f, 0.0f, 1.0f)) * directionVector;
        if (Mathf.Sign(transform.localScale.x) == -1.0f) {
            horizontalVector = Vector2.left;
            knockbackVector = Quaternion.AngleAxis(90, new Vector3(0.0f, 0.0f, 1.0f)) * directionVector;
        }
       
        //Get the angle between the vector connecting the attacker and defender
        //angle between vetors : cosTheta = a dot b (if normlized skip we can skip devision step)
        float attackAngle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(directionVector, horizontalVector));
        //if the attack angle is within 30 degrees  then its a crit
       if(Mathf.Abs(attackAngle) <= 40.0f)
            crit = true;
       else
            crit = false;

       switch(_animator.currAnim.currFrame) {
            case (0):
                if(crit) {
                    otherObj.DealTempDamage(damage * damageMod * 2.0f);
                    otherObj.LinearKnockback(knockbackVector, 0.4f);
                    if(!_audioSource.isPlaying) { 
                        _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("StrongPunch");
                        _audioSource.Play();
                        TempSpriteManager.GetInstance().PlayAnimation("Large_Hit_02", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), attackAngle, "Foreground2", 6);

                    }
                }
                else { 
                    otherObj.DealTempDamage(damage * damageMod);
                    otherObj.Knockback(knockbackVector, force, 0.15f);
                    if(!_audioSource.isPlaying) { 
                        _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumPunch");
                        _audioSource.Play();
                        TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position,  new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), attackAngle, "Foreground2", 6);
                    }
                }
                break;
            case (1):
                 if(crit) {
                    otherObj.DealTempDamage(damage * damageMod * 2.0f);
                    otherObj.LinearKnockback(knockbackVector, 0.4f);
                    if(!_audioSource.isPlaying) { 
                        _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("StrongPunch");
                        _audioSource.Play();
                        TempSpriteManager.GetInstance().PlayAnimation("Large_Hit_02", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), attackAngle, "Foreground2", 6);
                    }
                }
                 else {
                    otherObj.DealTempDamage(damage * damageMod);
                    otherObj.Knockback(knockbackVector, force, 0.15f);
                    if(!_audioSource.isPlaying) { 
                        _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumPunch");
                        _audioSource.Play();
                        TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), attackAngle, "Foreground2", 6);
                    }
                }
                break;
            case (2):
                otherObj.DealTempDamage(damage * damageMod);
                otherObj.Knockback(knockbackVector, force, 0.05f);
                if(!_audioSource.isPlaying) { 
                    _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("WeakKick");
                    _audioSource.Play();
                    TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), attackAngle, "Foreground2", 6);
                }
                break;
       }
    }
}
