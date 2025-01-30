#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.CODESYS;
using FTOptix.CommunicationDriver;
using FTOptix.Alarm;
#endregion

public class CreazioneAllarmi : BaseNetLogic
{
    [ExportMethod]
    public void GeneraAllarmi()
    {
        // Insert code to be executed by the method
        foreach (var item in Project.Current.Get("Alarms").Children)
        {
            item.Delete();
        }
        
        for (int i = 0; i < 3000; i++)
        {
            var allarme = InformationModel.Make<DigitalAlarm>("Allarme_" + i);
            allarme.InputValueVariable.SetDynamicLink(Project.Current.GetVariable("CommDrivers/CODESYSDriver1/CODESYSStation1/Tags/Application/PLC_PRG/MainQueue/" + i + "/ToAcknowledge"),DynamicLinkMode.Read);
            allarme.Message = "Testo allarme " + i;
            allarme.SeverityVariable.SetDynamicLink(Project.Current.GetVariable("CommDrivers/CODESYSDriver1/CODESYSStation1/Tags/Application/PLC_PRG/MainQueue/" + i + "/Tipo"),DynamicLinkMode.Read);
            allarme.AutoAcknowledge = true;
            allarme.AutoConfirm = true;
            Project.Current.Get("Alarms").Add(allarme);
    }
    }
}
