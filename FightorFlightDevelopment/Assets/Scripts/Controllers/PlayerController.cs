using UnityEngine;
using System.Collections;

public class PlayerController : Controller {

    public string jumpButton; //String reference to jump button axis
    public string lightAttackButton;
    public string attackButton; //String reference to attack button axis
    public string dodgeButton;      //String reference to the dodge button axis
    public string horizontalAxis;  //String reference to horizontal axis
    public string verticalAxis;  //String reference to horizontal axis

    private Rigidbody2D rbody;      //Reference to the players rigidbody

    public PlayerController(PlayableEntity player) : base(player) {
        jumpButton = player.jumpButton;
        lightAttackButton = player.lightAttackButton;
        attackButton = player.attackButton;
        dodgeButton = player.dodgeButton;
        horizontalAxis = player.horizontalAxis;
        verticalAxis = player.verticalAxis;
        rbody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void Update () {
        ///UPDATE PHYSICS AND RIGIDBODY MOVEMENT///  NOTE: This is only for moving the characters rigidbody as a result of user input.  This is not for normal physics updates such as friction
        //Cache the horiziontal input.
        float h = Input.GetAxis(horizontalAxis);
        //Cache the vertical input.
        float v = Input.GetAxis(verticalAxis);
        //Get user input
        if(!player.attacking && !player.beenHit)
            ParseUserInput(h, v);
        //BUG FIX*
        //If the player is not grounded or climbing but is colliding with the ground, set h to 0 so the player cannot cling to the corner
        if (player.playerState != PlayableEntity.PlayerState.GROUNDED && player.playerState != PlayableEntity.PlayerState.CLIMBING && player.GetComponent<CircleCollider2D>().IsTouchingLayers(1 << LayerMask.NameToLayer("Ground")))
            h = 0;

        if (!player.attacking /*&& !player.dodging*/) {
            switch (player.playerState) {
                case PlayableEntity.PlayerState.GROUNDED:
                    rbody.gravityScale = 1.0f;
                    // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
                    if (h * rbody.velocity.x < player.maxSpeed) { 
                        //Add a force to the player if they arn't crouching.
                        if (!player.crouching && !player.attacking) {
                            if (player.playerAnimState != PlayableEntity.PlayerAnimState.SLIDE)
                                rbody.AddForce(Vector2.right * h * player.moveSpeed);
                        }
                    }  
                    //If the player has reached the max speed...
                    if (Mathf.Abs(rbody.velocity.x) > player.maxSpeed) { 
                        //Set the player's velocity to the max speed.
                            rbody.velocity = new Vector2(Mathf.Sign(rbody.velocity.x) * player.maxSpeed, rbody.velocity.y);
                    }
                    // If the input is moving the player right and the player is facing left...
                    if (h > 0 && !player.facingRight) { 
                        // ... flip the player.                  
                        if(Mathf.Abs(rbody.velocity.x) < 0.1)
                            player.FlipX();
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (h < 0 && player.facingRight) { 
                        // ... flip the player.
                        if(Mathf.Abs(rbody.velocity.x) < 0.1)
                            player.FlipX();
                    }
                    break;
                case PlayableEntity.PlayerState.AIRBORNE:
                    // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
                    if (h * rbody.velocity.x < player.maxSpeed)
                        //Add a force to the player.
                        rbody.AddForce(Vector2.right * h * player.moveSpeed * player.airControl);
                    //If the player has reached the max speed...
                    if (Mathf.Abs(rbody.velocity.x) > player.maxSpeed) { 
                        //Set the player's velocity to the max speed.
                            rbody.velocity = new Vector2(Mathf.Sign(rbody.velocity.x) * player.maxSpeed, rbody.velocity.y);
                    }
                    // If the input is moving the player right and the player is facing left...
                    if (h > 0 && !player.facingRight)
                        // ... flip the player.
                        player.FlipX();
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (h < 0 && player.facingRight)
                        // ... flip the player.
                        player.FlipX();
                    break;
                case PlayableEntity.PlayerState.CLIMBING:
                    //If the player is trying to move away from the wall then let them
                    if (player.facingRight && h > 0 || !player.facingRight && h < 0)
                        //Add a force to the player.
                        rbody.AddForce(Vector2.right * h * player.moveSpeed);
                    //If the player has reached the max speed...
                    if (Mathf.Abs(rbody.velocity.x) > player.maxSpeed) { 
                        //Set the player's velocity to the max speed.
                            rbody.velocity = new Vector2(Mathf.Sign(rbody.velocity.x) * player.maxSpeed, rbody.velocity.y);
                    }            
                    break;
                case PlayableEntity.PlayerState.HANGING:
                    //Set gravity to 0 when hanging
                    rbody.gravityScale = 0.0f;
                    break;
            }
        }
        base.Update();
    }

    private void ParseUserInput(float h, float v) {
        ///UPDATE FLAGS AND ACTIONS//
        switch (player.playerState) {
            case PlayableEntity.PlayerState.GROUNDED:
                //If Jump button is pressed...
                if (Input.GetButtonDown(jumpButton))
                    player.Jump();
                //If player has used double jump...
                if (!player.doubleJump)
                    //Set the flag true so they can double jump again.
                    player.doubleJump = true;
                //If player has pressed attack
                if (Input.GetButtonDown(lightAttackButton))
                {
                    if (!player.dodging)
                        player.LightAttack();
                    else
                        player.vanishCounter = true;
                }
                if (Input.GetButtonDown(attackButton)) {
                    if (!player.dodging)
                        player.Attack(h, v);
                    else
                        player.vanishCounter = true;
                }
                //If player pressed dodge
                if (Input.GetButtonDown(dodgeButton) || Input.GetAxis(dodgeButton) != 0)
                    player.Dodge(new Vector2(h, v));
                //If the player is pressing down on the control stick...
                if (v > 0.75)
                    //Make the player crouch
                    player.crouching = true;
                else
                    player.crouching = false;
                break;
            case PlayableEntity.PlayerState.AIRBORNE:
                //If Jump button is pressed...
                if (Input.GetButtonDown(jumpButton) && player.doubleJump)
                    player.DoubleJump();

                //If player is jumping and lets go of the button, cut the jump short
                if (!Input.GetButton(jumpButton)) {
                    //If the player is still going up...
                    if (rbody.velocity.y > 0)
                        //Reduce the y velocity in half.  NOTE: this transition may need to be smoother...
                        rbody.velocity = Vector2.Lerp(rbody.velocity, new Vector2(rbody.velocity.x, 0.0f), 0.5f);
                }
                if (Input.GetButtonDown(lightAttackButton))
                {
                    if (!player.dodging)
                        player.LightAttack();
                    else
                        player.vanishCounter = true;
                }
                //If player has pressed attack
                if (Input.GetButtonDown(attackButton))
                {
                    if (!player.dodging)
                        player.Attack(h, v);
                    else
                        player.vanishCounter = true;
                }
                //If player pressed dodge
                 if (Input.GetButtonDown(dodgeButton) || Input.GetAxis(dodgeButton) != 0)
                    player.Dodge(new Vector2(h, v));
                break;
            case PlayableEntity.PlayerState.CLIMBING:
                //If player pressed dodge
                 if (Input.GetButtonDown(dodgeButton) || Input.GetAxis(dodgeButton) != 0)
                    player.Dodge(new Vector2(h, v));
                //If Jump button is pressed...
                if (Input.GetButtonDown(jumpButton))
                    player.WallJump();
                break;
            case PlayableEntity.PlayerState.HANGING:
                //If the player isnt holding up on the control stick.
                if (Input.GetAxis(verticalAxis) > -0.75)
                    player.playerState = PlayableEntity.PlayerState.AIRBORNE;
                break;
        }
    }
}
