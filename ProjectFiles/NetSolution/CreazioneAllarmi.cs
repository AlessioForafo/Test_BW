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
using FTOptix.OPCUAServer;
using FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.Report;
using FTOptix.EventLogger;
#endregion

public class CreazioneAllarmi : BaseNetLogic
{
    [ExportMethod]
    public void GeneraAllarmi()
    {
        // Insert code to be executed by the method
        foreach (var item in Project.Current.Get("Alarms/AllarmeIstanze").Children)
        {
            item.Delete();
        }
        Log.Info("Allarmi cancellati!");
        
        for (int i = 0; i < 3000; i++)
        {
            var allarme = InformationModel.MakeObject("Allarme_" + i,Project.Current.Get("Alarms/AllarmeTipo/AllarmeDigitaleBW").NodeId);
            var allarmeAppoggio = (DigitalAlarm)allarme;
            allarmeAppoggio.Message = "Testo allarme " + i;
            //allarme.GetVariable("Message").Value = "Testo allarme " + i;
            allarme.SetAlias("VarPLC", Project.Current.Get("CommDrivers/CODESYSDriver1/CODESYSStation1/Tags/Application/PLC_PRG/MainQueue/" + i));
            Project.Current.Get("Alarms/AllarmeIstanze").Add(allarme);
    }
    }

    [ExportMethod]
    public void CancellaAllarmi()
    {
        // Insert code to be executed by the method
        foreach (var item in Project.Current.Get("Alarms/AllarmeIstanze").Children)
        {
            item.Delete();
        }
    }
}
