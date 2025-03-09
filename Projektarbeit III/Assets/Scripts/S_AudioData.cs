using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class S_AudioData : ScriptableObject
{
    [SerializeField] private SoundList[] allAudioClips_;

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        public AudioClip[] sounds;
    }

    public enum AudioIndex
    {
        Player_Run,
        Player_Jump,
        Player_Dash,
        Player_Death,
        Environment_Well,
        Environment_BreakingBlock,
        Environment_FallingBlock,
        Enemy_Attack,
        Enemy_Death,
        Music_Level1,
        Music_Level2,
        Music_Level3,
        UI_Timer,
        UI_Leaderboard,
        UI_PauseMenu,
        UI_MainMenu,
        UI_LevelSelect,
        UI_LevelTransition,
        UI_Settings,

    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(AudioIndex));
        Array.Resize(ref allAudioClips_, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            allAudioClips_[i].name = names[i];
        }
    }
#endif

    public AudioClip GetAudioClip(AudioIndex index)
    {
        int rand = UnityEngine.Random.Range(0, allAudioClips_[(int)index].sounds.Length - 1);
        return allAudioClips_[(int)index].sounds[rand];
    }
}