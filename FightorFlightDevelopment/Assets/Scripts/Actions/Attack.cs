using UnityEngine;
using System.Collections;

public class Attack : Action {

    //Move these into concrete implementations later when more unique effects are availible.
    private string[] explosionSFX;
    private string[] wooshSFX;
  
    public string[] _hazardRef;     //Reference to the hazard created by the attack, (Attack hazards are unique in that they are only created through an attack object) "non environmental"
    protected GameObject[] _hazardList;

    protected bool _spawnedHazard = false;


    protected override void Start() {
        //Move to concrete implementation
        explosionSFX = new string[4];
        wooshSFX = new string[3];
        for (int i = 0; i < 4; ++i)
            explosionSFX[i] = "Explosions_" + i;
        for(int i = 0; i < 3; ++i)
            wooshSFX[i] = "Wooshes_" + i;

        //Get the hazards from the hazard manager
        _hazardList = new GameObject[_hazardRef.Length];
        for(int i = 0; i < _hazardList.Length; i++) {
            print("Requesting hazard: " + _hazardRef[i]);
            _hazardList[i] = ResourceManager.GetInstance().GetHazardManager().findHazard(_hazardRef[i]);
        }

        base.Start();
    }

    private void PlayerAttackCooldown() {
        _player.canAttack = true;
    }

    public override void OnEnter() {
        _player.attacking = true;
        _player.canAttack = false;
        Invoke("PlayerAttackCooldown", _player.attackCD);
        base.OnEnter();
    }


    public override void Finish() {
        _spawnedHazard = false;
        _player.attacking = false;
        base.Finish();
    }

    protected void spawnHazard(int index, Vector3 position, Quaternion rotation, Vector3 scale, bool parented, float damageMod) {
        if (!_spawnedHazard) {
            GameObject hazard = _hazardList[index];
            hazard.transform.position = new Vector3(position.x, position.y, -1.0f);
            hazard.transform.rotation = rotation;
            hazard.transform.localScale = scale;
            hazard.GetComponent<Hazard>().player = _player.gameObject;
            hazard.GetComponent<Hazard>().damageMod = damageMod;
            hazard.layer = LayerMask.NameToLayer("Hazard");
            //if (parented)
            //    GameObject.Instantiate(hazard, _player.transform);
            //else
            GameObject newHazard = GameObject.Instantiate(hazard);
            //newHazard.transform.parent = _player.transform;
        }
        _spawnedHazard = true;
    }
}
