Imports System.ComponentModel

Public Class CanvasForm
    Private lines As Graphics

    Private Sub CanvasForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        lines = Me.CreateGraphics()
    End Sub

    Private Sub CanvasForm_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        If AVLTree.root Is Nothing Then Exit Sub
        drawLines(AVLTree.root)
    End Sub

    Private Sub CanvasForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        lines.Clear(Color.White)
        lines.Dispose()
        lines = Me.CreateGraphics()
        If AVLTree.root Is Nothing Then Exit Sub
        Me.outputTree(AVLTree.root)
        Me.drawLines(AVLTree.root)
    End Sub

    Private Sub CanvasForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        lines.Dispose()
    End Sub

    Public Sub clear()
        Me.Controls.Clear()
        lines.Clear(Color.White)
    End Sub

    Public Sub flash()
        Me.clear()
        If AVLTree.root Is Nothing Then Exit Sub
        Me.getSpace(AVLTree.root)
        Me.outputTree(AVLTree.root)
        Me.drawLines(AVLTree.root)
    End Sub

    Public Sub mark(ByRef node As BiTreeNode)
        node.labelOnCanvas.BackColor = Color.LightBlue
    End Sub

    Public Sub clearMarked()
        For Each label In Me.Controls
            If label.GetType.GetProperty("BackColor") IsNot Nothing Then
                CType(label, Label).BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub getSpace(ByRef node As BiTreeNode)
        Me.Controls.Add(node.labelOnCanvas)

        If node.leftTree Is Nothing Then
            node.leftSpace = 0
        Else
            getSpace(node.leftTree)
            node.leftSpace = node.leftTree.leftSpace + node.leftTree.labelOnCanvas.Size.Width + node.leftTree.rightSpace
        End If

        If node.rightTree Is Nothing Then
            node.rightSpace = 0
        Else
            getSpace(node.rightTree)
            node.rightSpace = node.rightTree.leftSpace + node.rightTree.labelOnCanvas.Size.Width + node.rightTree.rightSpace
        End If
    End Sub

    Private Sub outputTree(ByRef node As BiTreeNode)
        If node Is AVLTree.root Then
            node.labelOnCanvas.Location = New Point(CInt((Me.Size.Width - node.labelOnCanvas.Size.Width + node.leftSpace - node.rightSpace) / 2), 0)
        End If
        If node.leftTree IsNot Nothing Then
            node.leftTree.labelOnCanvas.Location = New Point(node.labelOnCanvas.Location.X - node.leftTree.labelOnCanvas.Size.Width - node.leftTree.rightSpace,
                                                              node.labelOnCanvas.Location.Y + node.labelOnCanvas.Size.Height + 10)
            outputTree(node.leftTree)
        End If
        If node.rightTree IsNot Nothing Then
            node.rightTree.labelOnCanvas.Location = New Point(node.labelOnCanvas.Location.X + node.labelOnCanvas.Size.Width + node.rightTree.leftSpace,
                                                               node.labelOnCanvas.Location.Y + node.labelOnCanvas.Size.Height + 10)
            outputTree(node.rightTree)
        End If
    End Sub

    Private Sub drawLines(ByRef node As BiTreeNode)
        If Not node.leftTree Is Nothing Then
            lines.DrawLine(Pens.Black,
                           Convert.ToSingle(node.labelOnCanvas.Location.X + node.labelOnCanvas.Size.Width / 2),
                           Convert.ToSingle(node.labelOnCanvas.Location.Y + node.labelOnCanvas.Size.Height),
                           Convert.ToSingle(node.leftTree.labelOnCanvas.Location.X + node.leftTree.labelOnCanvas.Size.Width / 2),
                           Convert.ToSingle(node.leftTree.labelOnCanvas.Location.Y))
            drawLines(node.leftTree)
        End If

        If Not node.rightTree Is Nothing Then
            lines.DrawLine(Pens.Black,
                           Convert.ToSingle(node.labelOnCanvas.Location.X + node.labelOnCanvas.Size.Width / 2),
                           Convert.ToSingle(node.labelOnCanvas.Location.Y + node.labelOnCanvas.Size.Height),
                           Convert.ToSingle(node.rightTree.labelOnCanvas.Location.X + node.rightTree.labelOnCanvas.Size.Width / 2),
                           Convert.ToSingle(node.rightTree.labelOnCanvas.Location.Y))
            drawLines(node.rightTree)
        End If

    End Sub
End Class
