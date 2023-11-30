using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace UodDtIotToAdtIngestionFunctionApp.Model
{
    public class Position6Model
    {
        public double p0 { get; set; }
        public double p1 { get; set; }
        public double p2 { get; set; }
        public double p3 { get; set; }
        public double p4 { get; set; }
        public double p5 { get; set; }

        public static Position6Model GetPosition6Model(JToken positionToken)
        {
            List<double> positionList = JsonConvert.DeserializeObject<List<double>>(positionToken.ToString());
            Position6Model position6Model = new Position6Model();
            position6Model.p0 = positionList[0];
            position6Model.p1 = positionList[1];
            position6Model.p2 = positionList[2];
            position6Model.p3 = positionList[3];
            position6Model.p4 = positionList[4];
            position6Model.p5 = positionList[5];
            return position6Model;
        }
    }
}