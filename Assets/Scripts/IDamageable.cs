
using UnityEngine;

public interface IDamageable {
        public void OnCharacterDeath();
        public float Health { set; get; }
        public bool Targetable { set; get; }
        public bool Invincible { set; get; }
        public bool ProjectileInvincible { set; get; }
        public void OnHit(float damage, Vector2 knockback);
        public void OnHit(float damage);
        public void OnObjectDestroyed();

    }

