using System;

namespace Weapons {
    [Serializable]
    public struct BaseWeaponStats {
        public float fireRate;
        public int bulletCount;
        public float[] bulletOffsets;
        public float[] bulletFireDelay;
        public BaseBulletStats bulletStats;
    }
}