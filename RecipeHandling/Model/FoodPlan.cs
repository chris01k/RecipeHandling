using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jamie.Model
{
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
            if (ItemToCompare == null) return false;
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(FoodPlanItem ItemToCompare)
        {
//            return Symbol.Equals(ItemToCompare.Symbol);
            return false; //muss noch angepasst werden.
        }
        public void SetDataReference(RecipeSet RecipeSetData)
        {
            _RecipeSetData = RecipeSetData;
        }
        public override string ToString()
        {
            return string.Format("{0,6} {1:d} - Portions: {2:N2} - Name: {3,10}", ID, DateToStartPreparation, TotalPortions, PlannedRecipe.Name);
        }
    }
    public class FoodPlanItemSet : ObservableCollection<FoodPlanItem>
    {
        //Constants
        private const string FileExtension = ".fpitm";// --> Data

        //static Variables
        private static long _MaxID = 0;
        private static RecipeSet _RecipeSetData;

        //Variables
        private FoodPlanItem _SelectedItem;

        //Constructors
        public FoodPlanItemSet()
        {
        }

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
        public FoodPlanItem SelectedItem
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
        }// teilweise Contains--> View
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
            var maxIDFromFile = this.Select(s => s.ID).Max();

            if (maxIDFromFile == null) _MaxID = 0;
            else _MaxID = (long)maxIDFromFile;
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

        }// --> Data
        public void SaveSet(string FileName)
        {
            FileName += FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

        }// --> Data
        public FoodPlanItem SelectItem(int ItemPos)
        {
            FoodPlanItem ReturnItem = null;
            if ((ItemPos > -1) && (ItemPos <= Count - 1))
            {
                ReturnItem = this[ItemPos];
                _SelectedItem = ReturnItem;
            }
            return ReturnItem;
        }
        public FoodPlanItem SelectItemByID(long IDToSelect)
        {
            FoodPlanItem LocalItemToSelect = new FoodPlanItem();
            LocalItemToSelect.ID = IDToSelect;

            int IndexOfSelectedItem = IndexOf(LocalItemToSelect);
            if (IndexOfSelectedItem == -1) return null;
            else return this[IndexOfSelectedItem];

        }
        public void SetDataReference(RecipeSet RecipeSetData)
        {
            _RecipeSetData = RecipeSetData;
            if (this.Count() != 0) this[0].SetDataReference(RecipeSetData);
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
/*        public void TransferToShoppingList(ShoppingListItemSet ShoppingListItems)
        {
            ShoppingListItem newItem;
            double quantityFactor;

            foreach (FoodPlanItem ListItem in this)
            {
                quantityFactor = ListItem.TotalPortions / ListItem.PlannedRecipe.PortionQuantity;

                foreach (IngredientItem RecipeIngredient in ListItem.PlannedRecipe.Ingredients)
                {
                    newItem = new ShoppingListItem();
                    newItem.Quantity = quantityFactor * RecipeIngredient.Quantity;
                    newItem.Unit = RecipeIngredient.Unit;
                    newItem.Ingredient = RecipeIngredient.Ingredient;
                    newItem.ReferredFoodPlanItem = ListItem;
                    ShoppingListItems.AddItem(newItem);
                }
            }
        }
*/
    }

}

