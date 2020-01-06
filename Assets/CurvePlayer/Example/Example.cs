using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour 
{
	public AnimationCurve xRotationCurve;
	public AnimationCurve yRotationCurve;
	
	public AnimationCurve xScaleCurve;
	public AnimationCurve yScaleCurve;
	public AnimationCurve zScaleCurve;
	
	public AnimationCurve yPosCurve;
	public AnimationCurve zPosCurve;
	
	private CurvePlayer xRotation;
	private CurvePlayer yRotation;
	
	private CurvePlayer xScale;
	private CurvePlayer yScale;
	private CurvePlayer zScale;
	
	private CurvePlayer yPos;
	private CurvePlayer zPos;
	
	Quaternion xQuat;
	Quaternion yQuat;
	
	GUIStyle style;
	
	void OnGUI()
	{	
		GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Press [spacebar] to animate.\nPress [R] to reverse.", style);
	}
	
	void Awake()
	{
		style = new GUIStyle();
		style.fontSize = 50;
		style.normal.textColor = Color.white;		
	}
	
	void Start()
	{
		xRotation = CurvePlayer.ReadyToPlayCurve(xRotationCurve, 1, 360, false, false, EvaluateCurve);
		yRotation = CurvePlayer.ReadyToPlayCurve(yRotationCurve, 1, 360, false, false, EvaluateCurve);
		
		xScale = CurvePlayer.ReadyToPlayCurve(xScaleCurve, 1, 1, false, false, EvaluateCurve);
		yScale = CurvePlayer.ReadyToPlayCurve(yScaleCurve, 1, 1, false, false, EvaluateCurve);
		zScale = CurvePlayer.ReadyToPlayCurve(zScaleCurve, 1, 1, false, false, EvaluateCurve);
		
		yPos = CurvePlayer.ReadyToPlayCurve(yPosCurve, 1, 1, false, false, EvaluateCurve);
		zPos = CurvePlayer.ReadyToPlayCurve(zPosCurve, 1, 1, false, false, EvaluateCurve);
		
		xRotation.OnFinish += HandleXRotationOnFinish;
	}
	
	void HandleXRotationOnFinish (CurvePlayer caller)
	{
		xRotation.reverse = !xRotation.reverse;
		xRotation.Pause();
		
		yRotation.reverse = !yRotation.reverse;
		yRotation.Pause();
		
		xScale.reverse = !xScale.reverse;
		xScale.Pause();
		
		yScale.reverse = !yScale.reverse;
		yScale.Pause();
		
		zScale.reverse = !zScale.reverse;
		zScale.Pause();
		
		yPos.reverse = !yPos.reverse;
		yPos.Pause();
		
		zPos.reverse = !zPos.reverse;
		zPos.Pause();
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			xRotation.Play();
			yRotation.Play();
			
			xScale.Play();
			yScale.Play();
			zScale.Play();
			
			yPos.Play();
			zPos.Play();
		}
		
		if(Input.GetKeyDown(KeyCode.R))
		{
			xRotation.reverse = !xRotation.reverse;
			yRotation.reverse = !yRotation.reverse;
			
			xScale.reverse = !xScale.reverse;
			yScale.reverse = !yScale.reverse;
			zScale.reverse = !zScale.reverse;
			
			yPos.reverse = !yPos.reverse;
			zPos.reverse = !zPos.reverse;
		}
	}
	
	public void EvaluateCurve(CurvePlayer caller, float value)
	{
		if(caller == xRotation)
		{
			xQuat = Quaternion.AngleAxis(value, Vector3.right);
			transform.rotation = yQuat * xQuat;
		}
		
		else if(caller == yRotation)
		{
			yQuat = Quaternion.AngleAxis(value, Vector3.up);
			transform.rotation = yQuat * xQuat;		
		}
		
		else if(caller.curve == xScaleCurve) transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
		else if(caller.curve == yScaleCurve) transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
		else if(caller.curve == zScaleCurve) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
		
		else if(caller.curve == yPosCurve) transform.position = new Vector3(transform.position.x, value, transform.position.z);
		else if(caller.curve == zPosCurve) transform.position = new Vector3(transform.position.x, transform.position.y, value);
	}
}
