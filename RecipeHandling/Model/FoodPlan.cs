using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Jamie.Model
{
    public class FoodPlan
    {
    }
    public class FoodPlanSet
    {
    }
    public class FoodPlanItem
    {
        //static Variables
        private static RecipeSet _RecipeSetData;

        // Variables
        private DateTime _DateToStartPreparation;
        private DateTime _DateToConsume;
        private long? _ID;
        private Recipe _PlannedRecipe;
        private float _TotalPortions;

        //Constructors
        public FoodPlanItem()
        {

        }
        public FoodPlanItem(RecipeSet RecipeSetData)
        {
            if (_RecipeSetData == null) _RecipeSetData = RecipeSetData;
        }
        public FoodPlanItem(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateObject();
        }

        //Properties
        public DateTime DateToStartPreparation
        {
            get
            {
                return _DateToStartPreparation;
            }

            set
            {
                _DateToStartPreparation = value;
            }
        }
        public DateTime DateToConsume
        {
            get
            {
                return _DateToConsume;
            }

            set
            {
                _DateToConsume = value;
            }
        }
        public long? ID
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
        public Recipe PlannedRecipe
        {
            get
            {
                return _PlannedRecipe;
            }

            set
            {
                _PlannedRecipe = value;
            }
        }
        public static RecipeSet RecipeSetData
        {
            get
            {
                return _RecipeSetData;
            }
        }
        public float TotalPortions
        {
            get
            {
                return _TotalPortions;
            }

            set
            {
                _TotalPortions = value;
            }
        }

        //Methods
        public bool Equals(FoodPlanItem ItemToCompare)
        {
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(FoodPlanItem ItemToCompare)
        {
//            return Symbol.Equals(ItemToCompare.Symbol);
            return false; //muss noch angepasst werden.
        }
        public void PopulateObject()
        {
            string InputString;
            Recipe ProcessedRecipe;


            do
            {
                Console.Write("Date of Preparation  : "); InputString = Console.ReadLine();
                try
                {
                    DateToStartPreparation = DateTime.Parse(InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);

            do
            {
                Console.Write("Date to Consume      : "); InputString = Console.ReadLine();
                try
                {
                    DateToConsume= DateTime.Parse(InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);

            do
            {
                Console.Write("Total Portions       : "); InputString = Console.ReadLine();
                try
                {
                    TotalPortions = float.Parse(InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);

            RecipeSetData.ViewSet();
            do
            {
                Console.Write("Recipe   : "); ProcessedRecipe = RecipeSetData.SelectItem(true);
            } while (ProcessedRecipe == null);
            PlannedRecipe = ProcessedRecipe;

        }
        public void SetDataReference(RecipeSet RecipeSetData)
        {
            if (_RecipeSetData == null) _RecipeSetData = RecipeSetData;
        }
        public override string ToString()
        {
            return string.Format("{0,6} {1:d} - Portions: {2:N2} - Name: {3,10}", ID, DateToStartPreparation, TotalPortions, PlannedRecipe.Name);
        }

    }
    public class FoodPlanItemSet : ObservableCollection<FoodPlanItem>
    {
        //Constants
        private const string FileExtension = ".fpitm";

        //static Variables
        private static long _MaxID = 0;
        private static RecipeSet _RecipeSetData;
        private static FoodPlanItem _SelectedItem;

        //Variables

        //Constructors
        public FoodPlanItemSet()
        {
        }
        //public FoodPlanItemSet(long ID)
        //{

        //}


        //Properties
        public static long MaxID //Readonly
        {
            get
            {
                return _MaxID;
            }
        }
        public static RecipeSet RecipeSetData
        {
            get
            {
                return _RecipeSetData;
            }

        }//Readonly
        public static FoodPlanItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }

            set
            {
                _SelectedItem = value;
            }
        }


        //Methods
        public bool AddItem()
        {

            FoodPlanItem newItem = new FoodPlanItem();
            if (Count == 0) newItem.SetDataReference(RecipeSetData);

            // Code oben ersetzt folgendes.... nach Test Kommentar löschen
            //            FoodPlanItem newItem;
            //if (Count != 0) newItem = new FoodPlanItem();
            //else newItem = new FoodPlanItem(RecipeSetData);


            newItem.PopulateObject();
            return AddItem(newItem);
        }
        public bool AddItem(FoodPlanItem ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded))
            {
                ItemToBeAdded.ID = ++_MaxID;
                Add(ItemToBeAdded);
                SelectedItem = ItemToBeAdded;
                return true;
            }
            else Console.WriteLine("Die Unit ist bereits vorhanden: \n {0}", ItemToBeAdded);
            return false;
        }
        public void DeleteSelectedItem()
        {
            int NewSelectedIndex;

            if ((Count == 0) || (SelectedItem == null)) return;
            if (Count > 1) NewSelectedIndex = IndexOf(_SelectedItem) - 1;
            else NewSelectedIndex = 1;
            Remove(SelectedItem);
            if (Count > 0) _SelectedItem = this[NewSelectedIndex];
            else _SelectedItem = null;


        }
        public void EvaluateMaxID()
        {
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
                Console.WriteLine("\nFoodPlanItem Menü");
                Console.WriteLine("---------\nSelected FoodplanItem: {0}\n", _SelectedItem);
                Console.WriteLine("A  Add FoodplanItem");
                Console.WriteLine("D  Delete Selected FoodplanItem");
                Console.WriteLine("S  Select FoodplanItem");
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
        public FoodPlanItemSet OpenSet(string FileName)
        {
            FoodPlanItemSet ReturnUnitSet = this;
            ReturnUnitSet.Clear();
            FileName += FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnUnitSet.GetType());
                ReturnUnitSet = (FoodPlanItemSet)x.Deserialize(fs);
            }

            EvaluateMaxID();
            return ReturnUnitSet;

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
        public FoodPlanItem SelectItem()
        {
            int ParsedIntValue;
            long SelectedID;
            string InputString;

            do
            {
                Console.Write("FoodPlan ID:"); InputString = Console.ReadLine();
            } while (!int.TryParse(InputString, out ParsedIntValue));
            SelectedID = ParsedIntValue;

            return SelectItem(SelectedID);

        }
        public FoodPlanItem SelectItem(long IDToSelect)
        {
            FoodPlanItem LocalItemToSelect = new FoodPlanItem();
            LocalItemToSelect.ID = IDToSelect;

            int IndexOfSelectedItem = IndexOf(LocalItemToSelect);
            if (IndexOfSelectedItem == -1) return null;
            else return this[IndexOfSelectedItem];

        }
        public void SetDataReference(RecipeSet RecipeSetData)
        {
            if (_RecipeSetData == null) _RecipeSetData = RecipeSetData;
            if (this.Count() != 0) this[0].SetDataReference(RecipeSetData);
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += string.Format("\nListe der FoodPlanItems - MaxID: {0}\n", MaxID);
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (FoodPlanItem ListItem in this)
                    ReturnString += ListItem.ToString() + "\n";
            }
            ReturnString += "\n";
            return ReturnString;
        }
    }

}

