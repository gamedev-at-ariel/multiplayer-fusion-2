using Fusion;
using UnityEngine;

public class Health: NetworkBehaviour
{
    [SerializeField] NumberField HealthDisplay;

    [Networked]
    public int NetworkedHealth { get; set; } = 100;

    private ChangeDetector changeDetector;

    public override void Spawned() {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        HealthDisplay.SetNumber(NetworkedHealth);
    }

    public override void Render() {
        foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer)) {
            switch (change) {
                case nameof(NetworkedHealth):
                    HealthDisplay.SetNumber(NetworkedHealth);
                    break;
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // only the StateAuthority calls this function; only the StateAuthority receives the call.
    public void DealDamageRpc(int damage, RpcInfo info = default) {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log($"Health.DealDamageRpc called by {info.Source} (IsInputAuthority: {info.IsInvokeLocal})");
        NetworkedHealth -= damage;
    }
}
