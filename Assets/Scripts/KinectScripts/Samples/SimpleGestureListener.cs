using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	// GUI Text to display the gesture messages.
	public TextMeshProUGUI GestureInfo;
	
	public TextMeshProUGUI ClickGestureInfo;
	public TextMeshProUGUI ZoomGestureInfo;
	public TextMeshProUGUI WheelGestureInfo;
	
	// private bool to track if progress message has been displayed
	private bool progressDisplayed;

	private string tempGestureText;
	
	public void UserDetected(uint userId, int userIndex)
	{
		// as an example - detect these user specific gestures
		KinectManager manager = KinectManager.Instance;

		manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
		manager.DetectGesture(userId, KinectGestures.Gestures.Squat);

//		manager.DetectGesture(userId, KinectGestures.Gestures.Push);
//		manager.DetectGesture(userId, KinectGestures.Gestures.Pull);
		
//		manager.DetectGesture(userId, KinectWrapper.Gestures.SwipeUp);
//		manager.DetectGesture(userId, KinectWrapper.Gestures.SwipeDown);
		
		if(GestureInfo != null)
		{
			GestureInfo.text = "SwipeLeft, SwipeRight, Jump or Squat.";
		}
	}
	
	public void UserLost(uint userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.text = string.Empty;
		}
	}

	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		// //GestureInfo.guiText.text = string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100));
		// if(gesture == KinectGestures.Gestures.Click && progress > 0.3f)
		// {
		// 	string sGestureText = string.Format ("{0} {1:F1}% complete", gesture, progress * 100);
		// 	if(ClickGestureInfo != null)
		// 		ClickGestureInfo.text = sGestureText;
		// 	
		// 	progressDisplayed = true;
		// }		
		// if((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
		// {
		// 	string sGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
		// 	if(ZoomGestureInfo != null)
		// 		ZoomGestureInfo.text = sGestureText;
		// 	
		// 	progressDisplayed = true;
		// }
		// if(gesture == KinectGestures.Gestures.Wheel && progress > 0.5f)
		// {
		// 	string sGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
		// 	if(WheelGestureInfo != null)
		// 		WheelGestureInfo.text = sGestureText;
		// 	
		// 	progressDisplayed = true;
		// }
		switch (gesture)
		{
			case KinectGestures.Gestures.Click when progress > 0.3f:
				tempGestureText = string.Format ("{0} {1:F1}% complete", gesture, progress * 100);
				if(ClickGestureInfo != null)
					ClickGestureInfo.text = tempGestureText;
			
				progressDisplayed = true;
				break;

			case KinectGestures.Gestures.ZoomOut when progress > 0.5f:
			case KinectGestures.Gestures.ZoomIn when progress > 0.5f:
				tempGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
				if(ZoomGestureInfo != null)
					ZoomGestureInfo.text = tempGestureText;
			
				progressDisplayed = true;
				break;
			
			case KinectGestures.Gestures.Wheel when progress > 0.5f:
				tempGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
				if(WheelGestureInfo != null)
					WheelGestureInfo.text = tempGestureText;
			
				progressDisplayed = true;
				break;
			
			default:
				break;
		}
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		tempGestureText = gesture + " detected";
		switch (gesture)
		{
			case KinectGestures.Gestures.Click:
				tempGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);

				if (ClickGestureInfo)
				{
					ClickGestureInfo.text = tempGestureText;
				}
				break;

			case KinectGestures.Gestures.ZoomOut:
			case KinectGestures.Gestures.ZoomIn:
				if (ZoomGestureInfo)
				{
					ZoomGestureInfo.text = tempGestureText;
				}
				break;
			
			case KinectGestures.Gestures.Wheel:
				if (WheelGestureInfo)
				{
					WheelGestureInfo.text = tempGestureText;
				}
				break;
			
			default:
				if (GestureInfo)
				{
					GestureInfo.text = tempGestureText;
				}
				break;
		}
		
		progressDisplayed = false;
		
		return true;
	}

	
	public UnityEvent OnRaiseLH;
	public UnityEvent OnRaiseRH;
	public UnityEvent OnPsi;
	public UnityEvent OnTpose;
	public UnityEvent OnStop;
	public UnityEvent OnWave;
	public UnityEvent OnClick;
	public UnityEvent OnSwipeLeft;
	public UnityEvent OnSwipeRight;
	public UnityEvent OnSwipeUp;
	public UnityEvent OnSwipeDown;
	public UnityEvent OnLeftHandCursor;
	public UnityEvent OnRightHandCursor;
	public UnityEvent OnRightHandCursorBothSides;
	public UnityEvent OnZoomIn;
	public UnityEvent OnZoomOut;
	public UnityEvent OnWheel;
	public UnityEvent OnJump;
	public UnityEvent OnSquat;
	public UnityEvent OnPush;
	public UnityEvent OnPull;
	public UnityEvent OnFloss;

	void SetBools(KinectGestures.Gestures gesture)
	{
		switch (gesture)
		{
			case KinectGestures.Gestures.None:
				break;
			case KinectGestures.Gestures.RaiseLeftHand:
				OnRaiseLH.Invoke();
				break;
			case KinectGestures.Gestures.RaiseRightHand:
				OnRaiseRH.Invoke();
				break;
			case KinectGestures.Gestures.Psi:
				OnPsi.Invoke();
				break;
			case KinectGestures.Gestures.Tpose:
				OnTpose.Invoke();
				break;
			case KinectGestures.Gestures.Stop:
				OnStop.Invoke();
				break;
			case KinectGestures.Gestures.Wave:
				OnWave.Invoke();
				break;
			case KinectGestures.Gestures.Click:
				OnClick.Invoke();
				break;
			case KinectGestures.Gestures.SwipeLeft:
				OnSwipeLeft.Invoke();
				break;
			case KinectGestures.Gestures.SwipeRight:
				OnSwipeRight.Invoke();
				break;
			case KinectGestures.Gestures.SwipeUp:
				OnSwipeUp.Invoke();
				break;
			case KinectGestures.Gestures.SwipeDown:
				OnSwipeDown.Invoke();
				break;
			case KinectGestures.Gestures.LeftHandCursor:
				OnLeftHandCursor.Invoke();
				break;
			case KinectGestures.Gestures.RightHandCursor:
				OnRightHandCursor.Invoke();
				break;
			case KinectGestures.Gestures.RightHandCursorBothSides:
				OnRightHandCursorBothSides.Invoke();
				break;
			case KinectGestures.Gestures.ZoomOut:
				OnZoomOut.Invoke();
				break;
			case KinectGestures.Gestures.ZoomIn:
				OnZoomIn.Invoke();
				break;
			case KinectGestures.Gestures.Wheel:
				OnWheel.Invoke();
				break;
			case KinectGestures.Gestures.Jump:
				OnJump.Invoke();
				break;
			case KinectGestures.Gestures.Squat:
				OnSquat.Invoke();
				break;
			case KinectGestures.Gestures.Push:
				OnPush.Invoke();
				break;
			case KinectGestures.Gestures.Pull:
				OnPull.Invoke();
				break;
			case KinectGestures.Gestures.Floss:
				OnFloss.Invoke();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(gesture), gesture, null);
		}
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		if(progressDisplayed)
		{
			tempGestureText = String.Empty;
			// clear the progress info
			switch (gesture)
			{
				case KinectGestures.Gestures.Click:
					if (ClickGestureInfo)
					{
						ClickGestureInfo.text = tempGestureText;
					}
					break;

				case KinectGestures.Gestures.ZoomOut:
				case KinectGestures.Gestures.ZoomIn:
					if (ZoomGestureInfo)
					{
						ZoomGestureInfo.text = tempGestureText;
					}
					break;
			
				case KinectGestures.Gestures.Wheel:
					if (WheelGestureInfo)
					{
						WheelGestureInfo.text = tempGestureText;
					}
					break;
			
				default:
					if (GestureInfo)
					{
						GestureInfo.text = tempGestureText;
					}
					break;
			}
			
			progressDisplayed = false;
		}
		
		return true;
	}
	
}
