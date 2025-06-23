using System;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public CameraModel cameraSettings;
    public GameModel settings;
    public List<Stat> stats;
    public List<Buff> buffs;
}

[Serializable]
public class CameraModel
{
    public float roundDuration;
    public float roundRadius;
    public float height;
    public float lookAtHeight;
    public float roamingRadius;
    public float roamingDuration;
    public float fovMin;
    public float fovMax;
    public float fovDelay;
    public float fovDuration;
}

[Serializable]
public class GameModel
{
    public int playersCount;
    public int buffCountMin;
    public int buffCountMax;
    public bool allowDuplicateBuffs;

}

public enum StatType
{
    Health,
    Armor,
    Damage,
    Vampirism,
}

public enum BuffType
{
    Vampir,
    Armor,
    Super,
    Titan,
}

[Serializable]
public class Stat
{
    public StatType id;
    public string title;
    public string icon;
    public float value;
}

[Serializable]
public class BuffStat
{
    public float value;
    public StatType statId;
}

[Serializable]
public class Buff
{
    public string icon;
    public BuffType id;
    public string title;
    public List<BuffStat> stats;
}
