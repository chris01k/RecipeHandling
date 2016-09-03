
using System;
using System.Collections.ObjectModel;



namespace Jamie.Model
{   
    [Flags]

    //IngredientFlags im namespace kann von mehreren Klassen verwendet werden (z.B. Ingredient, Recipe)
     public enum IngredientFlags : int
    { IsVegetarian = 1, IsVegan = 2, IsLowCarb = 4, IsLowFat = 8 }

    /* Eine Zutat beschreibt ein Produkt, welches in einem Rezept verarbeitet werden kann. Zutaten werden im Gegensatz zu Werkzeugen verbraucht. 
     * Hat Eigenschaften: x kcal/100g, Ernährungsampel (rot, gelb, grün)
     * länderspezifische Zuordnung?
     */
    public class Ingredient : IEquatable<Ingredient> //: ObservableObject
    {

        //Constants
        const byte maxIngredientFlag = 15;

        //Variables
        private long? _ID;
        private string _Name;
        private IngredientFlags _IngredientType;

        //Constructors
        internal Ingredient()
        {
        }
        internal Ingredient(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateObject();
        }
        internal Ingredient(string Name, IngredientFlags IngredientType)
        {
            _Name = Name;
            _IngredientType = IngredientType;
        }

        //Properties
        public long? ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID == null) _ID = value;
                //                else throw exception;
            }
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value)
                    return;

                _Name = value;
                //                RaisePropertyChanged(() => Name);
            }
        }
        public IngredientFlags IngredientType
        {
            get
            {
                return _IngredientType;
            }

            set
            {
                _IngredientType = value;
            }
        }


        //Methods
        public bool Equals(Ingredient ItemToCompare)
        {
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(Ingredient ItemToCompare)
        {
            return Name.ToUpper().Equals(ItemToCompare.Name.ToUpper());
        }
        public void PopulateObject()
        {
            IngredientFlags FlagValue;
            string InputString;

            FlagValue = 0;

            Console.WriteLine("Eingabe neue Zutat:");
            Console.WriteLine("-------------------");
            Console.WriteLine();
            Console.Write("         Name  : "); Name = Console.ReadLine();

            for (int i = 1; i <= maxIngredientFlag; i = (i * 2))
            {
                Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
                if (InputString.Length > 0) FlagValue = (FlagValue | (IngredientFlags)i);
            }
            IngredientType = FlagValue;

        }
        public override string ToString()
        {
            return string.Format("{0,6}-Name: {1,15}  Type: {2}", ID, Name, IngredientType);
        }

    }

    public class IngredientSet : ObservableCollection<Ingredient>
    {


        private static RecipeDataSets _Data;
        private static long _MaxID = 0;

        //Constructors
        public IngredientSet(RecipeDataSets Data)
        {
            _Data = Data;
        }

        //Methods
        public void AddItem()
        {
            AddItem(new Ingredient(true));
        }
        public void AddItem(Ingredient ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }
            else Console.WriteLine("Die Zutat ist bereits vorhanden: \n {0}", ItemToBeAdded);
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nREcipe-Ingredient Menü");
                Console.WriteLine("------------------------");
                Console.WriteLine("A  Add Ingredient");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        ViewSet();
                        AddItem();
                        ViewSet();
                        break;
                    case "V":
                        ViewSet();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }

            }
        }
        public void PopulateSetWithDefaults()
        {
            IngredientFlags FlagsTobeSet;

            FlagsTobeSet = 0;
            FlagsTobeSet |= IngredientFlags.IsVegan;
            AddItem(new Ingredient("Zwiebeln", IngredientFlags.IsVegetarian
                                             | IngredientFlags.IsVegan
                                             | IngredientFlags.IsLowCarb
                                             | IngredientFlags.IsLowFat));
            AddItem(new Ingredient("Tomaten",  IngredientFlags.IsVegetarian
                                             | IngredientFlags.IsVegan
                                             | IngredientFlags.IsLowCarb
                                             | IngredientFlags.IsLowFat));
            AddItem(new Ingredient("Rinderfilet", IngredientFlags.IsLowCarb));
            AddItem(new Ingredient("Quinoa", IngredientFlags.IsVegetarian
                                           | IngredientFlags.IsVegan
                                           | IngredientFlags.IsLowFat));
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += "\nListe der Zutaten:\n";
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (Ingredient ListItem in this)
                    ReturnString += ListItem.ToString() + "\n";
            }
            ReturnString += "\n";
            return ReturnString;
        }
    }

    /* IngredientItem ist ein (Quanity, Unit, Ingredient) Triple, welches einem Rezept zugeordnet ist
     */
    public class IngredientItem  //: ObservableObject
    {
        //Variables
        private long? _ID;

        private float? _Quantity;
        private Ingredient _Ingredient;
        private Unit _Unit;


        //Constructors
        public IngredientItem()
        {
            _Ingredient = new Ingredient();
        }
        public IngredientItem(bool ToBePopulated)
        {
            _Ingredient = new Ingredient();
            if (ToBePopulated) PopulateObject();
        }

        //Properties
        public long? ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID == null) _ID = value;
                //                else throw exception;
            }
        }
        public Ingredient Ingredient
        {
            get { return _Ingredient; }
            set
            {
                if (_Ingredient == value)
                    return;

                _Ingredient = value;
//                RaisePropertyChanged(() => SpecificIngredient);
            }
        }
        public string Name
        {
            get { return _Ingredient != null ? _Ingredient.Name : ""; }
            set
            {
                if (_Ingredient.Name == value)
                    return;

                _Ingredient.Name = value;
                //RaisePropertyChanged(() => Name);
            }
        }
        public Unit Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit == value)
                    return;

                _Unit = value;
//                RaisePropertyChanged(() => Unit);

            }
        }
        public float? Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity == value)
                    return;

                _Quantity = value;
//                RaisePropertyChanged(() => Quantity);

            }
        }


        //Methods
        public void PopulateObject()
        {
            string InputString;
            float ParsedDoubleValue;

            Console.WriteLine("Eingabe neue Zutat zum Rezept:");
            Console.WriteLine("------------------------------");
            Console.WriteLine();
            do
            {
                Console.Write("Menge (Quantity): "); InputString = Console.ReadLine();
            } while (!float.TryParse(InputString, out ParsedDoubleValue));
            Quantity = ParsedDoubleValue;
            Console.Write("Unit: "); InputString = Console.ReadLine();


            Console.Write("         Name  : "); Name = Console.ReadLine();



            //for (int i = 1; i <= maxIngredientFlag; i = (i * 2))
            //{
            //    Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
            //    if (InputString.Length > 0) FlagValue = (FlagValue | (IngredientFlags)i);
            //}
            //IngredientType = FlagValue;

        }

    }

    /* IngredientRecipeSet ist eine Liste IngredientItems (Menge, Ingredient):
 * Ein Rezept enthält eine Zutatenliste bestehend aus x Einträgen, wobei jeder Eintrag eine Zutat sowie die 
 * erforderliche Menge beschreibt.
 */
    public class IngredientItemSet : ObservableCollection<IngredientItem>
    {
        //Variables
        private static UnitSet _UnitSetData;
        private static IngredientSet _IngredientSetData;
        private static Recipe _RelatedRecipe;
        private static long _MaxID;

        //Constructors
        public IngredientItemSet(UnitSet UnitSetData, IngredientSet IngredientSetData, Recipe RelatedRecipe)
        {
            _UnitSetData = UnitSetData;
            _IngredientSetData = IngredientSetData;
            _RelatedRecipe = RelatedRecipe;
        }

        //Properties
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }
        }
        public static Recipe RelatedRecipe
        {
            get
            {
                return _RelatedRecipe;
            }
        }

        //Methods
        public void AddItem()
        {
            AddItem(new IngredientItem(true));
        }
        public void AddItem(IngredientItem ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }
            else Console.WriteLine("Die Zutat ist bereits vorhanden: \n {0}", ItemToBeAdded);
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nRezept-Zutaten: {0}",RelatedRecipe);
                Console.WriteLine("-----------------");
                Console.WriteLine("A  Add Ingredient");
                Console.WriteLine("V  View Set");
                Console.WriteLine("-----------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        ViewSet();
                        AddItem();
                        ViewSet();
                        break;
                    case "V":
                        ViewSet();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }

            }
        }
        public void SetDataReference(UnitSet UnitSetData, IngredientSet IngredientSetData)
        {
            _UnitSetData = UnitSetData;
            _IngredientSetData = IngredientSetData;
        }
        public override string ToString()
        {
            string ReturnString = string.Format("{0,8} Zutaten im Rezept: {1}", MaxID, RelatedRecipe);

            if (Count == 0) ReturnString += "         -------> leer <-------\n         ";
            else
            {
                foreach (IngredientItem ListItem in this)
                    ReturnString += ListItem.ToString() + "\n      ";
            }
            ReturnString += "\n";
            return ReturnString;
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }

    }

}
