using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityARKitMultipeerConnectivity.Examples.CollaborativeSessions
{
    public class ARHitTester : MonoBehaviour, IPointerDownHandler
    {
        readonly ISubject<ARRaycastHit> hit = new Subject<ARRaycastHit>();

        static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

        [SerializeField]
        ARRaycastManager raycastManager;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!raycastManager.Raycast(eventData.position, hits, TrackableType.FeaturePoint)) return;

            hit.OnNext(hits[0]);
        }

        public IObservable<ARRaycastHit> HitAsObservable()
        {
            return hit;
        }
    }
}
