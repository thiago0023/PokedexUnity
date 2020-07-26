using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public class FormsHandler : MonoBehaviour
{

    public PokemonData pokemon;
    public GameObject content, prefab, scroll;
    

    // Start is called before the first frame update
    void Start()
    {
        pokemon = PokeDataFromJSON.dex.pokemon[2];
        foreach(Forms form in pokemon.info.varieties){
            var go = Instantiate(prefab);
            prefab.transform.SetParent(content.transform);
            prefab.transform.localScale = Vector3.one;
            prefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + form.n);
           
        }
        scroll.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
