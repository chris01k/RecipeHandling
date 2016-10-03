using System.IO;

namespace Jamie.Model
{
    public class EmbeddedDataSet
    {
        private AllDataSets _AllSetData;
        private static string filename = "DATA\\ObservedUnits.rds";

        //Constructors
        public EmbeddedDataSet()
        {
            AllSetData = new AllDataSets();
            OpenLists();
            AllSetData.ViewSet();

        }
        public EmbeddedDataSet(bool ToBePopulatedWithDefaults)
        {
            AllSetData = new AllDataSets(ToBePopulatedWithDefaults);

        }

        //Properties
        public AllDataSets AllSetData
        {
            get
            {
                return _AllSetData;
            }

            set
            {
                _AllSetData = value;
            }
        }

        //Methods
        public void ClearLists()
        {
            AllSetData.ClearList();
        }
        public void OpenLists()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(AllSetData.GetType());
                AllSetData = (AllDataSets)x.Deserialize(fs);
            }
            AllSetData.EvaluateMaxIDs();
            AllSetData.SetDataReference();
        }
        public void SaveLists()
        {
            AllSetData.SaveSet("DATA\\Jamie");

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(AllSetData.GetType());
                x.Serialize(fs, AllSetData);
            }

        }
    }
}