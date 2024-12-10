using UnityEngine;
using System;
using System.Collections.Generic;

public class InteractiveState : SingletonBase<InteractiveState>
{
    private const string FileName = "InteractiveState.dat";
    [SerializeField] private ObjectState[] objData;
    
    [Serializable]
    private class ObjectState
    {
        public string objName;
        public bool state;
    }
    
    private void Awake()
    {
        if (FileHandler.HasFile(FileName))
        {
            //Debug.Log("Файл найден");
            Saver<ObjectState[]>.TryLoad(FileName,ref objData);
        }
        else
        {
            //Debug.Log("Создан новый файл" + FileName);
            InteractiveObject[] objects = FindObjectsOfType<InteractiveObject>();
            var listObjState = new List<ObjectState>();
            
            for (int i = 0; i < objects.Length; i++)
            {
                ObjectState state = new ObjectState();
                
                state.objName = objects[i].name;
                state.state = objects[i].WosActive;
                
                listObjState.Add(state);
            }
            objData = listObjState.ToArray();
            Saver<ObjectState[]>.Save(FileName, objData);
        }
        Init();
    }
    
    public void Save(InteractiveObject currentObj,bool state)
    {
        if (Instance)
        {
            foreach (var item in objData)
            {
                if (item.objName == currentObj.name)
                {
                    if (state != item.state)
                    {
                        item.state = state;
                        Saver<ObjectState[]>.Save(FileName, objData);
                        return;
                    }
                }
            }
           
        }
        else
        {
            Debug.Log("Bruuuh");
        }     
    }
    
    public bool GetState(InteractiveObject currentObj)
    {
        foreach (var item in objData)
        {
            if (item.objName == currentObj.name)
            {
               return item.state;
            }
        }
        return false;
    }

    public void ResetState()
    {
        FileHandler.Reset(FileName);
    }
}
