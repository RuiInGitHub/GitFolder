@Code
    Layout = Nothing
End Code
<!-- Incluir o logo (O   P o r t a l   d o   C a n e c o)-->
@Html.Partial("_Logo")

<h2 style="text-align:center; color:#4285f4;">Eventos no Ano: @ViewBag.Ano</h2>
<h1 style="text-align: center; color:#006400">Dinastias</h1>

<div style="margin-left:auto; margin-right:auto; width:80%;">
    @If Model.ResultadosDinastias IsNot Nothing Then
        @For Each dinastia In Model.ResultadosDinastias
            @<div class="result" style="display: flex; align-items: center; margin-bottom: 20px;">
                @Code
                    ' Constrói o caminho da imagem usando o caminho virtual configurado
                    Dim imgPathDinastias = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(dinastia.Fotos_Dinastia)
                End Code
                <div style="flex-grow: 1;">
                    <h2 style="display:inline; color:#4285f4;">@dinastia.AnoDinastico</h2>

                    <h3 style="display:inline; margin-left:10px; font-weight:normal;">@dinastia.Fotos_Dinastia_legenda</h3>
                    <h4 style="font-weight:normal;">@dinastia.OBS</h4>
                    <h4 style="display: inline; color: #4285f4; font-weight: normal;">
                        <a href="@Url.Action("MostrarMonarca_Reinado", "Home", New With { .monarca = dinastia.Monarca, .pais = dinastia.PaisRef })" style="text-decoration: none; color: #4285f4;">
                            @dinastia.Monarca, @dinastia.PaisRef
                        </a>
                    </h4>
                    <h4 style="display: inline; color: #4285f4; font-weight: normal;">@dinastia.ReinadoInicio-@dinastia.ReinadoFim</h4>
                </div>

                <!-- Envolve a imagem em um link que leva para o método MostrarRegistoUnico -->
                <a href="@Url.Action("MostrarRegistoUnico", "Home", New With {.ID = dinastia.ID_Dinastias, .searchTerm = ViewBag.SearchTerm})">
                    <img src="@imgPathDinastias" alt="Imagem Mundo" style="width: 200px; margin-left: 20px; float: right; border-radius: 10px;" /> <!-- Imagem com cantos arredondados e margem entre imagem e texto -->
                </a>

            </div>
        Next
    Else
        @<p>Nenhuma dinastia encontrada para o século selecionado.</p>
    End If
</div>

<h1 style="text-align: center; color:#006400">Eventos no Mundo</h1>

<div style="margin-left:auto; margin-right:auto; width:80%;">
    @If Model.ResultadosMundo IsNot Nothing Then
        @For Each evento In Model.ResultadosMundo
            @<div class="result" style="display: flex; align-items: center; margin-bottom: 20px;">
                @Code
                    Dim imgPathMundo = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(evento.Foto)
                End Code
                <div>
                    <h2 style="display:inline; color:#4285f4;">@evento.AnoDominus -</h2>

                    <h3 style="display:inline; margin-left:10px; font-weight:normal;">@evento.DescritivoDaFoto</h3>
                </div>

                <!-- Envolve a imagem em um link que leva para o método MostrarRegistoUnico -->
                <a href="@Url.Action("MostrarRegistoUnico", "Home", New With {.ID = evento.IDLinksPorAno, .searchTerm = ViewBag.SearchTerm})">
                    <img src="@imgPathMundo" alt="Imagem Mundo" style="width: 200px; margin-left: 20px; float: right; border-radius: 10px;" /> <!-- Imagem com cantos arredondados e margem entre imagem e texto -->
                </a>
            </div>
        Next
    Else
        @<p>Nenhum evento encontrado para o século selecionado.</p>
    End If


