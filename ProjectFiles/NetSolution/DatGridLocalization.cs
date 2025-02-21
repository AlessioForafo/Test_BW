#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.CODESYS;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.OPCUAServer;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.Store;
using FTOptix.SQLiteStore;
#endregion

public class DatGridLocalization : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        var miaGrid = (DataGrid)Owner;
        LocalizedText ID = miaGrid.GetVariable("Columns/ID/DataItemTemplate/Text").Value;
        Log.Info(ID.ToString());
        Log.Info(miaGrid.GetVariable("Columns/DataGridColumn1/DataItemTemplate/Text/DynamicLink").Value);
        
        
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
