using UnityEngine;
using System.Collections;


public class BasicCombo3 : Attack {

    Rigidbody2D rbody;

    protected override void Start() {
        base.Start();
        rbody = _player.GetComponent<Rigidbody2D>();
    }

    public override void Enter() {
        OnExecute();
    }

    public void SpawnHazard() {
        _player.audioSource2.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MeleeMiss2");
        _player.audioSource2.Play();
        Vector2 direction = Vector2.right;
        if (!_player.facingRight)
            direction = Vector2.left;
        EffectManager.GetInstance().Shake(direction, Camera.main.gameObject, 1.0f, 0.25f, 2.0f);
        Vector3 offset = new Vector3(0, 0, 0);
        spawnHazard(0, transform.position + offset, transform.rotation, transform.localScale, true, 1.0f);
    }

    public override void Execute() {
        OnExit();
    }

    public override void OnEnter() {
        if(Mathf.Abs(rbody.velocity.x) < _player.maxSpeed / 2.0f)
            rbody.AddForce(new Vector2(Mathf.Sign(_player.transform.localScale.x) * 250, 0));
        _player.animator.ForceAnimationChange((int)PlayableEntity.PlayerAnimState.ATK_BASIC_COMBO3);
        base.OnEnter();
    }

    public override void OnExecute() {
        Invoke("SpawnHazard", 0.1f);
        base.OnExecute();
    }

    public override void OnExit() {
        base.OnExit();
    }
    public override void Exit() {
        if (_player.animator.currAnim.finished)
            Finish();
    }

   public override void Finish() {
        base.Finish();
   }
}