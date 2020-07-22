using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [Header("Basic Info")]
    public int id;
    public string name;

    public string url;

    [Header("Sprites")]
    public Sprites sprites;

    public Sprite icon;

    public Texture2D pokeIcon;

    [Header("Types")]
    public List<Types> types;

    public Info info;

    [Header("Stats")]
    public List<Stat> stats;

    [Header("Abilities")]
    public List<Abilities> abilities;
    
    public bool Loaded = false;
    
}

[System.Serializable]
public class Sprites{
    public string front_default;
    public string front_shiny;
}

[System.Serializable]
public class Type{
    public string name;
}

[System.Serializable]
public class Types{
    public Type type;
    public int slot;
    
}


[System.Serializable]
public class Info{
    public List<Description> flavor_text_entries;
}
[System.Serializable]
public class Description{
    public string flavor_text;
}

[System.Serializable]
public class Result{
    public List<Pokemon> results;
}

[System.Serializable]
public class Stat{
    public int base_stat;
}

[System.Serializable]
public class Ability{
    public string name;
    public string url;
}

[System.Serializable]
public class Abilities{
    public Ability ability;
    public int slot;

    public bool is_hidden;
}

