using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempSpriteManager {

    private static TempSpriteManager _instance;

    private static Dictionary<string, Animation2D> _spriteList;
    private static List<TempSprite> _activeSprites;


	//singleton so private constructor
    private TempSpriteManager() {
        _spriteList = new Dictionary<string, Animation2D>();
        _activeSprites = new List<TempSprite>();

    }

    //Get instance method
    public static TempSpriteManager GetInstance() {
        if (_instance == null)
            _instance = new TempSpriteManager();
        return _instance;
    }

    public void Update() {
        //Update any active sprites
        if(_activeSprites.Count > 0) {
            for (int i = _activeSprites.Count - 1; i >= 0; i--) {
                //Null check, incase a sprite was playing during a scene switch
                if (_activeSprites[i] != null)
                {
                    _activeSprites[i].Update();
                    if (_activeSprites[i].finished)
                    {
                        _activeSprites[i].Delete();
                        _activeSprites.Remove(_activeSprites[i]);
                    }
                }
                else //it is null so remove it from the list
                {
                    _activeSprites.Remove(_activeSprites[i]);
                }
            }
        }
    }

    public void AddAnimation(Animation2D anim) {
       _spriteList.Add(anim.name, anim);
    }

    public void PlayAnimation(string name, Vector2 pos) {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos);
        _activeSprites.Add(tmp);
    }

    public void PlayAnimation(string name, Vector2 pos, Vector2 scale)
    {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos, scale);
        _activeSprites.Add(tmp);
    }
    public void PlayAnimation(string name, Vector2 pos, Vector2 scale, string sortingLayer, int sortingOrder)
    {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos, scale, sortingLayer, sortingOrder);
        _activeSprites.Add(tmp);
    }

    public void PlayAnimation(string name, Vector2 pos, Vector2 scale, float roation, string sortingLayer, int sortingOrder)
    {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos, scale, roation, sortingLayer, sortingOrder);
        _activeSprites.Add(tmp);
    }

    
    public void PlayAnimation(string name, Vector2 pos, Vector2 scale, float roation, string sortingLayer, int sortingOrder, float delay)
    {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos, scale, roation, sortingLayer, sortingOrder);
        tmp.Delay(delay);
        _activeSprites.Add(tmp);
    }

    public void PlayAnimation(string name, Vector2 pos, Vector2 scale, Vector2 velocity, float roation, string sortingLayer, int sortingOrder, float delay)
    {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos, scale, velocity, roation, sortingLayer, sortingOrder);
        tmp.Delay(delay);
        _activeSprites.Add(tmp);
    }

    public void PlayAnimation(string name, Vector2 pos, Vector2 scale, Vector2 velocity, float roation, string sortingLayer, int sortingOrder, float delay, Color color)
    {
        //Create a new temp sprite and add it to the active sprite list
        TempSprite tmp = new TempSprite(new Animation2D(_spriteList[name]), pos, scale, velocity, roation, sortingLayer, sortingOrder, color);
        tmp.Delay(delay);
        _activeSprites.Add(tmp);
    }
}
