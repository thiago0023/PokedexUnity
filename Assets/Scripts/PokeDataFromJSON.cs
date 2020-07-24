using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public class PokeDataFromJSON : MonoBehaviour
{
    public static PokeDataFromJSON dex;
    public TextAsset pokemonData;
    public TextAsset DetailsData;

    public List<PokemonData> pokemon;

    public GameObject DexList, CardPrefab, DexArea;

    public TMP_Text Number, Name, FlavorText;
    public GameObject Abilities, StatsBars, TypeBar;
    public Image sprite;

    public TMP_Dropdown flavorDrop;

    public DexHandler handler;

    public Scene dexListScene;

    public Scrollbar scroll;



    public int currentPoke = 0;

    
    

    private void Awake()
    {
        Debug.Log("Started");
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        dex = this;
        DontDestroyOnLoad(this.gameObject);
        GetAllPokemon();
        foreach (PokemonData poke in pokemon)
        {
            GameObject go = Instantiate(CardPrefab, transform.position, Quaternion.identity);
            go.transform.SetParent(DexList.transform);
            go.transform.localScale = Vector3.one;
            string n = poke.id.ToString();
            if(poke.id < 10){
                n = "00" + n;
            }
            else if(poke.id < 100){
                n = "0" + n;
            }
            go.transform.GetChild(0).GetComponent<TMP_Text>().text = n;
            go.transform.GetChild(1).GetComponent<TMP_Text>().text = poke.N;
            go.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + poke.N);
            go.GetComponent<Button>().onClick.AddListener(() => LoadDexPage());
        }
        DexArea.AddComponent<UI_ScrollRectOcclusion>();
        SceneManager.sceneLoaded += OnDexPageLoaded;
        
    }
    public void GetAllPokemon(){
        var data = (JObject)JsonConvert.DeserializeObject(pokemonData.text);
        var data2 = (JObject)JsonConvert.DeserializeObject(DetailsData.text);
        List<PokeSpecieData> infoData = new List<PokeSpecieData>();
        for(int i = 0; i < 806; i++){
            infoData.Add(JsonUtility.FromJson<PokeSpecieData>(data2["pokemon-species"][i].ToString()));
        }
        for(int i = 1; i < 807; i++){
            pokemon.Add(JsonUtility.FromJson<PokemonData>(data["pokemon"][i.ToString()].ToString()));
            pokemon[i-1].info = infoData[i-1];
        }
        infoData.Clear();        

    }

    public void DrawDex(){
        sprite.gameObject.SetActive(false);
        PokemonData poke = pokemon[currentPoke];
        Number.text = poke.id.ToString();
        Name.text = poke.N;
        DropdownFill(poke);
        FlavorText.text = FixFlavor(poke.info.FTE[0].e);
        sprite.sprite = Resources.Load<Sprite>("Sprites/" + poke.N );
        sprite.gameObject.SetActive(true);
        DrawBaseStats();
        DrawAbilities(poke);
        ShowType(poke);
    }

    

    public void Next(){
        if(currentPoke < pokemon.Count -1){
            currentPoke++;
        }
        else{
            currentPoke = 0;
        }
        DrawDex();
    }

    public void Back(){
        if(currentPoke > 0){
            currentPoke--;
        }
        else{
            currentPoke = pokemon.Count - 1;
        }
        DrawDex();
    }

    public void DropdownFill(PokemonData poke){
        var options = new List<string>();
        foreach(FlavorText fte in poke.info.FTE)
        {   
            options.Add(fte.v);
        }
        flavorDrop.options.Clear();
        flavorDrop.AddOptions(options);
    }

    public void DropChange(){
        FlavorText.text = FixFlavor(pokemon[currentPoke].info.FTE[flavorDrop.value].e);
    }

    public string FixFlavor(string flavor){
        flavor = flavor.Replace("\n", " ");
        flavor = flavor.Replace("\u000c", " ");
        
        return flavor;
    }

    public void DrawBaseStats(){
        PokemonData poke = pokemon[currentPoke];
        for(int i = 0; i < 6; i++){
            float stat = poke.St[i].bs;
            DrawStats(stat, 200, i);
        }
    }

    public void DrawMaxStats(){
        PokemonData poke = pokemon[currentPoke];
        for(int i = 0; i < 6; i++){
            float stat = 0;
            bool isHP = false;
            if(i == 0){
                isHP = true;
            }
            else{
                isHP = false;
            }
            stat = StatsFormula(poke.St[i].bs, 100, 31, 252, 1.1f, isHP);
            DrawStats(stat, 1000, i);
        }
    }

    public void DrawMinStats(){
        PokemonData poke = pokemon[currentPoke];
        for(int i = 0; i < 6; i++){
            float stat = 0;
            bool isHP = false;
            if(i == 0){
                isHP = true;
            }
            else{
                isHP = false;
            }
            stat = StatsFormula(poke.St[i].bs, 1, 0, 0, 0.9f, isHP);
            DrawStats(stat, 15, i);
            
        }
    }

    public float StatsFormula(float basestat, float level, float iv, float ev, float nature, bool isHP){
        float stat = 0;
        if(isHP){
            stat = (((((2*basestat) + iv + (ev/4))*level)/100) + level + 10);
        }
        else{
            stat = (((((2*basestat) + iv + (ev/4))*level)/100) + 5) * nature;
        }
        
        stat = Mathf.Floor(stat);

        return stat;
    }

    void DrawStats(float stat, float statMax, int i){
        StatsBars.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = stat.ToString();
        StatsBars.transform.GetChild(i).GetComponent<Image>().fillAmount = stat/statMax;
    }

    void DrawAbilities(PokemonData poke){
        List<AbilityData> abilityDatas = poke.Ab;

        for(int i = 0; i < Abilities.transform.childCount; i++){
            Abilities.transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0; i < poke.Ab.Count; i ++){
            GameObject button = Abilities.transform.GetChild(i).gameObject;
            AbilityData ab = poke.Ab[i];
            TMP_Text abName = button.transform.GetChild(0).GetComponent<TMP_Text>();
            if(ab.isH){
                button.GetComponent<Image>().color = Color.gray;
                abName.color = Color.white;
            }
            else{
                button.GetComponent<Image>().color = Color.white;
                abName.color = Color.black;
            }
            abName.text = ab.n;
            button.SetActive(true);

        }

    }

    public void ShowType(PokemonData poke){
        HideTypeIcon();
        int count = 0;
        Sprite typeIcon = null;
        foreach(TypeData type in poke.T){
            switch(type.n){
               case "bug":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[0];
                    break;
                }
                case "dark":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[1];
                    break;
                }
                case "dragon":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[2];
                    break;
                }
                case "electric":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[3];
                    break;
                }
                case "steel":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[4];
                    break;
                }
                case "fairy":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[5];
                    break;
                }
                 case "fighting":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[6];
                    break;
                }
                 case "fire":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[7];
                    break;
                }
                 case "flying":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[8];
                    break;
                }
                 case "water":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[9];
                    break;
                }
                 case "ghost":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[10];
                    break;
                }
                 case "grass":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[11];
                    break;
                }
                 case "ground":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[12];
                    break;
                }
                 case "ice":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[13];
                    break;
                }
                 case "normal":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[14];
                    break;
                }
                 case "poison":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[15];
                    break;
                }
                 case "psychic":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[16];
                    break;
                }
                 case "rock":
                {
                    typeIcon = Resources.LoadAll<Sprite>("Type/PokeTypes")[17];
                    break;
                }
               
                
            }
            GameObject typeGO = TypeBar.transform.GetChild(count).gameObject;
            typeGO.GetComponent<Image>().sprite = typeIcon;
            typeGO.SetActive(true);
            count ++;
       
        }

        
    }

    void HideTypeIcon(){
        for(int i = 0; i < TypeBar.transform.childCount; i++){
            TypeBar.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void LoadDexPage(){
        currentPoke = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        dexListScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("DexJSON", LoadSceneMode.Additive);
    }

    public void OnDexPageLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == "DexJSON"){
            GetUI();
            DrawDex();
        }
    }

    public void GetUI(){
        handler = GameObject.Find("DexHandler").GetComponent<DexHandler>();
        Name = handler.Name;
        Number = handler.Number;
        FlavorText = handler.FlavorText;
        Abilities = handler.Abilities;
        StatsBars = handler.StatsBars;
        TypeBar = handler.TypeBar;
        sprite = handler.sprite;
        flavorDrop = handler.flavorDrop;
        handler.Next.onClick.AddListener(()=> Next());
        handler.Back.onClick.AddListener(()=> Back());
        handler.Base.onClick.AddListener(()=> DrawBaseStats());
        handler.Min.onClick.AddListener(()=> DrawMinStats());
        handler.Max.onClick.AddListener(()=> DrawMaxStats());
    }

    
    
    
}
