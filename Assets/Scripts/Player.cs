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

    private Vector3 moveDirection, velocity;
    public override void FixedUpdateNetwork() {    // Instead of  Update and FixedUpdate
        if (GetInput(out NetworkInputData inputData)) {
            if (inputData.moveActionValue.magnitude > 0) {
                moveDirection = new Vector3(inputData.moveActionValue.x, 0, inputData.moveActionValue.y);
                moveDirection.Normalize();
                velocity = transform.TransformDirection(moveDirection * speed); // Move in the direction you look:
                Vector3 DeltaX = velocity * Runner.DeltaTime;
                //Debug.Log($"moveDirection={moveDirection}, velocity={velocity}, DeltaX = {DeltaX}");
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
