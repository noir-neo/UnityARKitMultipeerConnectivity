using Networking;
using UI.View;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ConnectingPresenter : MonoBehaviour
    {
        [SerializeField]
        private Connector connector;
        [SerializeField]
        private ConnectingView view;

        void Start()
        {
            view.OnClickConnectAsHostAsObservable()
                .Subscribe(_ => connector.StartAsHost())
                .AddTo(this);

            view.OnClickConnectAsClientAsObservable()
                .Subscribe(_ => connector.StartAsClient())
                .AddTo(this);

            view.OnStopClickAsObservable()
                .Subscribe(_ => connector.StopAll())
                .AddTo(this);
        }
    }
}
