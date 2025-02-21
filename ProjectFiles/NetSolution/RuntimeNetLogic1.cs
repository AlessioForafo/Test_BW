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

public class RuntimeNetLogic1 : BaseNetLogic
{
    private DigitalAlarm allarme;

    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        //allarme = (DigitalAlarm)Owner;
        
        //allarme.GetVariable("z_BW_ID").VariableChange += InputValueVariable_VariableChange;
        //allarme.LocalizedMessage = allarme.GetVariable("z_BW_ID").Value;
        //SetMessage();
        
    }

    //private void InputValueVariable_VariableChange(object sender, VariableChangeEventArgs e)
    //{
    //    SetMessage();
    //}

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        //allarme.GetVariable("z_BW_ID").VariableChange -= InputValueVariable_VariableChange;
    }

    [ExportMethod]
    public void SetMessage(string chiave)
    {
        var messaggioAllarme = new LocalizedText(chiave);
        
        if (InformationModel.LookupTranslation(messaggioAllarme).HasTranslation)
        {
            LogicObject.GetVariable("MessaggioCustom").Value = messaggioAllarme;
        }
    }
}
