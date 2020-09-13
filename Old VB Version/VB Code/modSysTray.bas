Attribute VB_Name = "modSysTray"
Option Explicit

'For Shell_Notify
Public Const NIM_ADD As Long = &H0
Public Const NIM_MODIFY As Long = &H1
Public Const NIM_DELETE As Long = &H2
Public Const NIF_ICON As Long = &H2     'adding icon
Public Const NIF_TIP As Long = &H4      'adding tooltip
Public Const NIF_MESSAGE As Long = &H1  'should get messages

Private Const WM_LBUTTONDOWN As Long = &H201
Private Const WM_LBUTTONUP As Long = &H202
Private Const WM_LBUTTONDBLCLK As Long = &H203
Private Const WM_MBUTTONDOWN As Long = &H207
Private Const WM_MBUTTONUP As Long = &H208
Private Const WM_MBUTTONDBLCLK As Long = &H209
Private Const WM_RBUTTONDOWN As Long = &H204
Public Const WM_RBUTTONUP As Long = &H205
Private Const WM_RBUTTONDBLCLK As Long = &H206

'shell version / NOTIFYICONDATA struct size constants
Public Const NOTIFYICONDATA_V1_SIZE As Long = 88  'pre-5.0 structure size
Public Const NOTIFYICONDATA_V2_SIZE As Long = 488 'pre-6.0 structure size
Public Const NOTIFYICONDATA_V3_SIZE As Long = 504 '6.0+ structure size
Public NOTIFYICONDATA_SIZE As Long

Public Type GUID
   Data1 As Long
   Data2 As Integer
   Data3 As Integer
   Data4(7) As Byte
End Type

Public Type NOTIFYICONDATA
   cbSize As Long
   hwnd As Long
   uID As Long
   uFlags As Long
   uCallbackMessage As Long
   hIcon As Long
   szTip As String * 128
   dwState As Long
   dwStateMask As Long
   szInfo As String * 256
   uTimeoutAndVersion As Long
   szInfoTitle As String * 64
   dwInfoFlags As Long
   guidItem As GUID
End Type

Public NID As NOTIFYICONDATA

Public Declare Function Shell_NotifyIcon Lib "shell32" _
   Alias "Shell_NotifyIconA" _
   (ByVal dwMessage As Long, _
   lpData As NOTIFYICONDATA) As Long
   
Private Declare Function GetFileVersionInfoSize Lib "version.dll" _
   Alias "GetFileVersionInfoSizeA" _
  (ByVal lptstrFilename As String, _
   lpdwHandle As Long) As Long

Private Declare Function GetFileVersionInfo Lib "version.dll" _
   Alias "GetFileVersionInfoA" _
  (ByVal lptstrFilename As String, _
   ByVal dwHandle As Long, _
   ByVal dwLen As Long, _
   lpData As Any) As Long
   
Private Declare Function VerQueryValue Lib "version.dll" _
   Alias "VerQueryValueA" _
  (pBlock As Any, _
   ByVal lpSubBlock As String, _
   lpBuffer As Any, _
   nVerSize As Long) As Long

Private Declare Sub CopyMemory Lib "kernel32" _
   Alias "RtlMoveMemory" _
  (Destination As Any, _
   Source As Any, _
   ByVal Length As Long)


Private Function IsShellVersion(ByVal version As Long) As Boolean

  'returns True if the Shell version
  '(shell32.dll) is equal or later than
  'the value passed as 'version'
   Dim nBufferSize As Long
   Dim nUnused As Long
   Dim lpBuffer As Long
   Dim nVerMajor As Integer
   Dim bBuffer() As Byte
   
   Const sDLLFile As String = "shell32.dll"
   
   nBufferSize = GetFileVersionInfoSize(sDLLFile, nUnused)
   
   If nBufferSize > 0 Then
    
      ReDim bBuffer(nBufferSize - 1) As Byte
    
      Call GetFileVersionInfo(sDLLFile, 0&, nBufferSize, bBuffer(0))
    
      If VerQueryValue(bBuffer(0), "\", lpBuffer, nUnused) = 1 Then
         
         CopyMemory nVerMajor, ByVal lpBuffer + 10, 2
        
         IsShellVersion = nVerMajor >= version
      
      End If  'VerQueryValue
   End If  'nBufferSize
  
End Function


Public Sub SetShellVersion()

   Select Case True
      Case IsShellVersion(6)
         NOTIFYICONDATA_SIZE = NOTIFYICONDATA_V3_SIZE '6.0+ structure size
      
      Case IsShellVersion(5)
         NOTIFYICONDATA_SIZE = NOTIFYICONDATA_V2_SIZE 'pre-6.0 structure size
      
      Case Else
         NOTIFYICONDATA_SIZE = NOTIFYICONDATA_V1_SIZE 'pre-5.0 structure size
   End Select

End Sub

