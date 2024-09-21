Imports System.Web.Mvc
Imports PortalDoCaneco.PortalDoCaneco.Controllers
Imports System.Linq
Imports System.Data.OleDb
Imports PortalDoCaneco.PortalDoCaneco.Models

Namespace PortalDoCaneco.Controllers
    Public Class DinastiasController
        Inherits Controller

        ' GET: Dinastias
        Public Function Index(Optional searchTerm As String = "") As ActionResult
            ' Verifica se o searchTerm foi passado, caso contrário, usa um valor padrão
            If String.IsNullOrEmpty(searchTerm) Then
                searchTerm = "" ' Ou defina algum termo padrão, se necessário
            End If

            ' Busca as dinastias do banco de dados com base no termo de pesquisa
            Dim dinastias = GetDinastiasFromDatabase(searchTerm)

            ' Retorna a View com os resultados
            Return View(dinastias)
        End Function

        ' Função para buscar as Dinastias do banco de dados
        Private Function GetDinastiasFromDatabase(searchTerm As String) As List(Of Dinastia)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim dinastiasList As New List(Of Dinastia)
            ' Consultas SQL
            Dim strSQLReinados As String = "SELECT Min(tblTimelineEventos.ID_Dinastias) AS ID_Dinastias, tblTimelineEventos.AnoDinastico, tblTimelineEventos.Fotos_Dinastia, Min(tblTimelineEventos.PaisRef) AS MinOfPaisRef, tblTimelineEventos.Fotos_Dinastia_legenda, tblTimelineEventos.OBS," _
        & " tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Monarca, [tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério]![Monarca] & '', '' & [tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério]![tPaisRef] AS Monarca_País" _
        & " FROM tblTimelineEventos LEFT JOIN tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério ON (tblTimelineEventos.PaisRef = tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.tPaisRef) AND (tblTimelineEventos.AnoDinastico = tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.AnoDinastico)" _
        & " GROUP BY tblTimelineEventos.AnoDinastico, tblTimelineEventos.Fotos_Dinastia, tblTimelineEventos.Fotos_Dinastia_legenda, tblTimelineEventos.OBS," _
        & " tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Monarca, [tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério]![Monarca] & '', '' & [tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério]![tPaisRef]" _
        & " HAVING (((tblTimelineEventos.Fotos_Dinastia_legenda) Like '%" & searchTerm & "%')) Or (((tblTimelineEventos.OBS) Like '%" & searchTerm & "%'))" _
        & " ORDER BY tblTimelineEventos.AnoDinastico;"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Dim command As New OleDbCommand(strSQLReinados, connection)
                Using reader As OleDbDataReader = command.ExecuteReader()
                    While reader.Read()
                        ' Criação de um novo objeto Dinastia e preenchimento com os dados do banco
                        Dim dinastia As New Dinastia()
                        dinastia.ID_Dinastias = Convert.ToInt32(reader("ID_Dinastias"))
                        dinastia.Fotos_Dinastia_legenda = reader("Fotos_Dinastia_legenda").ToString()
                        dinastia.AnoDinastico = Convert.ToInt32(reader("AnoDinastico"))
                        dinastia.PaisRef = reader("MinOfPaisRef").ToString()
                        dinastia.Monarca_País = reader("Monarca_País").ToString()
                        ' Adicionar dinastia à lista
                        dinastiasList.Add(dinastia)
                    End While
                End Using
            End Using

            Return dinastiasList
        End Function
    End Class
End Namespace