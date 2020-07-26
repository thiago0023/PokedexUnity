using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPooling : MonoBehaviour
{
    public Transform cardList;
    public void AddToPool(GameObject obj){
        obj.SetActive(false);
        obj.transform.SetParent(cardList);
        
        
    }

    public GameObject InstantiateFromPool(){
        var go = cardList.GetChild(0).gameObject;
        go.SetActive(true);
        return go;
    }
}

