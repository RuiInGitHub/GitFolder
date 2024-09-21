
<h1 style="margin-top: 0px; text-align: center; color: #4285f4;">Pesquisar no Caneco</h1>

<div id="searchForm" style="margin-top: 30px; text-align:center;">
    <input type="text" id="searchtext" name="searchTerm" placeholder="Digite sua pesquisa aqui..."
           style="width:40%; padding:10px; font-size:18px; border-radius:5px; border:1px solid #ccc;" required>


    <!-- Checkboxes para opções -->
    <div style="margin-top: 10px;">
        <label>
            <input type="checkbox" id="caseSensitive" name="caseSensitive">
            Case Sensitive
        </label>

        <label>
            <input type="checkbox" id="palavraIsolada" name="palavraIsolada">
            Procurar palavra isolada
        </label>
    </div>

    <!-- Combobox para Tradução -->
    <div style="margin-top: 10px;">
        <label for="language">Traduzir para:</label>
        <select id="language" name="language">
            <option value="Português" selected>Português</option>
            <option value="Francês">Francês</option>
            <option value="Inglês">Inglês</option>
            <option value="Espanhol">Espanhol</option>
        </select>
    </div>




    <input type="button" value="Pesquisar"
           style="padding: 10px 20px; font-size: 18px; background-color: #4285f4; color: white; border: none;
           border-radius: 5px; cursor: pointer;" onclick="executarPesquisa()">
</div>

<p> </p>

<!-- Lista de países (ajustada para quebrar em duas linhas, se necessário) -->
<div style="display: flex; justify-content: center; align-items: center; flex-wrap: wrap; width: 100%; text-align:center;">
    @If Model.Países IsNot Nothing Then
        @<ul style="display: flex; justify-content: center; flex-wrap: wrap; list-style-type: none; padding: 0; margin: 0 auto; width: 60%; line-height: 1;">
            @For Each dinastia In Model.Países
                Dim pais = dinastia.PaisRef.Replace(" ", "&nbsp;") ' Substitui espaços por espaço inquebrável
                @<li style="list-style-type: none; margin: 3px; padding: 0px; border: 1px solid transparent;">
                    <a href="@Url.Action("pesqReinosporPaís", "Home", New With {.pais = dinastia.PaisRef})" style="text-decoration: none; font-size: 12px;">
                        @Html.Raw(pais)
                    </a>
                </li>
            Next
        </ul>
    End If
</div>

<p></p>

<!-- Lista de séculos em romano -->
<div style="display: flex; margin-bottom: 80px; justify-content: center; align-items: center; flex-wrap: wrap; width: 100%; text-align: center;">
    @For Each seculo In New List(Of String) From {"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII"}
        @<h6 style = "color: #4285f4; margin: 10px; cursor: pointer;" >
            <a href="@Url.Action("pesqReinosMundoPorSeculo", "Home", New With {.seculo = seculo})" style="text-decoration: none; color: #4285f4;">
                @seculo
            </a>
        </h6>
    Next
</div>

<script type="text/javascript">
    function executarPesquisa() {
        var searchText = document.getElementById('searchtext').value;
        var caseSensitive = document.getElementById('caseSensitive').checked;
        var palavraIsolada = document.getElementById('palavraIsolada').checked;
        var language = document.getElementById('language').value;

        if (searchText.trim()) {
            // Montar URL com os parâmetros caseSensitive, palavraIsolada e language
            var url = '@Url.Action("pesqLiteral", "Home")?searchTerm=' + encodeURIComponent(searchText) +
                      '&caseSensitive=' + caseSensitive + '&palavraIsolada=' + palavraIsolada +
                      '&language=' + encodeURIComponent(language);
            window.location.href = url;
        }
    }
</script>
