﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOption : MonoBehaviour {

	public enum menuOptions{
		ATTACK,
        CAPTURE,
		WAIT,
		END_TURN,
		TEST_OPTION
	}
		
	public menuOptions myOption;
}
