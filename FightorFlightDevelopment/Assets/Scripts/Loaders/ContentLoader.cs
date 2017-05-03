using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContentLoader : MonoBehaviour {

    private static ResourceManager _resourceManager;

    private bool _loadedSuccessfully = false;


    //Animation Container
    public NamedSprite[] animations;    //any animation that isnt part of set and isnt temorary.. example a mob that has only a walk cycle.  The walk cycle isnt temporary but its also just one animation so its not part of a set.  things like fire or a tree blowing in the wind would go here.
    public NamedSpriteSet[] animationSets; //list of animation sets such as character animation sets
    public NamedSprite[] temporarySprites;  //ie explosions, dust, flashes anything that gets removed after its done playing or has a lifespan.
    //Audio Containers
    public NamedAudioClipSet[] audioClipSets;
    public NamedAudioClip[] audioClips;
    //Hazards
    public NamedHazard[] hazards;
    //Actions
    public NamedAction[] actions;

    //TileList
    public TileList tileList;

	// Use this for initialization
    void Awake() {
        _resourceManager = ResourceManager.GetInstance();
    }


	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void LoadContent() {
        LoadAnimations();
        LoadAudio();
        LoadHazards();
        LoadActions();
        LoadTiles();
        _loadedSuccessfully = true;
    }
    
    void LoadAnimations() {
        //Load aniamtions
        foreach(NamedSprite animatedSprite in animations) {
            Animation2D anim = new Animation2D(animatedSprite.sprite, animatedSprite.priority, animatedSprite.name, false, animatedSprite.loop, animatedSprite.fadeOut);
            _resourceManager.GetAnimationManager().AddAnimation(anim);
        }
        //Load animation Sets
        foreach(NamedSpriteSet animatedSpriteSet in animationSets)
        {
            List<Animation2D> animList = new List<Animation2D>();
            foreach(NamedSprite animatedSprite in animatedSpriteSet.spriteList)
            {
                Animation2D anim = new Animation2D(animatedSprite.sprite, animatedSprite.priority, animatedSprite.name, false, animatedSprite.loop, animatedSprite.fadeOut);
                animList.Add(anim);
            }
            AnimationSet2D animSet = new AnimationSet2D(animatedSpriteSet.name, animList);
            _resourceManager.GetAnimationManager().AddAnimationSet(animSet);
        }

        //Load Temporary Sprites in the same fasion
        foreach(NamedSprite tempSprite in temporarySprites) {
            Animation2D anim = new Animation2D(tempSprite.sprite, tempSprite.priority, tempSprite.name, true, tempSprite.loop, tempSprite.fadeOut);
            TempSpriteManager.GetInstance().AddAnimation(anim);
        }
    }

    void LoadAudio() {
        //Load Audio Clip sets
        foreach (NamedAudioClipSet audioClipSet in audioClipSets)
            _resourceManager.GetAudioManager().AddAudioClips(audioClipSet.name, audioClipSet.set);
        //Load any individual sounds
        foreach (NamedAudioClip audioClip in audioClips)
            _resourceManager.GetAudioManager().AddAudioClip(audioClip.name, audioClip.clip);
    }

    void LoadHazards() {
        Dictionary<string, GameObject> hazardsToBeImported = new Dictionary<string, GameObject>();
        foreach (NamedHazard hazard in hazards)
            hazardsToBeImported.Add(hazard.name, hazard.hazard);
        if (!_resourceManager.GetHazardManager().importList(hazardsToBeImported))
            print("Hazards were not loaded correctly");
    }

    void LoadActions() {
        Dictionary<string, Component> actionsToBeImported = new Dictionary<string, Component>();
        foreach (NamedAction action in actions) {
            Component newAction = action.gameObj.GetComponent<Action>();
            actionsToBeImported.Add(action.name, newAction);
        }
        _resourceManager.GetActionManager().importList(actionsToBeImported);
    }

    void LoadTiles() {
        int x, y, z;
        x = y = z = 0;
        foreach (TileType type in tileList.tiles) {
            foreach (TileShape shape in type.shapes) {
                foreach (Sprite sprite in shape.sprites) {
                    string spriteName = "Tile_" + x + "_" + y + "_" + z;
                    print("Requesting: " + spriteName);
                    _resourceManager.GetSpriteManager().AddSprite(spriteName, sprite);
                    ++z;
                }
                ++y;
                z = 0;
            }
            ++x;
            y = 0;
        }
    }

    public bool HasLoaded() {
        return _loadedSuccessfully;
    }                          
}
[System.Serializable]
public struct NamedSpriteSet
{
    public string name;
    public NamedSprite[] spriteList;
}

[System.Serializable]
public struct NamedSprite {
    public string name;
    public Frame[] sprite;
    public bool loop;
    public bool fadeOut;
    public int priority;
}

[System.Serializable]
public struct NamedAudioClip {
    public string name;
    public AudioClip clip;
}

[System.Serializable]
public struct NamedAudioClipSet {
    public string name;
    public AudioClip[] set;
}

[System.Serializable]
public struct NamedHazard {
    public string name;
    public GameObject hazard;
}

[System.Serializable]
public struct TileList {
    public TileType[] tiles;
}

[System.Serializable]
public struct TileType {
    public TileShape[] shapes;
}

[System.Serializable]
public struct TileShape {
    public Sprite[] sprites;
}

[System.Serializable]
public struct NamedAction {
    public string name;
    public GameObject gameObj;
}