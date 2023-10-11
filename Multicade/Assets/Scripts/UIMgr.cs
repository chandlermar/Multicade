using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMgr : MonoBehaviour
{

    public static UIMgr inst;

    private void Awake()
    {
        inst = this;
    }

    //Slot 1-5 Used for Poker
    //Slot 1-12 Used for Blackjack
    public GameObject Slot1; 
    public GameObject Slot2;
    public GameObject Slot3;
    public GameObject Slot4;
    public GameObject Slot5;
    public GameObject Slot6;
    public GameObject Slot7;
    public GameObject Slot8;
    public GameObject Slot9;
    public GameObject Slot10;
    public GameObject Slot11;
    public GameObject Slot12;
    public GameObject Slot13;
    public GameObject Slot14;
    public GameObject Slot15;
    public GameObject Slot16;

    public TMP_Text winningText; //Poker
    public TMP_Text dealerScore; //BJ
    public TMP_Text playerScore; //BJ
    public TMP_Text playerScore2; //BJ
    public TMP_Text creditsText;
    public TMP_Text betAmtText;
    public TMP_Text resultsText; //BJ

    public GameObject getCardPrefab(CardMgr.Card card)
    {
        int suitIndex = (int)card.suit;
        int number = card.number;

        if (suitIndex >=0 && suitIndex < 4 && number >=2 && number <= 14)
        {
            int cardIndex = suitIndex * 13 + (number - 2);

            if (cardIndex >= 0 && cardIndex < CardMgr.inst.cardPrefabs.Length)
            {
                return CardMgr.inst.cardPrefabs[cardIndex]; 
            }
        }
        return null;
    }

    public void displayCardPrefab(GameObject cardPrefab, GameObject slot, int index, bool isForDealer)
    {
        if (GameMgr.inst.isPoker)
        {
            CardMgr.inst.cardsInHandPrefabs[index] = Instantiate(cardPrefab, slot.transform);
        }  
        else if (GameMgr.inst.isBlackjack)
        {
            if (isForDealer)
            {
                CardMgr.inst.cardsInDealersHandPrefabsBJ.Insert(index, Instantiate(cardPrefab, slot.transform));
            }
            else
            {
                CardMgr.inst.cardsInHandPrefabsBJ.Insert(index, Instantiate(cardPrefab, slot.transform));
            }
        }
    }

    
    void Update()
    {
        creditsText.text = MainManager.inst.credits.ToString();
        betAmtText.text = GameMgr.inst.betAmt.ToString();
    }

    public void displayWinningText(string input)
    {
        
    }
}
