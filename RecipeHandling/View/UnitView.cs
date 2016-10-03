using Jamie.Model;
using System;

namespace Jamie.View
{
    public class UnitView
    {
        //Variables
        private UnitSet _Units;

        //Constructors
        public UnitView(UnitSet Units)
        {
            _Units = Units;
        }

        //Properties
        public UnitSet Units
        {
            get
            {
                return _Units;
            }
        }

        //Methods
        public void Menu()
        {
            int HowManyItemsInSet = Units.Count;

            if (HowManyItemsInSet > 0) Units.SelectItem(HowManyItemsInSet - 1);
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nUnit Menü");
                Console.WriteLine("---------\nSelected Unit: {0}\n", UnitSet.SelectedItem);
                Console.WriteLine("A  Add Unit");
                Console.WriteLine("D  Delete Selected Unit");
                Console.WriteLine("S  Select Unit");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        Unit newItem = PopulateObject();
                        if ((newItem != null) && !Units.AddItem(newItem))
                        {
                            Console.WriteLine("Fehler: Einheit konnte nicht angelegt werden");
                            Console.ReadLine();
                        }
                        break;
                    case "D":
                        Units.DeleteSelectedItem();
                        break;
                    case "S":
                        SelectItem();
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
        public Unit PopulateObject()
        {
            string InputString;
            Unit ObjectToBePopulated = new Unit();
            UnitType UnitTypeOutput; 
                      

            Console.WriteLine("Eingabe neue Einheit:");
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("UnitName  : "); ObjectToBePopulated.Name = Console.ReadLine();
            Console.Write("UnitSymbol: "); ObjectToBePopulated.Symbol = Console.ReadLine();

            do
            {
                Console.Write("UnitType  : "); InputString = Console.ReadLine();
            } while (!UnitType.TryParse(InputString, out UnitTypeOutput));

            ObjectToBePopulated.Type = UnitTypeOutput;

            return ObjectToBePopulated;
        } 
        public Unit SelectItem()
        {
            string LocalUnitSymbol = "";

            Console.WriteLine("Unit suchen:");
            Console.WriteLine("------------");
            Console.WriteLine();
            Console.Write("UnitSymbol: "); LocalUnitSymbol = Console.ReadLine();

            return Units.SelectItem(LocalUnitSymbol);

        }
        public void ViewSet()
        {
            Console.WriteLine(Units.ToString());
        } 

    }
}