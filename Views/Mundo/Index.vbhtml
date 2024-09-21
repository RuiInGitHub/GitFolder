@Code
    Layout = Nothing
End Code

<h2>Resultados da pesquisa por: @ViewBag.SearchTerm</h2>

@Code
    If Model IsNot Nothing AndAlso Model.Any() Then
        For Each Mundo In Model
            Response.Write("<div class='result'>")
            Response.Write("<h3>" & Mundo.AnoDominus & " - " & evento.DescritivoDaFoto & "</h3>")
            Response.Write("<img src='" & evento.Foto & "' alt='Imagem Mundo' style='width: 100px;' />")
            Response.Write("<p> Arquivo: " & Mundo.ArquivoLocal & "</p>")
            Response.Write("</div>")
        Next
    Else
        Response.Write("<p>Nenhum resultado encontrado.</p>")
    End If
End Code

