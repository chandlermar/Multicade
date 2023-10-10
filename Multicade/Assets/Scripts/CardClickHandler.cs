using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickHandler : MonoBehaviour
{

    public void heldHandler()
    {
        if (GameMgr.inst.currentState != GameMgr.GameState.PostDeal)
        {
            foreach (Transform child in this.transform)
            {
                if (child.CompareTag("HELD"))
                {
                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                    AudioMgr.inst.playHeldSound();
                }
            }

            string whichSlot = this.name;

            switch (whichSlot)
            {
                case "Slot1":
                    //Debug.Log("Holding: " + CardMgr.inst.hand[0].number);
                    CardMgr.inst.hand[0].isHeld = !CardMgr.inst.hand[0].isHeld;
                    break;
                case "Slot2":
                    //Debug.Log("Holding: " + CardMgr.inst.hand[1].number);
                    CardMgr.inst.hand[1].isHeld = !CardMgr.inst.hand[1].isHeld;
                    break;
                case "Slot3":
                    //Debug.Log("Holding: " + CardMgr.inst.hand[2].number);
                    CardMgr.inst.hand[2].isHeld = !CardMgr.inst.hand[2].isHeld;
                    break;
                case "Slot4":
                    //Debug.Log("Holding: " + CardMgr.inst.hand[3].number);
                    CardMgr.inst.hand[3].isHeld = !CardMgr.inst.hand[3].isHeld;
                    break;
                case "Slot5":
                    //Debug.Log("Holding: " + CardMgr.inst.hand[4].number);
                    CardMgr.inst.hand[4].isHeld = !CardMgr.inst.hand[4].isHeld;
                    break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
