using UniRx;
using UnityEngine;
using UnityEngine.XR.iOS;

public class AnchorObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    void Start()
    {
        UnityARSessionNativeInterface.ARUserAnchorAddedAsObservable()
            .Subscribe(Spawn)
            .AddTo(this);
    }

    void Spawn(ARUserAnchor userAnchor)
    {
        var position = UnityARMatrixOps.GetPosition(userAnchor.transform);
        var rotation = UnityARMatrixOps.GetRotation(userAnchor.transform);

        Instantiate(prefab, position, rotation);
    }
}
