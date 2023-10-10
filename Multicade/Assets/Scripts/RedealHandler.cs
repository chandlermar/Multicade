using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedealHandler : MonoBehaviour
{
    public void betUpHandler()
    {
        if (GameMgr.inst.currentState == GameMgr.GameState.PostDeal || !GameMgr.inst.isPressStart)
        {
            AudioMgr.inst.playSelectSound();

            if (GameMgr.inst.betAmt < 5)
            {
                GameMgr.inst.betAmt += 1;
            }
            else if (GameMgr.inst.betAmt == 5)
            {
                GameMgr.inst.betAmt = 1;
            }
        }
        else if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.PostGame || !GameMgr.inst.isPressStart)
        {
            AudioMgr.inst.playSelectSound();

            if (GameMgr.inst.betAmt < 5)
            {
                GameMgr.inst.betAmt += 1;
            }
            else if (GameMgr.inst.betAmt == 5)
            {
                GameMgr.inst.betAmt = 1;
            }
        }
        else
        {
            AudioMgr.inst.playNullSound();
        }
    }

    public void handleRedeal()
    {
        if (!GameMgr.inst.isPressStart)
        {
            AudioMgr.inst.playSelectSound();
            GameMgr.inst.isPressStart = true;
        }
        if (!GameMgr.inst.isPressStartBJ)
        {
            AudioMgr.inst.playSelectSound();
            GameMgr.inst.isPressStartBJ = true;
        }

        if (GameMgr.inst.isPoker)
        {
            if (GameMgr.inst.currentState == GameMgr.GameState.PostDeal) //if in postdeal phase and user hits deal, set to predeal
            {
                if ((MainManager.inst.credits - GameMgr.inst.betAmt) >= 0) //Case where game is to be restarted
                {
                    AudioMgr.inst.playSelectSound();
                    CardMgr.inst.handleCleanup();
                    GameMgr.inst.currentState = GameMgr.GameState.PreDeal;
                }

            }
            else if (GameMgr.inst.currentState == GameMgr.GameState.HoldPhase) //Case where player has held cards, and new cards not held must be redealed
            {
                AudioMgr.inst.playSelectSound(); //Because they hit deal button
                int length = CardMgr.inst.hand.Length;
                for (int i = 0; i < length; i++)
                {
                    if (!CardMgr.inst.hand[i].isHeld)
                    {
                        Destroy(CardMgr.inst.cardsInHandPrefabs[i]); //Destroy card not held
                        CardMgr.Card card = CardMgr.inst.generateCard();
                        CardMgr.inst.hand[i] = new CardMgr.Card(card.number, card.suit);
                        string slotName = "Slot" + (i + 1);

                        switch (slotName)
                        {
                            case "Slot1":
                                UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot1, i, false);
                                break;
                            case "Slot2":
                                UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot2, i, false);
                                break;
                            case "Slot3":
                                UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot3, i, false);
                                break;
                            case "Slot4":
                                UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot4, i, false);
                                break;
                            case "Slot5":
                                UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot5, i, false);
                                break;
                        }
                    }
                }
                /*
                Debug.Log("Hand numbers-suit 1: " + CardMgr.inst.hand[0].number + " - " + CardMgr.inst.hand[0].suit);
                Debug.Log("Hand numbers-suit 2: " + CardMgr.inst.hand[1].number + " - " + CardMgr.inst.hand[1].suit);
                Debug.Log("Hand numbers-suit 3: " + CardMgr.inst.hand[2].number + " - " + CardMgr.inst.hand[2].suit);
                Debug.Log("Hand numbers-suit 4: " + CardMgr.inst.hand[3].number + " - " + CardMgr.inst.hand[3].suit);
                Debug.Log("Hand numbers-suit 5: " + CardMgr.inst.hand[4].number + " - " + CardMgr.inst.hand[4].suit);


                Debug.Log("=-=-=-=-=");
                Debug.Log("PREFABS");

                //unsorted prefabs debugging
                Debug.Log("Hand prefab 1: " + CardMgr.inst.cardsInHandPrefabs[0]);
                Debug.Log("Hand prefab 2: " + CardMgr.inst.cardsInHandPrefabs[1]);
                Debug.Log("Hand prefab 3: " + CardMgr.inst.cardsInHandPrefabs[2]);
                Debug.Log("Hand prefab 4: " + CardMgr.inst.cardsInHandPrefabs[3]);
                Debug.Log("Hand prefab 5: " + CardMgr.inst.cardsInHandPrefabs[4]);
                */
                GameMgr.inst.currentState = GameMgr.GameState.PostDeal;

            }
        }
        else if (GameMgr.inst.isBlackjack)
        {
            if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.PostGame) //if in postdeal phase and user hits deal, set to predeal
            {
                if ((MainManager.inst.credits - GameMgr.inst.betAmt) >= 0) //Case where game is to be restarted
                {
                    AudioMgr.inst.playSelectSound();
                    CardMgr.inst.handleCleanup();
                    UIMgr.inst.resultsText.text = "".ToString();
                    GameMgr.inst.currentStateBJ = GameMgr.GameStateBJ.PreDeal;
                }

            }
            else if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.HitPhase)
            {
                AudioMgr.inst.playNullSound();
            }
        }
    }

    public void handleSplit()
    {
        if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.HitPhase)
        {
            if (GameMgr.inst.canSplit)
            {
                AudioMgr.inst.playSelectSound();
            }
            else
            {
                AudioMgr.inst.playNullSound();
            }
        }
        else
        {
            AudioMgr.inst.playNullSound();
        }
    }

    public void handleDoubleDown()
    {
        if (GameMgr.inst.isBlackjack)
        {
            if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.HitPhase && !GameMgr.inst.isDoubleDown)
            {
                if (MainManager.inst.credits-1 >= 0 && GameMgr.inst.canDouble)
                {
                    MainManager.inst.credits -= 1;
                    GameMgr.inst.isDoubleDown = true;
                    handleHit();
                    GameMgr.inst.checkPlayerBust(); //Check if player busts after dbldown. Normally this check happens in hitPhase, but this function accelerates game phase to postGame, so check playerBust manually. 
                    GameMgr.inst.currentStateBJ = GameMgr.GameStateBJ.PostGame;
                }
                else
                {
                    AudioMgr.inst.playNullSound();
                }
            }
            else if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.HitPhase && GameMgr.inst.isDoubleDown )
                AudioMgr.inst.playNullSound();
            else if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.PostGame)
            {
                AudioMgr.inst.playNullSound();
            }

        }
        

    }
    public void handleHit ()
    {
        if (GameMgr.inst.isBlackjack)
        {
            if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.HitPhase)
            {
                //IF statement goes here seeing if they can hit, or if theyve bust
                //indent and throw everything underneath inside (TO BE ADDED)
                AudioMgr.inst.playSelectSound();

                GameMgr.inst.canDouble = false;

                CardMgr.Card card = CardMgr.inst.generateCard();
                CardMgr.inst.handBJ.Insert(CardMgr.inst.numCardsInHandBJ, card); //put new card in correct slot (we have not incremented the count of cards, so the index is correct)
                string slotName = "Slot" + (CardMgr.inst.numCardsInHandBJ + 1); //Correct slot is one more than old amt of cards in hand (ex: 3rd card being set, numCards = 2, thus 3rd card goes in slot 3 (numcards + 1)) 
                switch (slotName) //Manually find which slot gameobject matches desired slot name
                {
                    case "Slot1":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot1, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot2":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot2, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot3":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot3, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot4":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot4, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot5":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot5, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot6":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot6, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot7":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot7, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot8":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot8, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    case "Slot9":
                        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot9, CardMgr.inst.numCardsInHandBJ, false);
                        break;
                    default:
                        Debug.Log("Unhandled Behavior in Switch Statement");
                        break;
                }
                CardMgr.inst.numCardsInHandBJ += 1;
            }
            else
            {
                AudioMgr.inst.playNullSound();
            }
            
        }
    }

    public void handleStand ()
    {
        if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.HitPhase)
        {
            AudioMgr.inst.playSelectSound();
            //Reveal dealer's card until stop by logic (TO BE ADDED)

            GameMgr.inst.currentStateBJ = GameMgr.GameStateBJ.PostGame;
        }
        else if (GameMgr.inst.currentStateBJ == GameMgr.GameStateBJ.PostGame)
        {
            AudioMgr.inst.playNullSound();
        }
        
    }
}
