using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponLibrary : MonoBehaviour
{
    public enum weaponType { pistol, shotgun, smg, rifle };

    public class weapon
    {
        public weaponType type;

        public int ejectFrames, cycleFrames, maxRecoil, upRecoilFrames, downRecoilFrames, exitVelocity;
        public int magSize, totReloadFrames, startFrames, reloadPercent;
        public int baseDamage, shotCount, shellAmount, shotSpreadAngle, damage;
        public int penetrationAmount;
        public string gunname;
        public GameObject shell, bulletType;

        public void assignStats()
        {

        }
    }
}
