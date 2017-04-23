using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class WeaponManager : NetworkBehaviour
    {

        [SerializeField]
        private PlayerWeapon primaryWeapon;
        private PlayerWeapon currentWeapon;

        private WeaponGraphics currentWeaponGraphics;

        [SerializeField]
        private Transform weaponHolder;

        [SerializeField]
        private string weaponLayerName = "Weapon";

        private GameObject weaponInstance;

        void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public WeaponGraphics GetCurrentWeaponGraphics()
        {
            return currentWeaponGraphics;
        }
        public void EquipWeapon(PlayerWeapon _weapon)
        {
            currentWeapon = _weapon;

            weaponInstance = (GameObject) Instantiate(_weapon.Graphics, weaponHolder.position, weaponHolder.rotation);

            weaponInstance.transform.SetParent(weaponHolder);

            currentWeaponGraphics = weaponInstance.GetComponent<WeaponGraphics>();
            if (currentWeaponGraphics == null)
            {
                Debug.LogError("Current Weapon has no weapon graphics component has been assigned to the" + weaponInstance.name + " weapon object!!");
            }

            if (isLocalPlayer)
            {
                weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
                Util.SetLayerRecursivly(weaponInstance,LayerMask.NameToLayer(weaponLayerName));
            }

        }

    }
}
