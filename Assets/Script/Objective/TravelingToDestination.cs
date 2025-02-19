using UnityEngine;

public class TravelingToDestination : Objective
{
    Vector3 destination;
    Transform objectivier;

    public readonly float destinationRaduis = 2.5f;

    public TravelingToDestination(Transform objectivier,Vector3 destination)
    {
        ObjDescribe = "Reach to destination";
        this.destination = destination;
        this.objectivier = objectivier;
    }

    public override string ObjDescribe { get ; set ; }

    public void SetDestination(Vector3 destination) => this.destination = destination;

    public override bool PerformedDone()
    {
        Debug.Log("Distance = " + Vector3.Distance(destination, objectivier.position));
       if(Vector3.Distance(destination,objectivier.position) <= destinationRaduis)
            return true;

       return false;
    }
}
