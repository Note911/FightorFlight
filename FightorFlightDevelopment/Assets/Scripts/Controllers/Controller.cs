using UnityEngine;
using System.Collections;

public class Controller {

    //Reference to the playable entity script.
    protected PlayableEntity player;
   
    public Controller(PlayableEntity player) {
        this.player = player;
    }

    //Calls every frame
    virtual public void Update() {
        if (!player.knockedback && player.playerState != PlayableEntity.PlayerState.DEAD) {
            //Determine the player's current PlayerState. "Note hanging has been excluded from this script because it requires special conditions.
            //Using a linecast check if the player's groundCheck player.transform hits anything on the ground layer
            if (Physics2D.Linecast(player.groundCheck.position, player.groundCheck2.position, 1 << LayerMask.NameToLayer("Ground"))) { 
                if(player.playerState == PlayableEntity.PlayerState.AIRBORNE) {
                    if(!player.audioSource.isPlaying) {
                        player.audioSource.clip = ResourceManager.GetInstance().GetAudioManager().GetAudioClip("GroundHit1");
                        player.audioSource.Play();
                    }
                }
                player.playerState = PlayableEntity.PlayerState.GROUNDED;
            }
            //Using a linecast check if the player's wall check player.transforms hit anything on the ground layer. (Horizontally)
            //Check Left Side...
            else if (Physics2D.Linecast(player.transform.position, player.wallCheckLeft.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                //If player is facing the wall, flip them
                if (player.playerState != PlayableEntity.PlayerState.GROUNDED && !player.facingRight)
                    player.FlipX();
                //Set State to climbing
                player.playerState = PlayableEntity.PlayerState.CLIMBING;
            }
            //Check Right side...
            else if (Physics2D.Linecast(player.transform.position, player.wallCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                //If player is facing the wall, flip them
                if (player.playerState != PlayableEntity.PlayerState.GROUNDED && player.facingRight)
                    player.FlipX();
                //Set State to climbing
                player.playerState = PlayableEntity.PlayerState.CLIMBING;
            }
             //Check to see if the player can hang on a ceiling
            else if(Input.GetAxis(player.verticalAxis) < -0.75 && Physics2D.Linecast(player.transform.position, player.roofCheck.position, 1 << LayerMask.NameToLayer("Ground")))
                player.playerState = PlayableEntity.PlayerState.HANGING;
            else
                player.playerState = PlayableEntity.PlayerState.AIRBORNE;



        }
    }
}
