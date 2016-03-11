using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Net;
using System.IO;	

public class Main : MonoBehaviour {
	float Ycorrection = 100f;
	public GameObject GOSlot;
    public GameObject BtnRemoveSlot;
    public GameObject InputSlot;
	public GameObject BtnAddSlot;
	private  const float  HEIGHT_OF_LIST_ELEMENT = 25f; //temporary fixed size
	private const int MAX_LIST_LENGTH = 100; //maximum number of product on list
	public List<ListElement> Lista;
	public GameObject PanelSlot;
	public InputField productName;
	Vector2 scrollPosition;
	private JSONObject jo;
	int enviromentIndex = 1;
	 string [] url ={"http://127.0.0.1:8000/shopping_list.json","http://d3vpl.pythonanywhere.com/shopping_list.json"} ;	
	 string [] deleteUrl ={ "http://127.0.0.1:8000/shopping_list/delete/","http://d3vpl.pythonanywhere.com/shopping_list/delete/"};
	 string [] urlGetByName ={"http://127.0.0.1:8000/shopping_list/get/","http://d3vpl.pythonanywhere.com/shopping_list/get/"};
		// Use this for initialization
  
	void Start()
    {
		Lista = new List<ListElement>();
		FetchTheList();//it's important to fetch the list before creating product name. Logic depends on it.

		FetchTheListFromWeb();

		productName = InputSlot.GetComponentInChildren<InputField>();
		PanelSlot.transform.SetSiblingIndex(0);
		InputSlot.transform.SetSiblingIndex(1);
		BtnAddSlot.transform.SetSiblingIndex(2);
    }

	public void AddNewListElement(string tekstElementu,int isBought, bool fromFetch) {
		if (isListElementExistent(tekstElementu)) return;
		ListElement li = new ListElement();
		li.tekst = tekstElementu;	
		li.isBought = isBought;
		Lista.Add(li);

		li.GO = Instantiate(GOSlot, new Vector2(-78f, Ycorrection - Lista.IndexOf(li)* HEIGHT_OF_LIST_ELEMENT),
			Quaternion.identity) as GameObject;
		li.GO.transform.SetParent(PanelSlot.transform);
		TextMesh tm = li.GO.GetComponent<TextMesh>();
		if (li.isBought==1){ 
			tm.color = Color.gray;
		}
		li.BtnRemove = Instantiate(BtnRemoveSlot, new Vector2(65f, Ycorrection- Lista.IndexOf(li)* HEIGHT_OF_LIST_ELEMENT),
            Quaternion.identity) as GameObject;
		li.GO.GetComponent<TextMesh>().text = li.tekst;
        li.BtnRemove.GetComponent<TextMesh>().text = "X";     
		li.BtnRemove.transform.SetParent(PanelSlot.transform);

		if (!fromFetch) PersistTheElementOnWeb(li);
	}
    public void RemoveListElement(GameObject liGO) {
		//FetchTheListFromWeb();
		int removedIndex = 0;
		foreach (ListElement li in Lista) {
            if (li.BtnRemove.Equals(liGO)) {
				removedIndex = Lista.IndexOf(li);
				Destroy(li.GO);
                Destroy(li.BtnRemove);
				try {
					DeleteTheElementOnWeb(li);
				} catch (WebException we) {
					Debug.Log(we.ToString());
				}
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
	
		//PersistTheList();

    }
	public void PersistTheList () {
		PlayerPrefs.DeleteAll();
		foreach (ListElement li in Lista){
			PlayerPrefs.SetString(Lista.IndexOf(li).ToString(),li.tekst);
			PlayerPrefs.SetInt("Bought_"+Lista.IndexOf(li).ToString(),li.isBought);
		}

	}
	public void PersistTheElementOnWeb(ListElement li){

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlGetByName[enviromentIndex]+li.tekst);
		request.Method = "GET";

		try {HttpWebResponse response = (HttpWebResponse)request.GetResponse();}
		catch (WebException wex) {
			HttpWebResponse response2 =(HttpWebResponse)wex.Response;
		
		if (response2.StatusCode == HttpStatusCode.NotFound) {
			WWWForm wwwForm = new WWWForm();
			wwwForm.AddField("item_name",li.tekst);
				WWW www = new WWW(url[enviromentIndex],wwwForm);
		} else {
			Debug.Log ("Duplicate element already exists on the server?"+ response2.StatusCode.ToString());
		}
		}

	}
	public void DeleteTheElementOnWeb(ListElement li) {

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(deleteUrl[enviromentIndex]+li.tekst);
		request.Method = "DELETE";
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		if (response.StatusCode == HttpStatusCode.NoContent) {
			Debug.Log ("Delete successful");
		} else {
			Debug.Log ("Delete failed "+ response.StatusCode.ToString());
		}
	}
	public void FetchTheList () {
		for (int i=0;i<=MAX_LIST_LENGTH;i++){
			
			try {
				string tekstToAdd =(PlayerPrefs.GetString(i.ToString()));
				int bought = PlayerPrefs.GetInt("Bought_"+i.ToString());
				if (tekstToAdd =="") break; //this means we can't let input blank product
				AddNewListElement(tekstToAdd,bought,true);
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
	public bool isListElementExistent (string tekst) {
		foreach (ListElement li in Lista) {
			if (li.tekst == tekst) return true;
		}
		return false;
	}

	void ConnectToServer() {
		WWW www = new WWW(url[enviromentIndex]);
		StartCoroutine(WaitForRequest(www));
		while (!www.isDone) {
		}
		if (string.IsNullOrEmpty(www.error)){
			jo = new JSONObject(www.text);
		} else {
			Debug.Log ("WWW error: "+ www.error);
		}
	}
	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
	}
	public void FetchTheListFromWeb() {
		ConnectToServer();

		if (jo != null) {
			List<JSONObject> jlist = new List<JSONObject>();
			foreach(JSONObject j in jo.list){
				jlist.Add(j);
			}
			foreach (JSONObject k in jlist) {
				for(int i = 1; i < k.list.Count; i++){ //starts from 1 to prevent fetching ids
					//string key = (string)k.keys[i];
					JSONObject j = (JSONObject)k.list[i];

					int bought = 0;
					if (!isListElementExistent(j.str)) AddNewListElement(j.str,bought,true);
				}
			}
		}
	}
}


