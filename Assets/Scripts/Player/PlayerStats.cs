using System;

namespace Player {
    [Serializable]
    public class PlayerStats {
        public float movementSpeed = 5f;
        public float dodgeSpeed = 15f;
        public float dodgeTime = 0.33f;
        public int currHealth = 1;
        public int maxHealth = 1;
        public int currStamina = 10;
        public int maxStamina = 10;
    }
}
