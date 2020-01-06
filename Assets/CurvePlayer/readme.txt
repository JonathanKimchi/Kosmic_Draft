Product : Animation Curve Player
Studio : Arkham Interactive
Date : September 17th, 2013
Version : 1.0
Email : support@arkhaminteractive.com

How To Use:
	1) Add CurvePlayer package to your Unity project
	2a) Add CurvePlayer.cs script from Assets/CurvePlayer to any object
	2b) Alternatively, call 'CurvePlayer.PlayCurve' or 'CurvePlayer.ReadyToPlayCurve' from code, including your desired animation curve 
	3) Call 'CurvePlayer.Play' from code

 - CurvePlayer contains Stop(), Pause() and SetTime() functions to control playback
 - The value passed to the callback function is multiplied by 'valueMultiplier' so the animation curve doesn't end up having ridiculous dimensions
 - The 'playSpeed' value is used to control playback speed, again so the animation curve doesn't end up having ridiculous dimensions