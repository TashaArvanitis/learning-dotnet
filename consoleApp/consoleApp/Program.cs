using System;
using System.Collections.Generic;
using System.Linq;

namespace consoleApp
{
    public class Disaster
    {
        public string name;
        public Lifeline[] lifelinesToStabilize;
        List<Lifeline> lifelinesApplied;
        bool[] stabilized
        {
            get
            {
                // Figure out which lifelines have been stabilized.
                bool[] returnValue = new bool[lifelinesToStabilize.Length];
                Array.Fill(returnValue, false);
                List<Lifeline> applied = new List<Lifeline>(lifelinesApplied);
                for (int i = 0; i < lifelinesToStabilize.Length; i++)
                {
                    int idx2 = applied.IndexOf(lifelinesToStabilize[i]);
                    if (idx2 > -1)
                    {
                        // lifeline idx is stabilized!
                        returnValue[i] = true;
                        applied.RemoveAt(idx2);
                    }
                }
                return returnValue;
            }
        }
        // Initialization
        public Disaster(string name, Lifeline[] lifelines)
        {
            this.name = name;
            Array.Sort(lifelines);
            this.lifelinesToStabilize = lifelines;
            this.lifelinesApplied = new List<Lifeline>();
        }

        public static Disaster Random()
        {
            Random random = new Random();
            int nLifelines = random.Next(1, 10);
            Lifeline[] lifelines = new Lifeline[nLifelines];
            for (int i = 0; i < nLifelines; i++)
            {
                Lifeline lifeline = Extensions.GetRandomLifeline();
                lifelines[i] = lifeline;
            }
            Disaster disaster = new Disaster("Hurricane", lifelines);

            return disaster;
        }

        // String representation
        public override string ToString()
        {
            string llstr = "[";
            for (int i = 0; i < lifelinesToStabilize.Length; i++)
            {
                if (stabilized[i]) {
                    llstr += "✓ " + lifelinesToStabilize[i] + ", ";
                } else {
                    llstr += lifelinesToStabilize[i] + ", ";
                }
            }
            llstr = llstr.Substring(0, llstr.Length - 2);
            llstr += "]";
            string v = $"{name}: {llstr}";
            return v;
        }

        // Apply an OperationsResource
        public void ApplyLifelines(Lifeline[] lifelines)
        {
            foreach (Lifeline lifeline in lifelines)
            {
                lifelinesApplied.Add(lifeline);
            }
        }

        // Check if the disaster is stabilized.
        public bool isStabilized {
            get {
                if (lifelinesToStabilize.Length == 0) {
                    return true;
                }
                bool allStabilized = true;
                bool[] stabilizedCopy = new bool[lifelinesToStabilize.Length];
                Array.Copy(stabilized, stabilizedCopy, lifelinesToStabilize.Length);
                for (int i = 0; i < stabilizedCopy.Length; i++)
                {
                    allStabilized &= stabilizedCopy[i];
                }
                return allStabilized;
            }
        } 
    }


    public class OpsCard
    {
        public string name;
        public Lifeline[] resolves;

        public OpsCard(string name, Lifeline[] lifelines)
        {
            this.name = name;
            Array.Sort(lifelines);
            this.resolves = lifelines;
        }

        public static OpsCard Random()
        {
            Random random = new Random();
            int nLifelines = random.Next(1, 4);
            Lifeline[] lifelines = new Lifeline[nLifelines];
            Lifeline lifeline = Extensions.GetRandomLifeline();
            for (int i = 0; i < nLifelines; i++)
            {
                lifelines[i] = lifeline;
            }

            string name;
            switch (lifeline) {
                case Lifeline.Communication:
                    name = "Telecom Repair Team";
                    break;
                case Lifeline.Energy:
                    name = "Generator";
                    break;
                case Lifeline.FoodWaterShelter:
                    name = "Local School";
                    break;
                case Lifeline.HazardousMaterial:
                    name = "HazMat Team";
                    break;
                case Lifeline.HealthMedical:
                    name = "Paramedic";
                    break;
                case Lifeline.SafetySecurity:
                    name = "Police Officer";
                    break;
                case Lifeline.Transportation:
                    name = "Bus";
                    break;
                default:
                    name = "Random resource!";
                    break;
            }
            OpsCard card = new OpsCard(name, lifelines);

            return card;
        }

        public override string ToString()
        {
            string llstr = "[";
            for (int i = 0; i < resolves.Length; i++)
            {
                 llstr += resolves[i] + ", ";
            }
            llstr = llstr.Substring(0, llstr.Length - 2);
            llstr += "]";
            string v = $"OpsCard {name}: {llstr}";
            return v;
        }

    }

    public class Game
    {
        public Disaster disaster;
        public List<OpsCard> hand;

        public Game(Disaster disaster, List<OpsCard> hand)
        {
            this.disaster = disaster;
            this.hand = hand;
        }

        public string state
        {
            get
            {
                string returnVal = "";
                returnVal += "You need to stabilize a " + disaster + "\n";
                returnVal += "Your hand contains:\n";
                for (int i = 0; i < hand.Count; i++)
                {
                    returnVal += $"  ({i}) " + hand[i] + "\n";
                }
                return returnVal;
            }
        }

        public bool TryPlay(int cardIndex)
        {
            if (cardIndex > hand.Count) {
                return false;
            }
            disaster.ApplyLifelines(hand[cardIndex].resolves);
            hand.RemoveAt(cardIndex);
            return true;
        }

        public bool isComplete
        {
            get
            {
                return disaster.isStabilized;
            }
        }

        public void Draw(int nCards)
        {
            for (int i = 0; i < nCards; i++)
            {
                hand.Add(OpsCard.Random());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = NewGame();
            Console.Write(game.state);
            PrintOptions();

            int iterCount = 0;
            do
            {
                ReadInputAndAct(game);
                iterCount += 1;
            } while (!game.isComplete);
            string turnStr = iterCount > 1 ? "turns" : "turn";
            Console.WriteLine($"Congrats, you stabilized the disaster in {iterCount} {turnStr}!");
        }

        static Game NewGame()
        {
            Disaster disaster = Disaster.Random();

            int handCount = 5;
            List<OpsCard> hand = new List<OpsCard>();
            for (int i = 0; i < handCount; i++)
            {
                hand.Add(OpsCard.Random());
            }

            Game game = new Game(disaster, hand);
            return game;
        }

        

        static void ReadInputAndAct(Game game)
        {
            string input = Console.ReadLine();
            // play 2 ===> play card 2
            switch (input)
            {
                case string s when s.StartsWith("play"):
                    int cardIndex = 0;
                    if (Int32.TryParse(s.Split(" ")[1], out cardIndex))
                    {
                        // Check we have such a card.
                        if (game.TryPlay(cardIndex)) {
                            PrintState(game);
                        } else
                        {
                            Console.WriteLine("Sorry, that card doesn't exist or is invalid.");
                        }
                    } else
                    {
                        Console.WriteLine($"Sorry, can't get an int from {s.Split(" ")[1]}");
                    }
                    break;
                case string s when s.StartsWith("draw"):
                    int cardCount = 0;
                    if (Int32.TryParse(s.Split(" ")[1], out cardCount))
                    {
                        game.Draw(cardCount);
                    }
                    else
                    {
                        game.Draw(1);
                    }
                    PrintState(game);
                    break;
                case string s when s.StartsWith("state"):
                    PrintState(game);
                    break;
                case string s when s.StartsWith("options"):
                    PrintOptions();
                    break;
                default:
                    Console.WriteLine("Sorry, I don't understand what you're trying to do. Maybe try again?");
                    break;
            }
        }

        static void PrintState(Game game)
        {
            Console.Write(game.state);
        }

        static void PrintOptions()
        {
            // TODO: find a way to do this that doesn't keep a list of options
            // in two places
            Console.WriteLine("Turn options: ");
            Console.WriteLine("  play i : Play card number i");
            Console.WriteLine("  draw n : Draw n cards");
            Console.WriteLine("  state  : Show game state");
            Console.WriteLine("  options: Show turn options");
        }
    }
}
