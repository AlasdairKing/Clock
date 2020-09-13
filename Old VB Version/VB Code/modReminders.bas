Attribute VB_Name = "modReminders"
Option Explicit

Private mReminders As Collection

Public Sub AddReminder(NewReminder As clsReminder)
    On Error Resume Next
    If mReminders Is Nothing Then Set mReminders = New Collection
    Call mReminders.Add(NewReminder)
End Sub

Public Sub Check()
    On Error Resume Next
    Dim d As Date
    Dim r As clsReminder
    Dim s As String
    
    d = Format(Now, "hh:mm")
    If Not mReminders Is Nothing Then
        For Each r In mReminders
            If r.fired <> d Then
                If r.when = d Then
                    s = s & r.reminder & vbNewLine
                    r.fired = d
                End If
            End If
        Next r
        If Len(s) > 0 Then
            Call Load(frmReminder)
            Call frmReminder.StartReminder(s)
            frmReminder.Visible = True
        End If
    End If
End Sub

Public Function reminders() As Collection
    On Error Resume Next
    Set reminders = mReminders
End Function

Public Sub SetReminders(doc As DOMDocument30)
    On Error Resume Next
    Dim n As IXMLDOMNode
    Dim r As clsReminder
    
    For Each n In doc.documentElement.selectNodes("reminderItem")
        If mReminders Is Nothing Then Set mReminders = New Collection
        Set r = New clsReminder
        Call r.ParseXML(n)
        Call mReminders.Add(r)
    Next n
End Sub

Public Sub DeleteReminder(r As clsReminder)
    On Error Resume Next
    Dim i As Long
    If Not (mReminders Is Nothing) Then
        For i = 1 To mReminders.Count
            If mReminders.Item(i) Is r Then
                Call mReminders.Remove(i)
                Exit For
            End If
        Next i
    End If
End Sub

