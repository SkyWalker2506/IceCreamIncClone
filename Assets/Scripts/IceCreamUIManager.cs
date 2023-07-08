using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IceCreamUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _colorButtonPrefab;
    [SerializeField] private Transform _buttonHolder;
    public static Action<Color> OnCreatePiece;
    public static Action OnStopCreate;

    public void CreateButtons(Color[] colors)
    {
        foreach (var child in _buttonHolder.GetComponentsInChildren<Transform>())
        {
            if (child != _buttonHolder)
            {
                Destroy(child);
            }
        }
        
        for (int i = 0; i < colors.Length; i++)
        {
            GameObject button = Instantiate(_colorButtonPrefab, _buttonHolder);
            Color color = colors[i];
            button.GetComponent<Image>().color = color;
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { OnCreatePiece?.Invoke(color); } );
            trigger.triggers.Add(entry);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { OnStopCreate?.Invoke(); } );
            trigger.triggers.Add(entry);
        }                
    }
    
    
}