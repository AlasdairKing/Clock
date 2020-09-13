VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "Clock"
   ClientHeight    =   2340
   ClientLeft      =   192
   ClientTop       =   816
   ClientWidth     =   15120
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   9.6
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmClock.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   2340
   ScaleWidth      =   15120
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer tmrShow 
      Enabled         =   0   'False
      Interval        =   433
      Left            =   7320
      Top             =   960
   End
   Begin VB.Timer tmrTime 
      Interval        =   200
      Left            =   1800
      Top             =   1320
   End
   Begin VB.TextBox txtTime 
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   72
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2280
      Left            =   0
      Locked          =   -1  'True
      TabIndex        =   0
      Top             =   0
      Width           =   9495
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuFileHide 
         Caption         =   "&Hide"
         Shortcut        =   ^H
      End
      Begin VB.Menu mnuFileExit 
         Caption         =   "E&xit"
      End
   End
   Begin VB.Menu mnuChimes 
      Caption         =   "&Chimes"
      Begin VB.Menu mnuChimeOptions 
         Caption         =   "&No chimes"
         Index           =   0
         Tag             =   "frmMain.mnuChimeOptions(0)"
      End
      Begin VB.Menu mnuChimeOptions 
         Caption         =   "Every &Hour"
         Index           =   1
         Tag             =   "frmMain.mnuChimeOptions(1)"
      End
      Begin VB.Menu mnuChimeOptions 
         Caption         =   "Every &Thirty Minutes"
         Index           =   2
         Tag             =   "frmMain.mnuChimeOptions(2)"
      End
      Begin VB.Menu mnuChimeOptions 
         Caption         =   "Every &Fifteen Minutes"
         Index           =   3
         Tag             =   "frmMain.mnuChimeOptions(3)"
      End
   End
   Begin VB.Menu mnuReminders 
      Caption         =   "&Reminders"
      Begin VB.Menu mnuRemindersAddreminder 
         Caption         =   "&Add Reminder"
      End
      Begin VB.Menu mnuBar1 
         Caption         =   "-"
         Visible         =   0   'False
      End
      Begin VB.Menu mnuReminderList 
         Caption         =   ""
         Index           =   0
         Visible         =   0   'False
      End
   End
   Begin VB.Menu mnuOptions 
      Caption         =   "&Options"
      Begin VB.Menu mnuOptions24hour 
         Caption         =   "24-&hour clock"
      End
      Begin VB.Menu mnuOptionsShowseconds 
         Caption         =   "&Show seconds"
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "&Help"
      Begin VB.Menu mnuHelpManual 
         Caption         =   "&Manual"
         Shortcut        =   {F1}
      End
      Begin VB.Menu mnuHelpAbout 
         Caption         =   "&About"
      End
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

'1.0.0
'   First version
'1.1.0
'   Added ability to minimize to system tray. 28 Jan 2009.
'1.1.1
'   Fixed I18N of system tray icon.
'1.1.2
'   Fixed XP Style problem with large font.
'1.1.3
'   11 Sep 2009     Fixed always being maximised when starting.
'1.1.4
'   30 Oct 2009     Fixed chimes in menu always called "no chimes"
'1.2.0
'   20 August 2010  Added option for 24-hour clock.
'                   Added option for showing seconds.
'1.2.1
'   11 October 2011 Turned off debug flag.

Public voice As SpVoice

Private Const NO_CHIMES As Long = 0
Private Const CHIME_HOUR As Long = 1
Private Const CHIME_HALF As Long = 2
Private Const CHIME_QUARTER As Long = 3

Private Sub Form_GotFocus()
    On Error Resume Next
    Call txtTime.SetFocus
End Sub

Private Sub Form_Initialize()
    On Error Resume Next
    Call modXPStyle.InitCommonControlsVB
End Sub

Private Sub Form_Load()
    On Error Resume Next
    Set voice = New SpVoice
    Call modPath.DetermineSettingsPath(App.companyName, App.Title, "1")
    Call modI18N.ApplyUILanguageToThisForm(Me)
    Debug.Print modI18N.GetLanguage
    'Handle large fonts in Windows
    Call modLargeFonts.ApplySystemSettingsToForm(Me, , True)
    Me.Show
    Call modRememberPosition.LoadPosition(Me)
    Call modUpdate.CheckForUpdates
    
    'Load settings
    mnuOptionsShowseconds.Checked = modPath.GetSettingIni("Clock", "Display", "ShowSeconds", CStr(False))
    mnuOptions24hour.Checked = modPath.GetSettingIni("Clock", "Display", "24Hour", IIf(modI18N.GetLanguage = "en-us", CStr(False), CStr(True)))
    
    Call LoadReminders
    Call DisplayReminders
    Call tmrTime_Timer
    Call mnuChimeOptions_Click(CInt(modPath.GetSettingIni("Clock", "Chimes", "ChimeFrequency", CStr(CHIME_HOUR))))
End Sub

Private Sub Form_Resize()
    On Error Resume Next
    txtTime.Top = 0
    txtTime.Left = 0
    txtTime.Width = Me.ScaleWidth
End Sub

Private Sub Form_Unload(Cancel As Integer)
    On Error Resume Next
    Dim f As Form
    Dim i As Integer
    
    For i = mnuChimeOptions.LBound To mnuChimeOptions.UBound
        If mnuChimeOptions.Item(i).Checked Then Call modPath.SaveSettingIni("Clock", "Chimes", "ChimeFrequency", CStr(i))
    Next i
    
    'Save settings
    Call modPath.SaveSettingIni("Clock", "Display", "ShowSeconds", mnuOptionsShowseconds.Checked)
    Call modPath.SaveSettingIni("Clock", "Display", "24Hour", mnuOptions24hour.Checked)
    
    Call modRememberPosition.SavePosition(Me)
    Call SaveReminders
    For Each f In Forms
        If f.Name <> Me.Name Then
            Call Unload(f)
        End If
    Next f
End Sub

Private Sub mnuChimeOptions_Click(index As Integer)
    On Error Resume Next
    Dim i As Long
    
    For i = mnuChimeOptions.LBound To mnuChimeOptions.UBound
        mnuChimeOptions(i).Checked = (i = index)
    Next i
End Sub

Private Sub mnuFileExit_Click()
    On Error Resume Next
    Call Unload(Me)
End Sub

Private Sub mnuFileHide_Click()
    On Error Resume Next
    Call Load(frmSystemTray)
    Me.Visible = False
End Sub

Private Sub mnuHelpAbout_Click()
    On Error Resume Next
    MsgBox App.Title & vbTab & App.Major & "." & App.Minor & "." & App.Revision & vbNewLine & "Package Version" & vbTab & modVersion.GetPackageVersion & vbNewLine & "Alasdair King, http://www.webbie.org.uk", vbInformation, App.Title
End Sub

Private Sub mnuHelpManual_Click()
    On Error Resume Next
    Call Load(frmHelp)
    frmHelp.Icon = Me.Icon
    Call frmHelp.Show(, Me)
End Sub

Private Sub mnuOptions24hour_Click()
    On Error Resume Next
    mnuOptions24hour.Checked = Not mnuOptions24hour.Checked
End Sub

Private Sub mnuOptionsShowseconds_Click()
    On Error Resume Next
    mnuOptionsShowseconds.Checked = Not mnuOptionsShowseconds.Checked
End Sub

Private Sub mnuReminderList_Click(index As Integer)
    On Error Resume Next
    Call frmSchedule.SetReminder(modReminders.reminders.Item(index + 1))
    Call frmSchedule.Show(vbModal, Me)
    Call DisplayReminders
End Sub

Private Sub mnuRemindersAddreminder_Click()
    On Error Resume Next
    Call frmSchedule.NewReminder
    Call frmSchedule.Show(vbModal, Me)
    Call DisplayReminders
End Sub

Private Sub tmrShow_Timer()
    On Error Resume Next
    Static busy As Boolean
    If busy Then
    Else
        busy = True
        tmrShow.Enabled = False
        Me.Visible = True
        DoEvents
        Call Me.SetFocus
        busy = False
    End If
End Sub

Private Sub tmrTime_Timer()
    On Error Resume Next
    Dim d As Date
    Static lastPlayedHour As Date
    Static lastPlayed15 As Date
    Static lastPlayed30 As Date
    Dim timeStyle As String
    Dim newTime As String
    
    timeStyle = modI18N.GetText("TimeStyle")
    If timeStyle = "TimeStyle" Then
        timeStyle = "hh:mm ddd mm yyyy"
    End If
    If Me.mnuOptions24hour.Checked Then
        timeStyle = Replace(timeStyle, " AMPM", "")
    Else
        'AM/PM view
        If InStr(1, timeStyle, "hh") > 0 Then timeStyle = Replace(timeStyle, "hh", "h", , 1)
        If InStr(1, timeStyle, "AMPM") = 0 Then
            'Need to add it
            timeStyle = Replace(timeStyle, ":mm:ss ", ":mm:ss AMPM ")
            If InStr(1, timeStyle, "AMPM") = 0 Then
                timeStyle = Replace(timeStyle, ":mm ", ":mm AMPM ")
            End If
        Else
            'Already there.
        End If
    End If
    If Me.mnuOptionsShowseconds.Checked Then
        'Need to add seconds
        If InStr(1, timeStyle, ":ss") = 0 Then
            timeStyle = Replace(timeStyle, ":mm ", ":mm:ss ", , 1)
        Else
            'Already there!
        End If
    Else
        'Need to remove seconds
        timeStyle = Replace(timeStyle, ":ss ", " ")
    End If
    newTime = Format(Now, timeStyle)
    If newTime <> txtTime.text Then txtTime.text = newTime
    If mnuChimeOptions(CHIME_HOUR).Checked Then
        d = CDate(Format(Now, "hh:mm"))
        If d <> lastPlayedHour Then
            If Right(Format(d, "hh:mm"), 2) = "00" Then
                lastPlayedHour = d
                Call voice.Speak(modI18N.GetText("Hour. It's now") & " " & Format(d, "hh:mm"))
            End If
        End If
    End If
    If mnuChimeOptions(CHIME_HALF).Checked Then
        d = CDate(Format(Now, "hh:mm"))
        If d <> lastPlayed30 Then
            If Right(Format(d, "hh:mm"), 2) = "00" Or Right(Format(d, "hh:mm"), 2) = "30" Then
                lastPlayed30 = d
                Call voice.Speak(modI18N.GetText("Half hour. It's now") & " " & Format(d, "hh:mm"))
            End If
        End If
    End If
    If mnuChimeOptions(CHIME_QUARTER).Checked Then
        d = CDate(Format(Now, "hh:mm"))
        If d <> lastPlayed15 Then
            If Right(Format(d, "hh:mm"), 2) = "00" Or Right(Format(d, "hh:mm"), 2) = "15" Or Right(Format(d, "hh:mm"), 2) = "30" Or Right(Format(d, "hh:mm"), 2) = "45" Then
                lastPlayed15 = d
                Call voice.Speak(modI18N.GetText("Quarter Hour. It's now") & " " & Format(d, "hh:mm"))
            End If
        End If
    End If
    Call modReminders.Check
End Sub

Private Sub txtTime_GotFocus()
    On Error Resume Next
    'Don't think I should do this: inverts the colour scheme selected by the user, and a screenreader
    'will read out the change anyway.
'    txtTime.SelStart = 0
'    txtTime.SelLength = Len(txtTime.text)
End Sub

Private Sub txtTime_KeyPress(KeyAscii As Integer)
    On Error Resume Next
    If KeyAscii = vbKeyReturn Then
        Call voice.Speak(txtTime.text)
        KeyAscii = False
    End If
End Sub

Private Sub DisplayReminders()
    On Error Resume Next
'Goes through the collection of reminders, displaying them in the Reminders menu.
    Dim i As Long
    
    If modReminders.reminders Is Nothing Then
    Else
        If modReminders.reminders.Count = 0 Then
            For i = mnuReminderList.UBound To 0 Step -1
                If i = 0 Then
                    mnuReminderList(0).Caption = ""
                    mnuReminderList(0).Visible = False
                    mnuBar1.Visible = False
                Else
                    Call Unload(mnuReminderList(i))
                End If
            Next i
        Else
            For i = 1 To reminders.Count
                If mnuReminderList.UBound + 1 < i Then
                    Call Load(mnuReminderList(i - 1))
                End If
                mnuBar1.Visible = True
                mnuReminderList(i - 1).Visible = True
                mnuReminderList(i - 1).Caption = reminders(i).reminder & " " & Format(reminders(i).when, "hh:mm")
            Next i
            For i = mnuReminderList.UBound To reminders.Count Step -1
                If i > 0 Then
                    Call Unload(mnuReminderList(i))
                End If
            Next i
        End If
    End If
End Sub

Private Sub SaveReminders()
    On Error Resume Next
    Dim r As clsReminder
    Dim xml As String
    Dim doc As DOMDocument30
    
    If Not (modReminders.reminders Is Nothing) Then
        xml = "<reminders>" & vbNewLine
        For Each r In modReminders.reminders
            xml = xml & r.toXML & vbNewLine
        Next r
        xml = xml & "</reminders>"
        Set doc = New DOMDocument30
        doc.async = False
        Call doc.loadXML(xml)
        Call doc.save(modPath.settingsPath & "\reminders.xml")
    End If
End Sub

Private Sub LoadReminders()
    On Error Resume Next
    Dim doc As DOMDocument30
    Dim n As IXMLDOMNode
    
    Set doc = New DOMDocument30
    doc.async = False
    
    If Len(Dir(modPath.settingsPath & "\reminders.xml", vbNormal)) > 0 Then
        Call doc.Load(modPath.settingsPath & "\reminders.xml")
        Call modReminders.SetReminders(doc)
    End If
End Sub
