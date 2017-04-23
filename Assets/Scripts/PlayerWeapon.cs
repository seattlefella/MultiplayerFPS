
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class PlayerWeapon
    {
        // The weapons operational parameters
        public string Name = "Glock";
        public float FireRate = 10f;
        public int Damage = 10;
        public float Range = 100f;

        // The visual image of the weapon
        public GameObject Graphics;





    }
}
