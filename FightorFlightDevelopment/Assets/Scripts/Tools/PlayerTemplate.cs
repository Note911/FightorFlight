using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTemplate {

    public enum CharacterIndex {FIGHTER, NINJA, WIZARD, KNIGHT};
    public CharacterIndex characterIndex;
    public int skinIndex = 0;

    public PlayerTemplate(CharacterIndex character, int skin) {
        characterIndex = character;
        skinIndex = skin;
    }
}
