Public Class BiTreeNode
    Public data As Integer

    Public leftTree As BiTreeNode
    Public rightTree As BiTreeNode
    Public height As Integer

    Public WithEvents labelOnCanvas As Label
    Public leftSpace As Integer
    Public rightSpace As Integer

    Private Sub willDelThisNode(sender As Object, e As EventArgs) Handles labelOnCanvas.Click
        If MessageBox.Show("Are you sure you want to DELETE this node?", "FBI Warning",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            AVLTree.delete(Me)
            CanvasForm.flash()
        End If
    End Sub

    Public Sub New(ByVal data As Integer)
        Me.data = data
        Me.height = 0
        Me.leftTree = Nothing
        Me.rightTree = Nothing
        Me.labelOnCanvas = New Label
        With Me.labelOnCanvas
            .Text = data.ToString()
            .AutoSize = True
            .BorderStyle = BorderStyle.FixedSingle
            .Font = New Font("宋体", 10)
        End With
    End Sub
End Class
