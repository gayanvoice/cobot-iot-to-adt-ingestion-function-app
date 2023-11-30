using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace UodDtIotToAdtIngestionFunctionApp.Model
{
    public class Position3Model
    {
        public double p0 { get; set; }
        public double p1 { get; set; }
        public double p2 { get; set; }

        public static Position3Model GetPosition3Model(JToken positionToken)
        {
            List<double> positionList = JsonConvert.DeserializeObject<List<double>>(positionToken.ToString());
            Position3Model position3Model = new Position3Model();
            position3Model.p0 = positionList[0];
            position3Model.p1 = positionList[1];
            position3Model.p2 = positionList[2];
            return position3Model;
        }
    }
}