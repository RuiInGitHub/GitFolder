Namespace PortalDoCaneco.Models
    Public Class Monarca_Reinado_Model
        Public Property Monarca As String
        Public Property Monarca_Detalhes As String
        Public Property Monarca_Fotos As String
        Public Property Dinastia As String
        Public Property PaisRef As String
        Public Property ReinadoInicio As String
        Public Property ReinadoFim As String

        Public Property Eventos As New List(Of Evento)
    End Class

    Public Class Evento
        Public Property ID_Dinastias As String
        Public Property AnoDinastico As String
        Public Property Fotos_Dinastia As String
        Public Property Fotos_Dinastia_legenda As String
        Public Property OBS As String
        ' Adicione outros campos relevantes do evento, como ID, fotos, etc.
    End Class

End Namespace



