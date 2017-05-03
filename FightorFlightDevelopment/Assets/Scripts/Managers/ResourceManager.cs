using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager {
    private static ResourceManager _instance;

    private AnimationManager _animationManager;
    public AnimationManager GetAnimationManager() { return _animationManager; }

    private AudioManager _audioManager;
    public AudioManager GetAudioManager() { return _audioManager; }

    private ActionManager _actionManager;
    public ActionManager GetActionManager() { return _actionManager; }

    private HazardManager _hazardManager;
    public HazardManager GetHazardManager() { return _hazardManager; }

    private SpriteManager _spriteManager;
    public SpriteManager GetSpriteManager() { return _spriteManager; }


    private ResourceManager(){
        _animationManager = new AnimationManager();
        _audioManager = new AudioManager();
        _actionManager = new ActionManager();
        _hazardManager = new HazardManager();
        _spriteManager = new SpriteManager();

    }

    public static ResourceManager GetInstance() {
        if (_instance == null)
            _instance = new ResourceManager();
        return _instance;
    }


}
