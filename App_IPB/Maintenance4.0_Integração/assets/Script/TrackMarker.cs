using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackMarker : MonoBehaviour, ITrackableEventHandler
{

    TrackableBehaviour image_TrackableBehaviour;

    VuMarkManager myVuMark;
    // Use this for initialization
    void Start()
    {
        image_TrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (image_TrackableBehaviour != null)
        {
            image_TrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        myVuMark = TrackerManager.Instance.GetStateManager().GetVuMarkManager();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        print("Status: " + newStatus);
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackerFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackerLost();
        }
    }
    void OnTrackerFound()
    {
        foreach (var item in myVuMark.GetActiveBehaviours())
        {
            string targetObj = item.VuMarkTarget.InstanceId.StringValue;
            transform.GetComponent(targetObj).gameObject.SetActive(true);
        }

    }

    void OnTrackerLost()
    {
        foreach (var item in myVuMark.GetActiveBehaviours())
        {
            string targetObj = item.VuMarkTarget.InstanceId.StringValue;
            transform.GetComponent(targetObj).gameObject.SetActive(false);
        }

    }
}
