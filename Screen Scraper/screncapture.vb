Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.windows.forms

Module ScreenCapture

    Public Function ScreenCap(Optional IncludeMouse As Boolean = True) As Bitmap
        Dim bmp = New Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height, Imaging.PixelFormat.Format32bppRgb)
        Dim gdest As Graphics = Graphics.FromImage(bmp)
        Dim hDestDC As IntPtr = gdest.GetHdc()
        Dim gsrc As Graphics = Graphics.FromHwnd(GetDesktopWindow) ' or use, .FromHwnd(IntPtr.Zero)
        Dim hSrcDC As IntPtr = gsrc.GetHdc()
        BitBlt(hDestDC, 0, 0, bmp.Width, bmp.Height, hSrcDC, 0, 0, CInt(CopyPixelOperation.SourceCopy))
        If IncludeMouse Then
            Dim pcin As New CURSORINFO()
            pcin.cbSize = Marshal.SizeOf(pcin)
            If GetCursorInfo(pcin) Then
                Dim piinfo As ICONINFO
                If GetIconInfo(pcin.hCursor, piinfo) Then
                    DrawIcon(hDestDC, pcin.ptScreenPos.x - piinfo.xHotspot, pcin.ptScreenPos.y - piinfo.yHotspot, pcin.hCursor)
                    If Not piinfo.hbmMask.Equals(IntPtr.Zero) Then DeleteObject(piinfo.hbmMask)
                    If Not piinfo.hbmColor.Equals(IntPtr.Zero) Then DeleteObject(piinfo.hbmColor)
                End If
            End If
        End If
        gdest.ReleaseHdc()
        gdest.Dispose()
        gsrc.ReleaseHdc()
        gsrc.Dispose()
        Return bmp
    End Function

    ' API...
    <StructLayout(LayoutKind.Sequential)>
    Private Structure POINTAPI
        Public x As Int32
        Public y As Int32
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Private Structure CURSORINFO
        Public cbSize As Int32
        Public flags As Int32
        Public hCursor As IntPtr
        Public ptScreenPos As POINTAPI
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Private Structure ICONINFO
        Public fIcon As Boolean
        Public xHotspot As Int32
        Public yHotspot As Int32
        Public hbmMask As IntPtr
        Public hbmColor As IntPtr
    End Structure
    <DllImport("user32.dll", EntryPoint:="GetCursorInfo")>
    Private Function GetCursorInfo(ByRef pci As CURSORINFO) As Boolean
    End Function
    <DllImport("user32.dll")>
    Private Function DrawIcon(hDC As IntPtr, X As Int32, Y As Int32, hIcon As IntPtr) As Boolean
    End Function
    <DllImport("user32.dll", EntryPoint:="GetIconInfo")>
    Private Function GetIconInfo(hIcon As IntPtr, ByRef piconinfo As ICONINFO) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=False)>
    Private Function GetDesktopWindow() As IntPtr
    End Function
    <DllImport("gdi32.dll")>
    Private Function BitBlt(ByVal hdc As IntPtr, ByVal nXDest As Int32, ByVal nYDest As Int32, ByVal nWidth As Int32, ByVal nHeight As Int32, ByVal hdcSrc As IntPtr, ByVal nXSrc As Int32, ByVal nYSrc As Int32, ByVal dwRop As Int32) As Boolean
    End Function
    <DllImport("gdi32.dll")>
    Private Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
End Module