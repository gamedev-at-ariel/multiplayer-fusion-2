using Fusion;
using UnityEngine;

/**
 * This component represents a ball moving at a constant speed.
 */
public class Ball: NetworkBehaviour {
    [Networked] private TickTimer lifeTimer { get; set; }

    [SerializeField] float lifeTime = 5.0f;
    [SerializeField] float speed = 5.0f;
    [SerializeField] int damagePerHit = 1;

    public override void Spawned() {
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    public override void FixedUpdateNetwork() {
        //Debug.Log(lifeTimer);
        if (lifeTimer.Expired(Runner))
            Runner.Despawn(Object);
        else
            transform.position += speed * transform.forward * Runner.DeltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (!HasStateAuthority) return;  // Without this line, the trigger will happen on each client, and thus remove multiple health points

        Health health = other.GetComponent<Health>();
        if (health != null) {
            Debug.Log("OnTriggerEnter: hit an object with Health: " + other.gameObject.name + " " + other.gameObject.tag);
            health.DealDamageRpc(damagePerHit);
        }
    }

}