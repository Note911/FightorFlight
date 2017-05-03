using UnityEngine;
using System.Collections;

public class Block : Action {

    public Block() : base(){}

    public override void Enter() {
        _player.animator.ChangeAnimation((int)PlayableEntity.PlayerAnimState.BLOCK);
        TempSpriteManager.GetInstance().PlayAnimation("Pow", _player.transform.position, new Vector2(4.5f,4.5f), "Foreground", 0);
        _player.blocking = true;
    }

    public override void Execute() {
        if (_player.animator.currAnim.finished)
        {
            Exit();
        }
    }
    public override void Exit() {
        _player.blocking = false;
    }
}
