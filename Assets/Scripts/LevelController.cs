using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    GameObject[] borderBlocks;

	// Use this for initialization
	void Start ()
    {
        //Find all the border blocks and give them a random colour
        borderBlocks = GameObject.FindGameObjectsWithTag("borderBlock");
        foreach(GameObject bb in borderBlocks)
        {
            bb.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }

        //Hide the BlockArea marker used for designing levels
        GameObject[] bam = GameObject.FindGameObjectsWithTag("blockAreaMarker");
        foreach(GameObject b in bam)
        {
            b.GetComponent<Renderer>().enabled = false;
        }
	}
	
}
