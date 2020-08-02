using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilityInfo : MonoBehaviour
{
    public AbilityData abinfo;

    public void GetAbilityInfo(){
        PokeDataFromJSON.dex.handler.AbName.text = abinfo.n.Replace("-", " ");
        AbEffectInfo ab = PokeDataFromJSON.dex.AbilityList.abilities.Find(a => a.id == abinfo.id);
        if(ab!=null){
            PokeDataFromJSON.dex.handler.AbShort.text = ab.effect_entries.short_effect;
            PokeDataFromJSON.dex.handler.AbDetail.text = ab.effect_entries.effect;
        }
        else{
            PokeDataFromJSON.dex.handler.AbShort.text = "No official data revealed...";
            PokeDataFromJSON.dex.handler.AbDetail.text = "No official data revealed...";
        }
        
        PokeDataFromJSON.dex.handler.abPanel.gameObject.SetActive(true);
    }
}
