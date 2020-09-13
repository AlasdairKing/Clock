VERSION 5.00
Begin VB.Form frmSystemTray 
   Caption         =   "Podcast Downloader"
   ClientHeight    =   3090
   ClientLeft      =   225
   ClientTop       =   825
   ClientWidth     =   4680
   Icon            =   "frmSystemTray.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3090
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.Menu mnuSystemTray 
      Caption         =   ""
      Begin VB.Menu mnuSystemTrayExit 
         Caption         =   "E&xit"
      End
   End
End
Attribute VB_Name = "frmSystemTray"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private mInSystemTray As Boolean
'PostMessage to let application close properly
Private Declare Function PostMessage Lib "User32" _
   Alias "PostMessageA" _
   (ByVal hwnd As Long, _
    ByVal wMsg As Long, _
    ByVal wParam As Long, _
    lParam As Any) As Long
Private Const WM_CLOSE As Long = &H10


Private Sub Form_Load()
    On Error Resume Next
     'add an icon to the system tray. If it is successful (returns 1) then subclass
     'to intercept messages
     
     If ShellTrayAdd = 1 Then
         'prepare to receive the systray messages
         SubClass frmSystemTray.hwnd
         mInSystemTray = True
    End If

End Sub

Private Sub Form_Unload(Cancel As Integer)
    On Error Resume Next
    If mInSystemTray Then
        'clear system tray icon
        ShellTrayRemove
        'remove subclassing
        UnSubClass
    End If
End Sub

Public Function ShellTrayAdd() As Long
    On Error Resume Next
 'prepare the NOTIFYICONDATA type with the
 'required parameters:
 
 '.cbSize: Size of this structure, in bytes.
 '
 '.hwnd:   Handle of the window that will receive
 '         notification messages associated with
 '         an icon in the taskbar status area.
 '
 'uID:     Application-defined identifier of
 '         the taskbar icon. In an application
 '         with a single tray icon, this can be
 '         an arbitrary number.  For apps with
 '         multiple icons, each icon ID must be
 '         different as this member identifies
 '         which of the icons was selected.
 '
 '.uFlags: flags that indicate which of the other
 '         members contain valid data. This member
 '         can be a combination of the following:
 '         NIF_ICON    hIcon member is valid.
 '         NIF_MESSAGE uCallbackMessage member is valid.
 '         NIF_TIP     szTip member is valid.
 '
 'uCallbackMessage: Application-defined message identifier.
 '         The system uses this identifier for
 '         notification messages that it sends
 '         to the window identified in hWnd.
 '         These notifications are sent when a
 '         mouse event occurs in the bounding
 '         rectangle of the icon. (Note: 'callback'
 '         is a bit misused here (in the context of
 '         other callback demonstrations); there is
 '         no systray-specific callback defined -
 '         instead the form itself must be subclassed
 '         to respond to this message.
 '
 'hIcon:   Handle to the icon to add, modify, or delete.
 '
 'szTip:   Tooltip text to display for the icon. Must
 '         be terminated with a Chr$(0).
 
 'Shell_NotifyIcon messages:
 'dwMessage: Message value to send. This parameter
 '           can be one of these values:
 '           NIM_ADD     Adds icon to status area
 '           NIM_DELETE  Deletes icon from status area
 '           NIM_MODIFY  Modifies icon in status area
 '
 'pnid:      Address of the prepared NOTIFYICONDATA.
 '           The content of the structure depends
 '           on the value of dwMessage.

   If NOTIFYICONDATA_SIZE = 0 Then SetShellVersion
    
   With NID
      .cbSize = NOTIFYICONDATA_SIZE
      .hwnd = Me.hwnd
      .uID = 125&
      .uFlags = NIF_ICON Or NIF_TIP Or NIF_MESSAGE
      .uCallbackMessage = WM_MYHOOK
      .hIcon = Me.Icon
      .szTip = GetText("Clock") & Chr$(0)
    End With
   
    ShellTrayAdd = Shell_NotifyIcon(NIM_ADD, NID)

End Function


Private Sub ShellTrayRemove()

  'Remove the icon from the taskbar
   Call Shell_NotifyIcon(NIM_DELETE, NID)
   
End Sub


Private Sub UnSubClass()

  'restore the default message handling
  'before exiting
   If defWindowProc Then
      SetWindowLong Me.hwnd, GWL_WNDPROC, defWindowProc
      defWindowProc = 0
   End If
   
End Sub


Private Sub SubClass(hwnd As Long)

  'assign our own window message
  'procedure (WindowProc)
  
   On Error Resume Next
   defWindowProc = SetWindowLong(hwnd, GWL_WNDPROC, AddressOf WindowProc)
   
End Sub

    
Private Sub mnuSystemTrayExit_Click()
    On Error Resume Next
        'Executing 'Unload Me' from within a
        'menu event invoked from a systray icon
        'will cause a GPF. The proper way to
        'terminate under these circumstances
        'is to send a WM_CLOSE message to the
        'form. The form will process the
        'message as though the user had selected
        'Close from the sysmenu, invoking the
        'normal chain of shutdown events, removing
        'the tray icon, terminating the subclassing
        'cleanly and ultimately preventing the GPF.
        '
        'This code can also be called directly from
        'the form's menu as well, so no special coding
        'is required to differentiate between an end
        'command from a popup systray menu, or from
        'a normal form menu.
        '
        'The UnloadMode of QueryUnload/UnloadMode
        'will equal vbFormControlMenu when this
        'close method is used.
         Call PostMessage(frmMain.hwnd, WM_CLOSE, 0&, ByVal 0&)
End Sub

