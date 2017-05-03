using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager {
//singleton class
    private Dictionary<string, Sprite> _spriteList;

    //singleton so private constructor
    public SpriteManager() {
        _spriteList = new Dictionary<string, Sprite>();
    }
    public void AddSprite(string name, Sprite sprite) {
        _spriteList.Add(name, sprite);
    }

    //public void AddAnimationSet(string name, Animation2D[] animations) {
    //    for(int i = 0; i < animations.Length; ++i) {
    //        string _name = name;
    //        _name = name + "_" + i;
    //        _animationList.Add(_name, animations[i]);
    //    }
    //}

    public Sprite GetSprite(string name) {
       return _spriteList[name];
    }
}
