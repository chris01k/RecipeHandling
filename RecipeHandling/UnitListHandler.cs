using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class UnitListHandler
    {
        public UnitList UL;
        private static string filename = "ObservedUnits.txt";
        

        internal UnitListHandler()
        {
            UL = new RecipeHandling.UnitList();
            OpenList();
            UL.ShowList();

        }

        internal UnitListHandler(bool ToBePopulatedWithDefaults)
        {
            UL = new RecipeHandling.UnitList(ToBePopulatedWithDefaults);

        }

        public void OpenList()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(UL.GetType());
                UL = (UnitList)x.Deserialize(fs);
            }
        }

        public void SaveList()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(UL.GetType());
                x.Serialize(fs, UL);
            }

        }



    }
}