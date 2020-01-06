#region UsingStatements

using UnityEngine;

#endregion

/// <summary>
/// Curve Player
///  - To use, attach this script to a GameObject and call Play() from code.
///  - Alternatively, use PlayCurve() or ReadyToPlayCurve() to generate automatically updating versions of this behaviour.
///  - By default, Curve Players generated with PlayCurve() or ReadyToPlayCurve() will self-destruct after raising the OnFinish event.
///  - This behaviour can be changed via the functions' arguments or the 'destroyOnFinish' field.
/// </summary>

public class CurvePlayer : MonoBehaviour
{
	#region PublicEnumerations
	
	// Enumeration 'PlayingState'
	// - Describes the current state of the CurvePlayer
	
	public enum PlayingState
	{
		NotPlaying,
		Playing,
		Paused,
	}
	
	#endregion
	
	#region PublicParameters
	
	// AnimationCurve 'curve'
	//  - The curve to be played by this CurvePlayer
	//  - Setting this parameter will result in the value of "length" changing
	
	[SerializeField] private AnimationCurve _curve;
	public AnimationCurve curve
	{
		get { return _curve; }
		set
		{
			if(_curve == value) return;
			length = (value.keys.Length < 1) ? 0 : (value[value.keys.Length-1].time - value[0].time);
			_curve = value;
		}
	}
	
	// PlayingState 'playing'
	//  - Current state of the CurvePlayer
	
	public PlayingState playing { get; private set; }
	
	// boolean 'isPlaying'
	//  - For ease-of-use
	//  - Returns true if 'playing' is currently set to 'PlayingState.Playing'

	public bool isPlaying { get { return playing == PlayingState.Playing; } }
	
	// boolean 'isPaused'
	//  - For ease-of-use
	//  - Returns true if 'playing' is currently set to 'PlayingState.Paused'
	
	public bool isPaused { get { return playing == PlayingState.Paused; } }
	
	// float 'time'
	//  - The last evaluated time of the curve
	
	public float time { get; private set; }
	
	// float 'normalizedTime'
	//  - For ease-of-use
	//  - The last evaluated time of the curve, divided by the curve's length
	
	public float normalizedTime { get; private set; }
	
	// float 'length'
	//  - The difference between time values of the first and the last key in the curve
	//  - Set automatically when 'curve' parameter is changed
	
	public float length { get; private set; }
	
	private bool _reverse;
	public bool reverse
	{
		get { return _reverse; }
		set 
		{
			if(_reverse == value) return;
			time = length - time;
			_reverse = value;
		}
	}
	
	#endregion
	
	#region PublicFields
	
	// float 'playSpeed'
	//  - The value each delta time is multiplied by before being added to the 'time' variable
	//  - The higher this value is, the faster the curve will play
	
	public float playSpeed = 1;
	
	// float 'valueMultiplier'
	//  - The value the evaluated curve value will be multiplied by before it is sent in the OnUpdate event
	
	public float valueMultiplier = 1;
	
	// boolean 'continueAfterEnd'
	//  - If set to true, the CurvePlayer will continue to evaluate the curve and raise the OnUpdate event after it has played for [length / playSpeed] seconds
	
	public bool continueAfterEnd = false;
	
	// boolean destroyOnFinish;
	//  - If set to true, this CurvePlayer will destroy itself after raising OnFinish event
	
	public bool destroyOnFinish;
	
	#endregion
	
	#region PublicDelegates
	
	// CurvePlayer Delegates
	//  - Used in CurvePlayer events
	
	public delegate void CP_Delegate(CurvePlayer caller);
	public delegate void CP_PauseDelegate(CurvePlayer caller, bool isPaused);
	public delegate void CP_ValueDelegate(CurvePlayer caller, float value);
	
	#endregion
	
	#region PublicEvents
	
	// event 'OnStart'
	//  - Raised when the curve begins playing
	//  - Contains a reference to this instance of CurvePlayer
	
	public event CP_Delegate OnStart;
	
	// event 'OnUpdate'
	//  - Raised every frame the curve is playing
	//  - Contains a reference to this instance of CurvePlayer
	//  - Contains the value associated with the curve's current time
	//  - Is not raised if the curve is stopped or paused
	
	public event CP_ValueDelegate OnUpdate;
	
	// event 'OnPause'
	//  - Raised when the curve pauses or unpauses
	//  - Contains a reference to this instance of CurvePlayer
	
	public event CP_PauseDelegate OnPause;
	
	// event 'OnContinue'
	//  - Raised when the curve has played for [length / playSpeed] seconds, but will continue playing
	//  - Raised only when 'continueAfterEnd' is set to true
	//  - Contains a reference to this instance of CurvePlayer
	
	public event CP_Delegate OnContinue;
	
	// event 'OnFinish'
	//  - Raised when the curve has played for [length / playSpeed] seconds and will not continue playing
	//  - Raised when Stop() is called and 'playing' is not set to 'PlayState.NotPlaying'
	//  - Contains a reference to this instance of CurvePlayer
	
	public event CP_Delegate OnFinish;
	
	#endregion
	
	#region PrivateFields
	
	// float 'lastContinueTime'
	//  - The last time the OnContinue event was raised
	
	private float lastContinueTime;
	
	#endregion
	
	#region PrivateStaticFields
	
	// GameObject 'updater'
	//  - Singleton GameObject used to update CurvePlayers created with 'PlayCurve' and 'ReadyToPlayCurve'
	//  - Hidden in heirarchy
	//  - Destroyed manually when a new level is loaded or when the application quits, ensuring no dangling references
	
	private static GameObject updater;
	
	#endregion
	
	#region MonobehaviourFunctions
	
	private void Awake()
	{
		if(curve != null) length = (curve[curve.keys.Length-1].time - curve[0].time);
	}
	
	private void Update()
	{	
		if(playing == PlayingState.Playing)
		{
			time += (Time.deltaTime * playSpeed);
			normalizedTime += (Time.deltaTime * playSpeed) / length;
			
			if(time - lastContinueTime >= length)
			{
				if(continueAfterEnd)
				{
					lastContinueTime = time;
					if(OnContinue != null) OnContinue(this);
					//if(OnUpdate != null) OnUpdate(this, curve.Evaluate(time) * valueMultiplier);
					if(OnUpdate != null)
					{
						if(!reverse) OnUpdate(this, curve.Evaluate(time) * valueMultiplier);
						else OnUpdate(this, curve.Evaluate(length - time) * valueMultiplier);
					}
				}
				
				else
				{
					time = length;
					normalizedTime = 1;
					//if(OnUpdate != null) OnUpdate(this, curve.Evaluate(length) * valueMultiplier);
					if(OnUpdate != null)
					{
						if(!reverse) OnUpdate(this, curve.Evaluate(time) * valueMultiplier);
						else OnUpdate(this, curve.Evaluate(length - time) * valueMultiplier);
					}
					
					playing = PlayingState.NotPlaying;
					if(OnFinish != null) OnFinish(this);
					if(destroyOnFinish) Destroy(this);
				}
			}
			
			else if(OnUpdate != null)
			{
				if(!reverse) OnUpdate(this, curve.Evaluate(time) * valueMultiplier);
				else OnUpdate(this, curve.Evaluate(length - time) * valueMultiplier);
			}
			//else if(OnUpdate != null) OnUpdate(this, curve.Evaluate(time) * valueMultiplier);
		}
	}
	
	private void OnApplicationQuit()
	{
		if(updater) Destroy(updater);
	}
	
	private void OnLevelLoaded()
	{
		if(updater) Destroy(updater);
	}
	
	#endregion
	
	#region PublicStaticFunctions
	
	// Play Curve
	//  - Sets up an instance of CurvePlayer to play the designated curve automatically
	//  - Largely for ease-of-use
	//  - The resulting CurvePlayer still raises all events/has all functionality of CurvePlayers created in the editor or generated programatically
	//  - Arguments:
	//    + curve - The curve to play
	//    + playSpeed - The speed at which to play the curve
	//    + valueMultiplier - The value to multiply the result by
	//    + continueAfterEnd - Sets if the player should continue evaluating/updating after it has played for the duration of the curve
	//    + destoryOnFinish - If true, the CurvePlayer will destroy itself after raising the OnFinish event
	//    + OnUpdate - The delegate to call when this curve updates
	//  - Return Value:
	//    + CurvePlayer with provided parameters, already playing the curve
	
	public static CurvePlayer PlayCurve(AnimationCurve curve, float playSpeed = 1, float valueMultiplier = 1, bool continueAfterEnd = false, bool destroyOnFinish = true, CP_ValueDelegate OnUpdate = null)
	{
		if(!updater)
		{
			updater = new GameObject("CP_Updater");
			updater.hideFlags = HideFlags.HideInHierarchy;
		}
		
		CurvePlayer newPlayer = updater.AddComponent<CurvePlayer>();
		newPlayer.curve = curve;
		newPlayer.playSpeed = playSpeed;
		newPlayer.valueMultiplier = valueMultiplier;
		newPlayer.continueAfterEnd = continueAfterEnd;
		if(OnUpdate != null) newPlayer.OnUpdate += OnUpdate;
		
		newPlayer.Play();
		
		return newPlayer;
	}
	
	// Ready-To-Play Curve
	//  - Sets up an instance of CurvePlayer to play the designated curve automatically
	//  - Largely for ease-of-use
	//  - The resulting CurvePlayer still raises all events/has all functionality of CurvePlayers created in the editor or generated programatically
	//  - Identical to "PlayCurve" function, but the resulting CurvePlayer has not yet started playing
	//  - Arguments:
	//    + curve - The curve to play
	//    + playSpeed - The speed at which to play the curve
	//    + valueMultiplier - The value to multiply the result by
	//    + continueAfterEnd - Sets if the player should continue evaluating/updating after it has played for the duration of the curve
	//    + destoryOnFinish - If true, the CurvePlayer will destroy itself after raising the OnFinish event
	//    + OnUpdate - The delegate to call when this curve updates
	//  - Return Value:
	//    + CurvePlayer with provided parameter
	
	public static CurvePlayer ReadyToPlayCurve(AnimationCurve curve, float playSpeed = 1, float valueMultiplier = 1, bool continueAfterEnd = false, bool destroyOnFinish = true, CP_ValueDelegate OnUpdate = null)
	{
		if(!updater)
		{
			updater = new GameObject("CP_Updater");
			updater.hideFlags = HideFlags.HideInHierarchy;
		}
		
		CurvePlayer newPlayer = updater.AddComponent<CurvePlayer>();
		newPlayer.curve = curve;
		newPlayer.playSpeed = playSpeed;
		newPlayer.valueMultiplier = valueMultiplier;
		newPlayer.continueAfterEnd = continueAfterEnd;
		if(OnUpdate != null) newPlayer.OnUpdate += OnUpdate;
		
		return newPlayer;
	}
	
	#endregion
	
	#region PublicFunctions
	
	// Play
	//  - Begins playing the curve
	//  - This will raise OnStart events
	
	public void Play()
	{
		if(playing == PlayingState.NotPlaying) 
		{
			time = 0;
			if(OnStart != null)OnStart(this);
		}
		
		playing = PlayingState.Playing;	
	}
	
	// Play
	//  - Begins playing the curve
	//  - This will raise OnStart events
	//  - Arguments :
	//    + continueAfterEnd - Sets if the player should continue evaluating/updating after it has played for the duration of the curve
	
	public void Play(bool continueAfterEnd)
	{	
		if(playing == PlayingState.NotPlaying) 
		{
			time = 0;
			if(OnStart != null)OnStart(this);
		}
		
		playing = PlayingState.Playing;
		this.continueAfterEnd = continueAfterEnd;
	}
	
	// Set Paused
	//  - Sets if the curve is paused
	//  - This will raise OnPause events
	//  - Arguments:
	//    + pause - Sets if the curve should pause
	
	public void Pause(bool pause = true)
	{
		if(pause)
		{
			if(playing != PlayingState.Paused)
			{
				playing = PlayingState.Paused;
				if(OnPause != null) OnPause(this, true);
			}
		}
		
		else
		{
			if(playing == PlayingState.Paused)
			{
				playing = PlayingState.Playing;
				if(OnPause != null) OnPause(this, false);
			}
		}
	}
	
	// Stop
	//  - Stops the animation, resets the time and normalized time to zero
	
	public void Stop()
	{
		time = 0;
		normalizedTime = 0;
		if(playing != PlayingState.NotPlaying)
		{
			playing = PlayingState.NotPlaying;
			if(OnFinish != null) OnFinish(this);
			if(destroyOnFinish) Destroy(this);
		}
	}
	
	// Set Time
	//  - Sets the time of the animation
	//  - Arguments:
	//    + time - The time to set the animation to
	//    + evaluate - if set to true, Update will be called now instead of on the next Update, and the OnUpdate event will be raised
	
	public void SetTime(float time, bool evaluate = false)
	{
		this.time = time;
		this.normalizedTime = time / length;
		if(evaluate) Update();
	}
	
	// Set Normalized Time
	//  - Sets the time of the animation, as a percentage of its length (0 = 0%, 1 = 100%)
	//  - Arguments:
	//    + time - The normalized time to set the animation to
	//    + evaluate - if set to true, Update will be called now instead of on the next Update, and the OnUpdate event will be raised
	
	public void SetNormalizedTime(float time, bool evaluate = true)
	{
		this.time = time * length;
		this.normalizedTime = time;
		if(evaluate) Update();
	}
	
	// Evaluate
	//  - Gets the value associated with 'time' argument, including the valueMultiplier modifier
	//  - Arguments:
	//    + time - The value used to evaluate the curve
	
	public float Evaluate(float time)
	{
		return curve.Evaluate(time) * valueMultiplier;
	}
	
	// Evaluate Normalized Time
	//  - Gets the value associated with 'time' argument as a percentage of its length (0 = 0%, 1 = 100%), including the valueMultiplier modifier
	//  - Arguments:
	//    + time - The normalized value used to evaluate the curve
	
	public float EvaluateNormalizedTime(float time)
	{
		time = Mathf.Clamp(time, 0f, 1f);
		return curve.Evaluate(time * length) * valueMultiplier;
	}
	
	#endregion
}