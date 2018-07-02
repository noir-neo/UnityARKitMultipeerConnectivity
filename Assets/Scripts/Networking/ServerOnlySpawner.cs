using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class ServerOnlySpawner : NetworkBehaviour
    {
        [SerializeField]
        GameObject prefab;

        public override void OnStartServer()
        {
            var obj = Instantiate(prefab);
            NetworkServer.Spawn(obj);
        }
    }
}
