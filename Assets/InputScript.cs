using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputScript : MonoBehaviour {
	GameObject MainGameObj;
	Main other;
	// Use this for initialization
	void Start () {
		MainGameObj= GameObject.Find("Main");
		other = (Main) MainGameObj.GetComponent(typeof(Main));	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
