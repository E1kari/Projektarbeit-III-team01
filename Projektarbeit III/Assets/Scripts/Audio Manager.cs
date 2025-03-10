using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static S_AudioData;

public class AudioManager : MonoBehaviour
{
    private S_AudioData audioData;
    private AudioSource musicSource;
    private AudioSource effectSource;


    void Start()
    {
        if (GameObject.FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioData = Resources.Load<S_AudioData>("Scriptable Objects/AudioData");

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        effectSource = gameObject.AddComponent<AudioSource>();

        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        musicSource.volume = audioData.masterVolume * audioData.musicVolume;
        effectSource.volume = audioData.masterVolume * audioData.effectVolume;
    }

    public void SetVolumes(float[] volumes)
    {
        audioData.masterVolume = volumes[0];
        audioData.musicVolume = volumes[1];
        audioData.effectVolume = volumes[2];

        audioData.saveJSON();

        UpdateVolumes();
    }

    public float[] GetVolumes()
    {
        return new float[] { audioData.masterVolume, audioData.musicVolume, audioData.effectVolume };
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
