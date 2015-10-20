Module BSTree
    Public headNode As BiTreeNode

    Public Function searchNode(ByRef bt As BiTreeNode, ByVal key As Integer) As BiTreeNode
        If bt Is Nothing Then
            Return Nothing
        ElseIf key = bt.data Then
            Return bt
        ElseIf key < bt.data Then
            Return searchNode(bt.leftChild, key)
        Else
            Return searchNode(bt.rightChild, key)
        End If
    End Function

    Public Function inseartNode(ByRef bt As BiTreeNode, ByVal num As Integer) As Boolean
        If bt Is Nothing Then
            bt = New BiTreeNode(num)
            Return True
        End If
        If num < bt.data Then
            If bt.leftChild Is Nothing Then
                bt.leftChild = New BiTreeNode(num, bt)
                Return True
            Else
                If inseartNode(bt.leftChild, num) Then
                    Return True
                Else
                    Return False
                End If
            End If
        ElseIf num > bt.data Then
            If bt.rightChild Is Nothing Then
                bt.rightChild = New BiTreeNode(num, bt)
                Return True
            Else
                If inseartNode(bt.rightChild, num) Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else 'num = bt.data（不需插入）
            Return False
        End If
    End Function

    Public Sub deleteNode(ByRef bt As BiTreeNode)
        If bt.leftChild Is Nothing AndAlso bt.rightChild Is Nothing Then
            If bt.father Is Nothing Then
                headNode = Nothing
            ElseIf bt Is bt.father.leftChild Then
                bt.father.leftChild = Nothing
                bt.father = Nothing
            ElseIf bt Is bt.father.rightChild Then
                bt.father.rightChild = Nothing
                bt.father = Nothing
            End If
        ElseIf bt.leftChild Is Nothing Then
            If bt.father Is Nothing Then
                headNode = bt.rightChild
                bt.rightChild.father = Nothing
                bt.rightChild = Nothing
            ElseIf bt Is bt.father.leftChild Then
                bt.father.leftChild = bt.rightChild
                bt.rightChild.father = bt.father
                bt.father = Nothing
                bt.leftChild = Nothing
            ElseIf bt Is bt.father.rightChild Then
                bt.father.rightChild = bt.rightChild
                bt.rightChild.father = bt.father
                bt.father = Nothing
                bt.rightChild = Nothing
            End If
        ElseIf bt.rightChild Is Nothing Then
            If bt.father Is Nothing Then
                headNode = bt.leftChild
                bt.leftChild.father = Nothing
                bt.leftChild = Nothing
            ElseIf bt Is bt.father.leftChild Then
                bt.father.leftChild = bt.leftChild
                bt.leftChild.father = bt.father
                bt.father = Nothing
                bt.leftChild = Nothing
            ElseIf bt Is bt.father.rightChild Then
                bt.father.rightChild = bt.leftChild
                bt.leftChild.father = bt.father
                bt.father = Nothing
                bt.leftChild = Nothing
            End If
        Else
            Dim targetNode = bt.leftChild
            While targetNode.rightChild IsNot Nothing
                targetNode = targetNode.rightChild
            End While

            If targetNode Is bt.leftChild Then '特殊情况
                If bt.father Is Nothing Then
                    targetNode.father = Nothing
                    headNode = targetNode
                ElseIf bt Is bt.father.leftChild Then
                    targetNode.father = bt.father
                    bt.father.leftChild = targetNode
                ElseIf bt Is bt.father.rightChild Then
                    targetNode.father = bt.father
                    bt.father.rightChild = targetNode
                End If
                targetNode.rightChild = bt.rightChild
                bt.rightChild.father = targetNode
                bt.rightChild = Nothing
                bt.leftChild = Nothing
                bt.father = Nothing
                Exit Sub
            End If

            If targetNode.leftChild Is Nothing Then
                targetNode.father.rightChild = Nothing
            Else
                targetNode.father.rightChild = targetNode.leftChild
                targetNode.leftChild.father = targetNode.father
            End If
            targetNode.leftChild = bt.leftChild
            bt.leftChild.father = targetNode
            targetNode.rightChild = bt.rightChild
            bt.rightChild.father = targetNode
            bt.rightChild = Nothing
            bt.leftChild = Nothing

            If bt.father Is Nothing Then
                targetNode.father = Nothing
                headNode = targetNode
            ElseIf bt Is bt.father.leftChild Then
                bt.father.leftChild = targetNode
                targetNode.father = bt.father
            ElseIf bt Is bt.father.rightChild Then
                bt.father.rightChild = targetNode
                targetNode.father = bt.father
            End If

            bt.father = Nothing
        End If
    End Sub

End Module
