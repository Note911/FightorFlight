using UnityEngine;
using System.Collections;

public class UpperCut : Attack {

    private float charge = 0.0f;
    public float chargeRate = 33.0f;  // determines the charge rate.  a rate of 1 is 100% of time so it would take 100 seconds to reach full charge.
    public float maxCharge = 100.0f;

    private Color originalColor;
    private SpriteRenderer playerRenderer;

    protected override void Start() {
        base.Start();
        playerRenderer = _player.GetComponent<SpriteRenderer>();
        originalColor = playerRenderer.color;
    }

    public override void Enter() {
        _player.animator.ChangeAnimation((int)PlayableEntity.PlayerAnimState.ATK_G_U_M);
        if(_player.animator.animState == (int)PlayableEntity.PlayerAnimState.ATK_G_U_M) { 
            playerRenderer.color = new Color((Random.Range(0, 100)/ 100.0f), (Random.Range(0, 100)/ 100.0f), (Random.Range(0, 100)/ 100.0f));
            charge += Time.deltaTime * chargeRate;
            if (charge >= maxCharge)
                OnExecute();
            else if (!Input.GetButton(_player.attackButton))
                OnExecute();
        }
    }

    public void SpawnHazard() {
        _player.audioSource2.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("MeleeMiss3");
        _player.audioSource2.Play();
        EffectManager.GetInstance().Shake(Vector2.up, Camera.main.gameObject, 2.0f, 0.2f, 2.0f);
        Vector3 offset = new Vector3(0, 0, 0);
        if(_player.facingRight)
            spawnHazard(0, transform.position + offset, Quaternion.Euler(0,0,90), transform.localScale, false, 1.0f + charge / maxCharge);
        else
            spawnHazard(0, transform.position + offset, Quaternion.Euler(0,0,-90), transform.localScale, false, 1.0f + charge / maxCharge);
    }

    public override void Execute() {
        playerRenderer.color = originalColor;
        OnExit();
    }

    public override void OnEnter() {
        _player.animator.ForceAnimationChange((int)PlayableEntity.PlayerAnimState.ATK_G_U_B);
        base.OnEnter();
    }

    public override void OnExecute() {
        _player.animator.ForceAnimationChange((int)PlayableEntity.PlayerAnimState.ATK_G_U_E);
        Invoke("SpawnHazard", 0.25f);
        base.OnExecute();
    }

    public override void OnExit() {

        base.OnExit();
    }
    public override void Exit() {
        if(_player.animator.currAnim.finished)
            Finish();
    }

    public override void Finish() {
        charge = 0.0f;
        base.Finish();
    }
}
