using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UnityARKitMultipeerConnectivity.Examples.CollaborativeSessions
{
    public class ReferencePointFactory : MonoBehaviour
    {
        [SerializeField]
        ARHitTester arHitTester;

        [SerializeField]
        ARReferencePointManager referencePointManager;

        readonly List<ARReferencePoint> referencePoints = new List<ARReferencePoint>();

        void Start()
        {
            arHitTester.HitAsObservable()
                .Subscribe(AddReferencePoint)
                .AddTo(this);
        }

        void AddReferencePoint(ARRaycastHit hit)
        {
            var hitPose = hit.pose;
            var referencePoint = referencePointManager.AddReferencePoint(hitPose);
            if (referencePoint == null)
            {
                Debug.Log("Error creating reference point");
            }
            else
            {
                referencePoints.Add(referencePoint);
            }
        }

        void Clear()
        {
            foreach (var referencePoint in referencePoints)
            {
                referencePointManager.RemoveReferencePoint(referencePoint);
            }
            referencePoints.Clear();
        }
    }
}
