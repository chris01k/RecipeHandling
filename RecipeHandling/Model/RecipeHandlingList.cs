using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class RecipeHandlingList
    {
        public ObservableCollection<Unit> Units;
        public ObservableCollection<UnitTranslation> UnitTranslations;


        internal RecipeHandlingList()
        {
            Units = new ObservableCollection<Unit>();
            UnitTranslations = new ObservableCollection<UnitTranslation>();

        }

        internal RecipeHandlingList(bool ToBePopulatedWithDefaults)
        {
            Units = new ObservableCollection<Unit>();
            UnitTranslations = new ObservableCollection<UnitTranslation>();

            if (ToBePopulatedWithDefaults) PopulateListsWithDefaults();

        }

        public void AddUnit()
        {
            Unit UnitToBeAdded = new Unit(true);
            AddUnit(UnitToBeAdded);
        }

        public void AddUnit(Unit UnitToBeAdded)
        {
            if (!Units.Contains(UnitToBeAdded)) Units.Add(UnitToBeAdded);
            else Console.WriteLine("Die Unit ist bereits vorhanden: \n {0}", UnitToBeAdded);
        }

        public void ClearLists()
        {
            Units.Clear();
            UnitTranslations.Clear();

        }

        private void PopulateListsWithDefaults()
        {
            Units.Add(new Unit("Kilogramm", "kg", "Masse"));
            Units.Add(new Unit("Gramm", "g", "Masse"));
            Units.Add(new Unit("Unze", "oz", "Masse"));
            Units.Add(new Unit("Liter", "l", "Volumen"));
            Units.Add(new Unit("Stück", "st", "Anzahl"));
            Units.Add(new Unit("Milliliter", "ml", "Volumen"));
            Units.Add(new Unit("Meter", "m", "Länge"));

            UnitTranslations.Add(new UnitTranslation("kg", "g", 1000.0, 0));
            UnitTranslations.Add(new UnitTranslation("g", "mg", 1000.0, 0));
            UnitTranslations.Add(new UnitTranslation("l", "ml", 1000.0, 0));
            UnitTranslations.Add(new UnitTranslation("oz", "g",28.3495, 0));

            UnitTranslations.Add(new UnitTranslation("l", "kg", 1.0, 3));

        }

        public void RemoveUnit()
        {
            string LocalUnitSymbol = "";

            Console.WriteLine("Unit Entfernen:");
            Console.WriteLine("---------------");
            Console.WriteLine();
            Console.Write("UnitSymbol: "); LocalUnitSymbol = Console.ReadLine();

            RemoveUnit(LocalUnitSymbol);

        }

        public void RemoveUnit(string UnitSymbolToBeRemoved)
        {

            Unit UnitToBeRemoved = new Unit();
            UnitToBeRemoved.UnitSymbol = UnitSymbolToBeRemoved;

            if (Units.Contains(UnitToBeRemoved)) Units.Remove(UnitToBeRemoved);
            else Console.WriteLine("UnitSymbol {0} konnte nicht gefunden werden", UnitSymbolToBeRemoved);
                       

        }

        public Unit SelectUnit()
        {
            string LocalUnitSymbol = "";

            Console.WriteLine("Unit suchen:");
            Console.WriteLine("------------");
            Console.WriteLine();
            Console.Write("UnitSymbol: "); LocalUnitSymbol = Console.ReadLine();

            return SelectUnit(LocalUnitSymbol);

        }

        public Unit SelectUnit(string UnitSymbolToBeSelected)
        {
            int IndexOfSelectedUnit = Units.IndexOf(new Unit("", UnitSymbolToBeSelected, ""));

            if (IndexOfSelectedUnit == -1)
            {
                Console.WriteLine();
                Console.WriteLine("---------------> UnitSymbol {0} nicht bekannt <---------------", UnitSymbolToBeSelected);
                return null;
            }
            else return Units[IndexOfSelectedUnit];

        }

        public void ShowList()
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Ausgabe der Listen: ");
            Console.WriteLine("Liste der Units:");
            if (Units.Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                foreach (Unit ListItem in Units)
                    Console.WriteLine(ListItem);
            }
            Console.WriteLine();
            Console.WriteLine("Liste der Translations:");
            if (UnitTranslations.Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                foreach (UnitTranslation ListItem in UnitTranslations)
                    Console.WriteLine(ListItem);
            }

        }

        public override string ToString()
        {
            string Returnstring = "";
            foreach (Unit ListItem in Units)
                Returnstring = Returnstring + ListItem.ToString() + "\n";

            return Returnstring;

        }
        
        public void ViewXML()
        {
            Console.WriteLine();
            Console.WriteLine("Ausgabe der Liste:");
            if (Units.Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Units.GetType());
                x.Serialize(Console.Out, Units);

            }
        }

    }
}