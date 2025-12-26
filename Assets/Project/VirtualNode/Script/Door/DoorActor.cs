using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using System.Linq;

public class DoorActor : Actor, I_Interactable
{

    Coroutine Coroutine;
    public enum DoorEvent
    {
        Open,
        Close
    }

    // 1 : Open to the back
    // -1 : Open to the front 
    [System.Serializable]
    public struct DoorTransformValue
    {
        public Transform door;

        public Vector3 localClosePosition;
        public Vector3 localCloseRotation;

        public Vector3 localOpenFrontPosition;
        public Vector3 localOpenFrontRotation;

        public Vector3 localOpenBackPosition;
        public Vector3 localOpenBackRotation;
    }
    [SerializeField] DoorTransformValue[] doors;

    [SerializeField] protected float curDoorWeight = .5f; // 0>.5>1
    [SerializeField] protected float targetDoorWeight; // 0>.5>1

    public bool isOpen { get => curDoorWeight == .5f ? false : true; }
    public virtual Collider _collider { get => this.collider; set => this.collider = value; }
    [SerializeField] private Collider collider;
    public virtual bool isBeenInteractAble { get => isInteractAble; set => isInteractAble = value; }
    [SerializeField] private bool isInteractAble;
    [SerializeField] protected bool lockedValue;
    [SerializeField] private Transform referenceInteractAbleTransform;


    public Transform _transform { get => referenceInteractAbleTransform; set { } }
    public virtual bool isLocked
    {
        get
        {
            if (isOpen)
            {
                lockedValue = false;
                return false;
            }
            return lockedValue;
        }
        set
        {
            lockedValue = value;
        }
    }
    private void Awake()
    {
        if (this.doors == null || this.doors.Length <= 0)
            return;

        for (int i = 0; i < this.doors.Length; i++)
        {
            this.doors[i].localClosePosition = this.doors[i].door.localPosition;
            this.doors[i].localCloseRotation = this.doors[i].door.localRotation.eulerAngles;
        }
    }
    public void Open()
    {
        this.Open(1);
    }
    public void Open(float weight)
    {

        this.targetDoorWeight = weight;

        if (this.Coroutine == null)
            this.Coroutine = StartCoroutine(DoorEventUpdate());

        base.NotifyObserver(DoorEvent.Open);
        this.NotifyObserver();
    }
    public void Close()
    {

        this.targetDoorWeight = .5f;

        if (this.Coroutine == null)
        {
            this.Coroutine = StartCoroutine(DoorEventUpdate());
        }

        base.NotifyObserver(DoorEvent.Close);
        this.NotifyObserver();
    }
    public void DoInteract()
    {
        this.DoInteract(null);
    }
    public virtual void DoInteract(I_Interacter i_Interacter)
    {

        if (isLocked)
            return;

        if (isOpen)
            Close();
        else
        {
            Debug.Log("I_Interacter = " + i_Interacter);
            if (i_Interacter is Character character)
            {
                float dot = Vector3.Dot(this.transform.forward, (character.transform.position - this.transform.position).normalized);
                Debug.Log("dot = " + dot);
                if (dot > 0)
                    this.Open(1);
                else
                    this.Open(0);
                return;
            }

            Open();
        }

    }
    protected List<IObserverDoor> observerDoors = new List<IObserverDoor>();
    public void AddOberver(IObserverDoor observerDoor)
    {
        this.observerDoors.Add(observerDoor);
    }
    public void RemoveOberver(IObserverDoor removedObserver)
    {
        this.observerDoors.Remove(removedObserver);
    }
    public void NotifyObserver()
    {
        if (this.observerDoors.Count <= 0)
            return;

        for (int i = 0; i < this.observerDoors.Count; i++)
        {
            this.observerDoors[i].OnNotify(this);
        }
    }

    protected override void OnDrawGizmos()
    {
        if (this.isEnableGizmos == false)
            return;

        Gizmos.color = color;
        Gizmos.DrawCube(_transform.position, Vector3.one * .3f);
        Vector3 cameraPos;
        if (Application.isPlaying)
        {
            cameraPos = Camera.main.transform.position;
        }
        else
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, this.transform.position) < base.drawNameDistance)
        {
            Handles.Label(_transform.position + (Vector3.up * .35f), this.name);
        }

        Gizmos.DrawRay(_transform.position, Vector3.up * .35f);
    }

    protected IEnumerator DoorEventUpdate()
    {

        while (this.curDoorWeight != this.targetDoorWeight)
        {
            this.curDoorWeight = Mathf.MoveTowards(this.curDoorWeight, this.targetDoorWeight, Time.deltaTime);
            Debug.Log("DoorEventUpdate");
            if (this.doors != null && this.doors.Length > 0)
            { 
                for (int i = 0; i < this.doors.Length; i++)
                {


                    this.doors[i].door.localPosition = BezierurveBehavior.GetPointOnBezierCurve
                        (this.doors[i].localClosePosition + this.doors[i].localOpenBackPosition
                        , this.doors[i].localClosePosition
                        , this.doors[i].localClosePosition + this.doors[i].localOpenFrontPosition
                        , this.curDoorWeight);

                    this.doors[i].door.rotation = Quaternion.Euler(
                        BezierurveBehavior.GetPointOnBezierCurve
                        (this.doors[i].localCloseRotation + this.doors[i].localOpenBackRotation
                        , this.doors[i].localCloseRotation
                        , this.doors[i].localCloseRotation + this.doors[i].localOpenFrontRotation
                        , this.curDoorWeight)
                        );
                   

                }

            }
            yield return null;

        }
        this.Coroutine = null;
    }
}

public interface IObserverDoor
{
    public void OnNotify(DoorActor door);
}
