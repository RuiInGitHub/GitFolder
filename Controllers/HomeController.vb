Imports PortalDoCaneco.PortalDoCaneco.Models
Imports System.Web.Mvc
Imports PortalDoCaneco.PortalDoCaneco.Controllers
Imports System.Linq
Imports System.Data.OleDb
'Imports Microsoft.Office.Interop.Word
Imports System.Text
'Imports Aspose.Words
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Wordprocessing
Imports DocumentFormat.OpenXml.Spreadsheet
Imports DocumentFormat.OpenXml.Drawing.Charts
Imports Microsoft.Office.Interop.Word
Imports DocumentFormat.OpenXml.EMMA
Imports System.Text.RegularExpressions


Namespace PortalDoCaneco.Controllers
    Public Class HomeController
        Inherits Controller
        'recordset, abrangendo tblTimelineEventos e tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério
        Dim qryDinastiasMundo As String = "SELECT tblTimelineEventos.*,tblTimelineEventos.AnoDinastico, tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Seculos, tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Monarca," _
        & " tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.ReinadoInicio, tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.ReinadoFim, tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Dinastia," _
        & " tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Monarca_Fotos" _
        & " FROM tblTimelineEventos " _
        & " LEFT JOIN tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério" _
        & " On (tblTimelineEventos.PaisRef = tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.tPaisRef) " _
        & " And (tblTimelineEventos.AnoDinastico = tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.AnoDinastico) "

        Public Function Index() As ActionResult ' Exibe o portal de pesquisa, listando: inputBox(Pesquisar), países e séculos
            ' Buscar os países do banco de dados
            Dim vPaíses = GetPaisesFromDatabase()

            ' Criar um ViewModel para passar para a View
            Dim model As New PesquisaViewModel With {
                .Países = vPaíses
            }

            ' Retornar a View com o ViewModel
            Return View(model)
        End Function
        Public Function pesqLiteral(searchTerm As String, caseSensitive As Boolean, palavraIsolada As Boolean, language As String) As ActionResult
            ' Guardar o termo de pesquisa original para exibição na View
            Dim searchTermOriginal As String = searchTerm

            ' Lógica para tratar o termo de pesquisa
            If Not caseSensitive Then
                'searchTerm = searchTerm.ToLower() ' (se necessário para comparação case-insensitive)
            End If

            ' Simular a lógica de tradução (isso pode ser substituído por uma API de tradução)
            If Not String.IsNullOrEmpty(language) Then
                searchTermOriginal = TraduzirTermo(searchTermOriginal, language) ' Também traduz o termo original para exibição
            End If

            ' Verificar se o usuário selecionou "palavra isolada"
            If palavraIsolada Then
                ' Adiciona bordas de palavra (\b) para garantir que a palavra seja isolada
                'searchTerm = "\b" & Regex.Escape(searchTermOriginal) & "\b"
            Else
                ' Apenas usa o termo de pesquisa como está, para procurar dentro das palavras
                'searchTerm = Regex.Escape(searchTermOriginal)
            End If

            ' Passe o searchTermOriginal para as funções de banco de dados, sem os delimitadores
            Dim resultadosDinastias = GetDinastiasFromDatabase(palavraIsolada, searchTermOriginal)
            Dim resultadosMundo = GetMundoFromDatabase(palavraIsolada, searchTermOriginal)

            ' Passe o searchTermOriginal para a view para ser mostrado nos resultados
            ViewBag.SearchTerm = searchTermOriginal
            Response.Write(searchTerm)

            ' Criar um ViewModel para passar para a View (opcional)
            Dim model As New PesquisaViewModel With {
        .ResultadosDinastias = resultadosDinastias,
        .ResultadosMundo = resultadosMundo
    }

            Return View(model)
        End Function

        Private Function TraduzirTermo(searchTerm As String, language As String) As String
            ' Exemplo simples de "tradução" - isso deve ser substituído por lógica real ou API
            Select Case language
                Case "Francês"
                    Return "termo_frances" ' Substitua pelo termo real em francês
                Case "Inglês"
                    Return "termo_ingles"
                Case "Espanhol"
                    Return "termo_espanhol"
                Case "Português"
                    Return searchTerm ' Não traduzir se for português
                Case Else
                    Return searchTerm ' Default: retorna o termo original
            End Select
        End Function
        Public Function pesqReinosporPaís(pais As String) As ActionResult ' Lista os monarcas por país escolhido

            ' Verifica se o parâmetro paisClicado está vazio ou nulo
            If String.IsNullOrEmpty(pais) Then
                Return RedirectToAction("Index") ' Redireciona para a página inicial ou retorna um erro, se o país não for passado
            End If

            ' Buscar os países do banco de dados

            Dim PaísReino = GetReinadosPorPaís(pais)

            ' Criar um ViewModel para passar para a View
            Dim model As New Dinastia With {
                .ResultPaísReino = PaísReino,
                .PaísDoMonarca = pais
            }

            ' Retornar a View com o ViewModel
            Return View(model)
        End Function

        Public Function pesqReinosMundoPorAno(ano As String) As ActionResult
            Dim resultadosDinastias = GetDinastiasPorAno(ano)
            Dim resultadosMundo = GetEventosMundoPorAno(ano)

            ' Criar um ViewModel para passar os dados para a View
            Dim model As New PesquisaViewModel With {
        .ResultadosDinastias = resultadosDinastias,
        .ResultadosMundo = resultadosMundo
    }

            ' Retorna a View com os resultados para o ano específico
            ViewBag.Ano = ano
            Return View(model)
        End Function
        Public Function pesqReinosMundoPorSeculo(seculo As String) As ActionResult ' Pesquisa dinastias e eventos por século
            Dim resultadosDinastias = GetDinastiasPorSeculo(seculo)
            Dim resultadosMundo = GetEventosMundoPorSeculo(seculo)

            ' Cria um ViewModel com os resultados filtrados
            Dim model As New PesquisaViewModel With {
        .ResultadosDinastias = resultadosDinastias,
        .ResultadosMundo = resultadosMundo
    }

            ' Retorna a view Resultados com o model
            ViewBag.Seculo = seculo
            Return View(model)
        End Function

        Private Function GetPaisesFromDatabase() As List(Of Dinastia)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim rPaíses As New List(Of Dinastia)

            ' Sua lógica de conexão com o banco de dados e consulta
            Dim query As String = "SELECT tblTimelineEventos.PaisRef " &
            "From tblTimelineEventos " &
            "Group By tblTimelineEventos.PaisRef " &
            "HAVING (((tblTimelineEventos.PaisRef) Is Not Null)) " &
            "ORDER BY tblTimelineEventos.PaisRef;"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    'command.Parameters.AddWithValue("@searchTerm", "%" & searchTerm & "%")
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim dinastia As New Dinastia()
                            dinastia.PaisRef = reader("PaisRef").ToString()
                            rPaíses.Add(dinastia)
                        End While
                    End Using
                End Using
            End Using

            Return rPaíses
        End Function

        Private Function GetDinastiasFromDatabase(palavraIsolada As String, searchTerm As String) As List(Of Dinastia)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim resultados As New List(Of Dinastia)

            Dim query As String

            If palavraIsolada Then
                ' Usar espaços antes e depois do termo para simular "palavra isolada"
                query = qryDinastiasMundo & " WHERE (((tblTimelineEventos.Fotos_Dinastia_legenda) LIKE '% " & searchTerm & " %' " _
                & " OR tblTimelineEventos.Fotos_Dinastia_legenda LIKE '" & searchTerm & " %' " _
                & " OR tblTimelineEventos.Fotos_Dinastia_legenda LIKE '% " & searchTerm & "' " _
                & " OR tblTimelineEventos.Fotos_Dinastia_legenda = '" & searchTerm & "')) " _
                & " OR (((tblTimelineEventos.OBS) LIKE '% " & searchTerm & " %' " _
                & " OR tblTimelineEventos.OBS LIKE '" & searchTerm & " %' " _
                & " OR tblTimelineEventos.OBS LIKE '% " & searchTerm & "' " _
                & " OR tblTimelineEventos.OBS = '" & searchTerm & "')) " _
                & " ORDER BY tblTimelineEventos.AnoDinastico;"
            Else
                ' Pesquisa dentro das palavras (como antes)
                query = qryDinastiasMundo & " WHERE (((tblTimelineEventos.Fotos_Dinastia_legenda) LIKE '%" & searchTerm & "%')) " _
                & " OR (((tblTimelineEventos.OBS) LIKE '%" & searchTerm & "%')) " _
                & " ORDER BY tblTimelineEventos.AnoDinastico;"
            End If

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@searchTerm", "%" & searchTerm & "%")
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim dinastia As New Dinastia()
                            dinastia.ID_Dinastias = Convert.ToInt32(reader("ID_Dinastias"))
                            dinastia.AnoDinastico = Convert.ToInt32(reader("tblTimelineEventos.AnoDinastico"))
                            dinastia.Fotos_Dinastia_legenda = reader("Fotos_Dinastia_legenda").ToString()
                            dinastia.Fotos_Dinastia = reader("Fotos_Dinastia").ToString()

                            dinastia.OBS = reader("OBS").ToString()
                            ' Destacar o termo de pesquisa no campo OBS
                            If Not String.IsNullOrEmpty(searchTerm) Then
                                dinastia.OBS = Regex.Replace(dinastia.OBS, Regex.Escape(searchTerm), "<span style=""background-color: yellow;"">$0</span>", RegexOptions.IgnoreCase)
                            End If


                            dinastia.Monarca = reader("Monarca").ToString()
                            dinastia.PaisRef = reader("PaisRef").ToString()
                            dinastia.ReinadoInicio = reader("ReinadoInicio").ToString()
                            dinastia.ReinadoFim = reader("ReinadoFim").ToString()
                            ' Adicionar mais campos conforme necessário
                            resultados.Add(dinastia)
                        End While
                    End Using
                End Using
            End Using

            Return resultados
        End Function

        Private Function GetMundoFromDatabase(palavraIsolada As String, searchTerm As String) As List(Of Mundo)

            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim resultados As New List(Of Mundo)

            Dim Query As String

            If palavraIsolada Then
                Response.Write("palavraIsolada=On, searchterm= " & searchTerm)
                Query = "SELECT tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.Foto, tblFotosLinksPorAno.ArquivoLocal " &
                "FROM tblFotosLinksPorAno " &
                "GROUP BY tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.ArquivoLocal, tblFotosLinksPorAno.Foto " &
                "HAVING (((tblFotosLinksPorAno.DescritivoDaFoto) LIKE '% " & searchTerm & " %' " &
                " OR tblFotosLinksPorAno.DescritivoDaFoto LIKE '" & searchTerm & " %' " &
                " OR tblFotosLinksPorAno.DescritivoDaFoto LIKE '% " & searchTerm & "' " &
                " OR tblFotosLinksPorAno.DescritivoDaFoto = '" & searchTerm & "')) " &
                "ORDER BY tblFotosLinksPorAno.AnoDominus;"
            Else
                ' Pesquisa dentro das palavras (como antes)
                Query = "SELECT tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.Foto, tblFotosLinksPorAno.ArquivoLocal " &
                "FROM tblFotosLinksPorAno " &
                "GROUP BY tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.ArquivoLocal, tblFotosLinksPorAno.Foto " &
                "HAVING (((tblFotosLinksPorAno.DescritivoDaFoto) LIKE '%" & searchTerm & "%')) " &
                "ORDER BY tblFotosLinksPorAno.AnoDominus;"
            End If

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(Query, connection)
                    command.Parameters.AddWithValue("@searchTerm", "%" & searchTerm & "%")
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            ' Criação de um novo objeto Dinastia e preenchimento com os dados do banco
                            Dim Mundo As New Mundo()
                            Mundo.IDLinksPorAno = Convert.ToInt32(reader("IDLinksPorAno"))
                            Mundo.DescritivoDaFoto = reader("DescritivoDaFoto").ToString()
                            ' Destacar o termo de pesquisa no campo DescritivoDaFoto
                            If Not String.IsNullOrEmpty(searchTerm) Then
                                Mundo.DescritivoDaFoto = Regex.Replace(Mundo.DescritivoDaFoto, Regex.Escape(searchTerm), "<span style=""background-color: yellow;"">$0</span>", RegexOptions.IgnoreCase)
                            End If
                            Mundo.AnoDominus = Convert.ToInt32(reader("AnoDominus"))
                            Mundo.ArquivoLocal = reader("ArquivoLocal").ToString()
                            Mundo.Foto = reader("Foto").ToString()
                            ' Adicionar dinastia à lista
                            resultados.Add(Mundo)
                        End While
                    End Using
                End Using
            End Using

            Return resultados
        End Function
        Private Function GetReinadosPorPaís(paisClicado As String) As List(Of Dinastia)

            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim PaísesReis As New List(Of Dinastia)

            Dim tabela As String = "[tblTimeline_" & paisClicado.Trim() & "]"


            Dim srSQL As String = "SELECT Monarca, Dinastia, ReinadoInicio, ReinadoFim, Monarca_Fotos, PaisRef " &
                      "FROM " & tabela & " " &
                      "GROUP BY Monarca, Dinastia, ReinadoInicio, ReinadoFim, Monarca_Fotos, PaisRef " &
                      "HAVING Monarca Is Not Null " &
                      "ORDER BY ReinadoInicio;"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(srSQL, connection)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim reinPaís As New Dinastia()
                            'reinPaís.ID_Dinastias = reader("ID_Dinastias").ToString()
                            reinPaís.Monarca = reader("Monarca").ToString()
                            reinPaís.Dinastia = reader("Dinastia").ToString()
                            reinPaís.PaisRef = reader("PaisRef").ToString()
                            ' Verifica se os campos ReinadoInicio e ReinadoFim não são nulos
                            If Not IsDBNull(reader("ReinadoInicio")) AndAlso Not IsDBNull(reader("ReinadoFim")) Then
                                reinPaís.ReinadoInicio = Convert.ToInt32(reader("ReinadoInicio"))
                                reinPaís.ReinadoFim = Convert.ToInt32(reader("ReinadoFim"))
                            Else
                                reinPaís.ReinadoInicio = 0 ' ou outro valor padrão
                                reinPaís.ReinadoFim = 0 ' ou outro valor padrão
                            End If

                            reinPaís.Monarca_Fotos = reader("Monarca_Fotos").ToString()
                            ' Segunda consulta para buscar o campo Memo "Monarca_Detalhes" (sem isto é truncado)
                            Dim srSQL_Detalhes As String = "SELECT Monarca_Detalhes FROM " & tabela & " WHERE Monarca = @monarca AND PaisRef = @pais"
                            Using command2 As New OleDbCommand(srSQL_Detalhes, connection)
                                command2.Parameters.AddWithValue("@monarca", reinPaís.Monarca)
                                command2.Parameters.AddWithValue("@pais", reinPaís.PaisRef)
                                Using reader2 As OleDbDataReader = command2.ExecuteReader()
                                    If reader2.Read() Then
                                        reinPaís.Monarca_Detalhes = reader2("Monarca_Detalhes").ToString()
                                    End If
                                End Using
                            End Using

                            ' Define o país do monarca
                            reinPaís.PaísDoMonarca = paisClicado
                            ' Adiciona o reinado à lista
                            PaísesReis.Add(reinPaís)
                        End While
                    End Using
                End Using
            End Using

            Return PaísesReis

        End Function
        Public Function MostrarMonarca_Reinado(monarca As String, pais As String) As ActionResult
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim model As New Monarca_Reinado_Model() ' Crie um ViewModel apropriado para armazenar os dados do monarca e eventos
            Dim tblTimelineDoPaís As String = "[" & "tblTimeline_" & pais & "]"

            ' Query SQL para buscar os detalhes do monarca e eventos de seu reinado
            Dim query As String = "SELECT " & tblTimelineDoPaís & ".AnoDinastico, tblTimelineEventos.ID_Dinastias, tblTimelineEventos.Fotos_Dinastia, tblTimelineEventos.Fotos_Dinastia_legenda, tblTimelineEventos.OBS, " &
            "" & tblTimelineDoPaís & ".Monarca, " & tblTimelineDoPaís & ".Dinastia, " & tblTimelineDoPaís & ".PaisRef, " & tblTimelineDoPaís & ".Seculos, " & tblTimelineDoPaís & ".Dinastia, " &
            "" & tblTimelineDoPaís & ".ReinadoInicio, " & tblTimelineDoPaís & ".ReinadoFim, " & tblTimelineDoPaís & ".Monarca & " & tblTimelineDoPaís & ".PaisRef As Monarca_País, " &
            "" & tblTimelineDoPaís & ".Monarca_Fotos, " & tblTimelineDoPaís & ".Monarca_Detalhes " &
            "FROM " & tblTimelineDoPaís & " LEFT JOIN tblTimelineEventos ON (" & tblTimelineDoPaís & ".PaisRef = tblTimelineEventos.PaisRef) " &
            "And (" & tblTimelineDoPaís & ".AnoDinastico = tblTimelineEventos.AnoDinastico) " &
            "WHERE (((" & tblTimelineDoPaís & ".Monarca) = @monarca) And ((" & tblTimelineDoPaís & ".PaisRef) = @pais)) " &
            "ORDER BY " & tblTimelineDoPaís & ".AnoDinastico;"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@monarca", monarca)
                    command.Parameters.AddWithValue("@pais", pais)

                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            ' Preencher o model com os dados do monarca e eventos do reinado
                            model.Monarca = reader("Monarca").ToString()
                            model.Monarca_Detalhes = reader("Monarca_Detalhes").ToString()
                            model.Monarca_Fotos = reader("Monarca_Fotos").ToString()
                            model.Dinastia = reader("Dinastia").ToString()
                            model.PaisRef = reader("PaisRef").ToString()
                            model.ReinadoInicio = reader("ReinadoInicio").ToString()
                            model.ReinadoFim = reader("ReinadoFim").ToString()

                            ' Adicionar o evento no reinado (outras propriedades podem ser incluídas conforme necessário)
                            Dim evento As New Evento()
                            evento.ID_Dinastias = reader("ID_Dinastias").ToString()
                            evento.AnoDinastico = reader("AnoDinastico").ToString()
                            evento.Fotos_Dinastia = reader("Fotos_Dinastia").ToString()
                            evento.Fotos_Dinastia_legenda = reader("Fotos_Dinastia_legenda").ToString()
                            evento.OBS = reader("OBS").ToString()
                            If Len(evento.Fotos_Dinastia) > 0 And Len(evento.OBS) > 0 Then
                                model.Eventos.Add(evento)
                            End If
                        End While
                    End Using
                End Using
            End Using

            ' Se não houver eventos, exiba uma mensagem apropriada
            If model.Eventos.Count = 0 Then
                ViewBag.MensagemSemEventos = "Não possuímos registo de eventos, para este caso particular..."
            End If

            ' Passa o model para a view
            Return View(model)
        End Function
        Private Function GetDinastiasPorAno(ano As String) As List(Of Dinastia)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim dinastias As New List(Of Dinastia)

            ' Query SQL para buscar eventos da Dinastia por AnoDinastico
            Dim query As String = qryDinastiasMundo & " WHERE tblTimelineEventos.AnoDinastico = " & ano & ";"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Ano", ano)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim dinastia As New Dinastia()
                            dinastia.ID_Dinastias = reader("ID_Dinastias").ToString()
                            dinastia.AnoDinastico = reader("tblTimelineEventos.AnoDinastico").ToString()
                            dinastia.Monarca = If(Not IsDBNull(reader("Monarca")), reader("Monarca").ToString(), String.Empty)
                            dinastia.ReinadoInicio = reader("ReinadoInicio").ToString()
                            dinastia.ReinadoFim = reader("ReinadoFim").ToString()
                            dinastia.PaisRef = reader("PaisRef").ToString()
                            dinastia.Fotos_Dinastia = reader("Fotos_Dinastia").ToString()
                            dinastia.Fotos_Dinastia_legenda = reader("Fotos_Dinastia_legenda").ToString()
                            dinastia.OBS = reader("OBS").ToString()
                            dinastia.Seculos = If(Not IsDBNull(reader("Seculos")), reader("Seculos").ToString(), String.Empty)
                            dinastias.Add(dinastia)
                        End While
                    End Using
                End Using
            End Using

            Return dinastias
        End Function
        Private Function GetDinastiasPorSeculo(seculo As String) As List(Of Dinastia)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim dinastias As New List(Of Dinastia)

            Dim query As String = qryDinastiasMundo & " WHERE tblRecSource_de_subFrm_qryMonarcasDinastias_SemCritério.Seculos = '" & seculo & "';"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Seculo", seculo)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim dinastia As New Dinastia()
                            ' Preencher os dados do modelo
                            dinastia.ID_Dinastias = reader("ID_Dinastias").ToString()
                            If Not IsDBNull(reader("tblTimelineEventos.AnoDinastico")) Then
                                dinastia.AnoDinastico = reader("tblTimelineEventos.AnoDinastico").ToString()
                            End If
                            If Not IsDBNull(reader("Monarca")) Then
                                dinastia.Monarca = reader("Monarca").ToString()
                            End If
                            dinastia.ReinadoInicio = reader("ReinadoInicio").ToString()
                            dinastia.ReinadoFim = reader("ReinadoFim").ToString()
                            dinastia.PaisRef = reader("PaisRef").ToString()
                            dinastia.Fotos_Dinastia = reader("Fotos_Dinastia").ToString()
                            dinastia.Fotos_Dinastia_legenda = reader("Fotos_Dinastia_legenda").ToString()
                            dinastia.OBS = reader("OBS").ToString()
                            If Not IsDBNull(reader("Seculos")) Then
                                dinastia.Seculos = reader("Seculos").ToString()
                            End If
                            ' Adicionar outras propriedades conforme necessário
                            dinastias.Add(dinastia)
                        End While
                    End Using
                End Using
            End Using

            Return dinastias
        End Function

        Private Function GetEventosMundoPorSeculo(seculo As String) As List(Of Mundo)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim eventosMundo As New List(Of Mundo)

            ' Consulta SQL para filtrar por século
            Dim query As String = "SELECT * FROM tblFotosLinksPorAno WHERE (((tblFotosLinksPorAno.Seculo)='" & seculo & "'));"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Seculo", seculo)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim evento As New Mundo()
                            evento.IDLinksPorAno = Convert.ToInt32(reader("IDLinksPorAno"))
                            evento.DescritivoDaFoto = reader("DescritivoDaFoto").ToString()
                            evento.AnoDominus = Convert.ToInt32(reader("AnoDominus"))
                            evento.ArquivoLocal = reader("ArquivoLocal").ToString()
                            evento.Foto = reader("Foto").ToString()
                            evento.Seculo = reader("Seculo").ToString()
                            ' Adicionar outras propriedades conforme necessário
                            eventosMundo.Add(evento)
                        End While
                    End Using
                End Using
            End Using

            Return eventosMundo
        End Function
        Private Function GetEventosMundoPorAno(ano As String) As List(Of Mundo)
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim eventosMundo As New List(Of Mundo)

            ' Query SQL para buscar eventos do Mundo por AnoDominus
            Dim query As String = "SELECT * FROM tblFotosLinksPorAno WHERE tblFotosLinksPorAno.AnoDominus = " & ano & ";"

            Using connection As New OleDbConnection(connectionString)
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Ano", ano)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim evento As New Mundo()
                            evento.IDLinksPorAno = Convert.ToInt32(reader("IDLinksPorAno"))
                            evento.DescritivoDaFoto = reader("DescritivoDaFoto").ToString()
                            evento.AnoDominus = Convert.ToInt32(reader("AnoDominus"))
                            evento.ArquivoLocal = reader("ArquivoLocal").ToString()
                            evento.Foto = reader("Foto").ToString()
                            evento.Seculo = reader("Seculo").ToString()
                            eventosMundo.Add(evento)
                        End While
                    End Using
                End Using
            End Using

            Return eventosMundo
        End Function

        ' Função principal que chama todo o processo
        Public Function MostrarRegistoUnico(ID As Long, searchTerm As String) As ActionResult
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Trabalho\Teresa\Moedas\MoedasWebSite\MoedasWebSite\App_Data\MoedasRecolha2007_Web.accdb"
            Dim model As New RegistoUnicoModel()
            Dim query As String

            Using conn As New OleDbConnection(connectionString)
                conn.Open()

                ' Dinastias: SQL Query
                query = qryDinastiasMundo & " WHERE (((tblTimelineEventos.ID_Dinastias)=" & ID & "))" _
                    & " ORDER BY tblTimelineEventos.AnoDinastico;"

                Dim cmd As New OleDbCommand(query, conn)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Preenche o model com os dados do banco
                        model.AnoDinastico = reader("tblTimelineEventos.AnoDinastico").ToString()
                        model.PaisRef = reader("PaisRef").ToString()
                        model.Fotos_Dinastia = reader("Fotos_Dinastia").ToString()
                        model.Fotos_Dinastia_legenda = reader("Fotos_Dinastia_legenda").ToString()
                        model.DinastiaLinks = reader("DinastiaLinks").ToString()
                        model.OBS = reader("OBS").ToString()

                        ' Verificar se o .docx existe e não está corrompido
                        If Not IsDBNull(reader("WordDoc")) AndAlso Not String.IsNullOrEmpty(reader("WordDoc").ToString()) Then
                            model.WordDoc = reader("WordDoc").ToString()
                            ' Verifica se o arquivo existe e está válido
                            If FileExists(model.WordDoc) AndAlso IsDocxValid(model.WordDoc) Then
                                model.WordContent = ExtractFormattedTextFromDocx(model.WordDoc)
                                model.WordContent = model.WordContent.Replace(searchTerm, "<span style=""background-color: yellow;"">" & searchTerm & "</span>")
                            Else
                                model.WordContent = "O arquivo " & model.WordDoc & ", está corrompido ou não existe."
                            End If
                        End If

                        ' Destacar o termo de pesquisa no campo OBS
                        If Not String.IsNullOrEmpty(searchTerm) Then
                            model.OBS = Regex.Replace(model.OBS, Regex.Escape(searchTerm), "<span style=""background-color: yellow;"">$0</span>", RegexOptions.IgnoreCase)
                        End If
                    End If
                End Using
                'End Using

                ' Mundo: SQL Query
                query = "SELECT tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.Foto, tblFotosLinksPorAno.ArquivoLocal " &
                "FROM tblFotosLinksPorAno " &
                "GROUP BY tblFotosLinksPorAno.IDLinksPorAno, tblFotosLinksPorAno.AnoDominus, tblFotosLinksPorAno.DescritivoDaFoto, tblFotosLinksPorAno.ArquivoLocal, tblFotosLinksPorAno.Foto " &
                "HAVING (((tblFotosLinksPorAno.IDLinksPorAno)=" & ID & "))" &
                "ORDER BY tblFotosLinksPorAno.AnoDominus;"
                Dim cmd1 As New OleDbCommand(query, conn)

                Using reader As OleDbDataReader = cmd1.ExecuteReader()
                    If reader.Read() Then
                        ' Preenche o model com os dados do banco
                        model.IDLinksPorAno = Convert.ToInt32(reader("IDLinksPorAno"))
                        model.DescritivoDaFoto = reader("DescritivoDaFoto").ToString()
                        model.AnoDominus = Convert.ToInt32(reader("AnoDominus"))
                        model.ArquivoLocal = reader("ArquivoLocal").ToString()
                        model.Foto = reader("Foto").ToString()
                    End If
                End Using
            End Using

            ' Passa o model para a view
            Return View(model)
        End Function
        ' Função para verificar se o arquivo existe
        Private Function FileExists(filePath As String) As Boolean
            FileExists = (Dir(filePath) <> "")
        End Function
        ' Função para verificar se o arquivo .docx não está corrompido
        Private Function IsDocxValid(filePath As String) As Boolean
            On Error GoTo ErrorHandler
            ' Tenta abrir o arquivo .docx usando o Open XML SDK
            Using wordDoc As DocumentFormat.OpenXml.Packaging.WordprocessingDocument = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(filePath, False)
                ' Se abrir sem erros, o arquivo é válido
                IsDocxValid = True
            End Using
            Exit Function

ErrorHandler:
            ' Se houver erro, o arquivo está corrompido ou não pôde ser aberto
            IsDocxValid = False
        End Function
        ' Função para extrair texto de .docx usando Open XML SDK
        Private Function ExtractFormattedTextFromDocx(ByVal filePath As String) As String
            Dim wordContent As New StringBuilder()

            ' Abrir o documento .docx usando Open XML SDK
            Using wordDoc As WordprocessingDocument = WordprocessingDocument.Open(filePath, False)
                ' Acessar o corpo do documento
                Dim body As DocumentFormat.OpenXml.Wordprocessing.Body = wordDoc.MainDocumentPart.Document.Body

                ' Percorre todos os elementos do corpo do documento
                For Each element In body.Elements()
                    If TypeOf element Is DocumentFormat.OpenXml.Wordprocessing.Paragraph Then
                        Dim para As DocumentFormat.OpenXml.Wordprocessing.Paragraph = CType(element, DocumentFormat.OpenXml.Wordprocessing.Paragraph)
                        Dim formattedText As String = ProcessParagraph(para)
                        wordContent.AppendLine("<p>" & HttpUtility.HtmlDecode(formattedText) & "</p>") ' Decodifica HTML para evitar caracteres especiais
                    ElseIf TypeOf element Is DocumentFormat.OpenXml.Spreadsheet.Table Then
                        ' Caso o documento contenha tabelas
                        Dim tableContent As String = ProcessTable(CType(element, DocumentFormat.OpenXml.Spreadsheet.Table))
                        wordContent.AppendLine("<table>" & HttpUtility.HtmlDecode(tableContent) & "</table>")
                    End If
                Next
            End Using

            Return wordContent.ToString()
        End Function


        ' Função para processar parágrafos com formatação básica
        Private Function ProcessParagraph(ByVal para As DocumentFormat.OpenXml.Wordprocessing.Paragraph) As String
            Dim paragraphContent As New StringBuilder()

            For Each run As DocumentFormat.OpenXml.Wordprocessing.Run In para.Elements(Of DocumentFormat.OpenXml.Wordprocessing.Run)()
                Dim textElement As DocumentFormat.OpenXml.Wordprocessing.Text = run.GetFirstChild(Of DocumentFormat.OpenXml.Wordprocessing.Text)()
                If textElement IsNot Nothing Then
                    Dim runProperties As DocumentFormat.OpenXml.Wordprocessing.RunProperties = run.RunProperties

                    ' Verifica formatação (negrito, itálico, sublinhado, etc.)
                    If runProperties IsNot Nothing Then
                        If runProperties.Bold IsNot Nothing Then
                            paragraphContent.Append("<strong>")
                        End If
                        If runProperties.Italic IsNot Nothing Then
                            paragraphContent.Append("<em>")
                        End If
                    End If

                    ' Adiciona o texto
                    paragraphContent.Append(textElement.Text)

                    ' Fecha as tags de formatação
                    If runProperties IsNot Nothing Then
                        If runProperties.Italic IsNot Nothing Then
                            paragraphContent.Append("</em>")
                        End If
                        If runProperties.Bold IsNot Nothing Then
                            paragraphContent.Append("</strong>")
                        End If
                    End If
                End If
            Next

            Return paragraphContent.ToString()
        End Function

        ' Função para processar tabelas, caso existam no documento
        Private Function ProcessTable(ByVal table As DocumentFormat.OpenXml.Spreadsheet.Table) As String
            Dim tableContent As New StringBuilder()

            For Each row As TableRow In table.Elements(Of TableRow)()
                tableContent.Append("<tr>")
                For Each cell As TableCell In row.Elements(Of TableCell)()
                    tableContent.Append("<td>")
                    For Each para As DocumentFormat.OpenXml.Wordprocessing.Paragraph In cell.Elements(Of DocumentFormat.OpenXml.Wordprocessing.Paragraph)()
                        tableContent.Append(ProcessParagraph(para))
                    Next
                    tableContent.Append("</td>")
                Next
                tableContent.Append("</tr>")
            Next

            Return tableContent.ToString()
        End Function

    End Class
End Namespace

