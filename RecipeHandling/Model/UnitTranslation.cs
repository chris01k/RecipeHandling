using System;
using System.Collections.ObjectModel;

namespace Jamie.Model
{
    public class UnitTranslationSet : ObservableCollection<UnitTranslation>
    {
        private static RecipeDataSets _Data;

        //Constructors
        public UnitTranslationSet(RecipeDataSets Data)
        {
            _Data = Data;
        }

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
        public void PopulateSetWithDefaults()
        {
            AddItem(new UnitTranslation("kg", "g", 1000.0, 0));
            AddItem(new UnitTranslation("g", "mg", 1000.0, 0));
            AddItem(new UnitTranslation("l", "ml", 1000.0, 0));
            AddItem(new UnitTranslation("oz", "g", 28.3495, 0));
            AddItem(new UnitTranslation("l", "kg", 1.0, 3));
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += "\nListe der Unit Umrechnungen:\n";
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (UnitTranslation ListItem in this)
                    ReturnString += ListItem.ToString() + "\n";
            }
            ReturnString += "\n";
            return ReturnString;
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }
    }

    public class UnitTranslation:IEquatable<UnitTranslation>
    {
        [Flags]
        public enum TranslationIndepedenceType
        {IsStandard = 0x0, IsDepedent = 0x1, IsDefault =0x2}

        private static long _MaxID;
        private long _ID;
        private string _BaseUnitSymbol;
        private string _TargetUnitSymbol;
        private double _TranslationFactor;
        private TranslationIndepedenceType _IngredientDependent;

        //Constructors
        internal UnitTranslation()
        {
            _ID = ++_MaxID;
            IngredientDependent = (TranslationIndepedenceType) 3;
        }
        internal UnitTranslation(bool ToBePopulated)
        {
            _ID = ++_MaxID;
            if (ToBePopulated) PopulateObject();
        }
        internal UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, int IngredientDependent)
        {
            _ID = ++_MaxID;
            _BaseUnitSymbol = BaseUnitSymbol;
            this.TargetUnitSymbol = TargetUnitSymbol;
            this.TranslationFactor = TranslationFactor;
            this.IngredientDependent = (TranslationIndepedenceType) IngredientDependent;
        }

        //Properties
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }

            set
            {
                _MaxID = value;
            }
        }
        public long ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }
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
        public bool Equals(UnitTranslation ItemToCompare)
        {
            return _ID.Equals(ItemToCompare._ID) | EqualKey(ItemToCompare);
        }       
        public bool EqualKey(UnitTranslation ItemToCompare)
        {
            return (BaseUnitSymbol.Equals(ItemToCompare.BaseUnitSymbol) &&
                    TargetUnitSymbol.Equals(ItemToCompare.TargetUnitSymbol));
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
            return string.Format("{0,6}-UnitTranslation: {1,5} =  {2,10:F3} {3,-5} {4}", ID, BaseUnitSymbol, TranslationFactor, TargetUnitSymbol,
                                  IngredientDependent);
        }
        
    }

}