
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameMgr : MonoBehaviour
    {

        public static GameMgr inst;

        private void Awake()
        {
            inst = this;
            currentState = GameState.PreDeal;
            currentStateBJ = GameStateBJ.PreDeal;  
        }

        public enum GameState
        {
            PreDeal,
            HoldPhase,
            PostDeal
        }

        public enum GameStateBJ
        {
            PreDeal,
            HitPhase,
            PostGame
        }

        public bool isPoker = false;
        public bool isBlackjack = true;

        public int betAmt = 1;

        public GameState currentState = GameState.PreDeal; //Poker
        public GameStateBJ currentStateBJ = GameStateBJ.PreDeal; //Blackjack

        public GameObject Slots; //Poker
        public GameObject SlotsBJ; //Blackjack
        public GameObject SlotsForSplitBJ;

        public bool scoringDone = false; //Poker
        public bool preDealDone = false; //Poker

        public bool scoringDoneBJ = false; //Blackjack
        public bool playerBust = false; //Blackjack
        public bool dealerBust = false; 
        public bool isDoubleDown = false;
        public bool isDealerDone = false;
        public bool isDealerDownCardRevealed = false;
        public int dealersAcesCount = 0;
        public int playersAcesCount = 0;
        public bool canDouble = true;
        public bool isPressStart;
        public bool isPressStartBJ;
        public bool canSplit = false;
        public bool isSplit = false;


        // Start is called before the first frame update
        void Start()
        {
            isPressStart = false;
            isPressStartBJ = false;
        }


        // Update is called once per frame
        void Update()
        {
            if (isPressStart)
            {
                if (isPoker)
                {
                    //Debug.Log("Current State: " + currentState);

                    if (currentState == GameState.PreDeal)
                    {
                        CardMgr.inst.handlePredeal();
                    }
                    else if (currentState == GameState.PostDeal && !scoringDone)
                    {
                        CardMgr.inst.handleScoring();
                    }
                }
            }

        if (isPressStartBJ)
        {
            if (isBlackjack)
            {
                if (currentStateBJ == GameStateBJ.PreDeal)
                {
                    CardMgr.inst.handlePredealBJ();
                }
                else if (currentStateBJ == GameStateBJ.HitPhase)
                {
                    if (!isSplit)
                    {
                        checkPlayerBust();
                        checkDealerHandValue();
                        if (playerBust)
                        {
                            currentStateBJ = GameStateBJ.PostGame;
                        }
                    }
                    else if (isSplit)
                    {

                    }

                }
                else if (currentStateBJ == GameStateBJ.PostGame)
                {
                    //checkDealerHandValue();


                    if (playerBust) //When player hits deal, redealHandler handles cleanup and launching into predeal phase
                    {
                        UIMgr.inst.resultsText.text = "You Bust".ToString();
                        CardMgr.inst.revealDealerDownCard();
                    }
                    else if (!isDealerDone) //player stands, dealer gets delt until bust/stand
                    {
                        CardMgr.inst.revealDealerDownCard();
                        CardMgr.inst.handleDealerPostGame();

                        if (isDealerDone)
                        {

                            if (dealerBust)
                            {
                                UIMgr.inst.resultsText.text = "Dealer Busts".ToString();
                                AudioMgr.inst.playWinSound();
                                if (isDoubleDown)
                                {
                                    MainManager.inst.credits += betAmt * 4; //covers double bet and double payout
                                }
                                else
                                    MainManager.inst.credits += betAmt * 2; //covers normal bet x2

                            }
                            else if (checkDealerHandValue() > checkPlayerHandValue())
                            {
                                UIMgr.inst.resultsText.text = "Dealer Wins".ToString();
                            }
                            else if (checkPlayerHandValue() > checkDealerHandValue())
                            {
                                UIMgr.inst.resultsText.text = "Player Wins".ToString();
                                AudioMgr.inst.playWinSound();
                                if (isDoubleDown)
                                {
                                    MainManager.inst.credits += betAmt * 4;
                                }
                                else
                                    MainManager.inst.credits += betAmt * 2;

                            }
                            else if (checkPlayerHandValue() == checkDealerHandValue())
                            {
                                UIMgr.inst.resultsText.text = "Push".ToString();
                                if (isDoubleDown)
                                {
                                    MainManager.inst.credits += betAmt * 2;
                                }
                                else
                                    MainManager.inst.credits += betAmt;
                            }
                        }
                        //display win/loss and add credits
                    }
                    //CardMgr.inst.handleScoringBJ();
                }
            }
        }
        }

    public void checkPlayerBust()
    {
        int handValue = checkPlayerHandValue();
        if (handValue > 21)
        {
            playerBust = true;
        }
    }

    public int checkPlayerHandValue()
    {
        if (CardMgr.inst.handBJ.Count > 0)
        {
            int handValue = 0;
            int playerAcesCount = 0;

            foreach (CardMgr.Card card in CardMgr.inst.handBJ)
            {
                if (card.number >= 10 && card.number != 14)
                {
                    card.rank = 10;
                }
                else if (card.number == 14)
                {
                    playerAcesCount += 1;
                    card.rank = 11;
                }
                else if (card.number >=2 && card.number<=9)
                {
                    card.rank = card.number;
                }
                    

                handValue += card.rank;
            }

            while (playerAcesCount > 0 && handValue > 21)
            {
                // Adjust the value of one Ace from 11 to 1
                handValue -= 10;
                playerAcesCount--;
            }
            
            UIMgr.inst.playerScore.text = handValue.ToString();
            return handValue;
        }
        else
            return 0;
    }

    public int checkDealerHandValue()
    {
        if (CardMgr.inst.handDealerBJ.Count > 0)
        {
            int handValue = 0;
            int dealerAcesCount = 0;

            foreach (CardMgr.Card card in CardMgr.inst.handDealerBJ)
            {
                if (card.number >= 10 && card.number != 14)
                {
                    card.rank = 10;
                }
                else if (card.number == 14)
                {
                    dealerAcesCount += 1;
                    card.rank = 11;
                }
                else if (card.number >= 2 && card.number <= 9)
                {
                    card.rank = card.number;
                }


                handValue += card.rank;
            }

            while (dealerAcesCount > 0 && handValue > 21)
            {
                // Adjust the value of one Ace from 11 to 1
                handValue -= 10;
                dealerAcesCount--;
            }

            if (!isDealerDownCardRevealed)
            {
                if (!playerBust)
                    handValue = CardMgr.inst.handDealerBJ[1].rank; //Handles the case of displaying only up card
                else if (playerBust)
                    handValue = CardMgr.inst.handDealerBJ[1].rank + CardMgr.inst.handDealerBJ[0].rank; //Handles the case of displaying both up and down card in the case that the player busts
            }

            UIMgr.inst.dealerScore.text = handValue.ToString();
            return handValue;
        }
        else
            return 0;

    }
}

    
