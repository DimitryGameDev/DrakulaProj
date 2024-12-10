using System;
using UnityEngine;

public class InteractiveStateManager : SingletonBase<InteractiveStateManager>
{
    private InteractiveState interactiveState;
    private InteractiveObject[] objects;
    
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        interactiveState = InteractiveState.Instance;
        objects = FindObjectsOfType<InteractiveObject>();

        Load();
    }

    private void Load()
    {
        for (int i = 0; i < objects.Length; i++) objects[i].LoadState();
    }

    public void Save()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            interactiveState.Save(objects[i], objects[i].WosActive);
        }
    }
}
