using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class JSON_List  {

	public JSON_ListElement[] JSONList;

	public static JSON_List CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<JSON_List>(jsonString);
	}
	}

