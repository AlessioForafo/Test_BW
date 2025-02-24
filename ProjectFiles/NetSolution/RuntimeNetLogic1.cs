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
    private DelayedTask myDelayedTask;

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



    [ExportMethod]
    public void SetMessage(string chiave)
    {
        var messaggioAllarme = new LocalizedText(chiave);

        if (InformationModel.LookupTranslation(messaggioAllarme).HasTranslation)
        {
            LogicObject.GetVariable("MessaggioCustom").Value = messaggioAllarme;
        }
    }

    [ExportMethod]
    public void attivaAllarme()
    {
        if (LogicObject.GetVariable("AttivaAllarme").Value == true)
            LogicObject.GetVariable("AttivaAllarme").Value = false;
        else
        {
            myDelayedTask = new DelayedTask(attiva_Allarme, 100, LogicObject);
            myDelayedTask.Start();
        }     
    }

    public override void Stop()
    {
        myDelayedTask?.Dispose();
    }

    private void attiva_Allarme()
    {
        LogicObject.GetVariable("AttivaAllarme").Value = !(LogicObject.GetVariable("AttivaAllarme").Value);
    }
}
