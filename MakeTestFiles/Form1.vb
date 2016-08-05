Imports System.IO
Imports System.Threading
Public Class Form1


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedItem() = ComboBox1.Items(0)
        Me.Icon = My.Resources.Icon1
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If (FolderBrowserDialog1.ShowDialog().ToString() = "OK") Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            TextBox1.Refresh()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If NumericUpDown1.Value > 0 And Directory.Exists(TextBox1.Text) Then
            Dim t As New Thread(New ParameterizedThreadStart(AddressOf MakeTread))
            t.Start({CUInt(NumericUpDown1.Value), TextBox1.Text, CUInt(NumericUpDown2.Value), ComboBox1.SelectedItem.ToString})
        End If
    End Sub

    Private Sub MakeTread(ByVal args As Object)
        Dim NumFiles As UInteger = args(0)
        Dim Filename As String = args(1)
        Dim SizeNum As UInteger = args(2)
        Dim SizeMult As String = args(3)
        For I As UInteger = 1 To NumFiles Step 1
            If Not CreateFile(Filename, CalculateSize(SizeMult, SizeNum)) Then
                MsgBox("File creation failed, aborting")
                Exit For
            End If
        Next
    End Sub

    Function CalculateSize(ByVal SizeMult As String, ByVal SizeNum As UInteger) As UInteger
        Dim Multi As UInteger
        If SizeMult = "BY" Then
            Multi = 1
        ElseIf SizeMult = "KB" Then
            Multi = 1000
        ElseIf SizeMult = "MB" Then
            Multi = 1000000
        ElseIf SizeMult = "GB" Then
            Multi = 1000000000
        Else
            Multi = 0
        End If
        Return SizeNum * Multi
    End Function

    Private Function CreateFile(ByVal Path As String, Size As UInteger) As Boolean
        Try
            Dim rand As New Random
            Dim buffer(9) As Byte
            Dim FileName As String = "TestFile.txt"
            Dim I As UInteger = 1
            Dim tFileName As String
            tFileName = FileName
            While File.Exists(Path & "\" & FileName)
                FileName = tFileName.Insert(tFileName.LastIndexOf("."), "(" & I & ")")
                I = I + 1
            End While
            Using fs As New FileStream(Path & "\" & FileName, FileMode.CreateNew)
                For I = 1 To Size Step 10
                    rand.NextBytes(buffer)
                    fs.Write(buffer, 0, 10)
                Next
            End Using
            Return True
        Catch
            Return False
        End Try
    End Function

End Class


