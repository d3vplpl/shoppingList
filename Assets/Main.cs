﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Main : MonoBehaviour {
	float Ycorrection = 5f;
	public GameObject GOSlot;
    public GameObject BtnRemoveSlot;
    public GameObject InputSlot;
	private  const float  HEIGHT_OF_LIST_ELEMENT = 1f; //temporary fixed size
	private const int MAX_LIST_LENGTH = 100; //maximum number of product on list
	public List<ListElement> Lista;
	public GameObject PanelSlot;
	public InputField productName;
	Vector2 scrollPosition;
    // Use this for initialization
    void Start()
    {
		Lista = new List<ListElement>();
		FetchTheList();//it's important to fetch the list before creating product name. Logic depends on it.
		productName = InputSlot.GetComponentInChildren<InputField>();

		InputSlot.transform.SetSiblingIndex(1);
		PanelSlot.transform.SetSiblingIndex(0);
	
    }

	public void AddNewListElement(string tekstElementu,int isBought) {
		ListElement li = new ListElement();
		li.tekst = tekstElementu;	
		li.isBought = isBought;
		Lista.Add(li);
		Vector3 panelPos = PanelSlot.transform.position;
		Ycorrection =  panelPos.y +29f;
		li.GO = Instantiate(GOSlot, new Vector2(-7f, Ycorrection - Lista.IndexOf(li)* HEIGHT_OF_LIST_ELEMENT),
			Quaternion.identity) as GameObject;
		li.GO.transform.SetParent(PanelSlot.transform);
		TextMesh tm = li.GO.GetComponent<TextMesh>();
		if (li.isBought==1){ 
			tm.color = Color.gray;
		}
		li.BtnRemove = Instantiate(BtnRemoveSlot, new Vector2(4f, Ycorrection- Lista.IndexOf(li)* HEIGHT_OF_LIST_ELEMENT),
            Quaternion.identity) as GameObject;
		li.GO.GetComponent<TextMesh>().text = li.tekst;
        li.BtnRemove.GetComponent<TextMesh>().text = "X";     
		li.BtnRemove.transform.SetParent(PanelSlot.transform);
	}
    public void RemoveListElement(GameObject liGO) {
		int removedIndex = 0;
		foreach (ListElement li in Lista) {
            if (li.BtnRemove.Equals(liGO)) {
				removedIndex = Lista.IndexOf(li);
				Destroy(li.GO);
                Destroy(li.BtnRemove);
                Lista.Remove(li);
				break;
            }
		};
		foreach (ListElement li in Lista.Skip(removedIndex)){
			float tempY = li.GO.transform.position.y;
			tempY += HEIGHT_OF_LIST_ELEMENT;
			Vector3 vYposGO = new Vector3(li.GO.transform.position.x, tempY,li.GO.transform.position.z);
			Vector3 vYposRmvBtn = new Vector3(li.BtnRemove.transform.position.x, tempY,li.BtnRemove.transform.position.z);

			li.GO.transform.position = vYposGO;
			li.BtnRemove.transform.position = vYposRmvBtn;
		}
		PersistTheList();
    }
	public void PersistTheList () {
		PlayerPrefs.DeleteAll();
		foreach (ListElement li in Lista){
			PlayerPrefs.SetString(Lista.IndexOf(li).ToString(),li.tekst);
			PlayerPrefs.SetInt("Bought_"+Lista.IndexOf(li).ToString(),li.isBought);
		}

	}
	public void FetchTheList () {
		for (int i=0;i<=MAX_LIST_LENGTH;i++){
			
			try {
				string tekstToAdd =(PlayerPrefs.GetString(i.ToString()));
				int bought = PlayerPrefs.GetInt("Bought_"+i.ToString());
				if (tekstToAdd =="") break; //this means we can't let input blank product
				AddNewListElement(tekstToAdd,bought);
			}
			catch {
				break;	
			}
		}
	}
	public void BuyListElement(GameObject buyGO) {
		foreach (ListElement li in Lista) {
			if (li.GO.Equals(buyGO)) {
				TextMesh tm = li.GO.GetComponent<TextMesh>();
				if (li.isBought ==0){
					li.isBought=1;
					tm.color = Color.gray;
				} else {
					li.isBought=0;
					tm.color = Color.white;
				}


				break;
			}
		};
		PersistTheList();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
