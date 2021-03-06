﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI.Windows.Components;

namespace UnityEngine.UI.Windows {

	public class WindowComponent : WindowComponentBase, IComponent {

		[Header("Sub Components")]
		public bool autoRegisterSubComponents = true;
		[SerializeField]
		private List<WindowComponent> subComponents = new List<WindowComponent>();

		private WindowLayoutBase layoutRoot;

		public List<WindowComponent> GetSubComponents() {

			return this.subComponents;

		}

		internal void Setup(WindowLayoutBase layoutRoot) {
			
			this.layoutRoot = layoutRoot;
			
			for (int i = 0; i < this.subComponents.Count; ++i) this.subComponents[i].Setup(this.layoutRoot);
			
		}
		
		internal override void Setup(WindowBase window) {
			
			base.Setup(window);
			
			for (int i = 0; i < this.subComponents.Count; ++i) this.subComponents[i].Setup(window);
			
		}

		public WindowLayoutBase GetLayoutRoot() {
			
			return this.layoutRoot;
			
		}
		
		public void RegisterSubComponent(WindowComponent subComponent) {

			if (this.subComponents.Contains(subComponent) == false) this.subComponents.Add(subComponent);
			
		}
		
		public void UnregisterSubComponent(WindowComponent subComponent) {

			this.subComponents.Remove(subComponent);
			
		}

		public override void OnInit() {

			base.OnInit();

			for (int i = 0; i < this.subComponents.Count; ++i) this.subComponents[i].OnInit();

		}

		public override void OnDeinit() {
			
			base.OnDeinit();

			for (int i = 0; i < this.subComponents.Count; ++i) this.subComponents[i].OnDeinit();

		}

		public override void OnShowEnd() {
			
			base.OnShowEnd();

			for (int i = 0; i < this.subComponents.Count; ++i) this.subComponents[i].OnShowEnd();

		}

		public override void OnHideEnd() {
			
			base.OnHideEnd();

			for (int i = 0; i < this.subComponents.Count; ++i) this.subComponents[i].OnHideEnd();
			
		}
		
		public override void OnShowBegin(System.Action callback) {
			
			var counter = 0;
			System.Action callbackItem = () => {
				
				++counter;
				if (counter < 2) return;
				
				if (callback != null) callback();
				
			};

			base.OnShowBegin(callbackItem);
			ME.Utilities.CallInSequence(callbackItem, this.subComponents, (e, c) => { e.OnShowBegin(c); });

		}
		
		public override void OnHideBegin(System.Action callback) {		
			
			var counter = 0;
			System.Action callbackItem = () => {
				
				++counter;
				if (counter < 2) return;
				
				if (callback != null) callback();
				
			};

			base.OnHideBegin(callbackItem);
			ME.Utilities.CallInSequence(callbackItem, this.subComponents, (e, c) => { e.OnHideBegin(c); });

		}

		#if UNITY_EDITOR
		public override void OnValidateEditor() {

			base.OnValidateEditor();

			if (this.autoRegisterSubComponents == true) {

				var components = this.GetComponentsInChildren<WindowComponent>(true).ToList();

				this.subComponents.Clear();
				foreach (var component in components) {

					if (component == this) continue;

					var parents = component.GetComponentsInParent<WindowComponent>(true).ToList();
					parents.Remove(component);

					if (parents.Count > 0 && parents[0] != this) continue;

					this.subComponents.Add(component);

				}

			} else {

				this.subComponents.Clear();

			}

		}
		#endif

	}

}