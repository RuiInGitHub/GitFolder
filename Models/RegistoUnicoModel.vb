
Namespace PortalDoCaneco.Models
    Public Class RegistoUnicoModel
        'Dinastias
        Public Property AnoDinastico As String
        Public Property Monarca As String
        Public Property Monarca_Detalhes As String
        Public Property Monarca_Fotos As String
        Public Property Monarca_País As String
        Public Property PaísDoMonarca As String
        Public Property Dinastia As String 'tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério
        Public Property PaisRef As String
        Public Property Fotos_Dinastia As String
        Public Property Fotos_Dinastia_legenda As String
        Public Property DinastiaLinks As String
        Public Property OBS As String
        Public Property ReinadoInicio As String
        Public Property ReinadoFim As String
        Public Property WordDoc As String
        Public Property WordContent As String
        Public Property MostrarRegistoUnico As List(Of RegistoUnicoModel)
        'Mundo
        Public Property IDLinksPorAno As Integer
        Public Property AnoDominus As String
        Public Property DescritivoDaFoto As String
        Public Property Foto As String
        Public Property ArquivoLocal As String
        Public Property MundoPorSeculo As List(Of RegistoUnicoModel)
    End Class
End Namespace