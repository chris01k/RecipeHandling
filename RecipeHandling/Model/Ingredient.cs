
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
//using GalaSoft.MvvmLight;
using System.Text.RegularExpressions;


namespace Jamie.Model
{


    // Ein Rezept enthält eine Zutatenliste bestehend aus x Einträgen, wobei jeder Eintrag eine Zutat sowie die erforderliche Menge beschreibt.
    public class IngredientRecipeSet : ObservableCollection<IngredientItem>
    {
    }

    public class IngredientSet : ObservableCollection<Ingredient>
    {
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
        //public void ViewSet()
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("Liste der Ingredients:");
        //    if (Count == 0) Console.WriteLine("-------> leer <-------");
        //    else
        //    {
        //        foreach (Ingredient ListItem in this)
        //            Console.WriteLine(ListItem);
        //    }

        //}
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
        Ingredient _SpecificIngredient;
        private Unit _Unit;
        float? _Quantity;


        public IngredientItem()
        {
            _SpecificIngredient = new Ingredient();
        }

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
        string _Name;
        IngredientFlags _IngredientType;

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
            this.Name = Name;
            this.IngredientType = IngredientType;
        }

        //Properties
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

            for (int i = 1; i <= maxIngredientFlag; i=(i*2))
            {
                Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
                if (InputString.Length > 0) FlagValue = (FlagValue | (IngredientFlags)i);
            }
            IngredientType = FlagValue;

        }
        public override string ToString()
        {
            return String.Format("Name: {0,10}  Type: {1,5}", Name, IngredientType);
        }

    }
}
