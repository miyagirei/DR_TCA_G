using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FusionGameManager : NetworkBehaviour
{
	[SerializeField] FusionPlayer PlayerPrefab;
    public FusionPlayer LocalPlayer { get; private set; }

	public override void Spawned()
	{
		PersonalDataController personal = new PersonalDataController();
		LocalPlayer = Runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
		LocalPlayer.SetCharacterType(personal.Load().CHARACTER_TYPE);
		Runner.SetPlayerObject(Runner.LocalPlayer, LocalPlayer.Object);
	}
}
