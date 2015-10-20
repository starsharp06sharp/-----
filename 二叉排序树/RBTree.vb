Module RBTree
    Public Const BLACK = True
    Public Const RED = False

    Public root As BiTreeNode
    Public NIL As BiTreeNode

    Public Sub init()
        NIL = New BiTreeNode(0)
        NIL.color = BLACK
        root = Nothing
    End Sub

    Public Function search(ByVal data As Integer) As BiTreeNode
        If root Is Nothing Then Return Nothing
        Return search(root, data)
    End Function

    Public Function insert(ByVal num As Integer) As Boolean
        If root Is Nothing Then
            root = New BiTreeNode(num)
            root.color = BLACK
            root.leftTree = NIL
            root.rightTree = NIL
            Return True
        Else
            Return insert(root, num)
        End If
    End Function

    Public Sub delete(ByRef p As BiTreeNode)
        If p.rightTree Is NIL Then
            deleteOneChild(p)
        Else
            Dim smallest = getSmallestChild(p.rightTree)
            swap(p.data, smallest.data)
            deleteOneChild(smallest)
        End If
    End Sub

    Private Sub swap(ByRef a As Integer, ByRef b As Integer)
        Dim t As Integer
        t = a
        a = b
        b = t
    End Sub

    Private Function search(ByRef p As BiTreeNode, ByVal data As Integer) As BiTreeNode
        If p.data > data Then
            If p.leftTree IsNot NIL Then
                Return search(p.leftTree, data)
            Else
                Return Nothing
            End If
        ElseIf p.data < data Then
            If p.rightTree IsNot NIL Then
                Return search(p.rightTree, data)
            Else
                Return Nothing
            End If
        Else
            Return p
        End If
    End Function

    Private Sub rotateRight(ByRef p As BiTreeNode)
        Dim gp = p.grandparent()
        Dim fa = p.parent
        Dim y = p.rightTree

        fa.leftTree = y

        If y IsNot NIL Then y.parent = fa
        p.rightTree = fa
        fa.parent = p

        If fa Is root Then root = p
        p.parent = gp

        If gp IsNot Nothing Then
            If fa Is gp.leftTree Then
                gp.leftTree = p
            Else
                gp.rightTree = p
            End If
        End If
    End Sub

    Private Sub rotateLeft(ByRef p As BiTreeNode)
        If p.parent Is Nothing Then
            root = p
            Exit Sub
        End If
        Dim gp = p.grandparent()
        Dim fa = p.parent
        Dim y = p.leftTree

        fa.rightTree = y

        If y IsNot NIL Then y.parent = fa
        p.leftTree = fa
        fa.parent = p

        If fa Is root Then root = p
        p.parent = gp

        If gp IsNot Nothing Then
            If fa Is gp.leftTree Then
                gp.leftTree = p
            Else
                gp.rightTree = p
            End If
        End If
    End Sub

    Private Function getSmallestChild(ByRef p As BiTreeNode) As BiTreeNode
        If p.leftTree Is NIL Then Return p
        Return getSmallestChild(p.leftTree)
    End Function

    Private Sub deleteOneChild(ByRef p As BiTreeNode)
        Dim child As BiTreeNode
        If p.leftTree Is NIL Then
            child = p.rightTree
        Else
            child = p.leftTree
        End If
        If p.parent Is Nothing AndAlso
           p.leftTree Is NIL AndAlso
           p.rightTree Is NIL Then
            p = Nothing
            root = p
            Exit Sub
        End If

        If p.parent Is Nothing Then
            child.parent = Nothing
            root = child
            root.color = BLACK
            Exit Sub
        End If

        If p Is p.parent.leftTree Then
            p.parent.leftTree = child
        Else
            p.parent.rightTree = child
        End If
        child.parent = p.parent

        If p.color = BLACK Then
            If child.color = RED Then
                child.color = BLACK
            Else
                deleteCase(child)
            End If
        End If
    End Sub

    Private Sub deleteCase(ByRef p As BiTreeNode)
        If p.parent Is Nothing Then
            p.color = BLACK
            Exit Sub
        End If
        If p.sibling().color = RED Then
            p.parent.color = RED
            p.sibling().color = BLACK
            If p Is p.parent.leftTree Then
                rotateLeft(p.sibling())
            Else
                rotateRight(p.sibling())
            End If
        End If
        If p.parent.color = BLACK AndAlso
           p.sibling().color = BLACK AndAlso
           p.sibling().leftTree.color = BLACK AndAlso
           p.sibling().rightTree.color = BLACK Then
            p.sibling().color = RED
            deleteCase(p.parent)
        ElseIf p.parent.color = RED AndAlso
               p.sibling().color = BLACK AndAlso
               p.sibling().leftTree.color = BLACK AndAlso
               p.sibling().rightTree.color = BLACK Then
            p.sibling().color = RED
            p.parent.color = BLACK
        Else
            If p.sibling().color = BLACK Then
                If p Is p.parent.leftTree AndAlso
                   p.sibling().leftTree.color = RED AndAlso
                   p.sibling().rightTree.color = BLACK Then
                    p.sibling().color = RED
                    p.sibling().leftTree.color = BLACK
                    rotateRight(p.sibling().leftTree)
                ElseIf p Is p.parent.rightTree AndAlso
                       p.sibling().leftTree.color = BLACK AndAlso
                       p.sibling().rightTree.color = RED Then
                    p.sibling().color = RED
                    p.sibling().rightTree.color = BLACK
                    rotateLeft(p.sibling().rightTree)
                End If
            End If
            p.sibling().color = p.parent.color
            p.parent.color = BLACK
            If p Is p.parent.leftTree Then
                p.sibling().rightTree.color = BLACK
                rotateLeft(p.sibling())
            Else
                p.sibling().leftTree.color = BLACK
                rotateRight(p.sibling())
            End If
        End If
    End Sub

    Private Function insert(ByRef p As BiTreeNode, ByVal data As Integer) As Boolean
        If p.data > data Then
            If p.leftTree IsNot NIL Then
                Return insert(p.leftTree, data)
            Else
                p.leftTree = New BiTreeNode(data, p)
                p.leftTree.leftTree = NIL
                p.leftTree.rightTree = NIL
                insertCase(p.leftTree)
                Return True
            End If
        ElseIf p.data < data
            If p.rightTree IsNot NIL Then
                Return insert(p.rightTree, data)
            Else
                p.rightTree = New BiTreeNode(data, p)
                p.rightTree.leftTree = NIL
                p.rightTree.rightTree = NIL
                insertCase(p.rightTree)
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Private Sub insertCase(ByRef p As BiTreeNode)
        If p.parent Is Nothing Then
            root = p
            p.color = BLACK
            Exit Sub
        End If
        If p.parent.color = RED Then
            If p.uncle() IsNot Nothing AndAlso p.uncle().color = RED Then
                p.parent.color = p.uncle().color = BLACK
                p.grandparent().color = RED
                insertCase(p.grandparent())
            Else
                If p Is p.parent.rightTree AndAlso
                   p.parent Is p.grandparent().leftTree Then
                    rotateLeft(p)
                    rotateRight(p)
                    p.color = BLACK
                    p.leftTree.color = RED
                    p.rightTree.color = RED
                ElseIf p Is p.parent.leftTree AndAlso
                       p.parent Is p.grandparent().rightTree Then
                    rotateRight(p)
                    rotateLeft(p)
                    p.color = BLACK
                    p.leftTree.color = RED
                    p.rightTree.color = RED
                ElseIf p Is p.parent.leftTree AndAlso
                       p.parent Is p.grandparent().leftTree Then
                    p.parent.color = BLACK
                    p.grandparent().color = RED
                    rotateRight(p.parent)
                ElseIf p Is p.parent.rightTree AndAlso
                           p.parent Is p.grandparent().rightTree
                    p.parent.color = BLACK
                    p.grandparent().color = RED
                    rotateLeft(p.parent)
                End If
            End If
        End If
    End Sub
End Module
