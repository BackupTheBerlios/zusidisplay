Public Class System
    Public Function GetZusiHwnd() As Long
        Dim ZusiHwnd As Long
        'Handle des Zusifensters suchen, je nach Zusiversion ist es eines der beiden:
        'die übergebenen Werte sind die Klasse des Fensters und der Text der Titelleiste
        ZusiHwnd = FindWindow("TFormZusiD3DApplication", "Zusi")  'V2.4
        If ZusiHwnd = 0 Then 'wenn das noch 0 ist, wurde Zusi2.4 nicht gefunden
            ZusiHwnd = FindWindow("THauptform", "Zusi")    'V2.3
        End If
        'Rückgabewert
        GetZusiHwnd = ZusiHwnd
    End Function

    Public Declare Function SetForegroundWindow Lib _
    "user32" (ByVal hwnd As Long) As Long

    Public Declare Function FindWindow Lib "user32" _
      Alias "FindWindowA" (ByVal sClassName As String, _
      ByVal lpWindowName As String) As Long

    'eine API-Funktion:
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Long) As Integer

    Public Function GetKey(ByVal vKey As Long) As Integer
        GetKey = GetAsyncKeyState(vKey)

    End Function

    Public Function CompKey(ByVal KCode&) As Boolean
        Dim KCode2 As Long
        'speziell auf Zusi-Problematik zugeschnitten:
        'NUM ... und Funktionstasten (Pfeile, Einfg, Bild-UP/DN etc) sind identisch

        If KCode = 105 Then KCode2 = 33 'NUM 9
        If KCode = 99 Then KCode2 = 34 'NUM 3
        If KCode = 97 Then KCode2 = 35 'NUM 1
        If KCode = 103 Then KCode2 = 36 'NUM 7
        If KCode = 100 Then KCode2 = 37 'NUM 4
        If KCode = 104 Then KCode2 = 38 'NUM 8
        If KCode = 102 Then KCode2 = 39 'NUM 6
        If KCode = 98 Then KCode2 = 40 'NUM 2
        If KCode = 96 Then KCode2 = 45 'NUM 0
        If KCode = 110 Then KCode2 = 46 'NUM ,

        If GetAsyncKeyState(KCode) = -32767 Or GetAsyncKeyState(KCode2) = -32767 Then
            CompKey = True
        Else
            CompKey = False
        End If
    End Function

End Class
