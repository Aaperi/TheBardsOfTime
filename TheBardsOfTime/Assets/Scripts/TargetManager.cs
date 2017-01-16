using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour {

    public List<GameObject> frontList = new List<GameObject>();
    public List<GameObject> aroundList = new List<GameObject>();
    public CC CCref;
    private int pointer = 0;

    void Start()
    {
        CCref = GameObject.Find("Player").GetComponent<CC>();
    }

    public void updateList(string imp, List<GameObject> list)
    {
        if (imp == "Front") {
            frontList = list;
        } else
            aroundList = list;

        if (frontList.Count < 1 && aroundList.Count < 1) {
            pointer = 0;
        }

        if (frontList.Count > 0 && aroundList.Count > 0) {
            foreach(GameObject go in aroundList) {
                if (frontList.Contains(go)) {
                    aroundList.Remove(go);
                }
            }
        }
    }

    public void changeTarget(string who)
    {
        Debug.Log("Pointer: " + pointer);
        if (who == "Next") {
            pointer += 1;
            if (pointer < frontList.Count && frontList.Count > 0) {
                CCref.target = frontList[pointer];
            } else {
                pointer = 0;
                CCref.target = frontList[pointer];
            }
            Debug.Log(CCref.target.name);
        } else {
            pointer = 0;
            if (frontList.Count > 0)
                CCref.target = frontList[pointer];
            /*else if (aroundList.Count > 0)
                CCref.target = aroundList[pointer];*/
            else
                CCref.target = null;
        }
    }
}
