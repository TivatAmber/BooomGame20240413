using System;
using UnityEngine;

namespace Units.SubPrefabs.Weapons.WeaponBullets
{
    internal abstract class BaseBullet: MonoBehaviour
    {
        protected float Damage;
        protected bool Penetrating;
        protected Vector3 Speed;
        protected float RecycleTime;
        protected float NowTime;
        protected virtual void Update()
        {
            if (NowTime < RecycleTime) NowTime += Time.deltaTime;
        }

        public virtual void Init(Vector3 position, Vector3 forward, Quaternion degree, 
            Vector3 speed, float damage, float recycleTime,
            bool penetrating)
        {
            transform.position = position;
            Speed = speed;
            Damage = damage;
            RecycleTime = recycleTime;
            Penetrating = penetrating;
            NowTime = 0f;
        }

        public BaseBullet SetForward(Quaternion forward)
        {
            Speed = forward * Speed;
            return this;
        }

        public BaseBullet SetDegree(Quaternion degree)
        {
            transform.rotation = degree;
            return this;
        }

        public BaseBullet SetSpeed(float speed)
        {
            if (Speed == Vector3.zero) Debug.LogError("No Speed!");
            Speed = Speed.normalized * speed;
            return this;
        }
        public BaseBullet SetDamage(float damage)
        {
            Damage = damage;
            return this;
        }

        public BaseBullet SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        public BaseBullet SetRecycleTime(float recycleTime)
        {
            RecycleTime = recycleTime;
            return this;
        }
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log("hit");
            if (other.CompareTag("Enemies"))
            {
                Entity entity = other.gameObject.GetComponent<Entity>();
                entity.ChangeHealth(-Damage);
                if (!Penetrating) RecycleTime = 0f;
            }
        }
    }
}