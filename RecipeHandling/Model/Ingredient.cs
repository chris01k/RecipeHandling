
using System;
using System.Collections.ObjectModel;



namespace Jamie.Model
{
    // Ein Rezept enthält eine Zutatenliste bestehend aus x Einträgen, wobei jeder Eintrag eine Zutat sowie die erforderliche Menge beschreibt.
    public class IngredientRecipeSet : ObservableCollection<IngredientItem>
    {
        private static RecipeDataSets _Data;

        //Constructors
        public IngredientRecipeSet(RecipeDataSets Data)
        {
            _Data = Data;
        }

        //Methods
        public void SetDataReference(RecipeDataSets Data)
        {
            _Data = Data;
        }
    }

    public class IngredientSet : ObservableCollection<Ingredient>
    {
        private static RecipeDataSets _Data;

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
        public void AddItem(Ingredient IngredientToBeAdded)
        {
            if (!Contains(IngredientToBeAdded)) Add(IngredientToBeAdded);
            else Console.WriteLine("Die Zutat ist bereits vorhanden: \n {0}", IngredientToBeAdded);
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nIngredient Menü");
                Console.WriteLine("----------------------");
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
            Ingredient.IngredientFlags FlagsTobeSet;

            FlagsTobeSet = 0;
            FlagsTobeSet |= Ingredient.IngredientFlags.IsVegan;
            AddItem(new Ingredient("Zwiebeln", Ingredient.IngredientFlags.IsVegetarian
                                             | Ingredient.IngredientFlags.IsVegan
                                             | Ingredient.IngredientFlags.IsLowCarb
                                             | Ingredient.IngredientFlags.IsLowFat));
            AddItem(new Ingredient("Tomaten", Ingredient.IngredientFlags.IsVegetarian
                                             | Ingredient.IngredientFlags.IsVegan
                                             | Ingredient.IngredientFlags.IsLowCarb
                                             | Ingredient.IngredientFlags.IsLowFat));
            AddItem(new Ingredient("Rinderfilet", Ingredient.IngredientFlags.IsLowCarb));
            AddItem(new Ingredient("Quinoa", Ingredient.IngredientFlags.IsVegetarian
                                             | Ingredient.IngredientFlags.IsVegan
                                             | Ingredient.IngredientFlags.IsLowFat));
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


    public class IngredientItem  //: ObservableObject
    {
        //Constructors
        public IngredientItem()
        {
            _SpecificIngredient = new Ingredient();
        }
        public IngredientItem(bool ToBePopulated)
        {
            _SpecificIngredient = new Ingredient();
            if (ToBePopulated) PopulateObject();
        }


        //Variables
        private float? _Quantity;
        private Ingredient _SpecificIngredient;
        private Unit _Unit;

        //Properties
        public Ingredient SpecificIngredient
        {
            get { return _SpecificIngredient; }
            set
            {
                if (_SpecificIngredient == value)
                    return;

                _SpecificIngredient = value;
//                RaisePropertyChanged(() => SpecificIngredient);
            }
        }
        public string Name
        {
            get { return _SpecificIngredient != null ? _SpecificIngredient.Name : ""; }
            set
            {
                if (_SpecificIngredient.Name == value)
                    return;

                _SpecificIngredient.Name = value;
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

    /* Eine Zutat beschreibt ein Produkt, welches in einem Rezept verarbeitet werden kann. Zutaten werden im Gegensatz zu Werkzeugen verbraucht. 
     * Hat Eigenschaften: x kcal/100g, Ernährungsampel (rot, gelb, grün)
     * länderspezifische Zuordnung?
     */

    public class Ingredient: IEquatable<Ingredient> //: ObservableObject
    {
        [Flags]
        public enum IngredientFlags:int
        { IsVegetarian = 1, IsVegan = 2, IsLowCarb = 4, IsLowFat = 8 }

        //Constants
        const byte maxIngredientFlag = 15;


        //Variables
        private static long _MaxID;
        private long _ID;
        private string _Name;
        private IngredientFlags _IngredientType;

        //Constructors
        internal Ingredient()
        {
            _ID = ++_MaxID;
        }
        internal Ingredient(bool ToBePopulated)
        {
            _ID = ++_MaxID;
            if (ToBePopulated) PopulateObject();
        }
        internal Ingredient(string Name, IngredientFlags IngredientType)
        {
            _ID = ++_MaxID;
            this.Name = Name;
            this.IngredientType = IngredientType;
        }

        //Properties
        public long ID
        {
            get
            {
                return _ID;
            }
        }
        public long MaxID
        {
            get
            {
                return _MaxID;
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
}
