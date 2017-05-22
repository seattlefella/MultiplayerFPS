using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using JetBrains.Annotations;
using UnityEngine;

public class KillFeed : MonoBehaviour
{

	[SerializeField] private GameObject killFeedItemPrefab;

	void Start ()
	{
		GameManager.Instance.onPlayerKilledCallback += OnKilled;

	}

	public void OnKilled(string _userName, string _killerName)
	{
	   GameObject go = (GameObject) Instantiate(killFeedItemPrefab, this.transform);
		go.GetComponent<KillFeedItem>().Setup(_userName, _killerName);

		Destroy(go, 30f);

	}

}
