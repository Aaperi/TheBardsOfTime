using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundScript : MonoBehaviour
{

    AudioMixer master;
    List<AudioSource> allClips;
    List<AudioMixerGroup> masterGroup;
    List<AudioSource> foley = new List<AudioSource>();
    List<AudioSource> music = new List<AudioSource>();
    [HideInInspector]
    public List<AudioMixerGroup> foleyGroup;
    [HideInInspector]
    public List<AudioMixerGroup> musicGroup;

    void Start()
    {
        master = Resources.Load<AudioMixer>("Sound/Mixer");
        musicGroup = new List<AudioMixerGroup>(master.FindMatchingGroups("Master/Music"));
        foleyGroup = new List<AudioMixerGroup>(master.FindMatchingGroups("Master/Foley"));
        masterGroup = new List<AudioMixerGroup>(master.FindMatchingGroups("Master"));

        foreach (AudioClip ac in Resources.LoadAll<AudioClip>("Sound/Foley")) {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.playOnAwake = false;
            temp.clip = ac;
            foley.Add(temp);
        }

        foreach (AudioClip ac in Resources.LoadAll<AudioClip>("Sound/Music")) {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.playOnAwake = false;
            temp.clip = ac;
            music.Add(temp);
        }

        allClips = new List<AudioSource>();
        allClips.AddRange(foley);
        allClips.AddRange(music);

        PlaySound("luonto_ambienssi_placeholder", musicGroup[1], true);
    }

    public void PlaySound(string Name, AudioMixerGroup Channel, bool Loopit)
    {
        AudioSource temp = null;

        if (foleyGroup.Contains(Channel))
            if (!foley.Find(item => item.clip.name == Name).isPlaying)
                temp = foley.Find(item => item.clip.name == Name);

        if (musicGroup.Contains(Channel))
            if (!music.Find(item => item.clip.name == Name).isPlaying)
                temp = music.Find(item => item.clip.name == Name);

        if (temp != null) {
            StopSound(Name);
            temp.loop = Loopit;
            temp.outputAudioMixerGroup = Channel;
            temp.Play();
        } else
            Debug.Log("Unable to play: " + Name + "\nIt might be already playing");
    }

    public void StopSound(string Name)
    {
        AudioSource temp = null;
        temp = allClips.Find(item => item.clip.name == Name);

        if (temp != null) {
            temp.Stop();
            temp.outputAudioMixerGroup = null;
        } else
            Debug.Log("Unable to stop: " + Name);
    }
}
