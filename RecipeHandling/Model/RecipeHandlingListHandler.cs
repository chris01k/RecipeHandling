using Jamie.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jamie.Model
{
    public class RecipeHandlingSetHandler
    {
        public RecipeHandlingSet RHL;
        private static string filename = "ObservedUnits.txt";
        
        internal RecipeHandlingSetHandler()
        {
            RHL = new RecipeHandlingSet();
            OpenList();
            RHL.ShowSet();

        }
        internal RecipeHandlingSetHandler(bool ToBePopulatedWithDefaults)
        {
            RHL = new RecipeHandlingSet(ToBePopulatedWithDefaults);

        }

        public void OpenList()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(RHL.GetType());
                RHL = (RecipeHandlingSet)x.Deserialize(fs);
            }
        }
        public void SaveList()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(RHL.GetType());
                x.Serialize(fs, RHL);
            }

        }
    }
}