using UnityEngine;
using System.Collections;

[System.Serializable]
public class JSON_ListElement {

	public int id;
	public string item_name;

	public static JSON_ListElement CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<JSON_ListElement>(jsonString);
	}
}

