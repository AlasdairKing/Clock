Attribute VB_Name = "modWinProc"
Option Explicit

'defWindowProc: Variable to hold the ID of the
'               default window message processing
'               procedure. Returned by SetWindowLong.
Public defWindowProc As Long

'isSubclassed: flag indicating that subclassing
'              has been done. Provides the means
'              to call the correct message-handler.
Private isSubclassed As Boolean

'Get/SetWindowLong messages
Private Const GWL_WNDPROC As Long = (-4)
Private Const GWL_HWNDPARENT As Long = (-8)
Private Const GWL_ID As Long = (-12)
Private Const GWL_STYLE As Long = (-16)
Private Const GWL_EXSTYLE As Long = (-20)
Private Const GWL_USERDATA As Long = (-21)

'general windows messages
Private Const WM_USER As Long = &H400
Private Const WM_APP As Long = &H8000&
Public Const WM_MYHOOK As Long = WM_APP + &H15
Private Const WM_NOTIFY As Long = &H4E
Private Const WM_COMMAND As Long = &H111
Private Const WM_CLOSE As Long = &H10

Private Const WM_KEYDOWN As Integer = &H100
Private Const WM_KEYUP As Integer = &H101
Private Const WM_SYSKEYDOWN As Integer = &H104
Private Const WM_SYSKEYUP As Integer = &H105

Private Const WM_LBUTTONDOWN As Long = &H201
Private Const WM_LBUTTONUP As Long = &H202
Private Const WM_LBUTTONDBLCLK As Long = &H203
Private Const WM_MBUTTONDOWN As Long = &H207
Private Const WM_MBUTTONUP As Long = &H208
Private Const WM_MBUTTONDBLCLK As Long = &H209
Private Const WM_RBUTTONDOWN As Long = &H204
Public Const WM_RBUTTONUP As Long = &H205
Private Const WM_RBUTTONDBLCLK As Long = &H206

Private Declare Function SetForegroundWindow Lib "User32" _
   (ByVal hwnd As Long) As Long
   
Private Declare Function PostMessage Lib "User32" _
   Alias "PostMessageA" _
   (ByVal hwnd As Long, _
    ByVal wMsg As Long, _
    ByVal wParam As Long, _
    lParam As Any) As Long
    
Private Declare Function SetWindowLong Lib "User32" _
   Alias "SetWindowLongA" _
   (ByVal hwnd As Long, _
    ByVal nIndex As Long, _
    ByVal dwNewLong As Any) As Long

Private Declare Function CallWindowProc Lib "User32" _
   Alias "CallWindowProcA" _
   (ByVal lpPrevWndFunc As Long, _
    ByVal hwnd As Long, _
    ByVal uMsg As Long, _
    ByVal wParam As Long, _
    ByVal lParam As Long) As Long
                            
                            
'our own window message procedure
Public Function WindowProc(ByVal hwnd As Long, _
                           ByVal uMsg As Long, _
                           ByVal wParam As Long, _
                           ByVal lParam As Long) As Long

  'window message procedure
  '
  'If the handle returned is to our form,
  'call a form-specific message handler to
  'deal with the tray notifications.  If it
  'is a general system message, pass it on to
  'the default window procedure.
  '
  'If it is ours, we look at lParam for the
  'message generated, and react appropriately.
   On Error Resume Next
  
   Select Case hwnd
   
     'form-specific handler
      Case frmSystemTray.hwnd
         Select Case uMsg
           'WM_MYHOOK was defined as
           'the .uCallbackMessage
           'message of NOTIFYICONDATA
            Case WM_MYHOOK

              
              'lParam is the value of the message
              'that generated the tray notification.
            Select Case lParam
                  Case WM_RBUTTONUP
                    'maintain focus on the app
                    'window to assure the menu
                    'disappears should the mouse
                    'be clicked outside the menu
                    Call SetForegroundWindow(frmMain.hwnd)
                    'show the menu
                    With frmSystemTray
                        .PopupMenu .mnuSystemTray
                    End With
                Case WM_LBUTTONUP
                    frmMain.tmrShow.Enabled = True
                Case WM_LBUTTONDBLCLK
                    frmMain.tmrShow.Enabled = True
            End Select
            
           'handle any other form messages by
           'passing to the default message proc
            Case Else
            
               WindowProc = CallWindowProc(defWindowProc, _
                                            hwnd, _
                                            uMsg, _
                                            wParam, _
                                            lParam)
               Exit Function
            
         End Select

     
     'this takes care of messages when the
     'handle specified is not that of the form
      Case Else
      
          WindowProc = CallWindowProc(defWindowProc, _
                                      hwnd, _
                                      uMsg, _
                                      wParam, _
                                      lParam)
   End Select
   
End Function



