using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class BaseGameEntity : MonoBehaviour {

    //NOTE: Animations handled by base game entity are Idle, Jump and Run, objects inheriting from this class can implement one to all of those animations
    protected List<Animation2D> animationList;     //Reference to the list of animations 
    //animation set
    public string animationSet;         //References to the animations contained in the animation Dictionary
    public AnimationController2D animator;   //Reference to an animation controller class

    private Vector2 _returnForce;          //If this vector does not equal vector.zero then unpausing will apply a force to the game entity.
    public bool paused = false;            //Determines if the character is paused
    private bool frozenGravity = false;   //Determines if gravity has been frozen by an outside class.  Gravity can not be adjusted internaly unless this flag is false.
    //Im making these fields public so that other classes my reference but they are hidden so they are not tampered with
    public bool facingRight = true;     //Determines the direction the unit is facing. 
    [HideInInspector]
    public float moveSpeed;            //The current acceleration added to left and right movement.  
    [HideInInspector]
    public float maxSpeed;             //The fastest the unit can move along the x axis. 
    [HideInInspector]
    public float jumpForce;            //The current amount of force added when the unit jumps

    public int lives = 1;
    protected bool dead = false;

    public float maxHealth = 100.0f;
    protected float health;
    protected float tempHealth;
    public float GetHealth() { return health; }
    public float GetTempHealth() { return tempHealth; }

    public bool beenHit = false;
    public bool knockedback = false;

    //all of these variables are for the linear knockback.
    //This is to allow us to lerp the players position dynamicly based on how far they travel
    protected bool linearKnockback = false;
    private RaycastHit2D hit;
    private Vector3 knockbackVector = Vector2.zero;
    private float startTime;
    private Vector3 startPos;
    private float distanceTraveled;

    public float baseMoveSpeed = 20f;  //Amound of acceleration added to left and right movement.
    public float baseMaxSpeed = 5f;    //The fastest the unit can move along the x axis, with no upgrades.
    public float baseJumpForce = 500f; //The amound of force added when the unit jumps.
    public float airControl = 0.5f;     //The percentage of control over movement while airborne.

    protected Rigidbody2D _rbody;
    protected SpriteRenderer _renderer;



    // Use this for initialization upon loading
    virtual protected void Awake()  {
        //init variables
        moveSpeed = baseMoveSpeed;
        maxSpeed = baseMaxSpeed;
        jumpForce = baseJumpForce;
        animationList = new List<Animation2D>();
        _rbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();

    }

    // Use this for initialization upon activation of the script
    virtual protected void Start () {
        health = maxHealth;
        tempHealth = health;
        
        GenerateAnimationList();
        //If the animator isnt already defined just make one
	    animator = new AnimationController2D(_renderer, animationList);

        //Ignore collisions
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Default"));
	}

    // Update is called once per frame 
    virtual protected void Update() {
        //TEST
        if (knockedback)
            _renderer.color = Color.red;
        else
            _renderer.color = Color.white;
        LinearKnockbackUpdate();
        //HP CHECK
        if (health <= 0) {
            health = 0;
            //playerState = PlayerState.DEAD;
        }
        if (health == 0)
            Kill();

      

        //Animator update just calls and updates the current animation, logic must be defined in child class for animation switches if applicable. 
        animator.Update();
    }

    //Collision call back
    virtual protected void OnTriggerEnter2D(Collider2D other) {}
    virtual protected void OnTriggerStay2D(Collider2D other) {}
    virtual protected void OnTriggerExit2D(Collider2D other) { }

    virtual protected void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            if (knockedback) {
                //DealTempDamage((health - tempHealth) * 0.1f);
                Invoke("ReleaseKnockback", 1.0f);
            }
        }
    }
    virtual protected void OnCollisionStay2D(Collision2D c) {}
    virtual protected void OnCollisionExit2D(Collision2D c) {}

    private void ReleaseKnockback() {
        knockedback = false;
    }

    public virtual void DealDamage(float damage) {
            health -= damage;

    }

    public virtual void DealTempDamage(float damage) {
       if(!beenHit) {
           tempHealth -= damage;
            if (tempHealth < 0) { 
                health -= (tempHealth * -1.0f);
                tempHealth = 0;
            }
        }
    }

    protected virtual void Kill()
    {
        if (!dead) {
            dead = true;
            lives--;
            if (lives > 0)
                StartCoroutine(Respawn(Vector2.zero));
            else
                gameObject.SetActive(false);
        }
    }

    protected  IEnumerator Respawn(Vector2 pos)
    {
        yield return new WaitForSeconds(3);
        Reinitalize(pos);

    }

    protected virtual void Reinitalize(Vector2 pos)
    {
        health = maxHealth;
        tempHealth = health;
        transform.position = pos;
        animator.currAnim.finished = true;
        knockedback = false;
        dead = false;
    }

    public virtual void Stagger(float duration) {
        if(!beenHit) {
            EffectManager.GetInstance().Shake(Vector2.up, gameObject, duration, 0.33f, 1.0f);
            beenHit = true;
            paused = true;
            _rbody.isKinematic = true;
            animator.Pause(duration);
            _rbody.velocity = Vector3.zero;
        }
    }

    public virtual void Knockback(Vector2 direction, float force, float delay) { 
        if (beenHit == false) {
            Stagger(delay);
            knockedback = true;
            StartCoroutine(InitiateKnockback(direction.normalized, force, delay));
        }
    }

    
    private IEnumerator InitiateKnockback(Vector2 direction, float force, float delay) {
        yield return new WaitForSeconds(delay);
        Unpause();
        _rbody.AddForce(direction * force);
    }

    public virtual void LinearKnockback(Vector2 direction, float delay) {
         if (beenHit == false) {
            knockedback = true;
            Stagger(delay);
            StartCoroutine(InitiateLinearKnockback(direction.normalized, delay));
        }
    }

    private void LinearKnockbackUpdate() {
        if(linearKnockback) {
           float distanceCovered = (Time.time - startTime) * maxSpeed * 50;
           Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, 0.0f);
           hitPoint += (transform.position - hitPoint).normalized * GetComponent<CircleCollider2D>().radius;
           transform.position = Vector3.Lerp(transform.position, hitPoint, distanceCovered / Vector3.Distance(startPos, hitPoint));
           if ((Vector2)(transform.position) == (Vector2)(hitPoint)) {
                LinearKnockbackEnd(hitPoint, hit.collider);
            }
        }
    }
    protected virtual void LinearKnockbackEnd(Vector3 hitPoint, Collider2D other)
    {
        beenHit = false;
        DealTempDamage(health - tempHealth);
        linearKnockback = false;
        Unpause();
        Knockback((Vector2)(hitPoint - startPos).normalized, 1000, 0.5f);
        EffectManager.GetInstance().Shake(hit.point.normalized, Camera.main.gameObject, 0.5f, 2.0f, 3.0f);
    }

    private IEnumerator InitiateLinearKnockback(Vector2 direction, float delay) {
        yield return new WaitForSeconds(delay);
        linearKnockback = true;
        //Gonna figure out the length of the vector along the direction line that is inbetween the player and the edge of the screen
        knockbackVector = new Vector3(direction.x, direction.y, 1.0f) * Screen.width + startPos;
        startPos = transform.position;
        hit = Physics2D.Linecast(startPos, knockbackVector, 1 << LayerMask.NameToLayer("Ground"));
        //Lerp the player toward that point
        startTime = Time.time;
        float travelTime = maxSpeed * 10 / hit.distance;
    }


    

    public virtual void Jump(){
        //Add force an up force to the rigidbody
        _rbody.AddForce(new Vector2(0f, jumpForce));
        
    }

     public void Pause(float duration) {
        if(!paused) { 
            animator.Pause(duration);
            paused = true;
            _rbody.isKinematic = true;
            Invoke("Unpause", duration);
        }     
    }


    public void Unpause() {
        if(paused) {
            paused = false;
            _rbody.isKinematic = false;
            beenHit = false;
        }
    }

    public void FreezeGravity(float targetScale, float duration) {
        frozenGravity = true;
        _rbody.gravityScale = targetScale;
        Invoke("ResetGravity", duration);
    }

    protected void SetGravity(float value) {
        if (!frozenGravity)
            _rbody.gravityScale = value;
    }
    private void ResetGravity() {
        frozenGravity = false;
        _rbody.gravityScale = 1.0f;
    }                              

    //Flips the players x scale so they face the other way
    public virtual void FlipX()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected virtual void GenerateAnimationList() {
        AnimationManager animManager = ResourceManager.GetInstance().GetAnimationManager();
        AnimationSet2D animSet = animManager.GetAnimationSet(animationSet);
        foreach (Animation2D anim in animSet.GetAnimationList()) {
            animationList.Add(anim);
        }
    }         
}
