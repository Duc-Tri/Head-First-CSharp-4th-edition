﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoFish
{
    public class Player
    {
        public static Random Random = new Random();
        private List<Card> hand = new List<Card>();
        private List<Values> books = new List<Values>();

        /// <summary>
        /// The cards in the player's hand
        /// </summary>
        public IEnumerable<Card> Hand => hand;

        /// <summary>
        /// The books that the player has pulled out
        /// </summary>
        public IEnumerable<Values> Books => books;
        public readonly string Name;

        /// <summary>
        /// Pluralize a word, adding "s" if a value isn't equal to 1
        /// </summary>
        public static string S(int s) => s < 2 ? "" : "s";

        /// <summary>
        /// Returns the current status of the player: the number of cards and books
        /// </summary>
        public string Status => $"Player {Name} has {hand.Count} card{S(hand.Count)} and {books.Count} book{S(books.Count)}";

        /// <summary>
        /// Constructor to create a player
        /// </summary>
        /// <param name="name">Player's name</param>
        public Player(string name) => Name = name;

        /// <summary>
        /// Alternate constructor (used for unit testing)
        /// </summary>
        /// <param name="name">Player's name</param>
        /// <param name="cards">Initial set of cards</param>
        public Player(string name, IEnumerable<Card> cards)
        {
            Name = name;
            hand.AddRange(cards);
        }

        /// <summary>
        /// Gets up to five cards from the stock
        /// </summary>
        /// <param name="stock">Stock to get the next hand from</param>
        public void GetNextHand(Deck stock)
        {
            while (stock.Count > 0 && hand.Count < 5)
                hand.Add(stock.Deal(0));
        }

        /// <summary>
        /// If I have any cards that match the value, return them. If I run out of cards, get
        /// the next hand from the deck.
        /// </summary>
        /// <param name="value">Value I'm asked for</param>
        /// <param name="deck">Deck to draw my next hand from</param>
        /// <returns>The cards that were pulled out of the other player's hand</returns>
        public IEnumerable<Card> DoYouHaveAny(Values value, Deck deck)
        {
            var cards = hand.Where(x => x.Value == value).OrderBy(x => x.Suit).ToList();

            hand = hand.Where(card => card.Value != value).ToList();
            //foreach (var card in cards) hand.Remove(card);

            if (hand.Count == 0)
                GetNextHand(deck);

            return cards;
        }

        /// <summary>
        /// When the player receives cards from another player, adds them to the hand
        /// and pulls out any matching books
        /// </summary>
        /// <param name="cards">Cards from the other player to add</param>
        public void AddCardsAndPullOutBooks(IEnumerable<Card> cards)
        {
            hand.AddRange(cards);

            /*
            var groups = hand.GroupBy(v => v.Value);
            foreach (var g in groups)
                if (g.Count() == 4)
                {
                    books.Add(g.Key);
                    foreach (var c in g)
                        hand.Remove(c);
                }
            */

            // We used GroupBy to group the hand by value, then Where to include only the groups that have all four suits, and finally Select to convert each group to its key, the suit.
            var foundBooks = hand
            .GroupBy(card => card.Value)
            .Where(group => group.Count() == 4)
            .Select(group => group.Key);


            // Once the method finds the books, it adds them to its private books field, and then updates its private hand field to remove any cards that match a found book.
            books.AddRange(foundBooks);
            books.Sort();
            hand = hand
            .Where(card => !books.Contains(card.Value))
            .ToList();
        }

        /// <summary>
        /// Draws a card from the stock and add it to the player's hand
        /// </summary>
        /// <param name="stock">Stock to draw a card from</param>
        public void DrawCard(Deck stock)
        {
            if (stock.Count > 0)
                AddCardsAndPullOutBooks(new List<Card>() { stock.Deal(0) });
        }

        /// <summary>
        /// Gets a random value from the player's hand
        /// </summary>
        /// <returns>The value of a randomly selected card in the player's hand</returns>
        public Values RandomValueFromHand() => hand[Random.Next(hand.Count)].Value;
        public override string ToString() => Name;
    }
}