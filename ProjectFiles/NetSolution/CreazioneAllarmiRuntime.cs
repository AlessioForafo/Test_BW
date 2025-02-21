#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.CODESYS;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.OPCUAServer;
using FTOptix.Store;
using FTOptix.SQLiteStore;
#endregion

public class CreazioneAllarmiRuntime : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        for (int i = 0; i < 3000; i++)
        {
            var allarme = InformationModel.Make<AllarmeDigitaleBW>("Allarme_" + i);
            allarme.Message = "Testo allarme " + i;
            allarme.SetAlias("VarPLC", Project.Current.Get("CommDrivers/CODESYSDriver1/CODESYSStation1/Tags/Application/PLC_PRG/MainQueue/" + i));
            Project.Current.Get("Alarms/AllarmeIstanze").Add(allarme);
        }
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
