<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Mania dos portais</title>

    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")

    

</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">

                <p>
                    <span style="color:#4285F4; font-size: 26px; font-family: 'Georgia';">O</span>
                    <span>&nbsp;</span> <!-- Espaço entre as palavras -->
                    <span style="color: #DB4437; font-size: 26px; font-family: 'Georgia';">P</span>
                    <span style="color:#F4B400; font-size: 26px; font-family: 'Georgia';">o</span>
                    <span style="color:#0F9D58; font-size: 26px; font-family: 'Georgia';">r</span>
                    <span style="color:#4285F4; font-size: 26px; font-family: 'Georgia';">t</span>
                    <span style="color:#DB4437; font-size: 26px; font-family: 'Georgia';">a</span>
                    <span style="color:#F4B400; font-size: 26px; font-family: 'Georgia';">l</span>
                    <span>&nbsp;</span> <!-- Espaço entre as palavras -->
                    <span style="color:#0F9D58; font-size: 26px; font-family: 'Georgia';">d</span>
                    <span style="color:#4285F4; font-size: 26px; font-family: 'Georgia';">o</span>
                    <span>&nbsp;</span> <!-- Espaço entre as palavras -->
                    <span style="color:#DB4437; font-size: 26px; font-family: 'Georgia';">C</span>
                    <span style="color:#F4B400; font-size: 26px; font-family: 'Georgia';">a</span>
                    <span style="color:#0F9D58; font-size: 26px; font-family: 'Georgia';">n</span>
                    <span style="color:#4285F4; font-size: 26px; font-family: 'Georgia';">e</span>
                    <span style="color:#DB4437; font-size: 26px; font-family: 'Georgia';">c</span>
                    <span style="color:#F4B400; font-size: 26px; font-family: 'Georgia';">o</span>
                </p>


            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li>@Html.ActionLink("Home", "Index", "Home")</li>
                    <li>@Html.ActionLink("About", "About", "Home")</li>
                    <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                </ul>
            </div>
        </div>
    </div>
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <div >
                <p>&copy; @DateTime.Now.Year - Mania dos portais</p>
            </div>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @* @Scripts.Render("~/bundles/bootstrap") *@
    @RenderSection("scripts", required:=False)
</body>
            </html>
