using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Heavy_Forward : Hazard {
    public float force = 3000f;
    public float critRange = 2.0f;

	public override void OnHit(Collider2D other) {
       BaseGameEntity otherObj = other.gameObject.GetComponent<BaseGameEntity>();
        Vector2 directionVector = Vector2.right;

        if (Mathf.Sign(transform.localScale.x) == -1)
            directionVector = Vector2.left;

        float cs = Mathf.Cos(transform.rotation.eulerAngles.z);
        float sn = Mathf.Sin(transform.rotation.eulerAngles.z);

        directionVector = new Vector2(directionVector.x * cs - directionVector.y * sn, directionVector.x * sn + directionVector.y * cs);


       switch(_animator.currAnim.currFrame) {
            case (0):
                //Crit Frame
               if (Mathf.Abs(((Vector2)(transform.position) - (Vector2)(other.transform.position)).magnitude) <= critRange) {
                    if(!_audioSource.isPlaying) { 
                        _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("StrongPunch");
                        _audioSource.Play();
                        TempSpriteManager.GetInstance().PlayAnimation("Large_Hit_02", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), "Foreground2", 6);
                    }
                    otherObj.DealTempDamage(damage * damageMod * 2.0f);
                    otherObj.LinearKnockback(directionVector, 0.4f);
                }
                else {
                    if(!_audioSource.isPlaying) {
                        _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumPunch");
                        _audioSource.Play();
                        TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), "Foreground2", 6);
                    }
                    otherObj.DealTempDamage(damage * damageMod);
                    otherObj.Knockback(directionVector, force, 0.2f);
                }
                break;
            case (1):
                if(!_audioSource.isPlaying) {
                    _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumPunch");
                    _audioSource.Play();
                    TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), "Foreground2", 6);
                }
                otherObj.DealTempDamage(damage * damageMod * 0.9f);
                otherObj.Knockback(directionVector, force, 0.15f);
                break;
            case (2):
                if(!_audioSource.isPlaying) {
                    _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumPunch");
                    _audioSource.Play();
                    TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), "Foreground2", 6);
                }
                otherObj.DealTempDamage(damage * damageMod * 0.8f);
                otherObj.Knockback(directionVector, force, 0.10f);
                break;
            case (3):
                if(!_audioSource.isPlaying) { 
                    _audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MediumPunch");
                    _audioSource.Play();
                    TempSpriteManager.GetInstance().PlayAnimation("Mid_Hit_01", other.transform.position, new Vector2(1 * Mathf.Sign(player.transform.localScale.x), 1), "Foreground2", 6);
                }
                otherObj.DealTempDamage(damage * damageMod * 0.7f);
                otherObj.Knockback(directionVector, force, 0.05f);
                break;
       }
    }
}
