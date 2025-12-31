using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : NetworkBehaviour
{
    [Networked]
    public Color NetworkedColor { get; set; }
    private MeshRenderer meshRendererToChangeColor;

    private ChangeDetector changeDetector;

    public override void Spawned() {
        meshRendererToChangeColor = GetComponentInChildren<MeshRenderer>();
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (NetworkedColor != null)
            meshRendererToChangeColor.material.color = NetworkedColor;
    }

    public override void Render() {
        foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer)) {
            switch (change) {
                case nameof(NetworkedColor):
                    meshRendererToChangeColor.material.color = NetworkedColor;
                    break;
            }
        }
    }

    public override void FixedUpdateNetwork() {
        if (GetInput(out NetworkInputData inputData)) {
            if (this.HasStateAuthority && inputData.colorActionValue) {
                    Debug.Log("Color Change");
                    var randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

                // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
                // meshRendererToChangeColor.material.color = randomColor;
                NetworkedColor = randomColor;
            }
        }
    }
}
