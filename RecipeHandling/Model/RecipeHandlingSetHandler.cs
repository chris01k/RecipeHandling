using System.IO;

namespace Jamie.Model
{
    public class RecipeHandlingSetHandler
    {
        private RecipeDataSets _JamieDataSet;
        private static string filename = "DATA\\ObservedUnits.rds";

        //Constructors
        public RecipeHandlingSetHandler()
        {
            JamieDataSet = new RecipeDataSets();
            OpenLists();
            JamieDataSet.ViewSet();

        }
        public RecipeHandlingSetHandler(bool ToBePopulatedWithDefaults)
        {
            JamieDataSet = new RecipeDataSets(ToBePopulatedWithDefaults);

        }

        //Properties
        public RecipeDataSets JamieDataSet
        {
            get
            {
                return _JamieDataSet;
            }

            set
            {
                _JamieDataSet = value;
            }
        }

        //Methods
        public void ClearLists()
        {
            JamieDataSet.ClearList();
        }
        public void OpenLists()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(JamieDataSet.GetType());
                JamieDataSet = (RecipeDataSets)x.Deserialize(fs);
            }


            JamieDataSet.EvaluateMaxIDs();
            JamieDataSet.SetDataReference();

        }
        public void SaveLists()
        {
            JamieDataSet.SaveSet("DATA\\Jamie");

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(JamieDataSet.GetType());
                x.Serialize(fs, JamieDataSet);
            }

        }
    }
}