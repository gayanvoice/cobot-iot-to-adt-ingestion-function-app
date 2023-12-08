using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace UodDtIotToAdtIngestionFunctionApp.Model
{
    public class PositionModel
    {
        public double Base { get; set; }
        public double Shoulder { get; set; }
        public double Elbow { get; set; }
        public double Wrist1 { get; set; }
        public double Wrist2 { get; set; }
        public double Wrist3 { get; set; }
        public static PositionModel GetEmptyPositionModel()
        {
            PositionModel positionModel = new PositionModel();
            positionModel.Base = 0;
            positionModel.Shoulder = 0;
            positionModel.Elbow = 0;
            positionModel.Wrist1 = 0;
            positionModel.Wrist2 = 0;
            positionModel.Wrist3 = 0;
            return positionModel;
        }
        public static PositionModel GetPositionModel(JToken positionToken)
        {
            List<double> positionList = JsonConvert.DeserializeObject<List<double>>(positionToken.ToString());
            PositionModel positionModel = new PositionModel();
            positionModel.Base = RadiansToDegrees(radians:Convert.ToDouble(positionList[0]));
            positionModel.Shoulder = RadiansToDegrees(radians: Convert.ToDouble(positionList[1]));
            positionModel.Elbow = RadiansToDegrees(radians: Convert.ToDouble(positionList[2]));
            positionModel.Wrist1 = RadiansToDegrees(radians: Convert.ToDouble(positionList[3]));
            positionModel.Wrist2 = RadiansToDegrees(radians: Convert.ToDouble(positionList[4]));
            positionModel.Wrist3 = RadiansToDegrees(radians: Convert.ToDouble(positionList[5]));
            return positionModel;
        }
        static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }
    }
}
