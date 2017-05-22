
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class PlayerWeapon
    {
        // The weapons operational parameters
        public string Name = "Glock";
        public float FireRate = -1f;
        public int Damage = 10;
        public float Range = 100f;

        // The size of the magazine clip
        public int MaxBullets = 20;

        // The number of bullets the player currently has remaining
        [HideInInspector] public int Bullets;

        // How long does it take for this weapon to reload.
        public float ReloadTime = 4f;

        // The visual image of the weapon
        public GameObject Graphics;


        // this class's constructor
        PlayerWeapon()
        {
            // On creation fill the magazine
            Bullets = MaxBullets;
        }


    }
}
