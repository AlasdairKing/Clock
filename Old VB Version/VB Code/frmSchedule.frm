VERSION 5.00
Begin VB.Form frmSchedule 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Schedule Reminder"
   ClientHeight    =   2160
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   6270
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   9.75
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmSchedule.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2160
   ScaleWidth      =   6270
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdDelete 
      Caption         =   "&Delete this reminder"
      Height          =   495
      Left            =   120
      TabIndex        =   4
      Top             =   1560
      Width           =   3255
   End
   Begin VB.TextBox txtTime 
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   4455
   End
   Begin VB.TextBox txtReminder 
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   1080
      Width           =   4455
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   495
      Left            =   4920
      TabIndex        =   6
      Top             =   720
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Enabled         =   0   'False
      Height          =   495
      Left            =   4920
      TabIndex        =   5
      Top             =   120
      Width           =   1215
   End
   Begin VB.Label lblReminder 
      AutoSize        =   -1  'True
      Caption         =   "&Reminder"
      Height          =   240
      Left            =   120
      TabIndex        =   2
      Top             =   840
      Width           =   825
   End
   Begin VB.Label lblTime 
      AutoSize        =   -1  'True
      Caption         =   "Time"
      Height          =   240
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   435
   End
End
Attribute VB_Name = "frmSchedule"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private mCurrentReminder As clsReminder
Private REMINDER_PROMPT As String

Private Sub cmdCancel_Click()
    On Error Resume Next
    Call Me.Hide
End Sub

Private Sub cmdDelete_Click()
    On Error Resume Next
    Call modReminders.DeleteReminder(mCurrentReminder)
    Call Me.Hide
End Sub

Private Sub cmdOK_Click()
    On Error Resume Next
    Dim r As New clsReminder
    
    If mCurrentReminder Is Nothing Then
        Set r = New clsReminder
        r.reminder = txtReminder.text
        r.when = CDate(txtTime.text)
        Call modReminders.AddReminder(r)
    Else
        mCurrentReminder.reminder = txtReminder.text
        mCurrentReminder.when = CDate(txtTime.text)
    End If
    Call Me.Hide
End Sub


Private Sub Form_Initialize()
    On Error Resume Next
    Call modXPStyle.InitCommonControlsVB
End Sub

Private Sub Form_Load()
    On Error Resume Next
    Call modI18N.ApplyUILanguageToThisForm(Me)
    'Handle large fonts in Windows
    Call modLargeFonts.ApplySystemSettingsToForm(Me)
    REMINDER_PROMPT = modI18N.GetText("Type your reminder here.")
    
'    Dim i As Long
'    Dim h As String
    
'    For i = 0 To 23
'        h = i
'        If Len(h) = 1 Then h = "0" & i
'        Call lstTime.AddItem(h & ":00")
'        Call lstTime.AddItem(h & ":15")
'        Call lstTime.AddItem(h & ":30")
'        Call lstTime.AddItem(h & ":45")
'    Next i
'    lstTime.ListIndex = 0
End Sub

Public Sub SetReminder(r As clsReminder)
    On Error Resume Next
    txtTime.text = Format(r.when, "hh:mm")
    txtReminder.text = r.reminder
    txtReminder.SelStart = 0
    Set mCurrentReminder = r
    cmdOK.Enabled = True
    cmdDelete.Enabled = True
End Sub

Public Sub NewReminder()
    On Error Resume Next
    Set mCurrentReminder = Nothing
    txtReminder.text = REMINDER_PROMPT
    txtReminder.SelStart = 0
    txtTime = Format(Now + 1 / 24, "hh:mm")
    cmdOK.Enabled = False
    cmdDelete.Enabled = False
End Sub

Private Sub txtReminder_Change()
    On Error Resume Next
    cmdOK.Enabled = (Len(txtReminder.text) > 0)
End Sub

Private Sub txtReminder_GotFocus()
    On Error Resume Next
    If txtReminder.SelStart = 0 Then
        txtReminder.SelLength = Len(txtReminder.text)
    End If
End Sub

Private Sub txtTime_GotFocus()
    On Error Resume Next
    txtTime.SelStart = 0
    txtTime.SelLength = Len(txtTime.text)
End Sub

Private Sub txtTime_KeyDown(KeyCode As Integer, Shift As Integer)
    On Error Resume Next
    Dim d As Date
    
    If KeyCode = vbKeyUp Then
        KeyCode = 0
        d = CDate(txtTime.text)
        d = d - CDate(1 / 24 / 60)
        txtTime.text = Format(d, "hh:mm")
        txtTime.SelLength = Len(txtTime.text)
    ElseIf KeyCode = vbKeyDown Then
        KeyCode = 0
        d = CDate(txtTime.text)
        d = d + CDate(1 / 24 / 60)
        txtTime.text = Format(d, "hh:mm")
        txtTime.SelLength = Len(txtTime.text)
    End If
End Sub

