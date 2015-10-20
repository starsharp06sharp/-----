Module AVLTree
    Public root As BiTreeNode

    Public Function search(ByVal key As Integer) As BiTreeNode
        Return search(root, key)
    End Function

    Public Function insert(ByVal data As Integer) As Boolean
        Try
            root = AVLTree.insert(root, data)
        Catch ex As Exception
            If ex.Message <> "ALready" Then
                Debug.WriteLine("An error occured: " & ex.Message)
            End If
            Return False
        End Try
        Return True
    End Function

    Public Sub delete(ByRef node As BiTreeNode)
        root = deleteNode(root, node.data)
    End Sub

    Public Sub destroy()
        root = Nothing
    End Sub

    Private Function max(ByVal a As Integer, ByVal b As Integer) As Integer
        Return CInt(IIf(a > b, a, b))
    End Function

    Private Function getHeight(ByRef p As BiTreeNode) As Integer
        If p Is Nothing Then Return -1
        Return p.height
    End Function

    Private Function search(ByRef node As BiTreeNode, ByVal key As Integer) As BiTreeNode
        If node Is Nothing Then
            Return Nothing
        ElseIf key < node.data Then
            Return search(node.leftTree, key)
        ElseIf node.data < key
            Return search(node.rightTree, key)
        Else
            Return node
        End If
    End Function

    Private Function findMax(ByRef node As BiTreeNode) As BiTreeNode
        If node.rightTree Is Nothing Then Return node
        Return findMax(node.rightTree)
    End Function

    Private Function getParent(ByRef node As BiTreeNode, ByVal data As Integer) As BiTreeNode
        If node Is Nothing OrElse node.data = data Then
            Return Nothing
        ElseIf (node.leftTree IsNot Nothing AndAlso node.leftTree.data = data) OrElse
                   (node.rightTree IsNot Nothing AndAlso node.rightTree.data = data) Then
            Return node
        ElseIf node.leftTree IsNot Nothing AndAlso data < node.leftTree.data Then
            Return getParent(node.leftTree, data)
        Else
            Return getParent(node.rightTree, data)
        End If
    End Function

    Private Function deleteNode(ByRef node As BiTreeNode, ByVal data As Integer) As BiTreeNode
        If node Is Nothing Then
            Return Nothing
        End If

        If data = node.data Then
            If node.leftTree Is Nothing Then
                Return node.rightTree
            ElseIf node.rightTree Is Nothing Then
                Return node.leftTree
            End If
            Dim max = findMax(node.leftTree)
            node.data = max.data
            node.labelOnCanvas.Text = node.data.ToString()
            If max IsNot node.leftTree Then
                Dim parent = getParent(node.leftTree, max.data)
                Debug.WriteLine(parent.data.ToString())
                parent.rightTree = max.leftTree
                If getHeight(parent.leftTree) - getHeight(parent.rightTree) > 1 Then
                    Dim grandParent = getParent(node, parent.data)
                    If (getHeight(parent.leftTree.rightTree) > getHeight(parent.leftTree.leftTree)) Then
                        grandParent = LRRotation(parent)
                    Else
                        grandParent = LLRotation(parent)
                    End If
                End If
            Else
                node.leftTree = max.leftTree
                If getHeight(node.rightTree) - getHeight(node.leftTree) > 1 Then
                    If getHeight(node.rightTree.leftTree) > getHeight(node.rightTree.rightTree) Then
                        node = RLRotation(node)
                    Else
                        node = RRRotation(node)
                    End If
                End If
            End If
        ElseIf data < node.data
            node.leftTree = deleteNode(node.leftTree, data)
            If getHeight(node.rightTree) - getHeight(node.leftTree) > 1 Then
                If node.rightTree IsNot Nothing Then
                    If (getHeight(node.rightTree.leftTree) > getHeight(node.rightTree.rightTree)) Then
                        node = RLRotation(node)
                    Else
                        node = RRRotation(node)
                    End If
                End If
            End If
        ElseIf data > node.data
            node.rightTree = deleteNode(node.rightTree, data)
            If getHeight(node.leftTree) - getHeight(node.rightTree) > 1 Then
                If (node.leftTree IsNot Nothing) Then
                    If (getHeight(node.leftTree.rightTree) > getHeight(node.leftTree.leftTree)) Then
                        node = LRRotation(node)
                    Else
                        node = LLRotation(node)
                    End If
                End If
            End If
        End If

        If node IsNot Nothing Then
            node.height = max(getHeight(node.leftTree), getHeight(node.rightTree)) + 1
        End If
        Return node
    End Function

    Private Function insert(ByRef node As BiTreeNode, ByVal data As Integer) As BiTreeNode
        If node Is Nothing Then
            Return New BiTreeNode(data)
        ElseIf data < node.data Then
            node.leftTree = insert(node.leftTree, data)
            If (getHeight(node.leftTree) - getHeight(node.rightTree) > 1) Then
                If (data < node.leftTree.data) Then
                    node = LLRotation(node)
                Else
                    node = LRRotation(node)
                End If
            End If
        ElseIf data > node.data Then
            node.rightTree = insert(node.rightTree, data)
            If (getHeight(node.rightTree) - getHeight(node.leftTree) > 1) Then
                If (data > node.rightTree.data) Then
                    node = RRRotation(node)
                Else
                    node = RLRotation(node)
                End If
            End If
        ElseIf data = node.data Then
            Throw New Exception("ALready")
        End If
        node.height = max(getHeight(node.leftTree), getHeight(node.rightTree)) + 1
        Return node
    End Function

    Private Function LLRotation(ByRef a As BiTreeNode) As BiTreeNode
        Dim b = a.leftTree
        a.leftTree = b.rightTree
        b.rightTree = a
        a.height = max(getHeight(a.leftTree), getHeight(a.rightTree)) + 1
        b.height = max(getHeight(b.leftTree), a.height) + 1
        Return b
    End Function

    Private Function LRRotation(ByRef a As BiTreeNode) As BiTreeNode
        a.leftTree = RRRotation(a.leftTree)
        Return LLRotation(a)
    End Function

    Private Function RRRotation(ByRef a As BiTreeNode) As BiTreeNode
        Dim b = a.rightTree
        a.rightTree = b.leftTree
        b.leftTree = a
        a.height = max(getHeight(a.leftTree), getHeight(a.rightTree)) + 1
        a.height = max(a.height, getHeight(b.rightTree)) + 1
        Return b
    End Function

    Private Function RLRotation(ByRef a As BiTreeNode) As BiTreeNode
        a.rightTree = LLRotation(a.rightTree)
        Return RRRotation(a)
    End Function
End Module
