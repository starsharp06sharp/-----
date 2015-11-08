Public Class ControlPannel
    Private Sub ControlPannel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AVLTree.root = Nothing
        CanvasForm.Show()
        Me.BringToFront()
    End Sub

    Public Shared Sub Sleep(ByVal Interval)
        Dim __time As DateTime = DateTime.Now
        Dim __Span As Int64 = Interval * 10000   '因为时间是以100纳秒为单位。   
        While (DateTime.Now.Ticks - __time.Ticks < __Span)
            Application.DoEvents()
        End While
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

    Private Sub btnPreOrder_Click(sender As Object, e As EventArgs) Handles btnPreOrder.Click
        Dim stack = New System.Collections.Stack
        If AVLTree.root IsNot Nothing Then stack.Push(New ergodic(AVLTree.root, 0))
        While stack.Count > 0
            Dim now = stack.Peek
            Select Case now.status
                Case 0
                    If now.node Is Nothing Then
                        stack.Pop()
                        Continue While
                    End If
                    CanvasForm.mark(now.node)
                    Sleep(1000)
                    now.node.labelOnCanvas.BackColor = Color.White
                    now.status = 1
                    stack.Pop()
                    stack.Push(now)
                    stack.Push(New ergodic(now.node.leftTree, 0))
                Case 1
                    now.status = 2
                    stack.Pop()
                    stack.Push(now)
                    stack.Push(New ergodic(now.node.rightTree, 0))
                Case 2
                    stack.Pop()
            End Select
        End While
    End Sub

    Private Sub btnMidOrder_Click(sender As Object, e As EventArgs) Handles btnMidOrder.Click
        Dim stack = New System.Collections.Stack
        If AVLTree.root IsNot Nothing Then stack.Push(New ergodic(AVLTree.root, 0))
        While stack.Count > 0
            Dim now = stack.Peek
            Select Case now.status
                Case 0
                    If now.node Is Nothing Then
                        stack.Pop()
                        Continue While
                    End If
                    now.status = 1
                    stack.Pop()
                    stack.Push(now)
                    stack.Push(New ergodic(now.node.leftTree, 0))
                Case 1
                    CanvasForm.mark(now.node)
                    Sleep(1000)
                    now.node.labelOnCanvas.BackColor = Color.White
                    now.status = 2
                    stack.Pop()
                    stack.Push(now)
                    stack.Push(New ergodic(now.node.rightTree, 0))
                Case 2
                    stack.Pop()
            End Select
        End While
    End Sub

    Private Sub btnAfterOrder_Click(sender As Object, e As EventArgs) Handles btnAfterOrder.Click
        Dim stack = New System.Collections.Stack
        If AVLTree.root IsNot Nothing Then stack.Push(New ergodic(AVLTree.root, 0))
        While stack.Count > 0
            Dim now = stack.Peek
            Select Case now.status
                Case 0
                    If now.node Is Nothing Then
                        stack.Pop()
                        Continue While
                    End If
                    now.status = 1
                    stack.Pop()
                    stack.Push(now)
                    stack.Push(New ergodic(now.node.leftTree, 0))
                Case 1
                    now.status = 2
                    stack.Pop()
                    stack.Push(now)
                    stack.Push(New ergodic(now.node.rightTree, 0))
                Case 2
                    CanvasForm.mark(now.node)
                    Sleep(1000)
                    now.node.labelOnCanvas.BackColor = Color.White
                    stack.Pop()
            End Select
        End While
    End Sub
End Class

Class ergodic
    Public node As BiTreeNode
    Public status As Int16
    Public Sub New(ByRef node As BiTreeNode, ByVal status As Int16)
        Me.node = node
        Me.status = status
    End Sub
End Class