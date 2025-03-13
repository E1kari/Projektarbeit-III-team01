using UnityEngine;
using UnityEngine.UI;
using static S_AudioData;

public class SettingManager : MonoBehaviour
{
    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GetVolumes();
    }

    public void SetVolumes()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayAudio(AudioIndex.UI_sliderGrab);

        float masterVolume = GameObject.Find("Master Volume Slider").GetComponent<Slider>().value / 10f;
        float musicVolume = GameObject.Find("Music Volume Slider").GetComponent<Slider>().value / 10f;
        float effectVolume = GameObject.Find("Effect Volume Slider").GetComponent<Slider>().value / 10f;

        audioManager.SetVolumes(new float[] { masterVolume, musicVolume, effectVolume });
    }

    public void GetVolumes()
    {
        float[] volumes = audioManager.GetVolumes();

        GameObject.Find("Master Volume Slider").GetComponent<Slider>().value = volumes[0] * 10f;
        GameObject.Find("Music Volume Slider").GetComponent<Slider>().value = volumes[1] * 10f;
        GameObject.Find("Effect Volume Slider").GetComponent<Slider>().value = volumes[2] * 10f;
    }
}
