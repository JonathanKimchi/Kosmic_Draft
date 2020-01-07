using UnityEngine;

public class BasicPlayScript : MonoBehaviour
{
    // Start is called before the first frame update
    public StoreTransform[] targets;//stores unfiltered frames.
    public int targetTotalLength=0;
    public StoreTransform[] keyFrames;//stores keyframes.  
    public int targetTracker = 0;//marks current place in playing iteration.
    public float initTime = 0;//initial time.  will always start at 0.
    public float finalTime = 0;//finalTime for the next keyFrame.  keyFrames[n+1].keySeconds - 
                               //keyFrames[n].keySeconds;
                               //if finalTime==0, set finalTime==key...
    public int keyTracker = 0;//tracks which key the iter is on.  should never be >= keyframes.lenggth-2
    public int playState = 0;
    public BVHRecorder recorder;

    public void FilterTargets()
    {
        int lengthTemp = targets.Length;
        for (int i = 0; i < lengthTemp; i++)
        {
            if (targets[i] != null)//check
            {
                targetTotalLength++;
            }
        }
        keyFrames = new StoreTransform[targetTotalLength];
        for (int i = 0; i < lengthTemp; i++)
        {
            if (targets[i] != null)
            {
                keyFrames[targetTracker] = targets[i];
                targetTracker++;
            }
        }
        targetTracker = 0;
        targetTotalLength = 0;
    }
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
     * precondition- keyframeTracker<keyFrames.length-1.  The timeline has to have at least two keyframes
     * for an animation to occur.  
     * 
     *(this animation script will probably need to be rewritten to account for deadspace.  A new
     * keyframe can automatically be added at the end of an animation that copies that locrot of 
     * the previous keyframe.
     *  Just realized that this animation isn't going to work all that well with BVHExporter due to
     *  extraneous keyframes.  That's okay--I'm sure there is a way to filter out those things 
     *  later on.
     *  
     */
    void Start()
    {
        GameObject temp = GameObject.Find("Save");
        recorder = temp.GetComponent<BVHRecorder>();
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
        SerialAdd temp1 = GetComponent<SerialAdd>();
        targets = temp1.transformStorage;
        FilterTargets();
        //recorder.capturing = true;
        playState = 1;
        
    }
    public void RecordStart()
    {
        SerialAdd temp1 = GetComponent<SerialAdd>();
        targets = temp1.transformStorage;
        FilterTargets();
        recorder.capturing = true;
        playState = 1;

    }
    public void PositionUpdate(int currentTime,int maxKeyFrame)
    {
        StoreTransform position1 = new StoreTransform();
        StoreTransform position2 = new StoreTransform();
        for (int i = currentTime; i >= 0; i--)
        {
            if (targets[i] != null)
            {
                position1 = targets[i];
                i = -1;
            }
            //position1 = targets[i];
        }
        //position1 = targets[0];
        for (int i = currentTime; i <= maxKeyFrame; i++)
        {
            if (targets[i] != null)
            {
                position2 = targets[i];
                i = maxKeyFrame+2;
            }
        }
        if (position1 == null)
        {
            return;
        }
        if (position2 == null)
        {
            return;
        }
        // position1 = targets[0];
        float endingFrames = position2.storeFrame - position1.storeFrame;
        float lerpRatio = (currentTime - position1.storeFrame)/endingFrames;
        //float lerpRatio = (float)(position1.current)
        transform.localPosition = Vector3.Lerp(position1.position, position2.position, lerpRatio);
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
                if (recorder.capturing == true)
                {
                    //recorder.saveBVH();
                    recorder.capturing = false;
                    recorder.saveBVH();

                }
                //recorder.capturing = false;
            }
            else
            {
                if (finalTime == 0)
                {
                    finalTime = (float)(keyFrames[keyTracker + 1].keySecond - keyFrames[keyTracker].keySecond);
                    //recorder.capturing = true;

                }
                initTime += Time.deltaTime;
                if (initTime >= finalTime)
                {
                    initTime = finalTime;
                }
                float lerpRatio = initTime / finalTime;
                transform.localPosition = Vector3.Lerp(keyFrames[keyTracker].position, keyFrames[keyTracker + 1].position, lerpRatio);
                transform.localRotation = Quaternion.Lerp(keyFrames[keyTracker].rotation, keyFrames[keyTracker + 1].rotation, lerpRatio);
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