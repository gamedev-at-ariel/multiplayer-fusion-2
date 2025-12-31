using UnityEngine;
using Fusion;

public class Player: NetworkBehaviour
{
    private CharacterController _cc;

    [SerializeField] float speed = 5f;
    [SerializeField] GameObject ballPrefab;

    private Camera firstPersonCamera;
    public override void Spawned() {   // Instead of Awake, Start
        Debug.Log("Player object is spawned");
        _cc = GetComponent<CharacterController>();
        if (this.HasStateAuthority) {
            firstPersonCamera = Camera.main;
            var firstPersonCameraComponent = firstPersonCamera.GetComponent<FirstPersonCamera>();
            if (firstPersonCameraComponent && firstPersonCameraComponent.isActiveAndEnabled)
                firstPersonCameraComponent.SetTarget(this.transform);
        }
    }

    private Vector3 moveDirection;
    public override void FixedUpdateNetwork() {    // Instead of  Update and FixedUpdate
        if (GetInput(out NetworkInputData inputData)) {
            if (inputData.moveActionValue.magnitude > 0) {
                inputData.moveActionValue.Normalize();   //  Ensure that the vector magnitude is 1, to prevent cheating.
                moveDirection = new Vector3(inputData.moveActionValue.x, 0, inputData.moveActionValue.y);
                Vector3 DeltaX = speed * moveDirection * Runner.DeltaTime;
                //Debug.Log($"{speed} * {moveDirection} * {Runner.DeltaTime} = {DeltaX}");
                _cc.Move(DeltaX);
            }

            if (HasStateAuthority) { // Only the server can spawn new objects ; otherwise you will get an exception "ClientCantSpawn".
                if (inputData.shootActionValue) {
                    Debug.Log("SHOOT!");
                    Runner.Spawn(ballPrefab,
                        transform.position + moveDirection, Quaternion.LookRotation(moveDirection),
                        Object.InputAuthority);
                }
            }
        }
    }
}
