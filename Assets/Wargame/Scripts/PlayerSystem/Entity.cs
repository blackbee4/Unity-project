using UnityEngine;

namespace WargameSystem.PlayerSystem
{
    public class Entity : MonoBehaviour
    {
        public int maxHealth = 100;
        
        private int _curHealth;

        void Start()
        {
            _curHealth = maxHealth;
        }

        public void OnDamaged(int damage)
        {
            _curHealth -= damage;

            if (_curHealth <= 0)
                OnDeath();
        }

        public void OnDeath()
        {
            
        }
    }
}