using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSet2D {
    private List<Animation2D> _animationList;
    public string name;

	public AnimationSet2D(string name, List<Animation2D> animationList){
        this.name = name;
        _animationList = animationList;
    }

    public AnimationSet2D(AnimationSet2D prevSet)
    {
        name = prevSet.name;
        _animationList = prevSet.GetAnimationList();
    }

    public List<Animation2D> GetAnimationList() { return _animationList; }
}
