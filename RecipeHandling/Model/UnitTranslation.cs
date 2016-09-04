using System;
using System.IO;
using System.Collections.ObjectModel;

namespace Jamie.Model
{

    public class UnitTranslationSet : ObservableCollection<UnitTranslation>
    {
        //Variables
        private static long _MaxID = 0;
        private const string FileExtension = ".tran";
        private static UnitSet _UnitSetData;

        //Constructors
        public UnitTranslationSet(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }

        //Properties
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }
        } //Readonly
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
            //set
            //{
            //    _UnitSetData = value;
            //}
        } //Readonly

        //Methods
        public void AddItem()
        {
            UnitTranslation NewUnitTranslation = new UnitTranslation();

            if (Count == 0)
            {
                NewUnitTranslation.SetDataReference(_UnitSetData);
            }
            AddItem(NewUnitTranslation);
        }
        public void AddItem(UnitTranslation ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }

            else
            {
               int ExistingIndex = IndexOf(ItemToBeAdded);
               Console.WriteLine("Die Unit Translation\n{0} ......ist bereits vorhanden als\n{1}", ItemToBeAdded, this[ExistingIndex]);
            }
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
        public UnitTranslationSet OpenSet(string FileName)
        {
            UnitTranslationSet ReturnSet = this;
            ReturnSet.Clear();
            FileName += FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnSet.GetType());
                ReturnSet = (UnitTranslationSet)x.Deserialize(fs);
            }
            return ReturnSet;

        }
        public void PopulateSetWithDefaults()
        {
            AddItem(new UnitTranslation("kg", "g", 1000.0, 0));
            AddItem(new UnitTranslation("g", "mg", 1000.0, 0));
            AddItem(new UnitTranslation("l", "ml", 1000.0, 0));
            AddItem(new UnitTranslation("oz", "g", 28.3495, 0));
            AddItem(new UnitTranslation("l", "kg", 1.0, 3));
        }
        public void SaveSet(string FileName)
        {
            FileName += FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

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

    /* UnitTranslation - Grundsätze
     * 
     * 1. Es sollte alle Umrechnungen geben vom Typ "Immer gültig"
     * 2. Je IngredientType (Flüssig, Fest, Pulver, Kräuter, ....)
     *    gibt es genau eine Default-Umrechnung.
     * 3. Wird eine Defaultumrechnung verwendet, dann wird diese für die Zutat protokolliert, 
     *    damit die fehlende Zutaten spezifische Umrechnung nachgetragen und verifiziert werden kann
     * 4. Für jede Zutat und Typenwechsel darf es nur einen "Von Zutat abhängiger UnitTypenWechsel" geben
     * 5. Für jede Zutat und Typenwechsel können unter Berücksichtigung der Einträge "Immer gültig"
     *    alle weiteren notwendigen Umrechnungsfaktoren berechnet werden. 
     *    
     * Beschreibung der Umrechnung für eine Zutat:
     * 1. Ermittle StartUnit aus IngredientItem.Unit
     * 2. Ermittle ZielUnit aus Ingredient.TargetUnit
     * 3. Vergleiche UnitType
     *    3a  UnitType gleich
     *    3a1 Ermittle UnitTranslation Fall 0 mit StartUnit und ZielUnit
     *    3a2 Falls vorhanden --> Rechne um
     *    3a3 Falls nicht vorhanden --> Meldung "Umrechnung fehlt."
     *    
     * 
     * 
     * TranslationIndependenceType Flags - Anwendungsfälle
     * 
     * Fall 0: - Immer gültig 
     *         - Wert = 0,  <kein Flag gesetzt>
     *         - Unabhängig von der Zutat, UnitTypen sind gleich
     *         - Ingredient muss gleich >null< sein 
     *         - IngredientType muss gleich >null< sein 
     *         - Umrechnung gilt immer: Beispiel 1kg --> 1000g
     *           
     * Fall 1: - Defaultumrechnung, wenn noch kein spez. Eintrag für die Zutat besteht
     *         - Wert = 1, IsTypeChange
     *         - UnitTypen sind verschieden        
     *         - Ingredient muss gleich >null< sein 
     *         - IngredientType muss ungleich >null< sein 
     *         - z.B. 1l --> 1kg
     * 
     * Fall 2: - Wert = 2: - wird nicht verwendet
     * 
     * Fall 3: - "Von Zutat abhängiger UnitTypenWechsel", wenn noch kein spez. Eintrag für die Zutat besteht
     *         - Wert = 3, IsTypeChange, IsRelatedToIngredient
     *         - UnitTypen sind verschieden
     *         - Abhängig von der Zutat
     *         - Zutat muss ungleich >null< sein 
     *         - IngredientType spielt keine Rolle, sollte aber mit dem IngredientType der Zutat übereinstimmen
     *         - z.B. 1TL Salz --> 9g
     */
    public class UnitTranslation:IEquatable<UnitTranslation>
    {
        [Flags]
        public enum TranslationIndependenceType
        { IsTypeChange = 0x1, IsRelatedToIngredient = 0x2}

        //Variables
        private long? _ID;
        private static UnitSet _UnitSetData;
        private string _BaseUnitSymbol;
        private string _TargetUnitSymbol;
        private double _TranslationFactor;
        private TranslationIndependenceType _IngredientDependent;

        //Constructors
        internal UnitTranslation()
        {
            IngredientDependent = (TranslationIndependenceType) 0;
        }
        internal UnitTranslation(bool ToBePopulated, UnitSet UnitSetData)
        {
            if (ToBePopulated) PopulateObject(UnitSetData);
        }
        internal UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, int IngredientDependent)
        {
            _BaseUnitSymbol = BaseUnitSymbol;
            _TargetUnitSymbol = TargetUnitSymbol;
            _TranslationFactor = TranslationFactor;
            _IngredientDependent = (TranslationIndependenceType) IngredientDependent;
        }

        // Properties
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
        public UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //ReadOnly

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
        public TranslationIndependenceType IngredientDependent
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
        public void PopulateObject(UnitSet UnitSetData)
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
        public void SetDataReference(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }
        public override string ToString()
        {
            return string.Format("{0,6}-UnitTranslation: {1,5} =  {2,10:F3} {3,-5} {4}", ID, BaseUnitSymbol, TranslationFactor, TargetUnitSymbol,
                                  IngredientDependent);
        }
        
    }

}