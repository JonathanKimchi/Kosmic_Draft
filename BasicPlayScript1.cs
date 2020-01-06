using UnityEngine;

public class BasicPlayScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    public StoreTransform[] keyFrames;
    public float initTime = 0;//initial time.  will always start at 0.
    public float finalTime=0;//finalTime for the next keyFrame.  keyFrames[n+1].keySeconds - 
                             //keyFrames[n].keySeconds;
                             //if finalTime==0, set finalTime==key...
    public int keyTracker = 0;//tracks which key the iter is on.  should never be >= keyframes.lenggth-2
    public int playState = 0;

    /*
     * Goal of this script is to interpolate between the given keyframes within a certain
     * length of time.  The keyframes are going to store at least two things:
     * the position object of the keyframe, and the frame of the keyframe itself.  
     * Let us assume that the keySecond is stored as well, to make things easier on us.
     * finalTime will be set to the value of the second keyframe - first keyframe in 
     * seconds.  The initial value will be set to zero.  For every loop of the 
     * algorithm, we want the initial time  to be updated according to Time.deltaTime();
     * After initialTime>=finalTime, we add one to the keyframeTracker, then 
     * set the initialTime=0.  
     * if we post initialize initialTime, we won't have to rewrite initialTime every loop.
     * finalTime is rewritten every loop, I think.  That shouldn't matter though, since
     * finalTime doesn't change till the loop ends.  
     * That may cause some slowdowns, but that is still to be determined.  
     * 
     * precondiion- keyframeTracker<keyFrames.length-1.  The timeline has to have at least two keyframes
     * for an animation to occur.  
     * 
     *(this animation script will probably need to be rewritten to account for deadspace.  A new
     * keyframe can automatically be added at the end of an animation that copies that locrot of 
     * the previous keyframe.
     *  Just realized that this animation isn't going to work all that well with BVHExporter due to
     *  extraneous keyframes.  That's okay--I'm sure thhere is a way to filter out those tings 
     *  later on.
     *  
     */
    void Start()
    {

    }
    //public void calculateCurve(
    /*public float CalcFrameLength(int fps)
    {
        float tempTime = 1;
        frameLength = (float)(tempTime / fps);
        return frameLength;
    }//calculates how long a frame would be in seconds  
    public float CalcTimeTotal(int fps, int totalFrames)
    {
        lerpTime = (float)(totalFrames / fps);
        return lerpTime;
    }//calculates the time needed to execute.
    // Update is called once per frame*/
    public void PlayerStart()
    {
        playState = 1;
    }
    void Update()
    {
        if (playState != 0)
        {
            if (keyTracker >= keyFrames.Length - 1)
            {
                keyTracker = 0;
                finalTime = 0;
                initTime = 0;
                playState = 0;
            }
            else
            {
                if (finalTime == 0)
                {
                    finalTime = (float)(keyFrames[keyTracker + 1].keySecond - keyFrames[keyTracker].keySecond);
                }
                initTime += Time.deltaTime;
                if (initTime >= finalTime)
                {
                    initTime = finalTime;
                }
                float lerpRatio = initTime / finalTime;
                transform.position = Vector3.Lerp(keyFrames[keyTracker].position, keyFrames[keyTracker + 1].position, lerpRatio);
                if (lerpRatio == 1)
                {
                    //playState = 0;
                    finalTime = 0;
                    initTime = 0;
                    keyTracker++;//this keyTracker will allow iter to continue in order.
                }
            }
        }
    }
}



using UnityEngine;

public class BasicPlayScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform targetA;
    public Transform targetB;
    public AnimationCurve lerpCurve;
    public Vector3 lerpOffset;
    public float frameLength = 0;
    public float lerpTime = 0;
    public float timer = 0;
    public int playState = 0;
    void Start()
    {

    }
    //public void calculateCurve(
    /*public float CalcFrameLength(int fps)
    {
        float tempTime = 1;
        frameLength = (float)(tempTime / fps);
        return frameLength;
    }//calculates how long a frame would be in seconds  
    public float CalcTimeTotal(int fps, int totalFrames)
    {
        lerpTime = (float)(totalFrames / fps);
        return lerpTime;
    }//calculates the time needed to execute.
    // Update is called once per frame*/
    public void PlayerStart()
    {
        playState = 1;
    }
    void Update()
    {
        if (playState != 0)
        {
            timer += Time.deltaTime;
            if (timer > lerpTime)
            {
                timer = lerpTime;
            }
            float lerpRatio = timer / lerpTime;
            transform.position = Vector3.Lerp(targetA.position, targetB.position, lerpRatio);
            if (lerpRatio == 1)
            {
                playState = 0;
                this.GetComponent<PlayScript>().PlayerCalc(timer);
            }
        }
    }
}
