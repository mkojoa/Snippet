Namespace DatingConsole
    
    Public Class Dating
        
        Public Shared Sub Main(ByVal args() As String)
            Dim Me As Person = New Person("Michael")
            Dim You As Person = New Person
            For Each Girl In You.GirlFriends
                If ((Me.GirlFriend Is Nothing)  _
                            AndAlso Me.CrushOn(Girl)) Then
                    Try 
                        If (Me.AskGirlOut(Girl) = "Accepted") Then
                            Me.Happy = true
                            Me.EnjoyLife
                        Else
                            Me.ReTry
                        End If
                        
                    Catch rx As RelationshipExistException
                        Throw rx.ReTry
                    End Try
                    
                End If
                
            Next
        End Sub
    End Class
    
    'Implement yours
    Public Class Person
        
        Private v As String
        
        Public Property GirlFriend As Object
            Get
            End Get
            Set
            End Set
        End Property
        
        Public Property GirlFriends As IEnumerable(Of Object)
            Get
            End Get
            Set
            End Set
        End Property
        
        Public Property Happy As Boolean
            Get
            End Get
            Set
            End Set
        End Property
        
        'Parameterless Constuct
        Public Sub New()
            MyBase.New
            
        End Sub
        
        Public Sub New(ByVal v As String)
            MyBase.New
            Me.v = Me.v
        End Sub
        
        Friend Function AskGirlOut(ByVal girl As Object) As String
            Throw New NotImplementedException
        End Function
        
        Friend Function CrushOn(ByVal girl As Object) As Boolean
            Throw New NotImplementedException
        End Function
        
        Friend Sub EnjoyLife()
            Throw New NotImplementedException
        End Sub
        
        Friend Sub PrepareForMarriage()
            Throw New NotImplementedException
        End Sub
        
        Friend Sub ReTry()
            Throw New NotImplementedException
        End Sub
    End Class
    
    'Implement your exception
    <Serializable()>  _
    Class RelationshipExistException
        Inherits Exception
        
        Public Sub New()
            MyBase.New
            
        End Sub
        
        Public Sub New(ByVal message As String)
            MyBase.New(message)
            
        End Sub
        
        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
            
        End Sub
        
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
            
        End Sub
        
        Friend Function ReTry() As Exception
            Throw New NotImplementedException
        End Function
    End Class
End Namespace