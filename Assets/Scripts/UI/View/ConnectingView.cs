using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class ConnectingView : MonoBehaviour
    {
        [SerializeField]
        private Button ConnectAsHost;
        [SerializeField]
        private Button ConnectAsClient;
        [SerializeField]
        private Button StopConnecting;

        public IObservable<Unit> OnClickConnectAsHostAsObservable()
        {
            return ConnectAsHost.OnClickAsObservable();
        }

        public IObservable<Unit> OnClickConnectAsClientAsObservable()
        {
            return ConnectAsClient.OnClickAsObservable();
        }

        public IObservable<Unit> OnStopClickAsObservable()
        {
            return StopConnecting.OnClickAsObservable();
        }
    }
}
