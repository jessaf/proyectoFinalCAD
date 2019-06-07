Public Class Form3
    Private Sub CrearEntidadToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'crearEntidadLinea()
    End Sub

    Private Sub ConexionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConexionToolStripMenuItem.Click
        inicializa_conexion("2018")
        If Not DOCUMENTO Is Nothing Then
            dwgActual.Text = "Plano Conectado"
            dwgActual.BackColor = Color.Lime
            dwgActual.ToolTipText = "Conectado"


        End If
    End Sub

    Private Sub LeyendoUnaCoordenadaToolStripMenuItem_Click(sender As Object, e As EventArgs)
        '''adquiriendo una coordenada
        ''Dim p() As Double = Nothing
        ''Dim p2() As Double = Nothing
        ''Dim linea As AcadLine

        ''Me.Visible = False 'Ocultando la ventana actual
        '''appactivateAutoCAD() 'Lanzando el focus a Autcad
        ''p = getCoordenada("Indicame una coordenada")
        ''If Not IsNothing(p) Then
        ''    p2 = getCoordenada("Indicame siguiente coordenada", p) 'creando rubberband
        ''    'se requiere ambas coordenadas para crear una linea
        ''    If Not IsNothing(p) And Not IsNothing(p2) Then
        ''        linea = DOCUMENTO.ModelSpace.AddLine(p, p2)
        ''        linea.Update()
        ''    End If
        ''End If
        ''Me.Visible = True

    End Sub

    Private Sub SeleccionDeUnElementoToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'seleccionDeObjetos("D")
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SeleccionDeTodoslLosElementosDelPlanoToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'seleccionDeObjetos("C")
    End Sub

    Private Sub OrganizandoCituculosYLineasToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'OrganizandoCirculosYLineas()
    End Sub

    Private Sub SeleccionCircularToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub SelectivaToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'seleccionDeObjetos("A")
    End Sub

    Private Sub DentroDeUnRectanguloToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'seleccionDeObjetos("B")
    End Sub

    Private Sub AnalizandoObjetosToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'analizandoTodos()
    End Sub

    Private Sub AnalizandoEntornoRectangularToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'AnalizandoEntornoRectangular()
    End Sub

    Private Sub StatusStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles StatusStrip1.ItemClicked

    End Sub

    Private Sub AgregandoDiccionarioAUnObjetoToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'agregarDiccionarioEntidad()
    End Sub

    Private Sub RecuperandoElValorDeUnXRecordToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'recuperarDiccionarioAEntidad()
    End Sub

    Private Sub CarroSiguiendoLineaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CarroSiguiendoLineaToolStripMenuItem.Click
        seguimientoCalle()
    End Sub

    Private Sub CambiandoValoresDeDiccionarioToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'cambiarDiccionarioEntidad()
    End Sub

    Private Sub AcercaDeTenochtitlanCityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AcercaDeTenochtitlanCityToolStripMenuItem.Click
        Acerca.Show()

    End Sub

    Private Sub SimuladorRedesAguaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SimuladorRedesAguaToolStripMenuItem.Click

    End Sub

    Private Sub AgregarDatosAMapaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AgregarDatosAMapaToolStripMenuItem.Click
        ElementosCiudad.ShowDialog()
    End Sub
End Class