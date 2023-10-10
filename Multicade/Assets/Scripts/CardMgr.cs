
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class CardMgr : MonoBehaviour
    {
        public static CardMgr inst;

        private void Awake()
        {
            inst = this;
        }
        
        public GameObject[] cardPrefabs = new GameObject[52]; //Poker
        public GameObject[] cardsInHandPrefabs = new GameObject[5]; //Poker
        public List<GameObject> cardsInHandPrefabsBJ = new List<GameObject>(); //Blackjack
        public List<GameObject> cardsInDealersHandPrefabsBJ = new List<GameObject>(); //Blackjack

        public GameObject redBackPrefab;

        public enum CardSuits
        {
            Clubs,
            Hearts,
            Spades,
            Diamonds
        }

        public class Card : IComparable<Card>
        {
            public CardSuits suit;
            public int number;
            public bool isHeld = false;
            public int rank;

            public Card()
            {

            }
            public Card(int num, CardSuits suitName)
            {
                number = num;
                suit = suitName;
            }

            
            public int CompareTo(Card other)
            {
                // Compare by number first.
                int numberComparison = this.number.CompareTo(other.number);

                if (numberComparison != 0)
                {
                    return numberComparison;
                }

                // If the numbers are equal, compare by suit.
                return this.suit.CompareTo(other.suit);
            }
        }

        public List<int> availableIndices;

        public Card[] deck; 

        public Card[] hand; //Poker

        public List<Card> handBJ; //BJ

        public List<Card> hand2BJ; //BJ hand to handle split

        public int numCardsInHandBJ; //BJ

        public int numCardsInHand2BJ; //BJ

        public List<Card> handDealerBJ; //BJ

        public int numCardsInDealersHandBJ; //BJ

        public GameObject firstCardToHide;

        public void Start()
        {
        //setup 52 card deck
            deck = new Card[52];

            int index = 0;
            for (int suit = 0; suit < 4; suit++)
            {
                for (int number = 2; number <= 14; number++) // 2 to Ace
                {
                    deck[index] = new Card(number, (CardSuits)suit);
                    
                    //Debug.Log(index + " - " + number + " - " + suit);
                    //Debug.Log(deck[index].suit + " - " + deck[index].number);
                    index++;
                    
                    
                }
            }


            availableIndices.Clear(); //Clear just in case

            //setup used indice tracking
            availableIndices = new List<int>();
            for (int i = 0; i < 52; i++)
            {
                availableIndices.Add(i);
            }


            if (GameMgr.inst.isPoker)
            {
                //setup player's hand for poker
                hand = new Card[5];
            }
            else if (GameMgr.inst.isBlackjack)
            {
                //setup player's hand for blackjack
                handBJ = new List<Card>();
                handDealerBJ = new List<Card>();
            }
    }

        public Card generateCard()
        {

            int randomIndex = UnityEngine.Random.Range(0, availableIndices.Count);
            int randomCardNumber = availableIndices[randomIndex];

            availableIndices.RemoveAt(randomIndex);    

            Card card = deck[randomCardNumber];

            return card;
        
        }

    public void handlePredealBJ()
    {
        MainManager.inst.credits -= GameMgr.inst.betAmt;

        //Handle player predeal
        for (int i = 0; i < 2; i++)
        {
            Card card = generateCard();
            handBJ.Add(card);

            string slotName = "Slot" + (i + 1);

            switch (slotName)
            {
                case "Slot1":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot1, i, false);
                    break;
                case "Slot2":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(card), UIMgr.inst.Slot2, i, false);
                    break;
            }
        }

        numCardsInHandBJ = 2; //Initial 2 cards dealt for player, set count accurate

        if (handBJ[0].number == handBJ[0].number)
            GameMgr.inst.canSplit = true;

        //Handle dealer predeal

        //generate dealer down card
        Card cardDealer = generateCard();
        handDealerBJ.Add(cardDealer);
        Debug.Log("Adding First card to list");


        //hold off on displaying card, instead display back of card
        UIMgr.inst.displayCardPrefab(redBackPrefab, UIMgr.inst.Slot10, 0, true);
        Debug.Log("Displaying red back");


        //generate dealer up card
        cardDealer = generateCard();
        handDealerBJ.Add(cardDealer);
        UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(cardDealer), UIMgr.inst.Slot11, 1, true);
        Debug.Log("Displaying and adding second card to list");
        //Display dealer's shown hand value (not down card)
        //UIMgr.inst.dealerScore.text = 

        numCardsInDealersHandBJ = 2; //Initial 2 dealer cards dealt (only 1 showing), set count accurate


        GameMgr.inst.checkDealerHandValue();
        if ((handDealerBJ[0].rank + handDealerBJ[1].rank) == 21)
        {
            GameMgr.inst.isDealerDone = true;
            GameMgr.inst.currentStateBJ = GameMgr.GameStateBJ.PostGame;
        }
        else
            GameMgr.inst.currentStateBJ = GameMgr.GameStateBJ.HitPhase;
    }

    public void handleDealerPostGame ()
    {
        
        int dealerHandValue = GameMgr.inst.checkDealerHandValue();

        if (dealerHandValue < 17)
        {
            Card dealerCard = generateCard();
            handDealerBJ.Insert(numCardsInDealersHandBJ, dealerCard); //put new card in correct slot (we have not incremented the count of cards, so the index is correct)
            string slotName = "Slot" + (numCardsInDealersHandBJ + 10); //(DEALER OFFSET OF 10: 9 + 1 + numcards Correct slot is one more than old amt of cards in hand (ex: 3rd card being set, numCards = 2, thus 3rd card goes in slot 3 (numcards + 1)) 
            
            switch (slotName) //Manually find which slot gameobject matches desired slot name
            {
                case "Slot10":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot10, 0, true);
                    break;
                case "Slot11":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot11, 1, true);
                    break;
                case "Slot12":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot12, 2, true);
                    break;
                case "Slot13":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot13, 3, true);
                    break;
                case "Slot14":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot14, 4, true);
                    break;
                case "Slot15":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot15, 5, true);
                    break;
                case "Slot16":
                    UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(dealerCard), UIMgr.inst.Slot16, 6, true);
                    break;
                default:
                    Debug.Log("Unhandled Behavior in Switch Statement");
                    break;
            }
            
            numCardsInDealersHandBJ += 1;
        }
        else if (GameMgr.inst.checkDealerHandValue() > 21)
        {
            GameMgr.inst.isDealerDone = true;
            GameMgr.inst.dealerBust = true;
        }
        else if (GameMgr.inst.checkDealerHandValue() >= 17)
        {
            GameMgr.inst.isDealerDone = true;
        }
    }

    public void handleCleanup()
        {
            if (GameMgr.inst.isPoker)
            {
                Debug.Log("Handling Cleanup");
                for (int i = 0; i < 5; i++)
                {
                    hand[i].isHeld = false;
                    hand[i] = null;
                    Destroy(cardsInHandPrefabs[i]);
                    cardsInHandPrefabs[i] = null;

                    GameObject tempObject = GameObject.Find("Slot" + (i + 1));
                    foreach (Transform child in tempObject.transform)
                    {
                        if (child.CompareTag("HELD"))
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }

                //Clear index of cards used, refill it back up
                availableIndices.Clear();
                for (int i = 0; i < 52; i++)
                {
                    availableIndices.Add(i);
                }

                UIMgr.inst.winningText.text = null;
                GameMgr.inst.scoringDone = false;
                GameMgr.inst.preDealDone = false;
            }
            else if (GameMgr.inst.isBlackjack)
            {
                Debug.Log("Handling Cleanup BJ");
                for (int i = 0; i < handBJ.Count; i++)
                {
                    Destroy(cardsInHandPrefabsBJ[i]);
                    cardsInHandPrefabsBJ[i] = null;
                }
                handBJ.Clear();
                cardsInHandPrefabsBJ.Clear();

                for (int i = 0; i < handDealerBJ.Count; i++)
                {
                    Destroy(cardsInDealersHandPrefabsBJ[i]);
                    cardsInDealersHandPrefabsBJ[i] = null;
                }
                handDealerBJ.Clear();
                cardsInDealersHandPrefabsBJ.Clear();

                //Clear index of cards used, refill it back up
                availableIndices.Clear();
                for (int i = 0; i < 52; i++)
                {
                    availableIndices.Add(i);
                }

                GameMgr.inst.canSplit = false;
                GameMgr.inst.isSplit = false;
                GameMgr.inst. canDouble = true;
                UIMgr.inst.dealerScore.text = 0.ToString();
                GameMgr.inst.isDealerDownCardRevealed = false;
                GameMgr.inst.dealerBust = false;
                GameMgr.inst.isDoubleDown = false;
                GameMgr.inst.playerBust = false;
                GameMgr.inst.isDealerDone = false;
                numCardsInHandBJ = 0;
                numCardsInDealersHandBJ = 0;
                UIMgr.inst.resultsText.text = null;
                GameMgr.inst.scoringDoneBJ = false;
                //GameMgr.inst.preDealDone = false;
        }

    }

        public void handlePredeal()
            {
        /*
                //Testing
                Card card1 = deck[11];
                hand[0] = card1;
                Card card2 = deck[34];
                hand[1] = card2;
                Card card3 = deck[8];
                hand[2] = card3;
                Card card4 = deck[48];
                hand[3] = card4;
                Card card5 = deck[50];
                hand[4] = card5;
                //
        */

                MainManager.inst.credits -= GameMgr.inst.betAmt;

                for (int i = 0; i < 5; i++)
                {
                    Card card = generateCard();
                    hand[i] = card;

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
                /*
                    Debug.Log("PREDEAL");
                    Debug.Log("Hand numbers-suit 1: " + hand[0].number + " - " + hand[0].suit);
                    Debug.Log("Hand numbers-suit 2: " + hand[1].number + " - " + hand[1].suit);
                    Debug.Log("Hand numbers-suit 3: " + hand[2].number + " - " + hand[2].suit);
                    Debug.Log("Hand numbers-suit 4: " + hand[3].number + " - " + hand[3].suit);
                    Debug.Log("Hand numbers-suit 5: " + hand[4].number + " - " + hand[4].suit);


                    Debug.Log("=-=-=-=-=");
                    Debug.Log("PREFABS");

                    //unsorted prefabs debugging
                    Debug.Log("Hand prefab 1: " + cardsInHandPrefabs[0]);
                    Debug.Log("Hand prefab 2: " + cardsInHandPrefabs[1]);
                    Debug.Log("Hand prefab 3: " + cardsInHandPrefabs[2]);
                    Debug.Log("Hand prefab 4: " + cardsInHandPrefabs[3]);
                    Debug.Log("Hand prefab 5: " + cardsInHandPrefabs[4]);
                    GameMgr.inst.preDealDone = true;*/
                    GameMgr.inst.currentState = GameMgr.GameState.HoldPhase;
           
            
        }
    public void revealDealerDownCard()
    {    
        if (!GameMgr.inst.isDealerDownCardRevealed)
        {
            Destroy(cardsInDealersHandPrefabsBJ[0]);
            cardsInDealersHandPrefabsBJ.RemoveAt(0); //Remove redback prefab
            UIMgr.inst.displayCardPrefab(UIMgr.inst.getCardPrefab(handDealerBJ[0]), UIMgr.inst.Slot10, 0, true); //Insert face of card into slot 0 and display
            GameMgr.inst.isDealerDownCardRevealed = true;
        }
        
    }

    public void handleScoringBJ()
    {
        Debug.Log("Handle Blackjack scoring");
    }

    public void handleScoring()
    {
        CardSuits royalFlushSuit = CardSuits.Clubs;
        CardSuits flushSuit = CardSuits.Clubs;

        bool contains10 = false;
        bool containsJack = false;
        bool containsQueen = false;
        bool containsKing = false;
        bool containsAce = false;

        bool isFlush = true; //initialize to random value, will be set to false if flush isnt encountered.

        bool hasFourOfAKind = false;

        bool hasThreeOfAKind = false;

        int threeOfAKindInt = 0;

        bool hasStraight = false;

        bool hasTwoPair = false;

        bool hasFacePair = false;

        bool hasFullHouse = false;

        /*
        Debug.Log("Hand numbers-suit 1: " + hand[0].number + " - " + hand[0].suit);
        Debug.Log("Hand numbers-suit 2: " + hand[1].number + " - " + hand[1].suit);
        Debug.Log("Hand numbers-suit 3: " + hand[2].number + " - " + hand[2].suit);
        Debug.Log("Hand numbers-suit 4: " + hand[3].number + " - " + hand[3].suit);
        Debug.Log("Hand numbers-suit 5: " + hand[4].number + " - " + hand[4].suit);
        */
        Debug.Log(" Now Sorting! ");
        //Sort player hand
        Array.Sort(hand);
        /*
        //Sorted hand debugging
        Debug.Log("Hand numbers-suit 1: " + hand[0].number + " - " + hand[0].suit);
        Debug.Log("Hand numbers-suit 2: " + hand[1].number + " - " + hand[1].suit);
        Debug.Log("Hand numbers-suit 3: " + hand[2].number + " - " + hand[2].suit);
        Debug.Log("Hand numbers-suit 4: " + hand[3].number + " - " + hand[3].suit);
        Debug.Log("Hand numbers-suit 5: " + hand[4].number + " - " + hand[4].suit);

        Debug.Log("=-=-=-=-=");

        //unsorted prefabs debugging
        Debug.Log("Hand prefab 1: " + cardsInHandPrefabs[0]);
        Debug.Log("Hand prefab 2: " + cardsInHandPrefabs[1]);
        Debug.Log("Hand prefab 3: " + cardsInHandPrefabs[2]);
        Debug.Log("Hand prefab 4: " + cardsInHandPrefabs[3]);
        Debug.Log("Hand prefab 5: " + cardsInHandPrefabs[4]);
        */

        //Determine what hand player has--------------------------------------------------------------------------------

        //Check for royal flush
        foreach (Card card in hand)
        {
            if (card.number == 10)
            {
                royalFlushSuit = card.suit;
                contains10 = true;
            }
            else if (card.number == 11 && card.suit == royalFlushSuit)
            {
                containsJack = true;
            }
            else if (card.number == 12 && card.suit == royalFlushSuit)
            {
                containsQueen = true;
            }
            else if (card.number == 13 && card.suit == royalFlushSuit)
            {
                containsKing = true;
            }
            else if (card.number == 14 && card.suit == royalFlushSuit)
            {
                containsAce = true;
            }
        }
        

        //Check for x of a kind
        
        int matchingCount = 1;

        int matchingFaceCount = 1;
       
        int prevNumber = hand[0].number;

        for (int i = 1; i < hand.Length; i++)
        {
            int currentNumber = hand[i].number;
            
            if (currentNumber == prevNumber)
            {
                if (
                    currentNumber == 11 && prevNumber == 11 || 
                    currentNumber == 12 && prevNumber == 12 ||
                    currentNumber == 13 && prevNumber == 13 ||
                    currentNumber == 14 && prevNumber == 14
                    )
                {
                    matchingFaceCount++;
                }
                matchingCount++;
                if (matchingCount == 4)
                {
                    hasFourOfAKind = true;
                }
                else if (matchingCount == 3)
                {
                    hasThreeOfAKind = true;
                    threeOfAKindInt = currentNumber;
                }
                else if (matchingFaceCount == 2)
                {
                    Debug.Log(currentNumber + " - " + prevNumber);

                    hasFacePair = true;
                }
            }
            else
            {
                matchingCount = 1; 
            }
            prevNumber = currentNumber;
        }


        //Full House

        prevNumber = hand[0].number;
        matchingCount = 1;

        for (int i = 1; i < hand.Length; i++)
        {
            int currentNumber = hand[i].number;

            if (currentNumber == prevNumber)
            {
                matchingCount++;
                
                if (matchingCount == 2 && threeOfAKindInt != currentNumber && hasThreeOfAKind)
                {
                    hasFullHouse = true;
                }
            }
            else
            {
                matchingCount = 1;
            }
            prevNumber = currentNumber;
        }


        //Check for two pair
        int pairCount = 0;

        prevNumber = hand[0].number;

        for (int i = 1; i < 5; i++)
        {
            int currentNumber = hand[i].number;

            if (currentNumber == prevNumber)
            {
                pairCount++;
               
                if (pairCount == 2)
                {
                    hasTwoPair = true;
                }
                
            }

            prevNumber = currentNumber;
        }

       

        //Check for Flush
        
        flushSuit = hand[0].suit;

        foreach (Card card in hand)
        {
            if (card.suit != flushSuit)
            {
                isFlush = false;
                break;
            }
        }

        //Check for Straight

        int straightCount = 1;
        prevNumber = hand[0].number;

        for (int i = 1; i < hand.Length; i++)
        {
            int currentNumber = hand[i].number;

            if (currentNumber == prevNumber+1)
            {
                Debug.Log(straightCount);
                straightCount++;
                if (straightCount >= 5 )
                {
                    hasStraight = true;
                }
            }
            else
            {
                straightCount = 1;
            }
            
            prevNumber = currentNumber;

        }
        //Display winning hand -------------------------------------------------------------------------
        if (contains10 && containsJack && containsQueen && containsKing && containsAce)
        {
            Debug.Log("Hit the Royal Flush");
            UIMgr.inst.winningText.text = "Royal flush!";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (250 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        else if (isFlush && hasStraight)
        {
            UIMgr.inst.winningText.text = "Straight Flush.";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (50 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        else if (hasFourOfAKind == true)
        {
            UIMgr.inst.winningText.text = "Four of a kind.";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (25 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        else if (hasFullHouse)
        {
            UIMgr.inst.winningText.text = "Full House.";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (9 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        else if (isFlush)
        {
            UIMgr.inst.winningText.text = "Flush.";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (6 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        else if (hasStraight == true)
        {
            UIMgr.inst.winningText.text = "Straight";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (4 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        else if (hasThreeOfAKind == true)
        {
            UIMgr.inst.winningText.text = "Three of a kind";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (3 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        
        else if (hasTwoPair == true)
        {
            UIMgr.inst.winningText.text = "Two Pair";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (2 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }
        
        else if (hasFacePair == true)
        {
            UIMgr.inst.winningText.text = "Pair";
            AudioMgr.inst.playWinSound();
            MainManager.inst.credits += (1 * GameMgr.inst.betAmt);
            GameMgr.inst.scoringDone = true;
            return;
        }

        GameMgr.inst.scoringDone = true;
    }

    public CardSuits readCardSuit(int index)
        {
            return deck[index].suit;
        }

       public int readCardNumber(int index)
       {
            return deck[index].number;
       }
        
   }

    
