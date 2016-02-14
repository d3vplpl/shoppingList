using UnityEngine;
using System.Collections;

public class RemoveScript : MonoBehaviour {

    GameObject MainGameObj;
    Main other;
    void Start()
    {
        MainGameObj = GameObject.Find("Main");
        other = (Main)MainGameObj.GetComponent(typeof(Main));

    }
    void OnMouseDown() {
        Debug.Log("X pressed");
        other.RemoveListElement(this.gameObject);
        }
}
