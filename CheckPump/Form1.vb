Imports System.Data.SqlClient
Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim conn As New SqlConnection(ConnStr)
            conn.Open()
        Catch ex As Exception
            MsgBox("ไม่สามารถเชื่อมต่อฐานข้อมูลได้", MsgBoxStyle.OkOnly)
            Application.Exit()
        End Try
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        Try

            If CheckIsExistDeleted() = True Then
                Using New Centered_MessageBox(Me)
                    Dim confirm As DialogResult = MessageBox.Show("มีการตั้งค่าไม่ถูกต้อง", "", MessageBoxButtons.OK)
                    Exit Sub
                End Using
            End If


            If CheckPump() = False Then
                Using New Centered_MessageBox(Me)
                    Dim confirm As DialogResult = MessageBox.Show("มีการตั้งค่าไม่ถูกต้อง", "", MessageBoxButtons.OK)
                    Exit Sub
                End Using
            End If

            Using New Centered_MessageBox(Me)
                Dim confirm As DialogResult = MessageBox.Show("ตั้งค่าถูกต้องแล้ว", "", MessageBoxButtons.OK)
                Exit Sub
            End Using

        Catch ex As Exception
            Using New Centered_MessageBox(Me)
                Dim confirm As DialogResult = MessageBox.Show(ex.ToString, "", MessageBoxButtons.OK)
            End Using
        End Try
    End Sub

    Function CheckIsExistDeleted() As Boolean
        'True = Exist, False = Not

        Dim dt As New DataTable
        Dim sql As String = "select 'y' from Hoses where deleted = '1'"
        Dim conn As New SqlConnection(ConnStr)
        conn.Open()
        Dim cmd As New SqlCommand
        With cmd
            .CommandText = sql
            .CommandType = CommandType.Text
            .Connection = conn
        End With

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        conn.Close()

        If dt.Rows.Count = 0 Then
            Return False
        End If

        Return True
    End Function

    Function CheckPump() As Boolean
        'True = Not Problem, False = Problem
        Dim dt As New DataTable
        Dim sql As String = "select Hose_ID,Pump_ID from Hoses order by hose_ID"
        Dim conn As New SqlConnection(ConnStr)
        conn.Open()
        Dim cmd As New SqlCommand
        With cmd
            .CommandText = sql
            .CommandType = CommandType.Text
            .Connection = conn
        End With

        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        conn.Close()

        If dt.Rows.Count = 0 Then
            Return False
        End If


        Dim tempPump As Integer = 1
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim pump As Integer = CInt(dt.Rows(i)("Pump_ID"))
            Dim hose As Integer = CInt(dt.Rows(i)("Hose_ID"))

            If hose <> i + 1 Then
                Return False
            End If

            If tempPump <> pump Then
                If tempPump + 1 <> pump Then
                    Return False
                Else
                    tempPump = pump
                End If
            End If

        Next
        Return True
    End Function


    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Application.Exit()
    End Sub


#Region "Connect Database"   'Connect Database
    'Dim Server As String = "10.195.2.177"
    'Dim Password As String = "pTT!CT01"
    Public ConnStr As String = getConnectionString()
    Function getConnectionString() As String

        Dim Server As String = "(local)"
        'Dim Server As String = "10.195.2.177"

        Dim Database As String = "ENABLERDB"
        Dim Username As String = "sa"

        'Dim Password As String = "1qaz@WSX"
        Dim Password As String = "pTT!CT01"


        'Dim ini As New IniReader(INIFile)
        'ini.Section = "Setting"
        Return "Data Source=" & Server & ";Initial Catalog=" & Database & ";User ID=" & Username & ";Password=" & Password & ";Connect Timeout=1;"
    End Function

#End Region

End Class
