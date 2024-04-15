using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    public GameObject[] tileObjs;
    public Highwayman highwayman;
    private void Start(){
        highwayman.tiles = this;
    }
}
