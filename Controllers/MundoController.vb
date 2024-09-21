Imports System.Web.Mvc
Imports PortalDoCaneco.PortalDoCaneco.Controllers
Imports System.Linq
Imports System.Data.OleDb
Imports PortalDoCaneco.PortalDoCaneco.Models

Namespace PortalDoCaneco.Controllers
    Public Class MundoController
        Inherits Controller

        Public Function Index(Optional searchTerm As String = "") As ActionResult
            ' Verifica se o searchTerm foi passado, caso contrário, usa um valor padrão
            If String.IsNullOrEmpty(searchTerm) Then
                searchTerm = "" ' Ou defina algum termo padrão, se necessário
            End If
            Stop
            ' Busca os eventos do banco de dados com base no termo de pesquisa
            Dim eventosMundo = GetMundoFromDatabase(searchTerm)

            ' Retorna a View com os resultados
            Return View(eventosMundo)
        End Function

        Private Function GetMundoFromDatabase(searchTerm As String) As List(Of Mundo)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim MundoList As New List(Of Mundo)
            'Dim dinastiasList As New List(Of Dinastia)
            ' Consultas SQL
            Dim strSQLMundo As String = "SELECT tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.Foto, tblFotosLinksPorAno.ArquivoLocal " &
            "FROM tblFotosLinksPorAno " &
            "GROUP BY tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.ArquivoLocal, tblFotosLinksPorAno.Foto " &
            "HAVING (((tblFotosLinksPorAno.DescritivoDaFoto) LIKE '%" & searchTerm & "%')) " &
            "ORDER BY tblFotosLinksPorAno.AnoDominus;"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Dim command As New OleDbCommand(strSQLMundo, connection)
                Using reader As OleDbDataReader = command.ExecuteReader()
                    While reader.Read()
                        ' Criação de um novo objeto Dinastia e preenchimento com os dados do banco
                        Dim Mundo As New Mundo()
                        Mundo.IDLinksPorAno = Convert.ToInt32(reader("IDLinksPorAno"))
                        Mundo.DescritivoDaFoto = reader("DescritivoDaFoto").ToString()
                        Mundo.AnoDominus = Convert.ToInt32(reader("AnoDominus"))
                        Mundo.ArquivoLocal = reader("ArquivoLocal").ToString()
                        ' Adicionar dinastia à lista
                        MundoList.Add(Mundo)
                    End While
                End Using
            End Using

            Return MundoList
        End Function

    End Class
End Namespace

