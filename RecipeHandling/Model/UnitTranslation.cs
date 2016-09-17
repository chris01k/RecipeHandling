using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jamie.Model
{
    [Flags]
    public enum TranslationType
    { IsTypeChange = 0x1, IsIngredientDependent = 0x2 }


    /* UnitTranslation - Grundsätze
     * 
     * 1. Es sollte alle Umrechnungen geben vom Typ "Immer gültig"
     * 2. Je IngredientType (Flüssig, Fest, Pulver, Kräuter, ....)
     *    gibt es genau eine Default-Umrechnung für jede Kombination an UnitTypes
     * 3. Wird eine Defaultumrechnung verwendet, dann wird diese für die Zutat protokolliert, 
     *    damit die fehlende Zutaten spezifische Umrechnung nachgetragen und verifiziert werden kann
     * 4. Für jede Zutat und Typenwechsel darf es nur einen "Von Zutat abhängiger UnitTypenWechsel" geben
     * 5. Für jede Zutat und Typenwechsel können unter Berücksichtigung der Einträge "Immer gültig"
     *    alle weiteren notwendigen Umrechnungsfaktoren berechnet werden. 
     * 6. Synonyme bei Units können über UnitTranslations mit dem TranslationFactor 1,0 eingegeben werden.
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
     *         - Wert = 3, IsTypeChange, IsIngredientDependent
     *         - UnitTypen sind verschieden
     *         - Abhängig von der Zutat
     *         - Zutat muss ungleich >null< sein 
     *         - IngredientType spielt keine Rolle, sollte aber mit dem IngredientType der Zutat übereinstimmen
     *         - z.B. 1TL Salz --> 9g
     */
    public class UnitTranslation:IEquatable<UnitTranslation>
    {
        //static Variables
        private static UnitSet _UnitSetData;
        private static IngredientSet _IngredientSetData;

        //Variables
        private long? _ID;
        private Ingredient _AffectedIngredient;
        private Unit _BaseUnit;
        private IngredientType _IngredientType;
        private Unit _TargetUnit;
        private double _TranslationFactor;
        private TranslationType _TranslationFlag;

        //Constructors
        public UnitTranslation()
        {
            TranslationFlag = (TranslationType) 0;
        }
        public UnitTranslation(bool ToBePopulated, UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
            if (ToBePopulated) PopulateObject(UnitSetData, IngredientSetData);
        }
        public UnitTranslation(Unit Base, Unit Target, double TranslationFactor, UnitTranslation Template)
        {
            // auf der Basis einer Vorlage (Ingredient und TranslationFlag werden übernommen),

            _BaseUnit = Base;
            _TargetUnit = Target;
            _TranslationFactor = TranslationFactor;
            _AffectedIngredient = Template.AffectedIngredient;
            _TranslationFlag = Template.TranslationFlag;

        }
        public UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, int IngredientDependent, UnitSet UnitSetData)
        {
            _BaseUnit = UnitSetData.SelectItem(BaseUnitSymbol);
            _TargetUnit = UnitSetData.SelectItem(TargetUnitSymbol);
            _TranslationFactor = TranslationFactor;
            _TranslationFlag = (TranslationType) IngredientDependent;
            _UnitSetData = UnitSetData;
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
        public Ingredient AffectedIngredient
        {
            get
            {
                return _AffectedIngredient;
            }

            set
            {
                _AffectedIngredient = value;
            }
        }
        public Unit BaseUnit
        {
            get { return _BaseUnit; }
            set { _BaseUnit = value; }
        }
        public IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }
        } //ReadOnly
        public IngredientType IngredientType
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
        public Unit TargetUnit
        {
            get { return _TargetUnit; }
            set { _TargetUnit = value; }
        }
        public double TranslationFactor
        {
            get { return _TranslationFactor; }
            set { _TranslationFactor = value; }
        }
        public TranslationType TranslationFlag
        {
            get { return _TranslationFlag; }
            set { _TranslationFlag = value; }
        }
        public UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //ReadOnly

        //Methods
        public bool Equals(UnitTranslation ItemToCompare)
        {
            return _ID.Equals(ItemToCompare._ID) | EqualKey(ItemToCompare);
        }       
        public bool EqualKey(UnitTranslation ItemToCompare)
        {
            return (BaseUnit.Equals(ItemToCompare.BaseUnit) && TargetUnit.Equals(ItemToCompare.TargetUnit)) ||
                    (BaseUnit.Equals(ItemToCompare.TargetUnit) && TargetUnit.Equals(ItemToCompare.BaseUnit)) &&
                    IngredientType.Equals(ItemToCompare.IngredientType) && 
                    AffectedIngredient.Equals(ItemToCompare.AffectedIngredient);
        }
        public UnitTranslation Inverse()
        {
            UnitTranslation ReturnItem;

            ReturnItem = new Model.UnitTranslation();

            ReturnItem.ID = 0;
            ReturnItem.BaseUnit = this.TargetUnit;
            ReturnItem.TargetUnit = this.BaseUnit;
            ReturnItem.TranslationFactor = (1 / this.TranslationFactor);

            return ReturnItem;
        }
        public void PopulateObject(UnitSet UnitSetData, IngredientSet IngredientSetData)
        {
            string InputString;
            double ParsedDoubleValue;
            Ingredient ProcessedIngredient;
            Unit ProcessedUnit;

            
            Console.WriteLine();
            Console.WriteLine("Eingabe neue Einheit:" );
            Console.WriteLine("---------------------");
            Console.WriteLine();
            UnitSetData.ViewSet();
            do
            {
                Console.Write("BaseUnitSymbol   : "); ProcessedUnit = UnitSetData.SelectItem(Console.ReadLine());
            } while (ProcessedUnit==null);
            BaseUnit  = ProcessedUnit;
            do
            {
                Console.Write("TargetUnitSymbol   : "); ProcessedUnit = UnitSetData.SelectItem(Console.ReadLine());
            } while (ProcessedUnit == null);
            TargetUnit = ProcessedUnit;

            do
            {
                Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
            } while (!double.TryParse(InputString, out ParsedDoubleValue));
            TranslationFactor = ParsedDoubleValue;

            if (BaseUnit.Type != TargetUnit.Type)
            {
                TranslationFlag |= TranslationType.IsTypeChange;
                Console.Write("IsIngredientDependent:"); InputString = Console.ReadLine();
                if (InputString.Length > 0) //Flag setzen, Ingredient eingeben
                {
                    TranslationFlag |= TranslationType.IsIngredientDependent;
                    do
                    {
                        Console.Write("  Affeced Ingredient  : "); ProcessedIngredient = IngredientSetData.SelectItem(Console.ReadLine());
                    } while (ProcessedIngredient == null);
                    AffectedIngredient = ProcessedIngredient;
                }
                else //IngredientType eingeben
                {
                    do
                    {
                        Console.Write("Ingredient Type  : "); InputString = Console.ReadLine();
                        try
                        {
                            IngredientType = (IngredientType)Enum.Parse(typeof(IngredientType), InputString);
                        }
                        catch
                        {
                            continue;
                        }
                        break;
                    } while (true);

                }

            }
        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
        }
        public override string ToString()
        {
            return string.Format("{0,6}-UnitTranslation: {1,5} =  {2,15:F6} {3,-5} {4}", ID, BaseUnit, TranslationFactor, TargetUnit,
                                  (TranslationFlag==0? "NoTypeChange, IngredientIndepedant" : string.Format("{0}",TranslationFlag)));
        }
        
    }

    public class UnitTranslationSet : ObservableCollection<UnitTranslation>
    {
        //static Variables
        private static IngredientSet _IngredientSetData;
        private static UnitTranslation _SelectedItem;
        private static UnitSet _UnitSetData;

        //Variables
        private const string FileExtension = ".tran";
        private static long _MaxID = 0;


        //Constructors
        public UnitTranslationSet(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }

        //Properties
        public static IngredientSet IngredientSetData //Readonly
        {
            get
            {
                return _IngredientSetData;
            }

        }
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }
        } //Readonly
        public static UnitTranslation SelectedItem
        {
            get
            {
                return _SelectedItem;
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
            NewUnitTranslation.PopulateObject(UnitSetData, IngredientSetData);
                                   
            if (Count == 0)
            {
                NewUnitTranslation.SetDataReference(IngredientSetData ,UnitSetData);
            }
            AddItem(NewUnitTranslation);
        }
        public void AddItem(UnitTranslation ItemToBeAdded)
        {
            //ItemToBeAdded und der Kehrwert nicht vorhanden
            if (!Contains(ItemToBeAdded))
            {


                if ((ItemToBeAdded.TranslationFlag & TranslationType.IsIngredientDependent)==TranslationType.IsIngredientDependent)
                // von Zutat ABHÄNGIG   

                {
                    if ((ItemToBeAdded.TranslationFlag & TranslationType.IsTypeChange) == TranslationType.IsTypeChange)
                    // Fall 3: von Zutat ABHÄNGIG - MIT Wechsel des UnitTyps
                    {

                    }
                    else
                    // Fall 2: von Zutat ABHÄNGIG - OHNE Wechsel des UnitTyps
                    {
                        //trow Exception --> Fall 2 kommt nicht vor
                    }
                                        
                }

                else
                // von Zutat UNABHÄNGIG
                {
                    if ((ItemToBeAdded.TranslationFlag & TranslationType.IsTypeChange) == TranslationType.IsTypeChange)
                    // Fall 1: von Zutat UNABHÄNGIG - MIT Wechsel des UnitTyps
                    {
                        ItemToBeAdded.ID = ++_MaxID;
                        Add(ItemToBeAdded);
                    }
                    else
                    // Fall 0: von Zutat UNABHÄNGIG - OHNE Wechsel des UnitTyps
                    {
                        ItemToBeAdded.ID = ++_MaxID;
                        Add(ItemToBeAdded);
                        AddAllItemsWithSameType(ItemToBeAdded);
                        _SelectedItem = ItemToBeAdded;
                    }

                }
            }

            else
            {
                int ExistingIndex = IndexOf(ItemToBeAdded);
                Console.WriteLine("Die Unit Translation\n{0} ......ist bereits vorhanden als\n{1}", ItemToBeAdded, this[ExistingIndex]);
            }
        }
        public void AddAllItemsWithSameType(UnitTranslation ItemToBeAdded)
        {
            UnitTranslation GeneratedUnitTranslation;
            UnitTranslation HelpingUnitTranslation;
            
            var ImplicitTranslations = (from u in UnitSetData
                                        where (u.Type == ItemToBeAdded.BaseUnit.Type) && (u != ItemToBeAdded.BaseUnit)
                                        select new {  Base = (Unit) ItemToBeAdded.BaseUnit, Target = (Unit) u }).ToList();

            ImplicitTranslations.AddRange((from u in UnitSetData
                                        where (u.Type == ItemToBeAdded.TargetUnit.Type) && (u != ItemToBeAdded.TargetUnit)
                                        select new { Base = (Unit) ItemToBeAdded.TargetUnit, Target = (Unit) u }).ToList());

            foreach (var i in ImplicitTranslations)
            {
                HelpingUnitTranslation = GetTranslation(i.Base, i.Target);
                if (HelpingUnitTranslation == null)
                {
                    if (ItemToBeAdded.BaseUnit.Equals(i.Base))
                    {
                       HelpingUnitTranslation = GetTranslation(ItemToBeAdded.TargetUnit, i.Target);
                        if (HelpingUnitTranslation != null)
                        {
                            GeneratedUnitTranslation = new UnitTranslation(ItemToBeAdded.BaseUnit, i.Target,
                                                              ItemToBeAdded.TranslationFactor * HelpingUnitTranslation.TranslationFactor,
                                                              ItemToBeAdded);
                            GeneratedUnitTranslation.ID = ++_MaxID;
                            Add(GeneratedUnitTranslation);
                        }
                    }
                    else  //ItemToBeAdded.TargetUnit.Equals(i.Base)
                    {

                        /*ItemToBeAdded oz -> dr
                          i (Missing)   dr -> kg
                                        dr -> oz -> kg
                                        Inverse(oz -> dr) -> (oz -> kg)
                                        ItemToBeAdded.Inverse() * Helping
                                        ItemToBeAdded.Inverse() * Get (oz -> kg)
                                        ItemToBeAdded.Inverse() * Get (ItemToBeAdded.Base -> i.Target)
                          Generated     Inverse(ItemToBeAdded) * ItemToBeAdded.Base
                                        Inverse(ItemToBeAdded) * Helping
                          Helping       ItemToBeAdded.Base -> i.Base
                        */
                        HelpingUnitTranslation = GetTranslation(ItemToBeAdded.BaseUnit, i.Target);
                        if (HelpingUnitTranslation != null)
                        {
                            GeneratedUnitTranslation = new UnitTranslation(ItemToBeAdded.TargetUnit, i.Target,
                                                           (1 / ItemToBeAdded.TranslationFactor) * HelpingUnitTranslation.TranslationFactor,
                                                           ItemToBeAdded);
                            GeneratedUnitTranslation.ID = ++_MaxID;
                            Add(GeneratedUnitTranslation);
                        }

                    }
                }


            }
      
        }
        public int CountSimilarItems(UnitTranslation ItemToCompare)
        {
            int ReturnValue = 0;

            var SimilarItems = (from u in this
                                where ((ItemToCompare.TranslationFlag == u.TranslationFlag) &&
                                       (ItemToCompare.BaseUnit.Type == u.BaseUnit.Type) &&
                                       (ItemToCompare.TargetUnit.Type == u.TargetUnit.Type) &&
                                       (ItemToCompare.AffectedIngredient)==u.AffectedIngredient) select u).ToList();
            ReturnValue += SimilarItems.Count;
            SimilarItems = (from u in this
                                where ((ItemToCompare.TranslationFlag == u.TranslationFlag) &&
                                       (ItemToCompare.TargetUnit.Type == u.BaseUnit.Type) &&
                                       (ItemToCompare.BaseUnit.Type == u.TargetUnit.Type) &&
                                       (ItemToCompare.AffectedIngredient) == u.AffectedIngredient)
                                select u).ToList();
            ReturnValue += SimilarItems.Count;
            return ReturnValue;
        }
        public void DeleteSelectedItem()
        {
            int NewSelectedIndex;

            if ((Count == 0) || (SelectedItem == null)) return;

            if (Count > 1)
            {
                NewSelectedIndex = IndexOf(SelectedItem) - 1;
                if (NewSelectedIndex < 0) NewSelectedIndex = 0;
            }
            else NewSelectedIndex = 1;

            Remove(SelectedItem);

            if (Count > 0) _SelectedItem = this[NewSelectedIndex];
            else _SelectedItem = null;
        }
        public void EvaluateMaxID()
        {
            //            var maxIDFromFile = (from s in this select s.ID).Max();

            var maxIDFromFile = this
                                .Select(s => s.ID).Max();

            if (maxIDFromFile == null) _MaxID = 0;
            else _MaxID = (long)maxIDFromFile;
        }
        public UnitTranslation GetTranslation (Unit Base, Unit Target)
        {
            UnitTranslation ReturnObject = null;

            foreach (UnitTranslation ListItem in this)
            {
                if (ListItem.BaseUnit.Equals(Base) && (ListItem.TargetUnit.Equals(Target))) 
                {
                    ReturnObject = ListItem;
                    break;
                }

                if (ListItem.BaseUnit.Equals(Target) && (ListItem.TargetUnit.Equals(Base)))
                {
                    ReturnObject = ListItem.Inverse();
                    break;
                }

            }
            return ReturnObject;
        }
        public void Menu()
        {
            int HowManyItemsInSet = Count;

            if (HowManyItemsInSet>0) _SelectedItem = this[HowManyItemsInSet - 1];
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nUnit Translation Menü");
                Console.WriteLine("----------------------");
                Console.WriteLine("Selected Ingredient {0}\n", _SelectedItem);
                Console.WriteLine();
                Console.WriteLine("A  Add Unit Translation");
                Console.WriteLine("D  Delete Unit Translation");
                Console.WriteLine("E  Edit Unit Translation");
                Console.WriteLine("S  Select Unit Translation");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        AddItem();
                        break;
                    case "D":
                        DeleteSelectedItem();
                        break;
                    case "E":
                        //EditSelectedItem();
                        break;
                    case "S":
                        SelectItem();
                        break;
                    case "V":
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
            Clear();
            AddItem(new UnitTranslation("kg", "g", 1000.0, 0, UnitSetData));
            AddItem(new UnitTranslation("g", "mg", 1000.0, 0, UnitSetData));
            AddItem(new UnitTranslation("l", "ml", 1000.0, 0, UnitSetData));
            AddItem(new UnitTranslation("oz", "g", 28.3495, 0, UnitSetData));
            AddItem(new UnitTranslation("l", "kg", 1.0, 3, UnitSetData));
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
        public void SelectItem()
        {
            string InputString;
            int ParsedIntValue;
            int SelectedID;

            do
            {
                Console.Write("Unit Translation ID:"); InputString = Console.ReadLine();
            } while (!int.TryParse(InputString, out ParsedIntValue));
            SelectedID = ParsedIntValue;

            foreach (UnitTranslation ListItem in this)
            {
                if (ListItem.ID == SelectedID)
                {
                    _SelectedItem = ListItem;
                    break;
                }
            }

        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
        }
        public bool TypeTranslationExists(UnitType BaseType, UnitType TargetType)
        {
            bool ReturnFlag = false;

            if (BaseType != TargetType)
            {
                foreach (UnitTranslation Item in this)
                {
                    if ((BaseType == Item.BaseUnit.Type && TargetType == Item.TargetUnit.Type)
                        || (BaseType == Item.TargetUnit.Type && TargetType == Item.BaseUnit.Type)) 
                    {
                        ReturnFlag = true;
                        break;
                    }
                }
            }
            return ReturnFlag;
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += string.Format("\nListe der Unit Umrechnungen: MaxID {0}\n",_MaxID);
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

}