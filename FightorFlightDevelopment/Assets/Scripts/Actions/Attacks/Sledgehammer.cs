using UnityEngine;
using System.Collections;

public class Sledgehammer : Attack {

	   Rigidbody2D rbody;

    protected override void Start() {
        base.Start();
        rbody = _player.GetComponent<Rigidbody2D>();
    }

    public override void Enter() {
        OnExecute();
    }

    public void SpawnHazard() {
        _player.audioSource2.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MeleeMiss3");
        _player.audioSource2.Play();
        Vector2 direction = new Vector2(1.0f, -1.0f);
        if (!_player.facingRight)
            direction.x = -1;
        direction.Normalize();
        EffectManager.GetInstance().Shake(direction, Camera.main.gameObject, 2.0f, 0.2f, 2.0f);
        Vector3 offset = new Vector3(0, 1.0f, 0);
        spawnHazard(0, transform.position + offset, transform.rotation, transform.localScale / 3.0f, true, 1.0f);
    }

    public override void Execute() {
        OnExit();
    }

    public override void Inactive(){
        if (_player.playerState == PlayableEntity.PlayerState.GROUNDED)
            canExecute = true;
    }

    public override void OnEnter() {
        _player.FreezeGravity(0.0f, 0.5f);
        _player.animator.ForceAnimationChange((int)PlayableEntity.PlayerAnimState.ATK_A_D);
        base.OnEnter();
    }

    public override void OnExecute() {
        canExecute = false;
        Invoke("SpawnHazard", 0.4f);
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
