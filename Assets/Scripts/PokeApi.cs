using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class PokeApi : MonoBehaviour
{

    #region Singleton
    
    public Pokedex pokedex;
    public static PokeApi api;
    

    private void Awake() {
        api = this;
    }

    #endregion

    private bool imageReady = false, descReady = false;
    public List<Pokemon> pokemon;

    public int currentPoke = 0;
    public const string  apiLink = "https://pokeapi.co/api/v2/pokemon/";
    public const string infoLink = "https://pokeapi.co/api/v2/pokemon-species/";

    public TMP_Text pokeName, pokeNumber, PokeDescription;
    public Image pokeSprite;

    public Image[] typeImages = new Image[2];

    public Image[] statsBars = new Image[6];

    public GameObject abilityPanel;

    public Button[] moveButtons;


    private void Start() {
        
        StartCoroutine(GetAllPokemon());
            
    }

    public IEnumerator GetAllPokemon(){
        UnityWebRequest webRequest = UnityWebRequest.Get(apiLink + "?limit=807");
        yield return webRequest.SendWebRequest();

        if(webRequest.isNetworkError){
            Debug.Log("No Internet");
        }
        else{
            Result results = JsonUtility.FromJson<Result>(webRequest.downloadHandler.text);
            pokemon = results.results;
            DrawDex(pokemon.First());
        }
    }
    
    public IEnumerator GetPokemon(Pokemon poke){
        OnOffButton(false);
        imageReady = false;
        descReady = false;
        
        UnityWebRequest webRequest = UnityWebRequest.Get(poke.url);
        yield return webRequest.SendWebRequest();
        if(webRequest.isNetworkError){
            Debug.Log("Deu ruim!");
        }
        else{
            JsonUtility.FromJsonOverwrite(webRequest.downloadHandler.text, poke);
            StartCoroutine(URLtoTexture(poke));
            StartCoroutine(GetDescription(poke));
            
            
            yield return new WaitUntil(() => imageReady && descReady);
            
            poke.Loaded = true;
        
    
            
            pokeName.text = poke.name;
            pokeNumber.text = ShowPokeNumber(poke.id);
            ShowType(poke);
            DrawBaseStats();
            DrawAbilities();
            pokeSprite.sprite = poke.icon;
            pokeSprite.gameObject.SetActive(true);
            string desc = poke.info.flavor_text_entries[7].flavor_text.Replace('\n', ' ');
            

            PokeDescription.text = desc;

            OnOffButton(true);
        }

        
    }
    
    public IEnumerator URLtoTexture(Pokemon poke){
        UnityWebRequest texRequest = UnityWebRequestTexture.GetTexture(poke.sprites.front_default);
        yield return texRequest.SendWebRequest();
        if(texRequest.isNetworkError){
            Debug.Log("Deu ruim na imagem");
        }
        else{
            poke.pokeIcon = ((DownloadHandlerTexture)texRequest.downloadHandler).texture;
            if(poke.pokeIcon != null){
                poke.pokeIcon.filterMode = FilterMode.Point;
                Sprite sprite = Sprite.Create(poke.pokeIcon, new Rect(0.0f, 0.0f, poke.pokeIcon.width, poke.pokeIcon.height), new Vector2(0.5f, 0.5f), 96f);
                poke.icon = sprite;
                imageReady = true;
            }
            else{
                poke.icon = null;
                imageReady = true;
            }
            
        }
                
    }
    
    string ShowPokeNumber(int id){
        if(id < 10){
            return "00" + id.ToString();
        }
        else if(id<100){
            return "0" + id.ToString();
        }
        else{
            return id.ToString();
        }
    }
    
    public void ShowType(Pokemon poke){
        HideTypeIcon();
        int count = 0;
        Sprite typeIcon = null;
        foreach(Types type in poke.types){
            switch(type.type.name){
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
            typeImages[count].sprite = typeIcon;
            typeImages[count].gameObject.SetActive(true);
            count ++;
       
        }

        
    }
    
    public IEnumerator GetDescription(Pokemon poke){
        using(UnityWebRequest webRequest = UnityWebRequest.Get(infoLink + poke.id.ToString())){
            yield return webRequest.SendWebRequest();
            if(webRequest.isNetworkError){
                Debug.Log("Deu ruim!");
            }
            else{
                poke.info = JsonUtility.FromJson<Info>(webRequest.downloadHandler.text);
                descReady = true;
            }
        }
    }

    public void DrawDex(Pokemon poke){
        StartCoroutine(GetPokemon(poke));
    }

    public void NextPokemon(){
        if(currentPoke == pokemon.Count-1){
            DrawDex(pokemon.First());
            currentPoke = 0;
        }
        else{
            currentPoke ++;
            DrawDex(pokemon[currentPoke]);
            
        }
        
    }

    public void BackPokemon(){
        if(currentPoke <= 0){
            
            DrawDex(pokemon.Last());
            currentPoke = pokemon.Count-1;
        }
        
        else{
            currentPoke --;
            DrawDex(pokemon[currentPoke]);
        }
        
    }

    public void HideTypeIcon(){
        foreach (Image icon in typeImages)
        {
            icon.gameObject.SetActive(false);
        }
    }

#region Stats
    public void DrawBaseStats(){
        Pokemon poke = pokemon[currentPoke];
        for(int i = 0; i < poke.stats.Count; i++){
            statsBars[i].fillAmount = (float)poke.stats[i].base_stat/200;
            StatToText(statsBars[i], poke.stats[i].base_stat);
        }
        
        
    }

    public void DrawMinStats(){
        Pokemon poke = pokemon[currentPoke];
        for(int i = 0; i < poke.stats.Count; i++){
            float stat = 0;
            statsBars[i].fillAmount = (float)poke.stats[i].base_stat/200;
            if(i == 0){
                stat = StatsFormula(poke.stats[i].base_stat, 50, 0, 0, 0.9f,true);
            }
            else{
                stat = StatsFormula(poke.stats[i].base_stat, 50, 0, 0, 0.9f,false);
            }
            stat = Mathf.Floor(stat);
            StatToText(statsBars[i], stat);
        }
    }

    public void DrawMaxStats(){
        Pokemon poke = pokemon[currentPoke];
        for(int i = 0; i < poke.stats.Count; i++){
            float stat = 0;
            statsBars[i].fillAmount = (float)poke.stats[i].base_stat/200;
            if(i == 0){
                stat = StatsFormula(poke.stats[i].base_stat, 100, 31, 252, 1.1f,true);
            }
            else{
                stat = StatsFormula(poke.stats[i].base_stat, 100, 31, 252, 1.1f,false);
            }
            stat = Mathf.Floor(stat);
            StatToText(statsBars[i], stat);
        }
    }

    void StatToText(Image bar, float stat){
        bar.transform.GetChild(0).GetComponent<TMP_Text>().text = stat.ToString();
    }

    public float StatsFormula(float basestat, float level, float iv, float ev, float nature, bool isHP){
        float stat = 0;
        if(isHP){
            stat = (((((2*basestat) + iv + (ev/4))*level)/100) + level + 10);
        }
        else{
            stat = (((((2*basestat) + iv + (ev/4))*level)/100) + 5) * nature;
        }
        

        return stat;
    }
#endregion

    void DrawAbilities(){
        List<Abilities> abilities = pokemon[currentPoke].abilities;
        for(int i = 0; i < abilityPanel.transform.childCount; i++){
            abilityPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        for(int i = 0; i < abilities.Count; i++){
            GameObject button = abilityPanel.transform.GetChild(i).gameObject;
            string abiName = abilities[i].ability.name.Replace('-', ' ');
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = abiName;
            if(abilities[i].is_hidden){
                button.GetComponent<Image>().color = Color.gray;
                button.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
            }
            else{
                button.GetComponent<Image>().color = Color.white;
                button.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.black;
            }
            button.SetActive(true);

        }
    }

    public void QuitApp(){
        Application.Quit();
    }

    void OnOffButton(bool state){
        foreach (Button button in moveButtons)
        {
            button.interactable = state;
        }
    }

    

}
