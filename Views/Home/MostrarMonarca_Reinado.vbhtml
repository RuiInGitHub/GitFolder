@Code
    Layout = Nothing
End Code
<!-- Incluir o logo (O   P o r t a l   d o   C a n e c o)-->
@Html.Partial("_Logo")

<h2 style="text-align: center; color:#4285f4;">@Model.PaisRef durante a regência de:</h2>

@Code
    ' Constrói o caminho da imagem usando o caminho virtual configurado
    Dim imgPathMonarca = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(Model.Monarca_Fotos)
End Code

<div style="margin-left: 50px; margin-right: 50px; overflow: auto;">
    <!-- Bloco que envolve o texto e a imagem -->
    <!-- Imagem posicionada à esquerda com quebra automática de texto -->
    <img src="@imgPathMonarca" alt="" style="width: 200px; margin-right: 20px; float: left; border-radius: 10px;" />

    <!-- O nome do monarca em uma linha separada -->
    <h2>@Model.Monarca</h2>

    <!-- O reinado em uma nova linha -->
    <span style="font-size: 1.17em; font-weight: bold; display:block;">@Model.ReinadoInicio - @Model.ReinadoFim</span>

    <!-- Os detalhes do monarca em uma nova linha -->
    <span style="font-size: 1.17em;">@Model.Monarca_Detalhes</span>
</div>

<!-- Isso garante que o conteúdo abaixo não seja afetado pelo float -->
<div style="clear: both;"></div>

<h2 style="text-align:center; color:#006400;">Eventos</h2>
@if Model.Eventos.Count > 0 Then
    @<table style="width: 70%; margin-left: auto; margin-right: auto;">
        <thead>
            <tr>
            </tr>
        </thead>
        <tbody>
            @For Each evento In Model.Eventos
                @<tr>
                    <td style="font-size: 20px; display: inline; margin-left: 10px; margin-top: 5px; font-weight: bold; color: #006400;">
                        @evento.AnoDinastico
                    </td>
                    <td>
                        <h5 style="display:inline; margin-left:10px; font-weight:bold;">@evento.Fotos_Dinastia_legenda</h5>
                    </td>
                    <td>
                        <h4 style="margin-left:10px; font-weight:normal;">@evento.OBS</h4>
                    </td>
                    <td>
                        @Code
                            ' Constrói o caminho da imagem usando o caminho virtual configurado
                            Dim imgPathReino = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(evento.Fotos_Dinastia)
                        End Code
                        <!-- Envolve a imagem em um link que leva para o método MostrarRegistoUnico -->
                        <a href="@Url.Action("MostrarRegistoUnico", "Home", New With {.ID = evento.ID_Dinastias, .searchTerm = ViewBag.SearchTerm})">
                            <img src="@imgPathReino" alt="Imagem Mundo" style="width: 200px; margin-left: 20px; float: right; border-radius: 10px;" /> <!-- Imagem com cantos arredondados e margem entre imagem e texto -->
                        </a>
                    </td>
                </tr>
                            Next
        </tbody>
    </table>
Else
    @<p style="text-align: center; color: #006400;">@ViewBag.MensagemSemEventos</p>
End If




