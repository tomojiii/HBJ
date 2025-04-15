Imports System.Xml

Public Class kysfmenu

    Private Sub ktif_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Call KYS_Initial()

            If My.Settings.update_fkg = "0" Then

                If UPDATE_CHECK() = True Then
                    My.Settings.update_fkg = "1"
                    My.Settings.Save()
                    Interaction.Shell("kyspcopy.exe", AppWinStyle.NormalFocus)
                    'MsgBox("OK")
                    Me.Close()
                    End
                End If

            End If

            My.Settings.update_fkg = "0"
            My.Settings.Save()

            Call form_init()
            Call form_clear()


        Catch ex As Exception

        End Try
    End Sub

    Sub form_init()
        Try
            PID.Text = "<" & Me.Name & ">"
            HIZUKE.Text = Now.ToString("yyyy/MM/dd")

        Catch ex As Exception

        End Try
    End Sub

    Sub form_clear()
        Try

        Catch ex As Exception

        End Try
    End Sub

    Private Sub HimfKey1_FKeyPress(sender As System.Object, e As System.EventArgs) Handles HimfKey1.FKeyPress
        Try
            Dim ky As KeyEventArgs = e

            Select Case ky.KeyData
                Case Keys.F1
                    Me.Close()
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click, Button2.Click, Button4.Click, Button5.Click, Button6.Click, Button8.Click
        Try
            Dim tar_bt As Button = CType(sender, Button)

            'Interaction.Shell("ktip050.exe", AppWinStyle.NormalFocus)
            Interaction.Shell(tar_bt.Tag, AppWinStyle.NormalFocus)
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub



    Function UPDATE_CHECK() As Boolean
        '更新ファイルがあるか判断する
        Dim ret As Boolean = False
        Dim xDocument As New XmlDocument
        Try

            Call xDocument.Load(CON_FILE) 'XMLファイルをロード 

            Dim src As XmlNode = xDocument.DocumentElement.SelectSingleNode("USERS/COPY_DIR/SRC")
            Dim dst As XmlNode = xDocument.DocumentElement.SelectSingleNode("USERS/COPY_DIR/DST")

            Dim src_dir As String = src.InnerText
            Dim dst_dir As String = dst.InnerText
            Dim fname As String
            Dim src_cdate As Date
            Dim dst_cdate As Date

            If src_dir.Substring(src_dir.Length - 1, 1) <> "\" Then
                src_dir &= "\"
            End If
            If dst_dir.Substring(dst_dir.Length - 1, 1) <> "\" Then
                dst_dir &= "\"
            End If

            If System.IO.Directory.Exists(src_dir) = False Then
                Exit Try
            End If

            If System.IO.Directory.Exists(dst_dir) = False Then
                Exit Try
            End If

            Dim di As New System.IO.DirectoryInfo(src_dir)
            Dim files As System.IO.FileInfo() = di.GetFiles("*.*", System.IO.SearchOption.AllDirectories)

            'ListBox1に結果を表示する
            For Each f As System.IO.FileInfo In files
                Dim src_fi As New System.IO.FileInfo(f.FullName)
                fname = src_fi.Name
                If System.IO.File.Exists(dst_dir & fname) = True Then
                    Dim dst_fi As New System.IO.FileInfo(dst_dir & fname)
                    src_cdate = src_fi.LastWriteTime
                    dst_cdate = dst_fi.LastWriteTime
                    If src_cdate.ToString("yyyy/MM/dd HH:mm:ss") > dst_cdate.ToString("yyyy/MM/dd HH:mm:ss") Then
                        ret = True
                        Exit For
                    End If
                Else
                    ret = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
        Return ret
    End Function


End Class

