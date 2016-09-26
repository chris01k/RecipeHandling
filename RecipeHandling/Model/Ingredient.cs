
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;



namespace Jamie.Model
{

    /* IngredientFlags, IngredientType im namespace:
     * können von mehreren Klassen verwendet werden (z.B. Ingredient, Recipe)
     */
    [Flags]
    public enum IngredientFlags : int
    { IsVegetarian = 1, IsVegan = 2, IsLowCarb = 4, IsLowFat = 8 }
    public enum IngredientType : int { IsFluid, IsSolid, IsCrystal, IsPowder, IsHerb, IsGranular }

    /* Eine Zutat beschreibt ein Produkt, welches in einem Rezept verarbeitet werden kann. Zutaten werden im Gegensatz zu Werkzeugen verbraucht. 
     * Hat Eigenschaften: x kcal/100g, Ernährungsampel (rot, gelb, grün)
     * länderspezifische Zuordnung?
     */

    public class Ingredient : IEquatable<Ingredient> //: ObservableObject
    {
        //Constants
        public const byte maxIngredientFlag = 15;

        //static Variables
        private static UnitSet _UnitSetData;

        //Variables
        private long? _ID;
        private IngredientFlags _Flags;
        private IngredientType _Type;
        private string _Name;
        private Unit _TargetUnit;  

        //Constructors
        public Ingredient()
        {

        }
        public Ingredient(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }
        public Ingredient(bool ToBePopulated, UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
            if (ToBePopulated) PopulateObject();
        }
        public Ingredient(string Name, IngredientFlags IngredientType, UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
            _Name = Name;
            _Flags = IngredientType;
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
        public IngredientFlags Flags
        {
            get
            {
                return _Flags;
            }

            set
            {
                _Flags = value;
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
        public Unit TargetUnit
        {
            get
            {
                return _TargetUnit;
            }

            set
            {
                _TargetUnit = value;
            }
        }
        public IngredientType Type
        {
            get
            {
                return _Type;
            }

            set
            {
                _Type = value;
            }
        }
        public UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //ReadOnly


        // Methods
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
            IngredientFlags FlagValue=0;
            
            string InputString;
            Unit ProcessedUnit;

            Console.WriteLine("Eingabe neue Zutat:");
            Console.WriteLine("-------------------");
            Console.WriteLine();
            Console.Write("         Name  : "); Name = Console.ReadLine();
//            Console.Write("  Target Unit  : "); InputString = Console.ReadLine();
            do
            {
                Console.Write("  Target Unit  : "); ProcessedUnit = UnitSetData.SelectItem(Console.ReadLine());
            } while (ProcessedUnit == null);
            TargetUnit = ProcessedUnit;
            do
            {
                Console.Write("Ingredient Type  : "); InputString = Console.ReadLine();
                try
                {
                    Type = (IngredientType)Enum.Parse(typeof(IngredientType), InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);



            for (int i = 1; i <= maxIngredientFlag; i = (i * 2))
            {
                Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
                if (InputString.Length > 0) FlagValue = (FlagValue | (IngredientFlags)i);
            }
            Flags = FlagValue;

        }
        public override string ToString()
        {
            return string.Format("{0,6}-Name: {1,15} Type: {2} Flags: {3}\n\t TargetUnit{4,5}", ID, Name, Type, Flags, TargetUnit);
        }
    }


    public class IngredientSet : ObservableCollection<Ingredient>
    {
        //Constants
        private const string FileExtension = ".ingr";

        //Variables
        private static long _MaxID = 0;
        private static Ingredient _SelectedItem; 
        private static UnitSet _UnitSetData; 

        //Constructors
        public IngredientSet(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }

        //Properties
        public Ingredient SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
        }//Readonly
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        }  //Readonly

        //Methods
        public void AddItem()
        {
            AddItem(new Ingredient(true, UnitSetData));
        }
        public void AddItem(Ingredient ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
                _SelectedItem = SelectItem(ItemToBeAdded.Name);
            }
            else Console.WriteLine("Die Zutat ist bereits vorhanden: \n {0}", ItemToBeAdded);
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
        public void EditSelectedItem()
        {
            string InputString = "";
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("Edit Selected Ingredient: {0}\n", _SelectedItem);
                Console.WriteLine("-------------------------\n");
                Console.WriteLine();
                Console.WriteLine("C  Change Field");
                //Console.WriteLine("R  Reset Field");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");
                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "C":
                        string[] FieldsToBeSelected = { "Name", "Flags", "TargetUnit", "Type" };

                        InputString = ListHelper.SelectField(FieldsToBeSelected);
                        if (InputString == "Name") SelectedItem.Name = ListHelper.ChangeStringField(InputString);
                        else if (InputString == "Flags") SelectedItem.Flags = ListHelper.ChangeIngredientFlagField(InputString);
                        else if (InputString == "TargetUnit") SelectedItem.TargetUnit = ListHelper.ChangeUnitField(InputString, _UnitSetData);
                        else if (InputString == "Type") SelectedItem.Type = ListHelper.ChangeIngredientTypeField(InputString);
                        break;

                    default:
                        Console.WriteLine();
                        break;
                }


            }
        }
        public void EvaluateMaxID()
        {
            //            var maxIDFromFile = (from s in this select s.ID).Max();

            var maxIDFromFile = this
                                .Select(s => s.ID).Max();

            if (maxIDFromFile == null) _MaxID = 0;
            else _MaxID = (long)maxIDFromFile;
        }
        public void Menu()
        {
            int HowManyItemsInSet = Count;

            if (HowManyItemsInSet > 0) _SelectedItem = this[HowManyItemsInSet - 1];
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nIngredient Menü");
                Console.WriteLine("------------------------\n");
                Console.WriteLine("Selected Ingredient {0}\n", _SelectedItem);
                Console.WriteLine();
                Console.WriteLine("A  Add Ingredient");
                Console.WriteLine("D  Delete Ingredient");
                Console.WriteLine("E  Edit Selected Ingredient");
                Console.WriteLine("S  Select Ingredient");
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
                        EditSelectedItem();
                        break;
                    case "S":
                        _SelectedItem = SelectItem();
                        break;
                    case "V":
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
                                             | IngredientFlags.IsLowFat,UnitSetData));
            AddItem(new Ingredient("Tomaten",  IngredientFlags.IsVegetarian
                                             | IngredientFlags.IsVegan
                                             | IngredientFlags.IsLowCarb
                                             | IngredientFlags.IsLowFat,UnitSetData));
            AddItem(new Ingredient("Rinderfilet", IngredientFlags.IsLowCarb,UnitSetData));
            AddItem(new Ingredient("Quinoa", IngredientFlags.IsVegetarian
                                           | IngredientFlags.IsVegan
                                           | IngredientFlags.IsLowFat, UnitSetData));
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
        public void SetDataReference(UnitSet UnitSetData)
        {
            if (_UnitSetData == null) _UnitSetData = UnitSetData;
        }
        public Ingredient SelectItem()
        {
            string InputItemText = "";

            Console.WriteLine("Unit suchen:");
            Console.WriteLine("------------");
            Console.WriteLine();
            Console.Write("IngredientName: "); InputItemText = Console.ReadLine();

            return SelectItem(InputItemText);

        }
        public Ingredient SelectItem(string ItemTextToBeSelected)
        {
            Ingredient LocalItemToSelect = new Ingredient(ItemTextToBeSelected, 0,UnitSetData);

            int IndexOfSelectedUnit = IndexOf(LocalItemToSelect);

            if (IndexOfSelectedUnit == -1) return null;
            else return this[IndexOfSelectedUnit];
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += string.Format("\nListe der Zutaten: MaxID {0}\n", _MaxID);
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

        private double _Quantity;
        private Ingredient _Ingredient;
        private Unit _Unit;


        //Constructors
        public IngredientItem()
        {

//            _Ingredient = new Ingredient(null);

        }
        public IngredientItem(double Quantity, Unit Unit, Ingredient Ingredient)
        {
            _Quantity = Quantity;
            _Unit = Unit;
            _Ingredient = Ingredient;
        }

        public IngredientItem(bool ToBePopulated)
        {
//            _Ingredient = new Ingredient(null);
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
        public double Quantity
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
            //Unit SelectedUnit;

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
        public override string ToString()
        {
            string ReturnString = string.Format(" {0,8} {1} {2}", Quantity, Unit, Ingredient.Name);

            ReturnString += "\n";
            return ReturnString;
        }


    }

    /* IngredientRecipeSet ist eine Liste IngredientItems (Menge, Ingredient):
 * Ein Rezept enthält eine Zutatenliste bestehend aus x Einträgen, wobei jeder Eintrag eine Zutat sowie die 
 * erforderliche Menge beschreibt.
 */
    public class IngredientItemSet : ObservableCollection<IngredientItem>
    {
        //Constants
        private const string FileExtension = ".rcig";

        //Variables
        private static long _MaxID;

        private Recipe _RelatedRecipe;
        private static UnitSet _UnitSetData;
        private static IngredientSet _IngredientSetData;

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
        public Recipe RelatedRecipe
        {
            get
            {
                return _RelatedRecipe;
            }
        }
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }

            set
            {
                _UnitSetData = value;
            }
        }
        public static IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }

            set
            {
                _IngredientSetData = value;
            }
        }

        //Methods
        public void AddItem()
        {
            string InputString;
            double ParsedDoubleValue;
            Unit InputUnit;
            Ingredient InputIngredient;
            IngredientItem NewIngredientItem = new IngredientItem();
            do
            {
                Console.Write("Quantity: "); InputString = Console.ReadLine();
            } while (!double.TryParse(InputString, out ParsedDoubleValue));
            NewIngredientItem.Quantity = ParsedDoubleValue;
            do
            {
                if (UnitSetData != null) UnitSetData.ViewSet();
                Console.WriteLine("Unit      : "); InputString = Console.ReadLine();
                InputUnit = UnitSetData.SelectItem(InputString);
            } while (InputUnit == null);
            NewIngredientItem.Unit = InputUnit;

            do
            {
                IngredientSetData.ViewSet();
                Console.WriteLine("Ingredient: "); InputString = Console.ReadLine();
                InputIngredient = IngredientSetData.SelectItem(InputString);
            } while (InputIngredient == null);


            NewIngredientItem.Ingredient = InputIngredient;

            AddItem(NewIngredientItem);
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
        public IngredientItemSet OpenSet(string FileName)
        {
            IngredientItemSet ReturnSet = this;
            ReturnSet.Clear();
            FileName += "." + RelatedRecipe.Name + FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnSet.GetType());
                ReturnSet = (IngredientItemSet)x.Deserialize(fs);
            }
            return ReturnSet;

        }
        public void SaveSet(string FileName)
        {
            FileName += "." + RelatedRecipe.Name + FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

        }
        public void SetDataReference(UnitSet UnitSetData, IngredientSet IngredientSetData)
        {
            _UnitSetData = UnitSetData;
            _IngredientSetData = IngredientSetData;
        }
        public override string ToString()
        {
            string ReturnString = "";
//                string.Format("{0,8} Zutaten im Rezept: {1} \n", MaxID, RelatedRecipe);

            if (Count == 0) ReturnString += "         -------> leer <-------\n         ";
            else
            {
                foreach (IngredientItem ListItem in this)
                    //                    ReturnString += ListItem.ToString() + "\n      ";
                    ReturnString += string.Format("{0,6} {1,5} {2,20} {3}  \n", ListItem.Quantity, ListItem.Unit.Symbol, ListItem.Ingredient.Name, RelatedRecipe.Name);
            }
            ReturnString += "\n";
            return ReturnString;
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }

    }

/*    public class IngredientType
    {

    }

    public class IngredientTypeSet
    {

    }
*/
}

