using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{

    [Header("Volume")]

    [Range(0, 1)]
    public float masterVolume = 1f;
    [Range(0, 1)]
    public float musicVolume = 1f;
    [Range(0, 1)]
    public float sfxVolume = 1f;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;


    public static AudioManager Instance { get; private set; }

    private void Update()//TEMP
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
    }


    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    public void UpdateMasterVolume(float volume) => masterBus.setVolume(volume);
    public void UpdateMusicVolume(float volume) => musicBus.setVolume(volume);
    public void UpdateSFXVolume(float volume) => sfxBus.setVolume(volume);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");



    }

    private void Start()
    {
        PlayOneShotString("BGnoise", Vector3.zero);
    }
    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;    
    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }
    public void SetMusicParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }   
    public void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateEventInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }
    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    //OVERIDE FMOD EMITTER ON OBJECT (WITH EMITTER COMPONENT)
    public StudioEventEmitter InitializeEventEmitter(EventReference eventRefrence, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventRefrence;
        eventEmitters.Add(emitter);
        return emitter;
    }

    //PLAY ONE SHOT AUDIO
    public void PlayOneShotString(string sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(FmodEvents.Instance.GetSound(sound), worldPos);
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);

    }
    //CREATE EVENT INSTANCE (PLAYS SOURCE OF AUDIO)
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }
    // i could make event instances into a list and edit parameters from there, but add functionality later.
    private void CleanUp()
    {
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        foreach(StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }
    private void OnDestroy()
    {
        CleanUp();
    }
}
