using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Evento : UnityEvent<ParamsEvt> { }

public class EventManager : MonoBehaviour
{

    public static EventManager Instance;

    private Dictionary<string, Evento> _eventDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _eventDictionary = new Dictionary<string, Evento>();
        }
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public static void StartListening(string eventName, UnityAction<ParamsEvt> listener)
    {
        Evento thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new Evento();
            thisEvent.AddListener(listener);
            Instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<ParamsEvt> listener)
    {
        if (Instance == null) return;
        Evento thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, ParamsEvt arg = null)
    {
        Evento thisEvent = null;
        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(arg);
        }
    }
}
