using UnityEngine;
using System.Collections;

public class AddScript : MonoBehaviour {

	GameObject MainGameObj;
	Main other;
	void Start () {
		MainGameObj= GameObject.Find("Main");
		other = (Main) MainGameObj.GetComponent(typeof(Main));	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown(){
		Debug.Log("Dodaj pressed");
		other.AddNewListElement();
		other.productName.text =""; //set product name input to blank after adding product
	}

}
