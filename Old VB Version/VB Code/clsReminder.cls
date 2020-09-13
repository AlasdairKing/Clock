VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsReminder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Public when As Date
Public reminder As String
Public fired As Date
Public messageBox As Boolean

Public Function toXML() As String
    On Error Resume Next
    toXML = "<reminderItem><when>" & when & "</when><reminder>" & reminder & "</reminder><fired>" & fired & "</fired></reminderItem>"
End Function

Public Function ParseXML(reminderNode As IXMLDOMNode)
    On Error Resume Next
    when = CDate(reminderNode.selectSingleNode("when").text)
    reminder = reminderNode.selectSingleNode("reminder").text
    fired = CDate(reminderNode.selectSingleNode("fired").text)
End Function
