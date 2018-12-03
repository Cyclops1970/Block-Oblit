using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour {

    public enum BlockType {normal, bomb, remove};  

    public int points;
    public BlockType type;
}
