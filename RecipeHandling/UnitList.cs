using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class UnitList
    {
        public ObservableCollection<Unit> Units;
        private static string filename = "ObservedUnits.txt";

        internal UnitList()
        {
            Units = new ObservableCollection<Unit>();
/*            Units.Add(new Unit("Kilogramm", "kg", "Masse"));
            Units.Add(new Unit("Liter", "l", "Volumen"));
            Units.Add(new Unit("Stück", "st", "Anzahl"));
            Units.Add(new Unit("Milliliter", "ml", "Volumen"));
            Units.Add(new Unit("Meter", "m", "Länge"));

            SaveList();

            Units.Clear(); 
*/
            OpenList();

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

        public void OpenList()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Units.GetType());
                Units = (ObservableCollection<Unit>)x.Deserialize(fs);
            }
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

        public void SaveList()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Units.GetType());
                x.Serialize(fs, Units);
            }

        }

        public void ShowList()
        {
            Console.WriteLine();
            Console.WriteLine("Ausgabe der Liste: {0}",filename);
            if (Units.Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                foreach (Unit ListItem in this.Units)
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
            Console.WriteLine("Ausgabe der Liste: {0}", filename);
            if (Units.Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Units.GetType());
                x.Serialize(Console.Out, Units);

            }
        }


    }
}