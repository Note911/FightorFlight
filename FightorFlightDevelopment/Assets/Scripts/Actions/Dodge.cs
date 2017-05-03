using UnityEngine;
using System.Collections;

public class Dodge : Action {

    public bool entering = false;
    public bool exiting = false;

    public float dodgeSpeed = 0.15f;        //How long it takes for the _player to re-appear
    protected float timeSinceDodged = 0.0f;
      
    public float dodgeDistance = 15.0f;

    protected Vector2 input;
    protected Vector2 prevVel;
    protected Vector2 startPos;

    protected float distanceTraveled;


    public void BeginDodge(Vector2 inputDir)
    {
        if (canExecute) {
            _player.dodging = true;
            _player.canDodge = false;
            Invoke("PlayerDodgeCooldown", actionCD);
            input = inputDir;
             _player.animator.ForceAnimationChange((int)PlayableEntity.PlayerAnimState.DODGE_ENTER);

            Color color = new Color(_player.color.r, _player.color.g, _player.color.b, 0.75f);
            Color color2 = new Color(_player.color2.r, _player.color2.g, _player.color2.b, 0.75f);

            TempSpriteManager.GetInstance().PlayAnimation("Dodge_Gleem_Back", (Vector2)_player.transform.position, new Vector2(1, 1), Vector2.zero, 0.0f, "Foreground2", 0, 0.0f, color);
            TempSpriteManager.GetInstance().PlayAnimation("Dodge_Gleem_Color", (Vector2)_player.transform.position, new Vector2(1, 1), Vector2.zero, 0.0f, "Foreground2", 1, 0.0f, color2);

            _rbody.isKinematic = true;
            _player.GetComponent<CircleCollider2D>().enabled = false;
            _rbody.velocity = Vector2.zero;

            prevVel = _rbody.velocity;
            _player.invincible = true;

            startPos = (Vector2)_player.transform.position;
            base.OnEnter();
        }
    }

    public override void OnEnter()
    {

        base.OnEnter();
    }

    private void PlayerDodgeCooldown() {
        _player.canDodge = true;
    }


    public override void Enter()
    {
        //if the current animation is on its end frame, the _player has finished vanishing, so move them and que the dust cloud, also flag entering false and exiting true
        if (_player.animator.currAnim.currFrame == _player.animator.currAnim.endFrame)
        {
            float inX = Input.GetAxis(_player.horizontalAxis);
            float inY = Input.GetAxis(_player.verticalAxis);

            input = new Vector2(inX, -inY).normalized;
            _player.animator.ChangeAnimation((int)PlayableEntity.PlayerAnimState.NONE);



            //move on to execute Call OnExectue();
            OnExecute();
        }
    }

    public override void Execute()
    {
        timeSinceDodged += Time.deltaTime;
        //Dodge Timer
        if (timeSinceDodged >= dodgeSpeed)
        {
            timeSinceDodged = 0.0f;
            _player.animator.ChangeAnimation((int)PlayableEntity.PlayerAnimState.DODGE_EXIT);



            //Disble physics temporarily
            _rbody.isKinematic = false;
            _player.GetComponent<CircleCollider2D>().enabled = true;
            Vector2 dodgeVector = Vector2.zero;

            //Determine exit location
            dodgeVector = input * dodgeDistance;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)_player.transform.position, input, dodgeDistance, 1 << LayerMask.NameToLayer("Ground"));
            distanceTraveled = hit.distance;
            

            //Move the player
            if (hit.collider != null) {
                _player.transform.position = hit.point;
            }
            else
                _player.transform.position += (Vector3)dodgeVector;



           

            //Move to exit call End();
            OnExit();
        }
    }

    public override void OnExecute()
    {
        //play streek
     
        base.OnExecute();
    }

    public override void Exit() {
        if (_player.animator.currAnim.currFrame == _player.animator.currAnim.endFrame)
            Finish();   
    }

    public override void OnExit()
    {
        float rotationAngle = Vector2.Angle(Vector2.right, input);
        float scaleX = distanceTraveled / 3.0f;
        if (scaleX == 0.0f)
            scaleX = dodgeDistance / 2.8f;

        Debug.Log("ScaleX " + scaleX + " Rotation " + rotationAngle);

        if (input.y < -0.001)
            rotationAngle = -rotationAngle;


        TempSpriteManager.GetInstance().PlayAnimation("Dodge_Blur_Back", startPos, new Vector2(scaleX, 0.8f), Vector2.zero, rotationAngle, "Foreground2", 0, 0.0f, _player.color);
        TempSpriteManager.GetInstance().PlayAnimation("Dodge_Blur_Color", startPos, new Vector2(scaleX, 0.8f), Vector2.zero, rotationAngle, "Foreground2", 1, 0.0f, _player.color2);



        base.OnExit();
    }

    public override void Finish()
    {
        Color color = new Color(_player.color.r, _player.color.g, _player.color.b, 0.75f);
        Color color2 = new Color(_player.color2.r, _player.color2.g, _player.color2.b, 0.75f);

        TempSpriteManager.GetInstance().PlayAnimation("Dodge_Gleem_Back", (Vector2)_player.transform.position, new Vector2(1, 1), Vector2.zero, 0.0f, "Foreground2", 0, 0.0f, color);
        TempSpriteManager.GetInstance().PlayAnimation("Dodge_Gleem_Color", (Vector2)_player.transform.position, new Vector2(1, 1), Vector2.zero, 0.0f, "Foreground2", 1, 0.0f, color2);

        _player.dodging = false;
        _player.invincible = false;


        float inX = Input.GetAxis(_player.horizontalAxis);
        float inY = Input.GetAxis(_player.verticalAxis);

        if (_player.vanishCounter)
        {
            _player.vanishCounter = false;
            if (_player.facingRight && inX < 0)
                _player.FlipX();
            else if (!_player.facingRight && inX > 0)
                _player.FlipX();
            input = new Vector2(inX, -inY).normalized;
            _player.Attack(inX, inY);
        }
        else
        {
            //_rbody.velocity = prevVel;
            _rbody.velocity = Vector2.zero;
        }
        base.Finish();
    }

}
