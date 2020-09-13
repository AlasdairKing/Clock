VERSION 5.00
Begin VB.Form frmReminder 
   Caption         =   "Reminder"
   ClientHeight    =   1320
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   4680
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   9.75
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmReminder.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   1320
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer tmrReminder 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   1800
      Top             =   480
   End
   Begin VB.CommandButton cmdStop 
      Cancel          =   -1  'True
      Caption         =   "Stop"
      Default         =   -1  'True
      Height          =   1095
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4455
   End
End
Attribute VB_Name = "frmReminder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private WithEvents voice As SpVoice
Attribute voice.VB_VarHelpID = -1
Private mReminder As String
Private mClosing As Boolean

Public Sub StartReminder(text As String)
    On Error Resume Next
    mReminder = text
    cmdStop.Caption = text
    tmrReminder.Enabled = True
    Me.Caption = modI18N.GetText("Reminder at") & " " & Format(Now, "hh:mm")
End Sub

Private Sub cmdStop_Click()
    On Error Resume Next
    Call Unload(Me)
End Sub

Private Sub Form_Load()
    On Error Resume Next
    Call modI18N.ApplyUILanguageToThisForm(Me)
    'Handle large fonts in Windows
    Call modLargeFonts.ApplySystemSettingsToForm(Me)
    Set voice = New SpVoice
End Sub

Private Sub Form_Unload(Cancel As Integer)
    On Error Resume Next
    mClosing = True
    tmrReminder.Enabled = False
    Call voice.Speak("", SVSFPurgeBeforeSpeak)
End Sub

Private Sub tmrReminder_Timer()
    On Error Resume Next
    tmrReminder.Enabled = False
    If Not mClosing Then
        Call Me.SetFocus
        Call voice.Speak(mReminder, SVSFlagsAsync)
    End If
End Sub

Private Sub voice_EndStream(ByVal StreamNumber As Long, ByVal StreamPosition As Variant)
    On Error Resume Next
    If Not mClosing Then
        tmrReminder.Enabled = True
    End If
End Sub
