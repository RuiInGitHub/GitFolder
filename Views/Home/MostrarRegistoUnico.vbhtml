@Code
    Layout = Nothing
End Code
<!-- Incluir o logo (O   P o r t a l   d o   C a n e c o)-->
@Html.Partial("_Logo")


<style>
    .event {
        margin-bottom: 20px;
        font-size: 18px;
        font-weight: bold;
    }

    /* Centralizar imagem e símbolos diretamente */
    .centralizado {
        text-align: center;
    }

    img {
        max-width: 80%; /* Reduz o tamanho da imagem em 20% */
        height: auto;
    }

    .symbol {
        font-size: 24px; /* Tamanho dos símbolos § */
        font-weight: bold;
        margin: 20px 5px;
    }
</style>



<!-- Parte referente à Dinastia, exibida apenas se houver dados -->
@If Not String.IsNullOrEmpty(Model.AnoDinastico) Then
    @<div class="event" style="display: inline; font-weight: bold; text-align: center;">
        <h1 style="color: #4285f4; font-weight: bold; font-size: 50px;">@Model.AnoDinastico</h1>
        <h3 style="font-weight: bold;">@Model.Fotos_Dinastia_legenda</h3>
    </div>

    @Code
        ' Constrói o caminho da imagem usando o caminho virtual configurado
        Dim imgPathDinastias = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(Model.Fotos_Dinastia)
    End Code


    @<div class="centralizado">
        <img src="@imgPathDinastias" alt="Imagem Dinastia">
        <p></p>
        <p></p>
    </div>

    @<div>@Html.Raw(Model.OBS)</div>

    @<div>@Html.Raw(Model.WordContent)</div>

    @<div Class="centralizado">
        <p></p>
        <span Class="symbol">§</span>
        <span Class="symbol">§</span>
    </div>

    @<div style="text-align: center;">
        <p></p>
        <p></p>
        <a href="@Model.DinastiaLinks" style="text-decoration: none; font-size: 12px;">
            Outros links: @Model.DinastiaLinks
        </a>
    </div>

End If

<!-- Parte referente ao Mundo, exibida apenas se houver dados -->
@If Not String.IsNullOrEmpty(Model.AnoDominus) Then

    @<div Class="centralizado" style="font-weight: bold; color: #4285f4; font-size: 50px;">
        @Model.AnoDominus
    </div>

    @Code
        ' Constrói o caminho da imagem usando o caminho virtual configurado para Mundo
        Dim imgPathMundo = "/ImagensPortalDoCaneco/" & System.IO.Path.GetFileName(Model.Foto)
    End Code

    @<div Class="centralizado"><img src="@imgPathMundo" alt="Imagem Mundo"></div>

    @<h3 style="font-weight: bold; font-weight: normal; margin-left: 10px;">@Model.DescritivoDaFoto </h3>

    @<div Class="centralizado">
        <p></p>
        <span Class="symbol"> §</span>
        <span Class="symbol"> §</span>
    </div>

    @Code
        ' Constrói o caminho da imagem usando o caminho virtual configurado para Mundo
        Dim pdfPath = "/PDFsPortalDoCaneco/" & System.IO.Path.GetFileName(Model.Foto)
    End Code

    @<div style="text-align: center;">
        <!-- Abrir o pdf -->
        <p></p>
        <p></p>
        <button style="background-color: transparent; border: none; cursor: pointer;" onclick="window.open('/PDFsPortalDoCaneco/@System.IO.Path.GetFileName(Model.ArquivoLocal)', '_blank')">
            <img src="/Content/ImagensApp/VisualizarPdf.jpg" alt="Visualizar PDF" style="width: 80px; height: auto; border-radius: 10px;">
        </button>
    </div>

End If

