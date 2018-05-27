using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour {
	
	public static class Gameplay {
		public const string MONEY_TEXT = "Money: ";
		public const string WAVE_NUMBER_TEXT = "Wave: ";
		public const string ENEMIES_KILLED_TEXT = "Killed: ";
		public const string LIVES_LEFT_TEXT = "Lives left: ";
		public const string NO_MONEY = "Not enough money, bitch!";
		public const string CANT_BUILD_THERE = "You can't build there!";
	}

	public static class Tags {
		public const string Enemy = "enemy";
		public const string Endpoint = "endpoint";
	}

}
