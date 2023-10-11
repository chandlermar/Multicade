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
                    CardMgr.inst.hand[0].isHeld = !CardMgr.inst.hand[0].isHeld;
                    break;
                case "Slot2":
                    CardMgr.inst.hand[1].isHeld = !CardMgr.inst.hand[1].isHeld;
                    break;
                case "Slot3":
                    CardMgr.inst.hand[2].isHeld = !CardMgr.inst.hand[2].isHeld;
                    break;
                case "Slot4":
                    CardMgr.inst.hand[3].isHeld = !CardMgr.inst.hand[3].isHeld;
                    break;
                case "Slot5":
                    CardMgr.inst.hand[4].isHeld = !CardMgr.inst.hand[4].isHeld;
                    break;
            }
        }
    }
}
