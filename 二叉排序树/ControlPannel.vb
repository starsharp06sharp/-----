Public Class ControlPannel
    Private Sub ControlPannel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AVLTree.root = Nothing
        CanvasForm.Show()
        Me.BringToFront()
    End Sub

    Private Function checkInput() As Boolean
        If Not IsNumeric(txtNumber.Text) Then
            MessageBox.Show("输入不是数字！", "别闹", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        If txtNumber.Text.IndexOf(".") <> -1 Then
            MessageBox.Show("请勿输入小数！", "别闹", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        If Convert.ToInt64(txtNumber.Text) > Int32.MaxValue Then
            MessageBox.Show("输入数字过大！", "别闹", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        If Convert.ToInt64(txtNumber.Text) < Int32.MinValue Then
            MessageBox.Show("输入数字过小！", "别闹", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Return True
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If checkInput() AndAlso AVLTree.insert(CInt(txtNumber.Text)) Then
            CanvasForm.flash()
        End If
        txtNumber.Text = ""
    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        If checkInput() Then
            Dim ans = AVLTree.search(CInt(txtNumber.Text))
            If ans Is Nothing Then
                MessageBox.Show("没找到你要的数字", "不好意思", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                CanvasForm.clearMarked()
                CanvasForm.mark(ans)
            End If
        End If
    End Sub

    Private Sub btnDel_Click(sender As Object, e As EventArgs) Handles btnDel.Click
        If checkInput() Then
            Dim ans = AVLTree.search(CInt(txtNumber.Text))
            If ans Is Nothing Then
                MessageBox.Show("没找到你要的数字", "不好意思", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                AVLTree.delete(ans)
                CanvasForm.flash()
            End If
        End If
    End Sub

    Private Sub btnReadFromFile_Click(sender As Object, e As EventArgs) Handles btnReadFromFile.Click
        If ofdSelectTxt.ShowDialog() = DialogResult.OK Then
            If Not IO.File.Exists(ofdSelectTxt.FileName) Then
                MessageBox.Show("文件不存在", "别闹", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            Dim textFromFile As String
            Using readFileStream As IO.StreamReader = IO.File.OpenText(ofdSelectTxt.FileName)
                textFromFile = readFileStream.ReadToEnd()
            End Using
            Dim nums = textFromFile.Split({" ", ",", "，", vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            For Each num In nums
                If IsNumeric(num) AndAlso num.IndexOf(".") = -1 Then
                    AVLTree.insert(CInt(num))
                End If
            Next
            CanvasForm.flash()
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        AVLTree.destroy()
        CanvasForm.flash()
    End Sub
End Class