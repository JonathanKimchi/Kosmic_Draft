using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class SimpleAttach : MonoBehaviour
{
    private Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnHandHoverBegin(Hand hand)
    {
        //hand.ShowGrabHint();
    }
    private void OnHandHoverEnd(Hand hand)
    {
        //hand.HideGrabHint();
    }
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabtype = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);
        if (interactable.attachedToHand == null && grabtype!=GrabTypes.None)
        {
            
            //Hand.AttachmentFlags.SnapOnAttach;
            hand.AttachObject(gameObject, grabtype);
            //hand;
            hand.HoverLock(interactable);
        }
        else if(isGrabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
