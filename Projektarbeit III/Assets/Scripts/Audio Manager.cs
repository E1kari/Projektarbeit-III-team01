using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static S_AudioData;

public class AudioManager : MonoBehaviour
{
    S_AudioData audioData;
    AudioSource musicSource;
    AudioSource effectSource;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioData = Resources.Load<S_AudioData>("Scriptable Objects/AudioData");
        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.ToLower().Contains("level"))
        {
            TriggerMusic();
        }
    }

    public void TriggerMusic()
    {
        S_SceneSaver sceneSaver = Resources.Load<S_SceneSaver>("Scriptable Objects/S_SceneSaver");
        Scene scene = SceneManager.GetActiveScene();

        if (sceneSaver.GetPreviousLevelSceneName() != scene.name)
        {
            string levelNameNumber = scene.name.Substring(scene.name.Length - 1, 1);
            int levelNum = int.Parse(levelNameNumber);

            PlayMusic(levelNum);
        }
    }
    public void PlayAudio(AudioIndex audioIndex)
    {
        AudioClip clip = audioData.GetAudioClip(audioIndex);
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(int levelNumber)
    {
        Enum.TryParse("Music_Level" + levelNumber, out AudioIndex index);

        AudioClip clip = audioData.GetAudioClip(index);
        musicSource.clip = clip;
        musicSource.Play();
    }
}
