using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

public class ARHitTester : MonoBehaviour, IPointerDownHandler
{
    readonly ISubject<UnityARUserAnchorData> hit = new Subject<UnityARUserAnchorData>();

    public void OnPointerDown(PointerEventData eventData)
    {
        var screenPosition = Camera.main.ScreenToViewportPoint(eventData.position);
        var point = new ARPoint {
            x = screenPosition.x,
            y = screenPosition.y
            };

        if (TryHitTest(point, ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, out var hitTestResult))
        {
            var anchorData = hitTestResult.ToUnityArUserAnchorData();
            hit.OnNext(anchorData);
        }
    }

    static bool TryHitTest(ARPoint point, ARHitTestResultType resultTypes, out ARHitTestResult hitTestResult)
    {
        var results = UnityARSessionNativeInterface.GetARSessionNativeInterface()
            .HitTest(point, resultTypes);
        if (results.Any())
        {
            hitTestResult = results.First();
            return true;
        }

        hitTestResult = default(ARHitTestResult);
        return false;
    }

    public IObservable<UnityARUserAnchorData> HitAsObservable()
    {
        return hit;
    }
}
