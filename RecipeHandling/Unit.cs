﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class Unit : IEquatable<Unit>
    {
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public string UnitType { get; set; }

        private static int maxUnitID = 0;
        private int unitID;
        private int parentUnit;

        internal Unit()
        {
            unitID = ++maxUnitID;
        }

        internal Unit(string UnitName, string UnitSymbol, string UnitType)
        {
            unitID = ++maxUnitID;
            this.UnitName = UnitName;
            this.UnitSymbol = UnitSymbol;
            this.UnitType = UnitType;
        }

        public  bool Equals(Unit unitToCompare)
        {
            return UnitSymbol.Equals(unitToCompare.UnitSymbol);
        }

        public override string ToString()
        {
            return String.Format("Unit {3,5} Name: {0,10}  Symbol: {1,5} Type: {2,10}", UnitName, UnitSymbol, UnitType, unitID);
        }


    }
    public class UnitList
    {
        public List<Unit> Units;
        private static string filename = "Units.txt";

        internal UnitList()
        {
            /*          Units = new List<Unit> (30);
                        Units.Add(new Unit("Kilogramm", "kg", "Masse"));
                        Units.Add(new Unit("Liter", "l", "Volumen"));
                        Units.Add(new Unit("Stück", "st", "Anzahl"));
                        Units.Add(new Unit("Milliliter", "ml", "Volumen"));
                        Units.Add(new Unit("Meter", "m", "Länge"));

                        SaveList();
            */

            Units = new List<Unit>(30);
            OpenList();
            
        }

        public void SaveList()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Units.GetType());
                x.Serialize(fs, Units);
            }

        }

        public void OpenList()
        {
            Stream fs = new FileStream(filename, FileMode.Open);
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Units.GetType());
            Units = (List<Unit>) x.Deserialize(fs);
        }
    }

}



