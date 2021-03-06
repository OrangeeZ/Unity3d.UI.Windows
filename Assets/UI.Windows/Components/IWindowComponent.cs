﻿using UnityEngine;
using System.Collections;

namespace UnityEngine.UI.Windows {
	
	public interface IWindowComponentLayout : IWindowEventsAsync {

		void Setup(WindowComponent component, Layout.Component activatorInstance);
		
	}

	public interface IWindowAnimation : IWindowEventsAsync {
		
		float GetAnimationDuration(bool forward);
		
	}

}