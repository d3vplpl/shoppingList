using UnityEngine;
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
	public List<ListElement> Lista;
	public InputField productName;
    // Use this for initialization
    void Start()
    {
		Lista = new List<ListElement>();

		productName = InputSlot.GetComponentInChildren<InputField>();

    }

	public void AddNewListElement() {
		ListElement li = new ListElement();
		li.tekst = productName.text;
		Lista.Add(li);
		li.GO = Instantiate(GOSlot, new Vector2(-7f, Ycorrection - Lista.IndexOf(li)* HEIGHT_OF_LIST_ELEMENT),
			Quaternion.identity) as GameObject;
        
		li.BtnRemove = Instantiate(BtnRemoveSlot, new Vector2(4f, Ycorrection- Lista.IndexOf(li)* HEIGHT_OF_LIST_ELEMENT),
            Quaternion.identity) as GameObject;

		li.GO.GetComponent<TextMesh>().text = li.tekst;
        li.BtnRemove.GetComponent<TextMesh>().text = "X";     

	}
    public void RemoveListElement(GameObject liGO) {
		int removedIndex = 0;
		List<ListElement> ListaForReEnumeration = new List<ListElement>();
		ListaForReEnumeration = Lista;
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
    }

	// Update is called once per frame
	void Update () {
	
	}
}
