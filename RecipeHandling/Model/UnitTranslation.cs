using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Jamie.Model
{
    public class UnitTranslationSet : ObservableCollection<UnitTranslation>
    {
        //Methods
        public void AddItem()
        {
            AddItem (new UnitTranslation(true));
        }
        public void AddItem(UnitTranslation UnitTranslationToBeAdded)
        {
            if (!Contains(UnitTranslationToBeAdded)) Add(UnitTranslationToBeAdded);
            else Console.WriteLine("Die Unit Translation ist bereits vorhanden: \n {0}", UnitTranslationToBeAdded);
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nUnit Translation Menü");
                Console.WriteLine("----------------------");
                Console.WriteLine("A  Add Unit Translation");
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
        public void ViewSet()
        {
            Console.WriteLine();
            Console.WriteLine("Liste der Translations:");
            if (Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                foreach (UnitTranslation ListItem in this)
                    Console.WriteLine(ListItem);
            }

        }
    }

    public class UnitTranslation:IEquatable<UnitTranslation>
    {
        [Flags]
        public enum TranslationIndepedenceType
        {IsStandard = 0x0, IsDepedent = 0x1, IsDefault =0x2}

        private string _BaseUnitSymbol;
        private string _TargetUnitSymbol;
        private double _TranslationFactor;
        private TranslationIndepedenceType _IngredientDependent;

        //Constructors
        internal UnitTranslation()
        {
            IngredientDependent = (TranslationIndepedenceType) 3;
        }
        internal UnitTranslation(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateObject();
        }
        internal UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, int IngredientDependent)
        {
            this.BaseUnitSymbol = BaseUnitSymbol;
            this.TargetUnitSymbol = TargetUnitSymbol;
            this.TranslationFactor = TranslationFactor;
            this.IngredientDependent = (TranslationIndepedenceType) IngredientDependent;
        }

        //Properties
        public string BaseUnitSymbol
        {
            get { return _BaseUnitSymbol; }
            set { _BaseUnitSymbol = value; }
        }
        public string TargetUnitSymbol
        {
            get { return _TargetUnitSymbol; }
            set { _TargetUnitSymbol = value; }
        }
        public double TranslationFactor
        {
            get { return _TranslationFactor; }
            set { _TranslationFactor = value; }
        }
        public TranslationIndepedenceType IngredientDependent
        {
            get { return _IngredientDependent; }
            set { _IngredientDependent = value; }
        }

        //Methods
        public bool Equals(UnitTranslation UnitTranslationToCompare)
        {
            return (BaseUnitSymbol.Equals(UnitTranslationToCompare.BaseUnitSymbol) &&
                    TargetUnitSymbol.Equals(UnitTranslationToCompare.TargetUnitSymbol));
        }       
        public void PopulateObject()
        {
            string InputString;
            double ParsedDoubleValue;

            Console.WriteLine();
            Console.WriteLine("Eingabe neue Einheit:" );
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("BaseUnitSymbol   : "); BaseUnitSymbol = Console.ReadLine();
            Console.Write("TargetUnitSymbol : "); TargetUnitSymbol = Console.ReadLine();
                do
                {
                    Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
                } while (!double.TryParse(InputString, out ParsedDoubleValue));
                TranslationFactor = ParsedDoubleValue;

        }       
        public override string ToString()
        {
            return String.Format("UnitTranslation: {0,5} =  {1,10:F3} {2,-5} {3}", BaseUnitSymbol, TranslationFactor, TargetUnitSymbol,
                                  IngredientDependent);
        }
        
    }

}