using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	[RequireComponent(typeof(WeaponManager))]
	public class PlayerShoot : NetworkBehaviour
	{
		[SerializeField]
		private Camera cam;

		private PlayerWeapon currentWeapon;
		private WeaponGraphics currentWeaponGraphics;

		//[SerializeField]
		//private string weaponLayerName = "Weapon";

		private RaycastHit hit;
		private const string PLAYERTAG = "Player";

		[SerializeField]
		private LayerMask layerMask;

		private Player player;

		private WeaponManager weaponManager;

		private GameObject hitEffect;

		void Start()
		{
			if (cam == null)
			{
				Debug.LogError("PlayerShoot script: A camera should have been assigned");
				this.enabled = false;
			}
			weaponManager = GetComponent<WeaponManager>();
			currentWeaponGraphics = weaponManager.GetCurrentWeaponGraphics();

		}

		void Update()
		{
			currentWeapon = weaponManager.GetCurrentWeapon();

			// If the player has paused the game do not do anything !
			if (PauseMenu.IsOn)
			{
				return;
			}

			if (currentWeapon.Bullets < currentWeapon.MaxBullets)
			{
				if (Input.GetButtonDown("Reload"))
				{
					weaponManager.Reload();
					return;
				}		        
			}



			if (currentWeapon.FireRate <= 0f)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					Shoot();
				}
			}
			else
			{
				if (Input.GetButtonDown("Fire1"))
				{
					InvokeRepeating("Shoot", 0f, 1f/currentWeapon.FireRate);
				} else if (Input.GetButtonUp("Fire1"))
				{
					CancelInvoke("Shoot");
				}
			}
		}

		// Called on the server and is called by a player when they shoot.
		[Command]
		public void CmdOnShoot()
		{

			RpcDoShootEffect();
		}

		// Called on the server when a client hits something.
		// It takes in the point that was hit and it's normal to the surface.
		[Command]
		public void CmdOnHit(Vector3 _pos, Vector3 _normal)
		{
			RpcDoHittEffect(_pos, _normal);
		}

		// Called on all clients - displays all effects that result from a bullet hit.
		[ClientRpc]
		public void RpcDoHittEffect(Vector3 _pos, Vector3 _normal)
		{
			hitEffect = (GameObject) Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
			Destroy(hitEffect, 2f);
		}

		// The server sends the do shoot effect command to all clients
		[ClientRpc]
		public void RpcDoShootEffect()
		{
			weaponManager.GetCurrentWeaponGraphics().MuzzleFlash.Play();

		}

		[Client]
		private void Shoot()
		{
			if (!isLocalPlayer || weaponManager.IsReloading)
			{
				return;
			}


			if (currentWeapon.Bullets <= 0)
			{
				// We are out of Ammo so let's reload
				Debug.Log("The player:" + transform.name + " is out of bullets");
			   weaponManager.Reload();
				
				return;
			}

			// So we have some bullets left, let's decrement the bullet count
			currentWeapon.Bullets--;

			// The player wants to shoot so we call the OnShoot method on the server so it can tell each client to show shooting effects.
			CmdOnShoot();

			// only ray cast if we are the local player
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.Range, layerMask))
			{
				if (hit.collider.tag == PLAYERTAG)
				{
					CmdPlayerShot(hit.collider.name, currentWeapon.Damage, transform.name);
				}

				// We hit an object in the scene so call the OnHit method that is on the server.
				CmdOnHit(hit.point, hit.normal);			
			}

		    if (currentWeapon.Bullets <= 0)
		    {
		        weaponManager.Reload();
		    }

		}

		[Command]
		void CmdPlayerShot(string _playerID, int _damage, string _sourcePlayerID)
		{
			Debug.Log("Player-" + _playerID + " Has been hit!");
			player = GameManager.GetPlayer(_playerID);
			player.RpcTakeDamage(_damage, _sourcePlayerID);
		}
	}
}
