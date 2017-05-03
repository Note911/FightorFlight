using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayableEntity : BaseGameEntity {

    public int playerIndex = 0;
    public int skinIndex = 0;

    public float dragGround = 2.5f;
    public float dragSliding = 0.1f;
    public float dragAir = 0.0f;
    public float dragClimbing = 3.3f;

    private float timeGotHit = 0.0f;
    public float regenRate = 50.0f;
    public float regenCD = 3.0f;

    public enum PlayerAnimState { IDLE, CROUCH, RUN, SLIDE, JUMP, APEX, FALL, CLIMB, HANG,
        ATK_BASIC_COMBO1, ATK_BASIC_COMBO2, ATK_BASIC_COMBO3, 
        ATK_G_F_B, ATK_G_F_M, ATK_G_F_E,
        ATK_G_U_B, ATK_G_U_M, ATK_G_U_E,
        //ATK_G_D_B, ATK_G_D_M, ATK_G_D_E,
        //ATK_A_F_B, ATK_A_F_M, ATK_A_F_E,
        //ATK_A_B_B, ATK_A_B_M, ATK_A_B_E,
        //ATK_A_U_B, ATK_A_U_M, ATK_A_U_E,
        ATK_A_D,
        KNOCKEDBACK,
        DODGE_ENTER, DODGE_EXIT,
        NONE, BLOCK, DEAD, } //this is just to make it easier to pass things to the animator

    public PlayerAnimState playerAnimState = PlayerAnimState.NONE;

    public enum PlayerState { GROUNDED, AIRBORNE, CLIMBING, HANGING, DEAD };  //Dertermines the players current PlayerState
    public PlayerState playerState = PlayerState.GROUNDED; //Reference to player PlayerState enum

    public Transform groundCheck;      //A posistion transform at the players feet, used to check if they are grounded.
    public Transform groundCheck2;     //A posistion transform at the players feet, used to check if they are grounded.
    public Transform wallCheckLeft;    //The left position transform at the player's side, used to check if they are climbing a wall.
    public Transform wallCheckRight;   //The right position transform at the player's side, used to check if they are climbing a wall.
    public Transform roofCheck;        //The position at the players head, used to check if they can grab a ceiling.

    public string jumpButton = "Jump_P1"; //String reference to jump button axis
    public string lightAttackButton = "LightAttack_P1"; //string reference to the light attack button axis
    public string attackButton = "Attack_P1"; //String reference to attack button axis
    public string dodgeButton = "Dodge_P1"; //String reference to dodge button axis
    public string horizontalAxis = "Horizontal_P1";  //String reference to horizontal axis
    public string verticalAxis = "Vertical_P1";  //String reference to horizontal axis

    [HideInInspector]
        public bool doubleJump = false;    //Determines if the player can double jump.
    [HideInInspector]
        public bool invincible = false;    //Determines if the player can take damage.
    [HideInInspector]
        public bool crouching = false;     //Determines if the player is crouching.
    [HideInInspector]
        public bool attacking = false;     //Determines if the player is attacking.
    [HideInInspector]
        public bool blocking = false;      //Determines if the player is blocking.
    [HideInInspector]
        public bool dodging = false;       //Determines if the player is dodging. 
    [HideInInspector]
        public bool vanishCounter = false; //Determines if they player is trying to preform a vanish counter (dodge + attack)
    [HideInInspector]
        public bool canHit = false;        //Determines if the player can deal damage.
    [HideInInspector]
        public bool canDodge = true;      //Determines if the player can dodge and by extention block;
        public bool canAttack = true;     //Determines if the player can attack.

    public float attackCD = 1.0f;
    public float respawnCD = 2.5f;    //Time it takes to respawn

    private float timeSinceDied = 0.0f;

    private float timeSinceDodged = 0.0f;

    private float timeSinceAttacked = 0.0f;

    public List<Action> actionList;      //List of the availible attacks for the player, defined in the implementation
    public string[] actions;                //List of string references to actions, passed to a dictionary to return the actual attack object
    public Dodge dodgeAction;            //Players dodge action
    public Block blockAction;            //players block action
    public enum AttackState {ATK_B1, ATK_B2, ATK_B3, ATK_G_F, ATK_G_B, ATK_G_U, ATK_G_D, ATK_A_F, ATK_A_B, ATK_A_U, ATK_A_D, NONE};
    private AttackState attackState = AttackState.NONE;

    public Color color;
    public Color color2;

  
 

    public PhysicsMaterial2D bouncyMaterial;

    private PlayerController controller;    //Reference to the player controller
    public AudioSource audioSource, audioSource2;


    private bool kickedUpDust = false;

    // Use this for initialization upon loading
    protected override void Awake() {
        base.Awake();
        groundCheck = transform.Find("groundCheck");
        groundCheck2 = transform.Find("groundCheck2");
        wallCheckLeft = transform.Find("wallCheckLeft");
        wallCheckRight = transform.Find("wallCheckRight");
        roofCheck = transform.Find("roofCheck");
        controller = new PlayerController(this);
  
        actionList = new List<Action>();
    }

    // Use this for initialization upon activation
    protected override void Start () { 
        AssignActions();
        audioSource.volume = 0.2f;
        audioSource2.volume = 0.2f;

       
        switch(skinIndex) {
            case (0):
                color = Color.red;
                color2 = Color.white;  
                break;
            case (1):
                color = Color.blue;
                color2 = new Color(1.0f, 0.6f, 0.01f);
                break;
            case (2):
                color = Color.green;
                color2 = Color.gray;
                break;
            case (3):
                color = new Color(1.0f, 0.0f, 1.0f);
                color2 = Color.green;
                break;
        }             
        
        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.sortingLayerName = "Ground";
        trail.startColor = new Color(color.r, color.g, color.b, 0.0f);
        trail.endColor = color;          
        base.Start();
	
	}

    // Update is called once per frame
    protected override void Update () {
        if (health <= 0)
            Kill();

        //Regen HP
        if (timeGotHit > 0.0f)
        {
            if (Time.time - timeGotHit >= regenCD)
            {
                if (tempHealth < health)
                {
                    tempHealth += (regenRate * Time.deltaTime);
                }
                if (tempHealth > health)
                {
                    timeGotHit = 0.0f;
                    tempHealth = health;
                }
            }
        }
       
        //Jessica is Sexy! (.)(.) <3

        //Determine the updates on the player depending on player state
        //The player contorller handles user input based on these states. 
        //We must do all the rigidbody movement from outside forces, animations and soundeffects which belong to the character.  Temporary sprites involving player interaction will be handled outside this class
        playerAnimState = PlayerAnimState.NONE;  //making a temporary enum here so we dont call change animation a dozen times
        switch (playerState) {
            case (PlayerState.GROUNDED):
                SetGravity(1.0f);
                //set drag for ground
                if (!knockedback)
                    _rbody.drag = dragGround;
                else
                    _rbody.drag = dragSliding;

                transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                if (playerAnimState == PlayerAnimState.FALL) {
                    playerAnimState = PlayerAnimState.CROUCH;
                    break;
                }
                //We are grounded so check for movement to determine if idle or running
                //If the x velocity is greater then 0.1 or less then -0.1 we are moving
                if (_rbody.velocity.x > 0.1 || _rbody.velocity.x < -0.1) {
                    float h = Input.GetAxis(horizontalAxis);
                    if ((h > 0.01f && _rbody.velocity.x > 0.1f) || (h < -0.01f && _rbody.velocity.x < -0.1f))
                    {
                        playerAnimState = PlayerAnimState.RUN;
                        kickedUpDust = false;
                    }
                    else
                    {
                        if (!kickedUpDust && playerAnimState != PlayerAnimState.SLIDE)
                        {
                            kickedUpDust = true;
                            TempSpriteManager.GetInstance().PlayAnimation("Small_Dust_02", (Vector2)transform.position + new Vector2(Mathf.Sign(transform.localScale.x) * 1, 0), new Vector2(0.5f * Mathf.Sign(transform.localScale.x), 0.5f), new Vector2(0.02f * Mathf.Sign(transform.localScale.x), 0), 0.0f, "Background2", -1, 0.3f);
                        }
                        playerAnimState = PlayerAnimState.SLIDE;
                    }
                       
                }
                //We dont have to set idle because we did at the beginning.  If the player isnt running and is in this state nothing else will be called because of the break.
                //Jittering in the transition from idle and run can be solved using the horizontal axis, however the playercontroller class will have to be altered slightly
                else
                    playerAnimState = PlayerAnimState.IDLE;
                break;
            case (PlayerState.AIRBORNE):
                if (!knockedback)
                {
                    SetGravity(1.0f);
                }
                //PROBABLY WANNA CHANGE THIS AT SOME POINT TO BETTER CODE.... WAY TO LAZY RIGHT NOW
                if (attacking)
                    _rbody.drag = dragGround;
                else
                    _rbody.drag = dragAir;
                //playerAnimState = PlayerAnimState.FALL;

                if (Mathf.Abs(_rbody.velocity.y) <= 5.5f)
                    playerAnimState = PlayerAnimState.APEX;
                else if (_rbody.velocity.y > 0.1f)
                    playerAnimState = PlayerAnimState.JUMP;
                else
                    playerAnimState = PlayerAnimState.FALL;

                break;
            case (PlayerState.CLIMBING):
                _rbody.drag = dragClimbing;
                //If we are climbing then set the animation to climbing.  In this player state there isnt any other animation that could occur.
                transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                SetGravity(0.33f);
                playerAnimState = PlayerAnimState.CLIMB;
                break;
            case (PlayerState.HANGING):
                //If we are hanging then set the animation to hang. In this player state there isnt any other animation that could occur.
                playerAnimState = PlayerAnimState.HANG;
                SetGravity(0.0f);
                _rbody.velocity = Vector2.zero;
                break;
            case (PlayerState.DEAD):          
                timeSinceDied += Time.deltaTime;
                if(timeSinceDied >= respawnCD)
                {
                    timeSinceDied = 0.0f;
                    Respawn(GameObject.Find("Main Camera").transform.position);
                }
                break;
        }
        //Crouching?
        if (crouching)
            playerAnimState = PlayerAnimState.CROUCH;

        //If we have changed animations update the animator
        if (animator.animState != (int)playerAnimState)
            animator.ChangeAnimation((int)playerAnimState);
       
        //animator is updated in the base class
        base.Update();
        if(playerState != PlayerState.DEAD)
            controller.Update();
    }

    private void FootStep()
    {
        //helper function just to play a small foot step sprite which can be called with invoke 
        TempSpriteManager.GetInstance().PlayAnimation("Jump_Dust", (Vector2)transform.position, new Vector2(Mathf.Sign(transform.localScale.x), 1), "Background2", -1);
    }

    //Collison callback
    protected override void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player1") {
            if (attacking && canHit) {
                PlayableEntity defender = other.GetComponent<PlayableEntity>();
                if (defender.canHit)
                {
                    Vector2 knockback = transform.position - defender.transform.position;
                    knockback.Normalize();
                    knockback *= 100;
                    _rbody.AddForce(knockback);
                    GetComponent<ParticleSystem>().Play();
                }
                //else if(!other.gameObject.GetComponent<PlayableEntity>().invincible)
                    //actionList[(int)attackState].OnHit(other.gameObject.GetComponent<PlayableEntity>());
            }
        }
        base.OnTriggerStay2D(other);
    }
    override protected void OnCollisionStay2D(Collision2D c) {
        //if(c.gameObject.tag == "Player1") { 
        //    if (attacking && canHit) {
        //        if(!c.gameObject.GetComponent<PlayableEntity>().invincible)
        //            actionList[(int)attackState].OnHit(c.gameObject.GetComponent<PlayableEntity>());

        //    }
     
        base.OnCollisionStay2D(c);
    }

    override protected void OnCollisionEnter2D(Collision2D c) { 
        base.OnCollisionEnter2D(c);
    }
    protected override void OnCollisionExit2D(Collision2D c){

        base.OnCollisionExit2D(c);
    }
    protected override void Kill()
    {
        if (!dead)
        {
            attacking = false;
            playerState = PlayerState.DEAD;
            animator.ChangeAnimation((int)PlayerAnimState.NONE);
        }
        base.Kill();
    }
    protected override void Reinitalize(Vector2 pos)
    {
        playerState = PlayerState.AIRBORNE;
        base.Reinitalize(pos);
    }

    private void AttackCooldown() {
        canAttack = true;
    }

    public void LightAttack()
    {
        attackState = AttackState.ATK_B1;
        actionList[(int)attackState].Begin();
    }

    public void Attack(float h, float v)
    {
        if (canAttack)
        { 
            switch (playerState)
            {
                case (PlayerState.GROUNDED):
                    //If the absolute value of the horizontal axis is greater then vertical, then the attack is on the horizontal axis
                    if (Mathf.Abs(h) > Mathf.Abs(v))
                    {
                        //See if the attack is forward or back
                        //If facing right...
                        if (facingRight)
                        {
                            //then positive is forward
                            if (h > 0)
                            {
                                attackState = AttackState.ATK_G_F;
                            }
                            else
                            {
                                attackState = AttackState.ATK_G_B;
                            }
                        }
                        //If facing left...
                        else
                        {
                            //Then negitive is forward
                            if (h < 0)
                            {
                                attackState = AttackState.ATK_G_F;
                            }
                            else
                            {
                                attackState = AttackState.ATK_G_B;
                            }
                        }
                    }
                    //Else the attack is on the vertical axis
                    else
                    {
                        //Negitive is up
                        if (v < 0)
                        {
                            attackState = AttackState.ATK_G_U;
                        }
                        else
                        {
                            attackState = AttackState.ATK_G_D;
                        }
                    }
                    break;
                //Same logic just calls airborne versions of attacks
                case (PlayerState.AIRBORNE):
                    //If the absolute value of the horizontal axis is greater then vertical, then the attack is on the horizontal axis
                    if (Mathf.Abs(h) > Mathf.Abs(v))
                    {
                        //See if the attack is forward or back
                        //If facing right...
                        if (facingRight)
                        {
                            //then positive is forward
                            if (h > 0)
                            {
                                attackState = AttackState.ATK_A_F;
                            }
                            else
                            {
                                attackState = AttackState.ATK_A_B;
                            }
                        }
                        //If facing left...
                        else
                        {
                            if (h < 0)
                            {
                                attackState = AttackState.ATK_A_F;
                            }
                            else
                            {
                                attackState = AttackState.ATK_A_B;
                            }
                        }

                    }
                    //Else the attack is on the vertical axis
                    else
                    {
                        if (v < 0)
                        {
                            attackState = AttackState.ATK_A_U;
                        }
                        else
                        {
                            attackState = AttackState.ATK_A_D;
                        }
                    }
                    break;

            }
                //call the attack function
            if (attackState != AttackState.NONE) {
                actionList[(int)attackState].Begin();
            }
        }
    }

    public virtual void Dodge(Vector2 dInput) {
        if(canDodge) {
            audioSource2.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("Teleport2");
            audioSource2.Play();
            dodgeAction.BeginDodge(dInput);
        }
    }

    private void InvokeBaseClassJump()
    {
        base.Jump();
    }

    public override void Jump()
    {
        audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("Jump");
        audioSource.Play();
        //Set state to airborne
        playerState = PlayerState.AIRBORNE;
        //Change the animation state to jumping
        animator.ChangeAnimation((int)PlayerAnimState.JUMP);
        //Que dust to play under the players feet
        //float dustDelay = 0.25f;
        //if(Mathf.Abs(_rbody.velocity.x) < 0.1f) {
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Small", (Vector2)transform.position + new Vector2(1.5f,-1.5f), new Vector2(3,3), 0, "Foreground", 0, dustDelay);
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Small", (Vector2)transform.position + new Vector2(-1.5f,-1.5f), new Vector2(-3,3), 0, "Foreground", 0, dustDelay);
        //}
        //else if(facingRight) { 
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Small", (Vector2)transform.position + new Vector2(1.5f,-1.5f), new Vector2(3,3), 0, "Foreground", 0, dustDelay);
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Mid", (Vector2)transform.position + new Vector2(-1.5f,-1.5f), new Vector2(3,3), 0, "Foreground", 0, dustDelay);
        //}
        //else {        
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Small", (Vector2)transform.position + new Vector2(-1.5f,-1.5f), new Vector2(-3,3), 0, "Foreground", 0, dustDelay);
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Mid", (Vector2)transform.position + new Vector2(1.5f,-1.5f), new Vector2(-3,3), 0, "Foreground", 0, dustDelay);
        //}

        Invoke("InvokeBaseClassJump", 0.05f);
        TempSpriteManager.GetInstance().PlayAnimation("Jump_Dust", (Vector2)transform.position + new Vector2(0,0.5f), new Vector2(2 * Mathf.Sign(transform.localScale.x) , 2), "Background2", -1);

        TempSpriteManager.GetInstance().PlayAnimation("Dust_Trail_Left2", (Vector2)transform.position + new Vector2(-0.1f, -0.25f), new Vector2(0.5f, 0.5f), new Vector2(-0.02f, 0), 0.0f, "Background2", 1, 0.3f);
        TempSpriteManager.GetInstance().PlayAnimation("Dust_Trail_Right2", (Vector2)transform.position + new Vector2(0.1f, -0.25f), new Vector2(0.5f, 0.5f), new Vector2(0.02f, 0), 0.0f, "Background2", 1, 0.3f);

    }

    private void InvokeDoubleJump()
    {
        _rbody.AddForce(new Vector2(0f, jumpForce));
    }

    public void DoubleJump()
    {
        //Tell the animator to trigger the jump animation
        animator.ChangeAnimation((int)PlayerAnimState.JUMP);
        //Consume the double jump
        doubleJump = false;
        //Add a vertical force to the player.
        _rbody.velocity = new Vector3(_rbody.velocity.x, 0.0f, _rbody.velocity.y);
        Invoke("InvokeDoubleJump", 0.1f);

        TempSpriteManager.GetInstance().PlayAnimation("Jump_Dust", (Vector2)transform.position + new Vector2(0, -1.5f), new Vector2(-2 * Mathf.Sign(transform.localScale.x), -2), "Background2", -1);

        TempSpriteManager.GetInstance().PlayAnimation("Dust_Trail_Left2", (Vector2)transform.position + new Vector2(-0.1f, -0.25f), new Vector2(0.3f, 0.3f), new Vector2(-0.02f, 0), 0.0f, "Background2", -1, 0.3f);
        TempSpriteManager.GetInstance().PlayAnimation("Dust_Trail_Right2", (Vector2)transform.position + new Vector2(0.1f, -0.25f), new Vector2(0.3f, 0.3f), new Vector2(0.02f, 0), 0.0f, "Background2", -1, 0.3f);
    }

    private void InvokeWallJump()
    {
        //Jump away from the wall.
        float hForce = jumpForce / 3.0f;
        //If player is facing the left, inverse the forward force of the jump.
        if (!facingRight)
            hForce *= -1;
        //Add force to the player's rigidbody
        _rbody.AddForce(new Vector2(hForce, jumpForce));
        TempSpriteManager.GetInstance().PlayAnimation("Jump_Dust", (Vector2)transform.position + new Vector2(0.5f * Mathf.Sign(transform.localScale.x), 0), new Vector2(2, -2 * Mathf.Sign(hForce)), 90.0f, "Background2", -1);

    }

    public void WallJump()
    {
        //Set state to airborn
        playerState = PlayerState.AIRBORNE;
        //Tell the animator to trigger the jump animation
        animator.ChangeAnimation((int)PlayerAnimState.JUMP);
        audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("WallJump");
        audioSource.Play();
        Invoke("InvokeWallJump", 0.1f);

        //float dustDelay = 0.05f;

        //if(facingRight) { 
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Mid", (Vector2)transform.position + new Vector2(1.5f,-1.5f), new Vector2(-1.5f,1.5f), -90, "Foreground", 0, dustDelay);
        //}
        //else {        
        //    TempSpriteManager.GetInstance().PlayAnimation("Dust_Mid", (Vector2)transform.position + new Vector2(-1.5f,-1.5f), new Vector2(1.5f,1.5f), 90, "Foreground", 0, dustDelay);
        //}
    }

    public override void FlipX() {
        base.FlipX();
        //Make sure wall check transforms remaim relitive to world space
        Vector3 temp = wallCheckLeft.position;
        wallCheckLeft.position = wallCheckRight.position;
        wallCheckRight.position = temp;
    }

    public void Block()
    {
        blockAction.Begin();
    }

    public override void DealDamage(float damage)
    {
        timeGotHit = Time.time;
        base.DealDamage(damage);
    }

    public override void DealTempDamage(float damage)
    {
        timeGotHit = Time.time;
        base.DealTempDamage(damage);
    }

    public override void Stagger(float duration)
    {
        if (!beenHit && !invincible)
        {
            if (attackState != AttackState.NONE)
                actionList[(int)attackState].Cancel();
            base.Stagger(duration);
        }
    }

    public override void Knockback(Vector2 direction, float force, float delay)
    {
        base.Knockback(direction, force, delay);
    }

    public override void LinearKnockback(Vector2 direction, float delay)
    {
        base.LinearKnockback(direction, delay);
    }

    protected override void LinearKnockbackEnd(Vector3 hitPoint, Collider2D other)
    {
        //Get the vector between the player and the tile that hit
        Vector2 normal = Vector2.zero;
        if (other != null)
        {
            normal = hitPoint - other.transform.position;
        }

        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(Vector2.up, normal.normalized));

        if (angle > 135.0f)
            angle = 180.0f;
        else if (angle > 45.0f)
            angle = 90.0f;
        else
            angle = 0.0f;

        float n = normal.x;
        if (Mathf.Abs(normal.y) > Mathf.Abs(n))
            n = normal.y;
        if(n <= -0.001)
            EffectManager.GetInstance().CreateDustCloud(hitPoint, angle);
        else
            EffectManager.GetInstance().CreateDustCloud(hitPoint, -angle);


        base.LinearKnockbackEnd(hitPoint, other);
    }

    private void DisableTrailRenderer()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }

    private void AssignActions() {
        //parse through the characters string references and add them to the action list,  an order will have to be defined Attacks > Dodge > Block
        foreach (string action in actions) {
            print("Asking for action....." + action);
            ActionManager instance = ResourceManager.GetInstance().GetActionManager();
            Component original = instance.findAction(action);
            System.Type actionType = original.GetType();
            Component copyComponent = gameObject.AddComponent(actionType);

            System.Reflection.FieldInfo[] fields = actionType.GetFields();
            foreach(System.Reflection.FieldInfo field in fields) {
                field.SetValue(copyComponent, field.GetValue(original));
            }
            actionList.Add((Action)(gameObject.GetComponent(actionType)));
        }
        //parse through the actionList with all the actions now added to it, and pull out the dodge and block for quick reference
        dodgeAction = (Dodge)actionList[11];
        blockAction = (Block)actionList[12];
    }
    
    public void AssignInputAxis() {
        jumpButton = jumpButton.Insert(jumpButton.Length, (playerIndex + 1).ToString());
        lightAttackButton = lightAttackButton.Insert(lightAttackButton.Length, (playerIndex + 1).ToString());
        attackButton = attackButton.Insert(attackButton.Length, (playerIndex + 1).ToString());
        dodgeButton = dodgeButton.Insert(dodgeButton.Length, (playerIndex + 1).ToString());
        horizontalAxis = horizontalAxis.Insert(horizontalAxis.Length, (playerIndex + 1).ToString());
        verticalAxis = verticalAxis.Insert(verticalAxis.Length, (playerIndex + 1).ToString());

        Debug.Log(jumpButton + " \n" + attackButton + " \n" + dodgeButton + " \n" + horizontalAxis + " \n" + verticalAxis + " \n" );
    }

    protected override void GenerateAnimationList()
    {
        animationSet = animationSet.Insert(animationSet.Length, "_" + skinIndex.ToString());
        Debug.Log(animationSet);
        AnimationManager animManager = ResourceManager.GetInstance().GetAnimationManager();
        AnimationSet2D animSet = animManager.GetAnimationSet(animationSet);
        foreach (Animation2D anim in animSet.GetAnimationList())
        {
            animationList.Add(anim);
        }
    }

}
