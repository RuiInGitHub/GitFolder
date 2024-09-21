@Code
    Layout = Nothing
End Code

<h2>Resultados da pesquisa por: @ViewBag.SearchTerm</h2>

@Code
    If Model IsNot Nothing Then
        For Each dinastia In Model
            Response.Write("<div class='result'>")
            Response.Write("<h3>" & dinastia.AnoDinastico & " - " & dinastia.Fotos_Dinastia_legenda & "</h3>")
            Response.Write("<p>" & dinastia.OBS & "</p>")
            Response.Write("<p>" & dinastia.Monarca_País & "</p>")
            Response.Write("<img src='" & dinastia.Fotos_Dinastia & "' alt='Imagem Dinastia' style='width: 100px;' />")
            Response.Write("</div>")
        Next
    Else
        Response.Write("<p>Nenhum resultado encontrado.</p>")
    End If
End Code

