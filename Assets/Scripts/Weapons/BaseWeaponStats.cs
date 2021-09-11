using System;

namespace Weapons {
    [Serializable]
    public struct BaseWeaponStats {
        public int fireRate;
        public BaseBulletStats bulletStats;
    }
}