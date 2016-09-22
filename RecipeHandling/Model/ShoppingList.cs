﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace Jamie.Model
{
    public class ShoppingList
    {

    }

    public class ShoppingListSet
    {

    }

    public class ShoppingListItem
    {
        //static Variables
        private static IngredientSet _IngredientSetData;
        private static UnitSet _UnitSetData;

        //Variables
        private long? _ID;
        private Ingredient _Ingredient;
        private double _Quantity;
        private Unit _Unit;
        private FoodPlanItem _ReferredFoodPlanItem;


        //Constructors
        public ShoppingListItem()
        {

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
                _ID = value;
            }
        }
        public Ingredient Ingredient
        {
            get
            {
                return _Ingredient;
            }

            set
            {
                _Ingredient = value;
            }
        }
        public static IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }
        } //Readonly
        public double Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {
                _Quantity = value;
            }
        }
        public Unit Unit
        {
            get
            {
                return _Unit;
            }

            set
            {
                _Unit = value;
            }
        }
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //Readonly
        
        //Methods
        public bool Equals(ShoppingListItem ItemToCompare)
        {
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(ShoppingListItem ItemToCompare)
        {
            //            return Symbol.Equals(ItemToCompare.Symbol);
            return false; //muss noch angepasst werden.
        }
        public void PopulateObject()
        {
            string InputString;
            double ParsedDoubleValue;
            Unit ProcessedUnit;
            Ingredient ProcessedIngredient;

            do
            {
                Console.Write("Quantity  : "); InputString = Console.ReadLine();
            } while (!double.TryParse(InputString, out ParsedDoubleValue));
            Quantity = ParsedDoubleValue;

            do
            {
                UnitSetData.ViewSet();
                Console.Write("Unit      : "); ProcessedUnit = UnitSetData.SelectItem(Console.ReadLine());
            } while (ProcessedUnit == null);
            Unit = ProcessedUnit;

            do
            {
                IngredientSetData.ViewSet();
                Console.Write("Ingredient: "); ProcessedIngredient = IngredientSetData.SelectItem(Console.ReadLine());
            } while (ProcessedIngredient == null);
            Ingredient = ProcessedIngredient;

        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
        {
            //if (_IngredientSetData == null)
                _IngredientSetData = IngredientSetData;
            //if (_UnitSetData == null) 
                _UnitSetData = UnitSetData;

        }
        public override string ToString()
        {
            return string.Format("{0,6} {1} {2} {3,10}", ID, Quantity, Unit.Name, Ingredient.Name);
        }
    }

    public class ShoppingListItemSet : ObservableCollection<ShoppingListItem>
    {
        //Constants
        private const string FileExtension = ".shli";

        //static Variables
        private static long _MaxID = 0;
        private static IngredientSet _IngredientSetData;
        private static ShoppingListItem _SelectedItem;
        private static UnitSet _UnitSetData;


        //Variables
        private DateTime _DueDate;
        private string _Name;
        private string _Responsible;
        private string _Shop;

        //Constructors
        public ShoppingListItemSet()
        {

        }

        //Properties
        public DateTime DueDate
        {
            get
            {
                return _DueDate;
            }

            set
            {
                _DueDate = value;
            }
        }
        public static IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }
        }//Readonly
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }
        } //Readonly
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
        public string Responsible
        {
            get
            {
                return _Responsible;
            }

            set
            {
                _Responsible = value;
            }
        }
        public static ShoppingListItem SelectedItem
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
        public string Shop
        {
            get
            {
                return _Shop;
            }

            set
            {
                _Shop = value;
            }
        }
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //Readonly

        //Methods
        public bool AddItem()
        {
            ShoppingListItem newItem;

            newItem = new ShoppingListItem();
            if (Count == 0) newItem.SetDataReference(IngredientSetData, UnitSetData);
            newItem.PopulateObject();
            return AddItem(newItem);
        }
        public bool AddItem(ShoppingListItem ItemToBeAdded)
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
                Console.WriteLine("\nShoppingListItem Menü");
                Console.WriteLine("---------\nSelected ShoppingLisstItem: {0}\n", _SelectedItem);
                Console.WriteLine("A  Add ShoppingListItem");
                Console.WriteLine("D  Delete Selected ShoppingListItem");
                Console.WriteLine("S  Select ShoppingListItem");
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
        public ShoppingListItemSet OpenSet(string FileName)
        {
            ShoppingListItemSet ReturnUnitSet = this;
            ReturnUnitSet.Clear();
            FileName += FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnUnitSet.GetType());
                ReturnUnitSet = (ShoppingListItemSet)x.Deserialize(fs);
            }

            EvaluateMaxID();
            return ReturnUnitSet;

        }
        public void PopulateObject()
        {
            string InputString;

            do
            {
                Console.Write("Shoppinglist Date: "); InputString = Console.ReadLine();
                try
                {
                    DueDate = DateTime.Parse(InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);

            Console.Write("Name            : "); Name = Console.ReadLine();
            Console.Write("Responsible     : "); Responsible = Console.ReadLine();
            Console.Write("Shop            : "); Shop = Console.ReadLine();



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
        public ShoppingListItem SelectItem()
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
        public ShoppingListItem SelectItem(long IDToSelect)
        {
            ShoppingListItem LocalItemToSelect = new ShoppingListItem();
            LocalItemToSelect.ID = IDToSelect;

            int IndexOfSelectedItem= IndexOf(LocalItemToSelect);
            if (IndexOfSelectedItem == -1) return null;
            else return this[IndexOfSelectedItem];

        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
        {
            //if (_IngredientSetData == null)
            _IngredientSetData = IngredientSetData;
            //if (_UnitSetData == null) 
            _UnitSetData = UnitSetData;

            if (this.Count() != 0) this[0].SetDataReference(IngredientSetData,UnitSetData);
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());

            Console.WriteLine(_UnitSetData.ToString());
            Console.WriteLine(_IngredientSetData.ToString());
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += string.Format("\nListe der ShoppingListItems - MaxID: {0}\n", MaxID);
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (ShoppingListItem ListItem in this)
                    ReturnString += ListItem.ToString() + "\n";
            }
            ReturnString += "\n";
            return ReturnString;
        }


    }

}
