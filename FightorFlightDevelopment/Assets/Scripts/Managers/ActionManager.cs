using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager {

    private Dictionary<string, Component> _actionList;

    public ActionManager(){
        _actionList = new Dictionary<string, Component>();
    }

    public void addAction(string key, Action action) {
        _actionList.Add(key, action);
    }

     public void importList(Dictionary<string, Component> actionList) {
        _actionList = actionList;
    }

    public Dictionary<string, Component> getActionList() {
        return _actionList;
    }                                                    

    public Component findAction(string key) {
        return _actionList[key];
    }

    public bool isLoaded() {
        if (this == null)
            return false;
        else
            return true;
    }                                
}
