#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.OPCUAServer;
using FTOptix.CODESYS;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using System.Collections.Generic;
using System.Linq;
using FTOptix.Report;
using FTOptix.EventLogger;
#endregion

public class CreazioneMetodoSuAllarme : BaseNetLogic
{
  

        [ExportMethod]
        public void CreateEventHandlers()
        {
            var mainWindow = Project.Current.Get<WindowType>("UI/MainWindow");
            var generateReportButton = mainWindow.Get<Button>("GenerateReportButton");
            var changeColorButton = mainWindow.Get<Button>("ChangeColorButton");
            var button1 = mainWindow.Get<Button>("Button1");
            var button1BackgroundColor = button1.BackgroundColorVariable;

            // Make sure to remove old handlers (if any)
            var oldEventHandlers = generateReportButton.Children.OfType<FTOptix.CoreBase.EventHandler>().ToList();
            oldEventHandlers.AddRange(changeColorButton.Children.OfType<FTOptix.CoreBase.EventHandler>());
            foreach (var oldEventHandler in oldEventHandlers)
                oldEventHandler.Delete();

            // Report object that we want to create with a button
            var report1 = Project.Current.GetObject("Reports/Report1");

            MakeEventHandler( // Add the GeneratePdf method to the button
                generateReportButton, // Object where to create the method
                FTOptix.UI.ObjectTypes.MouseClickEvent, // Method to be created
                report1, // Object pointer where to call the method
                "GeneratePdf", // Method name
                new List<Tuple<string, NodeId, object>> // Method arguments
                {
                // Each row is a tuple, each tuple contains a string (name of the argument), a NodeId (DataType) and an Object (value)
                new("OutputPath", FTOptix.Core.DataTypes.ResourceUri, (string)ResourceUri.FromApplicationRelativePath("MyReport.pdf")),
                new("LocaleId", OpcUa.DataTypes.String, "en-US")
                }
            );

        MakeEventHandler( // Add the SetVariableValue method to the button
            changeColorButton, // Object where to create the method
            FTOptix.UI.ObjectTypes.MouseClickEvent, // Event to be added
            InformationModel.GetObject(FTOptix.CoreBase.Objects.VariableCommands), // Object that exposes the method
            "Set", // Name of the method
            new List<Tuple<string, NodeId, object>> // Method arguments
            {
                // Each row is a tuple, each tuple contains a string (name of the argument), a NodeId (DataType) and an Object (value)
                new("VariableToModify", FTOptix.Core.DataTypes.VariablePointer, NodeId.Empty),
                new("Value", FTOptix.Core.DataTypes.Color, new Color(0xff3480ebu).ARGB),
                new("ArrayIndex", OpcUa.DataTypes.UInt32, 0u)
            }


        );
        }
  
        private FTOptix.CoreBase.EventHandler MakeEventHandler(
        IUANode parentNode, // The parent node to which the event handler is to be added
        NodeId listenEventTypeId, // The NodeID of the event to be listened
        IUAObject callingObject, // The object on which the method is to be executed
        string methodName, // The name of the method to be called
        List<Tuple<string, NodeId, object>> arguments = null // List of input arguments (name, data type NodeID, value)
    )
    {
        // Create event handler object
        var eventHandler = InformationModel.MakeObject<FTOptix.CoreBase.EventHandler>("EventHandler");
        parentNode.Add(eventHandler);

        // Set the ListenEventType variable value to the Node ID of the event to be listened
        eventHandler.ListenEventType = listenEventTypeId;

        // Create method container
        // This must be an unique name if the multiple methods are added to the same button. A random name can also be used
        var methodIndex = eventHandler.MethodsToCall.Any() ? eventHandler.MethodsToCall.Count + 1 : 1;
        var methodContainer = InformationModel.MakeObject($"MethodContainer{methodIndex}");
        eventHandler.MethodsToCall.Add(methodContainer);

        // Create the ObjectPointer variable and set its value to the object on which the method is to be executed
        var objectPointerVariable = InformationModel.MakeVariable<NodePointer>("ObjectPointer", OpcUa.DataTypes.NodeId);
        objectPointerVariable.Value = callingObject.NodeId;
        methodContainer.Add(objectPointerVariable);

        // Create the Method variable and set its value to the name of the method to be called
        var methodNameVariable = InformationModel.MakeVariable("Method", OpcUa.DataTypes.String);
        methodNameVariable.Value = methodName;
        methodContainer.Add(methodNameVariable);

        if (arguments != null)
            CreateInputArguments(methodContainer, arguments);

        return eventHandler;
    }

    private void CreateInputArguments(
        IUANode methodContainer,
        List<Tuple<string, NodeId, object>> arguments)
    {
        IUAObject inputArguments = InformationModel.MakeObject("InputArguments");
        methodContainer.Add(inputArguments);

        foreach (var arg in arguments)
        {
            var argumentVariable = inputArguments.Context.NodeFactory.MakeVariable(
                NodeId.Random(inputArguments.NodeId.NamespaceIndex),
                arg.Item1,
                arg.Item2,
                OpcUa.VariableTypes.BaseDataVariableType,
                false,
                arg.Item3);

            inputArguments.Add(argumentVariable);
        }
    }
}
