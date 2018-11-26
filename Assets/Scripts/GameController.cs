using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController gc;

    public AudioClip testSound;

    [HideInInspector]
    public bool debug;

    void Awake()
    {
        // Ensure this is the only manager, and make it dontdestroyonload.
        if (gc == null)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //debug = true;
        debug = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    //easy way to reload scene
        if((Input.touchCount==6) || (Input.GetKey(KeyCode.F12)))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
	}
}
