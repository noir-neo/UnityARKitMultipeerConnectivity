using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityMultipeerConnectivity;

public class ConnectedPeersView : MonoBehaviour
{
    [SerializeField] Text text;

    void Start()
    {
        var mcSession = UnityMCSessionNativeInterface.GetMcSessionNativeInterface();
        var connectingPeers = new ReactiveCollection<string>();

        mcSession.StateChangedAsObservable()
            .Subscribe(t =>
            {
                switch (t.Item2)
                {
                    case UnityMCSessionState.Connecting:
                        break;
                    case UnityMCSessionState.Connected:
                        connectingPeers.Add(t.Item1.DisplayName);
                        break;
                    case UnityMCSessionState.NotConnected:
                        connectingPeers.Remove(t.Item1.DisplayName);
                        break;
                }
            })
            .AddTo(this);

        connectingPeers.ObserveAdd()
            .AsUnitObservable()
            .Merge(connectingPeers.ObserveRemove().AsUnitObservable())
            .Select(_ => connectingPeers)
            .Subscribe(Refresh)
            .AddTo(this);
    }

    void Refresh(IEnumerable<string> connectingPeers)
    {
        text.text = string.Join("\n", connectingPeers);
    }
}
