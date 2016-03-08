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
		if (other.productName.text!="")other.AddNewListElement(other.productName.text,0,false);
		other.productName.text =""; //set product name input to blank after adding product
		other.FetchTheListFromWeb();
		other.PersistTheList();

		//other.PersistTheListOnWeb();
	}

}
