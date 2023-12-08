// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.DigitalTwins.Core;
using System.Threading.Tasks;
using Azure;
using UodDtIotToAdtIngestionFunctionApp.Model;

namespace UodDtIotToAdtIngestionFunctionApp
{
    public static class Function
    {
        private static readonly string ADT_SERVICE_URL = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");

        [FunctionName("IotToAdtIngestionFunction")]
        public static async Task RunAsync([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            if (ADT_SERVICE_URL == null) log.LogError("Application setting 'ADT_SERVICE_URL' not set");
            else
                try
                {
                    DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();
                    DigitalTwinsClient digitalTwinsClient = new DigitalTwinsClient(new Uri(ADT_SERVICE_URL), defaultAzureCredential);
                    log.LogInformation($"ADT service client connection created.");

                    if (eventGridEvent != null && eventGridEvent.Data != null)
                    {
                        log.LogInformation(eventGridEvent.Data.ToString());
                        JObject eventGridDataJObject = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
                        string iotHubConnectionDeviceId = (string)eventGridDataJObject["systemProperties"]["iothub-connection-device-id"];
                        if (iotHubConnectionDeviceId.Equals("URCobot"))
                        {
                            JsonPatchDocument azureJsonPatchDocument = new JsonPatchDocument();
                            azureJsonPatchDocument.AppendAdd("/target_q", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["target_q"]));
                            azureJsonPatchDocument.AppendAdd("/target_qd", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["target_qd"]));
                            azureJsonPatchDocument.AppendAdd("/target_qdd", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["target_qdd"]));
                            azureJsonPatchDocument.AppendAdd("/target_current", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["target_current"]));
                            azureJsonPatchDocument.AppendAdd("/target_moment", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["target_moment"]));
                            azureJsonPatchDocument.AppendAdd("/actual_current", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["actual_current"]));
                            azureJsonPatchDocument.AppendAdd("/actual_q", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["actual_q"]));
                            azureJsonPatchDocument.AppendAdd("/actual_qd", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["actual_qd"]));
                            azureJsonPatchDocument.AppendAdd("/joint_control_output", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["joint_control_output"]));
                            azureJsonPatchDocument.AppendAdd("/actual_tcp_force", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["actual_tcp_force"]));
                            azureJsonPatchDocument.AppendAdd("/joint_temperatures", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["joint_temperatures"]));
                            azureJsonPatchDocument.AppendAdd("/joint_mode", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["joint_mode"]));
                            azureJsonPatchDocument.AppendAdd("/actual_tool_accelerometer", Position3Model.GetPosition3Model(positionToken: eventGridDataJObject["body"]["actual_tool_accelerometer"]));
                            azureJsonPatchDocument.AppendAdd("/speed_scaling", Convert.ToDouble(eventGridDataJObject["body"]["speed_scaling"]));
                            azureJsonPatchDocument.AppendAdd("/actual_momentum", Convert.ToDouble(eventGridDataJObject["body"]["actual_momentum"]));
                            azureJsonPatchDocument.AppendAdd("/actual_main_voltage", Convert.ToDouble(eventGridDataJObject["body"]["actual_main_voltage"]));
                            azureJsonPatchDocument.AppendAdd("/actual_robot_voltage", Convert.ToDouble(eventGridDataJObject["body"]["actual_robot_voltage"]));
                            azureJsonPatchDocument.AppendAdd("/actual_robot_current", Convert.ToDouble(eventGridDataJObject["body"]["actual_robot_current"]));
                            azureJsonPatchDocument.AppendAdd("/actual_joint_voltage", Position6Model.GetPosition6Model(positionToken: eventGridDataJObject["body"]["actual_joint_voltage"]));
                            azureJsonPatchDocument.AppendAdd("/runtime_state", Convert.ToInt32(eventGridDataJObject["body"]["runtime_state"]));
                            azureJsonPatchDocument.AppendAdd("/robot_mode", Convert.ToInt32(eventGridDataJObject["body"]["robot_mode"]));
                            azureJsonPatchDocument.AppendAdd("/safety_mode", Convert.ToInt32(eventGridDataJObject["body"]["safety_mode"]));
                            azureJsonPatchDocument.AppendAdd("/analog_io_types", Convert.ToInt32(eventGridDataJObject["body"]["analog_io_types"]));
                            azureJsonPatchDocument.AppendAdd("/io_current", Convert.ToDouble(eventGridDataJObject["body"]["io_current"]));
                            azureJsonPatchDocument.AppendAdd("/tool_mode", Convert.ToInt32(eventGridDataJObject["body"]["tool_mode"]));
                            azureJsonPatchDocument.AppendAdd("/tool_output_voltage", Convert.ToInt32(eventGridDataJObject["body"]["tool_output_voltage"]));
                            azureJsonPatchDocument.AppendAdd("/tool_output_current", Convert.ToDouble(eventGridDataJObject["body"]["tool_output_current"]));
                            await digitalTwinsClient.UpdateDigitalTwinAsync("URCobot", azureJsonPatchDocument);
                            azureJsonPatchDocument = new JsonPatchDocument();
                            azureJsonPatchDocument.AppendAdd("/IsHumanAssigned", false);
                            azureJsonPatchDocument.AppendAdd("/Position", PositionModel.GetPositionModel(positionToken: eventGridDataJObject["body"]["target_q"]));
                            await digitalTwinsClient.UpdateDigitalTwinAsync("URCobotT", azureJsonPatchDocument);
                            log.LogInformation("URCobot Digital Twin Updated");
                        }
                        else if (iotHubConnectionDeviceId.Equals("RobotiqGripper"))
                        {
                            JsonPatchDocument azureJsonPatchDocument = new JsonPatchDocument();
                            azureJsonPatchDocument.AppendAdd("/act", Convert.ToInt32(eventGridDataJObject["body"]["act"]));
                            azureJsonPatchDocument.AppendAdd("/gto", Convert.ToInt32(eventGridDataJObject["body"]["gto"]));
                            azureJsonPatchDocument.AppendAdd("/for", Convert.ToInt32(eventGridDataJObject["body"]["for"]));
                            azureJsonPatchDocument.AppendAdd("/spe", Convert.ToInt32(eventGridDataJObject["body"]["spe"]));
                            azureJsonPatchDocument.AppendAdd("/pos", Convert.ToInt32(eventGridDataJObject["body"]["pos"]));
                            azureJsonPatchDocument.AppendAdd("/sta", Convert.ToInt32(eventGridDataJObject["body"]["sta"]));
                            azureJsonPatchDocument.AppendAdd("/pre", Convert.ToInt32(eventGridDataJObject["body"]["pre"]));
                            azureJsonPatchDocument.AppendAdd("/obj", Convert.ToInt32(eventGridDataJObject["body"]["obj"]));
                            azureJsonPatchDocument.AppendAdd("/flt", Convert.ToInt32(eventGridDataJObject["body"]["flt"]));
                            await digitalTwinsClient.UpdateDigitalTwinAsync("RobotiqGripper", azureJsonPatchDocument);
                            azureJsonPatchDocument = new JsonPatchDocument();
                            azureJsonPatchDocument.AppendAdd("/IsHumanAssigned", false);
                            azureJsonPatchDocument.AppendAdd("/IsOpen", false);
                            await digitalTwinsClient.UpdateDigitalTwinAsync("RobotiqGripperT", azureJsonPatchDocument);
                            log.LogInformation("RobotiqGripper Digital Twin Updated");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError($"Error in ingest function: {ex.Message}");
                }
        }
    }
}
