using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAvatarInfo", menuName = "ScriptableObjects/PlayerAvatarInfo", order = 3)]
public class PlayerAvatarInfo : ScriptableObject
{
    public int index;
    public Sprite img;
}
