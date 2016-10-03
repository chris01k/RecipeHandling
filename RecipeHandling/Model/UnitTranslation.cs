using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;


namespace Jamie.Model
{
    [Flags] public enum TranslationType
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
        private ListEntryStatus _TranslationStatus;

        //Constructors
        public UnitTranslation()
        {
            TranslationFlag = (TranslationType) 0;
            TranslationStatus = ListEntryStatus.IsOK;
        }
        public UnitTranslation(Unit Base, Unit Target, double TranslationFactor, UnitTranslation Template)
        {
            // auf der Basis einer Vorlage (Ingredient und TranslationFlag werden übernommen),

            _BaseUnit = Base;
            _TargetUnit = Target;
            _TranslationFactor = TranslationFactor;
            _AffectedIngredient = Template.AffectedIngredient;
            _TranslationFlag = Template.TranslationFlag;
            _TranslationStatus = ListEntryStatus.IsOK;

        }
        public UnitTranslation(Unit Base, Unit Target, double TranslationFactor, Ingredient AffectedIngredient, 
                               ListEntryStatus Status)
        {
                _BaseUnit = Base;
                _TargetUnit = Target;
                _TranslationFactor = TranslationFactor;
                _AffectedIngredient = AffectedIngredient;
                _TranslationFlag = (TranslationType)0;
                if (AffectedIngredient != null) _TranslationFlag = TranslationType.IsIngredientDependent;
                if (Base.Type != Target.Type) _TranslationFlag |= TranslationType.IsTypeChange;
                _TranslationStatus = Status;
        }
        public UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, TranslationType TType, IngredientType IType, UnitSet UnitSetData)
        {
            _BaseUnit = UnitSetData.SelectItem(BaseUnitSymbol);
            _IngredientType = IType;
            _TargetUnit = UnitSetData.SelectItem(TargetUnitSymbol);
            _TranslationFactor = TranslationFactor;
            _TranslationFlag = TType;
            _TranslationStatus = ListEntryStatus.IsOK;
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
        public ListEntryStatus TranslationStatus
        {
            get
            {
                return _TranslationStatus;
            }

            set
            {
                _TranslationStatus = value;
            }
        }

        //Methods
        public bool Equals(UnitTranslation ItemToCompare)
        {
            if (ItemToCompare == null) return false;
            return _ID.Equals(ItemToCompare._ID) | EqualKey(ItemToCompare);
        }       
        public bool EqualKey(UnitTranslation ItemToCompare)
        {
            bool ReturnValue;

            
            ReturnValue = TranslationFlag.Equals(ItemToCompare.TranslationFlag);
            if (TranslationFlag == (TranslationType) 0)
            {
                ReturnValue &= (BaseUnit.Equals(ItemToCompare.BaseUnit) && TargetUnit.Equals(ItemToCompare.TargetUnit)) ||
                               (BaseUnit.Equals(ItemToCompare.TargetUnit) && TargetUnit.Equals(ItemToCompare.BaseUnit));
            }

            if ((AffectedIngredient != null) && TranslationFlag.HasFlag(TranslationType.IsIngredientDependent))
            {
                ReturnValue &= AffectedIngredient.Equals(ItemToCompare.AffectedIngredient);
            }

            if (TranslationFlag.HasFlag(TranslationType.IsTypeChange) && (!TranslationFlag.HasFlag(TranslationType.IsIngredientDependent)))
            {
                ReturnValue &= (IngredientType == ItemToCompare.IngredientType);
            }

            if ((TranslationFlag.HasFlag(TranslationType.IsIngredientDependent)) && (AffectedIngredient != null) && 
                (ItemToCompare.AffectedIngredient !=null)) ReturnValue &= AffectedIngredient.Equals(ItemToCompare.AffectedIngredient);
            

            return ReturnValue;
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
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
        }
        public override string ToString()
        {
            string ReturnString = (TranslationStatus == ListEntryStatus.IsNotConfirmed ? "invalid: " : "  valid: ");
            ReturnString += string.Format("{0,6}-UnitTranslation: {1,5} =  {2,15:F6} {3,-5} {4}\n   - IngredientType:{5} \n    {6}", ID, BaseUnit, TranslationFactor, TargetUnit,
                                  (TranslationFlag == 0 ? "NoTypeChange, IngredientIndepedant" : string.Format("{0}", TranslationFlag)),IngredientType, AffectedIngredient);
            return ReturnString;
        }
      
    }

    public class UnitTranslationSet : ObservableCollection<UnitTranslation>
    {
        //static Variables
        private static IngredientSet _IngredientSetData;
        private static UnitTranslation _SelectedItem;
        private static UnitSet _UnitSetData;

        //Variables
        private const string FileExtension = ".tran"; // --> Data
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
        } //Readonly

        //Methods
        /* AddInactiveTranslation(BaseUnit, TargetUnit, Ingredient)
        * - Creates UnitTranslation Base->Target for Ingredient to be verified.
        * 
        * AddInactiveTranslation (BaseUnit, TargetUnit, Ingredient.Type)
        * - Creates UnitTranslation Base->Target for Ingredient.Type to be verified.
        * 
        * AddInactiveranslation (BaseUnit, TargetUnit, Ingredient.Type)
        * - Creates UnitTranslation Base->Target for Ingredient.Type to be verified.
        */

        public void AddInactiveItem(Unit Base, Unit Target)
        {
            if (Base.Type == Target.Type)
            {
                UnitTranslation NewUnitTranslation = new UnitTranslation();
                NewUnitTranslation.BaseUnit = Base;
                NewUnitTranslation.TargetUnit = Target;
                NewUnitTranslation.AffectedIngredient = null;
                NewUnitTranslation.TranslationFactor = 0;
                NewUnitTranslation.TranslationFlag = (TranslationType) 0;
                NewUnitTranslation.TranslationStatus = ListEntryStatus.IsNotConfirmed;
                AddItem(NewUnitTranslation);
            }
        }

        public void AddInactiveItem(Unit Base, Unit Target, Ingredient AffectedIngredient)
        {
            UnitTranslation NewUnitTranslation = new UnitTranslation();
            NewUnitTranslation.BaseUnit = Base;
            NewUnitTranslation.TargetUnit = Target;
            NewUnitTranslation.AffectedIngredient = AffectedIngredient;
            NewUnitTranslation.IngredientType = AffectedIngredient.Type;
            NewUnitTranslation.TranslationFactor = 0;
            NewUnitTranslation.TranslationFlag = (TranslationType.IsIngredientDependent | TranslationType.IsTypeChange);
            NewUnitTranslation.TranslationStatus = ListEntryStatus.IsNotConfirmed;
            AddItem(NewUnitTranslation);
        }
        public void AddInactiveItem(Unit Base, Unit Target, IngredientType IType)
        {
            UnitTranslation NewUnitTranslation = new UnitTranslation();
            NewUnitTranslation.BaseUnit = Base;
            NewUnitTranslation.TargetUnit = Target;
            NewUnitTranslation.AffectedIngredient = null;
            NewUnitTranslation.IngredientType = IType;
            NewUnitTranslation.TranslationFactor = 0;
            NewUnitTranslation.TranslationFlag = TranslationType.IsTypeChange;
            NewUnitTranslation.TranslationStatus = ListEntryStatus.IsNotConfirmed;
            AddItem(NewUnitTranslation);
        }

        public bool AddItem(UnitTranslation ItemToBeAdded)
        {
            bool ReturnValue = true;

            if (Count == 0) ItemToBeAdded.SetDataReference(IngredientSetData, UnitSetData);

            //Fälle können reduziert werden

            //ItemToBeAdded und der Kehrwert nicht vorhanden
            if (!Contains(ItemToBeAdded))
            {
                if ((ItemToBeAdded.TranslationFlag & TranslationType.IsIngredientDependent) == TranslationType.IsIngredientDependent)
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
                    }
                    else
                    // Fall 0: von Zutat UNABHÄNGIG - OHNE Wechsel des UnitTyps
                    {
                        AddAllItemsWithSameType(ItemToBeAdded);
                        _SelectedItem = ItemToBeAdded;
                    }

                }
                ItemToBeAdded.ID = ++_MaxID;
                Add(ItemToBeAdded);
            }

            else ReturnValue = false;

            return ReturnValue;
        }
        public void AddAllItemsWithSameType(UnitTranslation ItemToBeAdded)
        {
            UnitTranslation GeneratedUnitTranslation;
            UnitTranslation HelpingUnitTranslation;

            if (ItemToBeAdded.BaseUnit.Type == ItemToBeAdded.TargetUnit.Type)
            {
                // TranslationTobeAdded.BaseUnit -> Different Unit with same Type
                var ImplicitTranslations = (from u in UnitSetData
                                            where (u.Type == ItemToBeAdded.BaseUnit.Type) && (u != ItemToBeAdded.BaseUnit)
                                            select new { Target = (Unit)u }).ToList();

                foreach (var i in ImplicitTranslations)
                {
                    // Factor(ItemToBeAdded.Base, i.Target) 
                    //       = ItemToBeAdded.Factor * Factor(ItemToBeAdded.Target, i.Target)
                    //       = ItemToBeAdded.Factor * HelpingUnitTranslation.Factor
                    //       --> HelpingUnitTranslation = ItemToBeAdded.Target --> i.Target

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
                // and TranslationTobeAdded.TargetUnit -> Different Unit with same Type
                ImplicitTranslations = (from u in UnitSetData
                                        where (u.Type == ItemToBeAdded.TargetUnit.Type) && (u != ItemToBeAdded.TargetUnit)
                                        select new { Target = (Unit)u }).ToList();

                foreach (var i in ImplicitTranslations)
                {
                    // Factor(ItemToBeAdded.Target, i.Target) 
                    //       =   Factor(ItemToBeAdded.Target, ItemToBeAdded.Base) * Factor(ItemToBeAdded.Base, i.Target) 
                    //       = 1/Factor(ItemToBeAdded.Base, ItemToBeAdded.Target) * Factor(ItemToBeAdded.Base, i.Target) 
                    //       =   Factor(ItemToBeAdded.Base, i.Target) / Factor(ItemToBeAdded.Base, ItemToBeAdded.Target)
                    //       =   HelpingUnitTranslation.Factor / ItemToBeAdded.Factor
                    //       --> HelpingUnitTranslation = ItemToBeAdded.Base --> i.Target

                    HelpingUnitTranslation = GetTranslation(ItemToBeAdded.BaseUnit, i.Target);
                    if (HelpingUnitTranslation != null)
                    {
                        GeneratedUnitTranslation = new UnitTranslation(ItemToBeAdded.TargetUnit, i.Target,
                                                           HelpingUnitTranslation.TranslationFactor / ItemToBeAdded.TranslationFactor,
                                                           ItemToBeAdded);
                        GeneratedUnitTranslation.ID = ++_MaxID;
                        Add(GeneratedUnitTranslation);
                    }

                }


            }
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
            var maxIDFromFile = this.Select(s => s.ID).Max();

            if (maxIDFromFile == null) _MaxID = 0;
            else _MaxID = (long)maxIDFromFile;
        }

        /* GetTranslation - Versions
         * -------------------------
         * 
         * GetTranslation (BaseUnit, TargetUnit, Ingredient) - done
         * - Calculates Translation Base->Target for Ingredient 
         * - if possible - otherwise Create Suggestion and return null
         * 
         * GetTranslation (BaseUnit, TargetUnit, Ingriedient.Type)
         * - Calculates Translation Base->Target for Ingredient.Type (if possible - otherwise null)
         * - if possible - otherwise Create Suggestion and return null
         * 
         * GetTranslation (BaseUnit, TargetUnit) - done
         * - Calculates Translation Base->Target, if Base and Target have same Unit.Type 
         * - if possible - otherwise Calculate missing entry and return null
         * 
         * AddInactiveTranslation (BaseUnit, TargetUnit, Ingredient)
         * - Creates UnitTranslation Base->Target for Ingredient to be verified.
         * 
         * AddInactiveTranslation (BaseUnit, TargetUnit, Ingredient.Type)
         * - Creates UnitTranslation Base->Target for Ingredient.Type to be verified.
         * 
         */

        public UnitTranslation GetTranslation(Unit Base, Unit Target)
        {
            UnitTranslation ReturnObject = null;

            if (Base.Type == Target.Type)
            {
                var TList = this
                            .Where(s => (s.AffectedIngredient == null) &&
                            (s.TranslationStatus != ListEntryStatus.IsNotConfirmed) &&
                            ((s.BaseUnit == Base) && (s.TargetUnit == Target)) ||
                            ((s.BaseUnit == Target) && (s.TargetUnit == Base)));

                if (TList.Count() >= 1) ReturnObject = TList.ElementAt(0);
                if ((ReturnObject != null) && (ReturnObject.BaseUnit != Base)) ReturnObject.Inverse();
            }
            return ReturnObject;
        }
        public UnitTranslation GetTranslation(Unit Base, Unit Target, Ingredient Ingred)
        {
            UnitTranslation BaseTranslation;
            UnitTranslation InterTypeTranslation;
            UnitTranslation ReturnObject = null;
            UnitTranslation TargetTranslation;

            if (Base.Type == Target.Type)
            {
                ReturnObject = GetTranslation(Base, Target);
            }
            else
            {
                InterTypeTranslation = GetTranslation(Base.Type, Target.Type, Ingred);

                if (InterTypeTranslation != null)
                {
                    BaseTranslation = GetTranslation(Base, InterTypeTranslation.BaseUnit);
                    TargetTranslation = GetTranslation(InterTypeTranslation.TargetUnit, Target);

                    if ((BaseTranslation != null) && (TargetTranslation!=null))
                    {
                        ReturnObject = new UnitTranslation();
                        ReturnObject.BaseUnit = Base;
                        ReturnObject.TargetUnit = Target;
                        ReturnObject.AffectedIngredient = Ingred;
                        ReturnObject.IngredientType = Ingred.Type;
                        ReturnObject.TranslationFlag = TranslationType.IsTypeChange | TranslationType.IsIngredientDependent;
                        ReturnObject.TranslationFactor = BaseTranslation.TranslationFactor *
                                                         InterTypeTranslation.TranslationFactor *
                                                         TargetTranslation.TranslationFactor;
                    }
                }
                else
                {
                    AddInactiveItem(Base, Target, Ingred);
                    InterTypeTranslation = GetTranslation(Base, Target, Ingred.Type);
                    if (InterTypeTranslation != null)
                    {
                        BaseTranslation = GetTranslation(Base, InterTypeTranslation.BaseUnit);
                        TargetTranslation = GetTranslation(InterTypeTranslation.TargetUnit, Target);
                        if ((BaseTranslation != null) && (TargetTranslation != null))
                        {
                            ReturnObject = new UnitTranslation();
                            ReturnObject.BaseUnit = Base;
                            ReturnObject.TargetUnit = Target;
                            ReturnObject.AffectedIngredient = null;
                            ReturnObject.IngredientType = Ingred.Type;
                            ReturnObject.TranslationFlag = TranslationType.IsTypeChange;
                            ReturnObject.TranslationFactor = BaseTranslation.TranslationFactor *
                                                             InterTypeTranslation.TranslationFactor *
                                                             TargetTranslation.TranslationFactor;
                        }
                    }
                    else AddInactiveItem(Base, Target, Ingred.Type);                       
                }

            }
            return ReturnObject;
        }
        public UnitTranslation GetTranslation(Unit Base, Unit Target, IngredientType IType)
        {
            UnitTranslation ReturnObject = null;

            // Ermittle alle UnitTranslations mit zugeordnetem Ingredient, welches den IngredientType IType hat
            // mit den beiden Units Base und Target
            var TList = this
                        .Where(s => (s.AffectedIngredient != null) && (s.IngredientType == IType) &&
                        (s.TranslationStatus != ListEntryStatus.IsNotConfirmed) &&
                        ((s.BaseUnit == Base) && (s.TargetUnit == Target)) ||
                        ((s.BaseUnit == Target) && (s.TargetUnit == Base)));

            if (TList.Count() >= 1) ReturnObject = TList.ElementAt(0);
            if ((ReturnObject != null) && (ReturnObject.BaseUnit != Base)) ReturnObject.Inverse();

            return ReturnObject;
        }
        public UnitTranslation GetTranslation(UnitType BaseType, UnitType TargetType, Ingredient Ingred)
        {
            UnitTranslation ReturnObject = null;

            var TList = this
                        .Where(s => (s.AffectedIngredient != null) && (s.AffectedIngredient == Ingred) &&
                                    (s.TranslationStatus != ListEntryStatus.IsNotConfirmed) &&
                                    (((s.BaseUnit.Type == BaseType) && (s.TargetUnit.Type == TargetType)) ||
                                    ((s.BaseUnit.Type == TargetType) && (s.TargetUnit.Type == BaseType))));

            if (TList.Count() == 1)
            {
                ReturnObject = TList.ElementAt(0);
                if (ReturnObject.BaseUnit.Type != BaseType) ReturnObject.Inverse();
            }

            return ReturnObject;
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

        }// --> Data
        public void PopulateSetWithDefaults()
        {

            TranslationType UTType = TranslationType.IsTypeChange;

            Clear();
            //IsWeight
            AddItem(new UnitTranslation("kg", "g", 1000.0, 0, 0, UnitSetData));
            AddItem(new UnitTranslation("g", "dg", 10.0, 0, 0, UnitSetData));   //dg: Dezi-Gramm
            AddItem(new UnitTranslation("g", "cg", 100.0, 0, 0, UnitSetData));  //cg: Zenti-Gramm
            AddItem(new UnitTranslation("g", "mg", 1000.0, 0, 0, UnitSetData)); //mg: Milli-Gramm
            AddItem(new UnitTranslation("g","dag", 0.1, 0, 0, UnitSetData)); //mg: Milli-Gramm
            AddItem(new UnitTranslation("pf", "g", 500.0, 0, 0, UnitSetData));  //pf: Pfund


            AddItem(new UnitTranslation("oz", "g", 28.3495, 0, 0, UnitSetData));   //oz: Unzen
            AddItem(new UnitTranslation("lb", "oz", 16.0, 0, 0, UnitSetData));  //lb: Pound
            AddItem(new UnitTranslation("oz", "dr", 16.0, 0, 0, UnitSetData));  //dr: dram
            AddItem(new UnitTranslation("lb", "gr", 7000.0, 0, 0, UnitSetData));  //gr: grain

            /*IsWeight: zu ermitteln
             *  Msp (Messerspitze) -> g (Gramm)
             *  Pr (Prise) -> g (Gramm)
             */

            //IsVolume
            AddItem(new UnitTranslation("l", "ml", 1000.0, 0, 0, UnitSetData));
            AddItem(new UnitTranslation("l", "cl", 100.0, 0, 0, UnitSetData));
            AddItem(new UnitTranslation("l", "dl", 10.0, 0, 0, UnitSetData));
            AddItem(new UnitTranslation("Ta", "ml", 237.0, 0, 0, UnitSetData));              //Ta = Tasse
            AddItem(new UnitTranslation("TL", "ml", 5.0, 0, 0, UnitSetData));                //TL = Teelöffel
            AddItem(new UnitTranslation("BL", "ml", 5.0, 0, 0, UnitSetData));                //BL = Barlöffel
            AddItem(new UnitTranslation("EL", "ml", 15.0, 0, 0, UnitSetData));               //TL = Teelöffel
            AddItem(new UnitTranslation("ml", "Tr", 30.0, 0, 0, UnitSetData));               //Tr = Tropfen
            AddItem(new UnitTranslation("ds", "ml", 0.6, 0, 0, UnitSetData));                //Dash = Spritzer
            AddItem(new UnitTranslation("Spr", "ml", 25.0, 0, 0, UnitSetData));              //sht = Schuss
            AddItem(new UnitTranslation("ga (US)", "l", 3.785, 0, 0, UnitSetData));          //ga(US) = US Galone
            AddItem(new UnitTranslation("ga (US)", "fl.oz (US)", 128, 0, 0, UnitSetData));   //fl.oz (US) = fluid ounce US
            AddItem(new UnitTranslation("qt (US)", "fl.oz (US)", 32, 0, 0, UnitSetData));    //qt (US) = fluid ounce US
            AddItem(new UnitTranslation("pt (US)", "fl.oz (US)", 16, 0, 0, UnitSetData));    //pt (US) = fluid ounce US


            AddItem(new UnitTranslation("ga (UK)", "l", 4.546, 0, 0, UnitSetData));          //ga(UK): ga(UK) = Imperial Galone
            AddItem(new UnitTranslation("ga (UK)", "fl.oz (UK)", 160, 0, 0, UnitSetData));   //fl.oz (US) = fluid ounce US
            AddItem(new UnitTranslation("qt (UK)", "fl.oz (UK)", 40, 0, 0, UnitSetData));    //qt (US) = fluid ounce US
            AddItem(new UnitTranslation("pt (UK)", "fl.oz (UK)", 20, 0, 0, UnitSetData));    //pt (US) = fluid ounce US





            // IsTypeChange = 0x1 --> max ein Eintrag je IngredientType
            UTType = TranslationType.IsTypeChange;

            //Volume -> Weight
            AddItem(new UnitTranslation("l", "kg", 1.0, UTType, IngredientType.IsFluid, UnitSetData)); //Volume -> Weight
            AddItem(new UnitTranslation("l", "kg", 1.0, UTType, IngredientType.IsSolid, UnitSetData)); //Volume -> Weight
            AddItem(new UnitTranslation("l", "kg", 1.0, UTType, IngredientType.IsCrystal, UnitSetData)); //Volume -> Weight
            AddItem(new UnitTranslation("l", "g", 1000.0 / 145 * 70, UTType, IngredientType.IsPowder, UnitSetData)); //Volume -> Weight
            AddItem(new UnitTranslation("l", "g", 1000.0 / 115 * 85, UTType, IngredientType.IsGranular, UnitSetData)); //Volume -> Weight
            AddItem(new UnitTranslation("l", "g", 1000.0 / 115 * 85, UTType, IngredientType.IsHerb, UnitSetData)); //Volume -> Weight

            //Count --> Weight
            AddItem(new UnitTranslation("st", "g", 100.0, UTType, IngredientType.IsSolid, UnitSetData)); //Count --> Weight


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
        public UnitTranslation SelectItem(int ItemPos)
        {
            UnitTranslation ReturnItem = null;
            if ((ItemPos > -1) && (ItemPos <= Count - 1))
            {
                ReturnItem = this[ItemPos];
                _SelectedItem = ReturnItem;
            }
            return ReturnItem;

        }
        public UnitTranslation SelectItem(UnitTranslation ItemToBeSelected)
        {
            UnitTranslation ReturnItem = null;

            int IndexOfSelectedUnit = IndexOf(ItemToBeSelected);
            if (IndexOfSelectedUnit >= -1) ReturnItem = SelectItem(IndexOfSelectedUnit);

            return ReturnItem;
        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
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
    }

}