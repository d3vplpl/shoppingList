using UnityEngine;
using System.Collections;

public class BuyScript : MonoBehaviour {

	GameObject MainGameObj;
	Main other;
	void Start()
	{
		MainGameObj = GameObject.Find("Main");
		other = (Main)MainGameObj.GetComponent(typeof(Main));

	}
	void OnMouseDown() {
		Debug.Log("Buy pressed");
		other.BuyListElement(this.gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
