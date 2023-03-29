using UnityEngine;
using System.Collections;
using Yudiz.Gaming.Engine;

public class BackImageRatio : _BackImageRatio
{
}
namespace Yudiz.Gaming.Engine
{
	public abstract class _BackImageRatio : MonoBehaviour
	{
		public ScreeRatioSetting[] Background;
		public GameObject DefaultBG;
		private bool isActive = false;
		// Use this for initialization
		void Start ()
		{
			DefaultBG.active = false;
			foreach (ScreeRatioSetting s in Background) {
				s.DisplayObject.active = false;
				if (System.Math.Round ((float)Screen.width / (float)Screen.height, 2) == System.Math.Round (s.ScreenRatio.x / s.ScreenRatio.y,
					   2)) {
					s.DisplayObject.active = true;
					isActive = true;
				} 
			}
			if (!isActive) {
				DefaultBG.active = true;
			}
		}
	}

	[System.Serializable]
	public class ScreeRatioSetting
	{
		public GameObject DisplayObject;
		public Vector2 ScreenRatio;
	}
}