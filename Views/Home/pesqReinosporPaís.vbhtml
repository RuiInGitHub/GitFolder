@Code
    Layout = Nothing
End Code
<!-- Incluir o logo (O   P o r t a l   d o   C a n e c o)-->
@Html.Partial("_Logo")

<h2 style="text-align:center; color:#4285f4;">Reinados por País</h2>
<h2 style="text-align:center; color:#4285f4;">@Model.PaísDoMonarca</h2>
<div style="margin-left:auto; margin-right:auto; width:80%;">
    <!-- Centraliza a lista na página com margens -->
    @If Model.ResultPaísReino IsNot Nothing Then
        @For Each reinado In Model.ResultPaísReino
            @<div class="result" style="display: flex; align-items: center; margin-bottom: 20px;">

                @Code
                    ' Constrói o caminho da imagem usando o caminho virtual configurado
                    Dim imgPathReinados = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(reinado.Monarca_Fotos)
                End Code

                <div style="flex-grow: 1; margin-left: 50px;">
                    <h2 style="display:inline; color:#4285f4;">@reinado.Monarca</h2>
                    <h3 style=" font-weight:normal;">@reinado.Dinastia</h3>
                    <h3 style="font-weight:normal;">Reinado: @reinado.ReinadoInicio - @reinado.ReinadoFim</h3>
                    <p style="font-weight: normal;margin-right:50px;">@reinado.Monarca_Detalhes</p>
                </div>

                <!-- Envolve a imagem em um link que leva para o método MostrarMonarca -->
                <a href="@Url.Action("MostrarMonarca_Reinado", "Home", New With {.monarca = reinado.Monarca, .pais = reinado.PaisRef})">
                    <img src="@imgPathReinados" alt="Imagem Monarca" style="width: 130px; margin-left: 60px; float: right; border-radius: 10px;" />
                </a>
            </div>
        Next
    Else
        @<p>Nenhum reinado encontrado para o país selecionado.</p>
    End If
</div>

