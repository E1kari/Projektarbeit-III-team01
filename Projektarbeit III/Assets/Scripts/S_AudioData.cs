using System;
using System.IO;
using SimpleJSON;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class S_AudioData : ScriptableObject
{
    [SerializeField] private SoundList[] allAudioClips_;

    string path_;
    public float masterVolume = 0.5f;
    public float musicVolume = 0.5f;
    public float effectVolume = 0.5f;

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        public AudioClip[] sounds;
    }

    public enum AudioIndex
    {
        Player_Jump,
        Player_WallJump,
        Player_Dash,
        Player_Death,
        Enemy_Attack,
        Enemy_Death,
        Player_GrapplingHook,
        Environment_GrappleSpeedBoost,
        Environment_FallingBlock,
        Environment_BreakableBlock,
        Environment_LevelStart,
        Environment_LevelEnd,
        Music_Level1,
        Music_Level2,
        Music_Level3,
        UI_buttonClick
    }


    private void OnEnable()
    {
#if UNITY_EDITOR
        string[] names = Enum.GetNames(typeof(AudioIndex));
        Array.Resize(ref allAudioClips_, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            allAudioClips_[i].name = names[i];
        }
#endif

        path_ = Application.persistentDataPath + "/settings.json";
        loadJSON();
    }


    public AudioClip GetAudioClip(AudioIndex index)
    {
        int rand = UnityEngine.Random.Range(0, allAudioClips_[(int)index].sounds.Length - 1);
        return allAudioClips_[(int)index].sounds[rand];
    }

    public void saveJSON()
    {
        JSONObject audioSettings = new JSONObject();
        JSONObject audioSettingEntries = new JSONObject();

        audioSettingEntries["Master Volume"] = masterVolume;
        audioSettingEntries["Music Volume"] = musicVolume;
        audioSettingEntries["Effect Volume"] = effectVolume;

        audioSettings.Add("Audio Settings", audioSettingEntries);
        File.WriteAllText(path_, audioSettings.ToString(4));
    }

    public void loadJSON()
    {
        if (!File.Exists(path_))
        {
            saveJSON();
        }

        String audioSettingsJSONString = File.ReadAllText(path_);
        JSONObject json = (JSONObject)JSON.Parse(audioSettingsJSONString);

        JSONNode audioSettings = json["Audio Settings"];

        masterVolume = audioSettings["Master Volume"].AsFloat;
        musicVolume = audioSettings["Music Volume"].AsFloat;
        effectVolume = audioSettings["Effect Volume"].AsFloat;
    }

    void OnDestroy()
    {
        saveJSON();
    }

}