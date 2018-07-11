using System.Collections;
using Networking.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Networking
{
    public class Connector : MonoBehaviour
    {
        [SerializeField]
        private EventPublishableNetworkDiscovery networkDiscovery;
        [SerializeField]
        private EventPublishableNetworkManager networkManager;

        void Start()
        {
            networkDiscovery.ReceivedBroadcastAsObservable()
                .Subscribe(x => StartClient(x.FromAddress))
                .AddTo(this);

            networkManager.StoppedServerAsObservable()
                .Subscribe(_ => StopBroadcast())
                .AddTo(this);

            this.OnDestroyAsObservable()
                .Subscribe(_ => StopAll())
                .AddTo(this);
        }

        public void StartAsHost()
        {
            networkManager.StartHost();
            networkDiscovery.Initialize();
            networkDiscovery.StartAsServer();
        }

        public void StartAsClient()
        {
            networkDiscovery.Initialize();
            networkDiscovery.StartAsClient();
        }

        public void StopAll()
        {
            StopBroadcast();
            networkManager.StopHost();
            networkManager.StopClient();
        }

        private void StartClient(string networkAddress)
        {
            if (networkManager.isNetworkActive) return;

            networkManager.networkAddress = networkAddress;
            networkManager.StartClient();

            StartCoroutine(StopBroadcastNextFrame());
        }

        private IEnumerator StopBroadcastNextFrame()
        {
            yield return null;
            StopBroadcast();
        }

        private void StopBroadcast()
        {
            if (networkDiscovery.running)
            {
                networkDiscovery.StopBroadcast();
            }
        }
    }
}
