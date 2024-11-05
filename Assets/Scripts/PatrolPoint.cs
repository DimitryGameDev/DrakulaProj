using System;
using UnityEngine;
using UnityEngine.UI;

public class PatrolPoint : MonoBehaviour
{
    [Space]
    [Header("Visual settings")]
    [SerializeField] protected float radius;
    [SerializeField] protected Color gColor = new(1, 0, 0, 0.3f);
    
    [Space]
    [Header("Debug settings")]
    [SerializeField] private bool isDebug;
    [Space]
    [SerializeField] private Canvas debugBox;
    [SerializeField] private Image image;
    [SerializeField] private Text weightText;
    [SerializeField] private Text lengthToStartText;
    [SerializeField] private Text lenghtToObjText;

    public int pathIndex;
    public int Weight { get; private set; }
    public int LengthToStart { get; private set; }
    public int LenghtToObj { get; private set; }
    public bool IsVisit { get; private set; }
    public float Radius => radius;

    private void Start()
    {
        if (isDebug)
        {
            debugBox.gameObject.SetActive(true);
            weightText.text = 0.ToString();
            lengthToStartText.text = 0.ToString();
            lenghtToObjText.text = 0.ToString();
        }
    }

    public float GetWeight()
    {
        Weight = Convert.ToInt32(LenghtToObj + LengthToStart);
        if (isDebug) weightText.text = Weight.ToString();
        return Weight;
    }
    
    public void SetDistanceToObject(Vector3 obj)
    { 
        LenghtToObj = Convert.ToInt32(Vector3.Distance(transform.position, obj));
        if (isDebug) lenghtToObjText.text = LenghtToObj.ToString();
    }
    
    public void SetDistanceToStart(Vector3 obj)
    {
        LengthToStart = Convert.ToInt32(Vector3.Distance(transform.position, obj));
        if (isDebug) lengthToStartText.text = LengthToStart.ToString();
    }

    public void Reset()
    {
        pathIndex = 0;
        IsVisit = false;
        if (isDebug) image.color = Color.white;
    }

    public void Visit()
    {
        IsVisit = true;
        if (isDebug)  image.color = Color.green;
    }

    public void FinalPoint()
    { 
        if (isDebug)  image.color = Color.yellow;
    }
    public void NoPath()
    { 
        if (isDebug)  image.color = Color.red;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}


