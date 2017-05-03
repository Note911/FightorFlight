using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController2D {
    
    //private bool paused = false;
    //private float timeSincePaused = 0.0f;
    //private float pauseLength = 0.0f;

    protected List<Animation2D> spriteList;    //List of sprite arrays (each object in the list is an array to hold an entire animation's frames)
    protected SpriteRenderer renderer;        //Reference to the objects sprite renderer component

    public int playSpeed = 1;

    public int animState;           //Current animation state (determines which animation is being rendered)
    public Animation2D currAnim;   //Reference to the current animation being played


    protected Timer animTimer = new Timer();  //Timer that determines when to switch frames based on the animation interval

    //constructor
    public AnimationController2D(SpriteRenderer renderer, List<Animation2D> spriteList) {
        //NOTE: This line is ok but we will have to either find the spriterendere on an object, make it required or simply remove
        //the component so that it's added through this code
        this.renderer = renderer;
        //init the sprite list
        this.spriteList = spriteList;
        //set anim state to 0 (defult animation) 
        //NOTE: this would be idle animation in most cases
        animState = 0;
        //NOTE: this is using sav's timer, look into building our own if necessary
        animTimer.StartTimer();
        //pass the first frame of the first animation to the renderer
        currAnim = spriteList[animState];
        //Update the renderer component to the first frame of the animation
        currAnim.UpdateFrame(renderer);
    }

    public AnimationController2D(SpriteRenderer renderer, Animation2D sprite) {
        //NOTE: This line is ok but we will have to either find the spriterendere on an object, make it required or simply remove
        //the component so that it's added through this code
        this.renderer = renderer;
        //init the sprite list
        spriteList = new List<Animation2D>();
        spriteList.Add(sprite);
        //set anim state to 0 (defult animation) 
        //NOTE: this would be idle animation in most cases
        animState = 0;
        //NOTE: this is using sav's timer, look into building our own if necessary
        animTimer.StartTimer();
        //pass the first frame of the first animation to the renderer
        currAnim = spriteList[animState];
        //Update the renderer component to the first frame of the animation
        currAnim.UpdateFrame(renderer);
    }

    //Updates the current animation
    public virtual void Update() {
        //if (paused) {
        //    timeSincePaused += Time.deltaTime;
        //    if (timeSincePaused >= pauseLength){
        //        timeSincePaused = 0.0f;
        //        paused = false;
        //    }
        //}
        //else
        currAnim.Play(animTimer, renderer, playSpeed);
    }

    //Changes the current animation being played
    public void ChangeAnimation(int state) {
        if (spriteList[state].priority >= currAnim.priority || currAnim.finished) {
            //Change the animation state
            animState = state;
            currAnim = spriteList[animState];
            currAnim.Restart(animTimer, renderer);
        }
    }

    public void ForceAnimationChange(int state) {
        currAnim.finished = true;
        ChangeAnimation(state);
    }


    //Adds an animation to the list
    public void AddAnimation(Animation2D anim) {
        spriteList.Add(anim);
    }

    public void Pause(float duration) {
        currAnim.Pause(duration);
    }

}
