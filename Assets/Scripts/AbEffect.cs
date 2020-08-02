using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbEffect
{
    public List<AbEffectInfo> abilities;
}

[System.Serializable]
public class AbEffectInfo
{
    public int id;
    public AbEffectData effect_entries;
}



[System.Serializable]
public class AbEffectData
{
    public string effect;
    public string short_effect;
}
