using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardManager {
    private Dictionary<string, GameObject> _hazardList;

    public HazardManager(){
        _hazardList = new Dictionary<string, GameObject>();
    }

    public void addHazard(string key, GameObject hazard) {
        _hazardList.Add(key, hazard);
    }

    public bool importList(Dictionary<string, GameObject> hazardList) {

        Dictionary<string, GameObject> tempList = new Dictionary<string, GameObject>();
        //Insure that all objects being added to the library are infact hazards, and have the required components
        foreach (KeyValuePair<string, GameObject> hazard in hazardList) {
            if (hazard.Value.GetComponent<Hazard>() != null)
                tempList.Add(hazard.Key, hazard.Value);
        }
        //if the count of the dictionary is equal to the hazard list count then all the hazards checked out
        if (tempList.Count == hazardList.Count)
        {
            _hazardList = hazardList;
            return true;
        }
        else
            return false;
       
    }

    public Dictionary<string, GameObject> getHazardList() {
        return _hazardList;
    }                                                    

    public GameObject findHazard(string key) {
        return _hazardList[key];
    }

    public bool isLoaded() {
        if (this == null)
            return false;
        else
            return true;
    }                                
}
