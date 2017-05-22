using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Assets.Scripts
{
    public class WeaponManager : NetworkBehaviour
    {

        [SerializeField]
        private PlayerWeapon primaryWeapon;
        private PlayerWeapon currentWeapon;

        // Reloading will take some time so we must track if we are in the reload state
        public bool IsReloading = false;

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

        public void Reload()
        {
            if (IsReloading)
            {
                return;
            }

            StartCoroutine(reload_CoRoutine());

        }

        private IEnumerator reload_CoRoutine()
        {
            IsReloading = true;

            CmdOnReload();

            // Let's replenish the magazine
            currentWeapon.Bullets = currentWeapon.MaxBullets;

            yield return new WaitForSeconds(currentWeapon.ReloadTime);

            IsReloading = false;
        }

        [Command]
        void CmdOnReload()
        {
            RpcOnReload();
        }

        [ClientRpc]
        void RpcOnReload()
        {
            Animator anmi = currentWeaponGraphics.GetComponent<Animator>();

            if (anmi != null)
            {
                anmi.SetTrigger("Reload");
            }
        }
    }
}
