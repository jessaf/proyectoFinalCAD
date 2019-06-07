Public Class ElementosCiudad
    Private Sub ElementosCiudad_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CmbTipoElem.Items.Add("CASA HABITACION")
        CmbTipoElem.Items.Add("CASA OFICINA")
        CmbTipoElem.Items.Add("CASA COMERCIO")
        CmbTipoElem.Items.Add("EDIFICIO HABITACIONAL")
        CmbTipoElem.Items.Add("EDIFICIO OFICINAS")
        CmbTipoElem.Items.Add("EDIFICIO COMERCIAL")
        CmbTipoElem.Items.Add("EDIFICIO GUBERNAMENTAL")
        CmbTipoElem.Items.Add("CENTRO COMERCIAL")
        CmbTipoElem.Items.Add("SUPERMERCADO")
        CmbTipoElem.Items.Add("MERCADO")
        CmbTipoElem.Items.Add("HOSPITAL")
        CmbTipoElem.Items.Add("CLINICA")
        CmbTipoElem.Items.Add("JARDIN")
        CmbTipoElem.Items.Add("PARQUE")
        CmbTipoElem.Items.Add("TERRENO BALDIO")
        CmbTipoElem.Items.Add("TERRENO PRIVADO")
        CmbTipoElem.Items.Add("INFRAESTRUCTURA PUBLICA")
        CmbTipoElem.Items.Add("INFRAESTRUCTURA PRIVADA")
        CmbTipoElem.Items.Add("TERRENO PUBLICO")
        CmbTipoElem.Items.Add("OFICINA DE GOBIERNO")
        CmbTipoElem.Items.Add("ZONA ECOLOGICA")
        CmbTipoElem.Items.Add("ZONA PROTEGIDA")
        CmbTipoElem.Items.Add("ZONA INDIGENA")

        cbIPub.Items.Add("LUMINARIA")
        cbIPub.Items.Add("REGISTRO AGUA")
        cbIPub.Items.Add("REGISTRO LUZ")
        cbIPub.Items.Add("VIALIDAD PRINCIPAL")
        cbIPub.Items.Add("VIALIDAD SECUNDARIA")
        cbIPub.Items.Add("CALLE")
        cbIPub.Items.Add("POSTE LUZ")
        cbIPub.Items.Add("DRENAJE")
        cbIPub.Items.Add("DUCTO AGUA")

        cbUsoSUelo.Items.Add("URBANO HABITACIONAL")
        cbUsoSUelo.Items.Add("URBANO COMERCIAL")
        cbUsoSUelo.Items.Add("URBANO INDUSTRIAL")
        cbUsoSUelo.Items.Add("URBANO TERRENO")
        cbUsoSUelo.Items.Add("RURAL AGRICOLA")
        cbUsoSUelo.Items.Add("RURAL COMERCIAL")
        cbUsoSUelo.Items.Add("RURAL TERRENO")
        cbUsoSUelo.Items.Add("RURAL TEMPORAL")

        cbIPub.Enabled = False
        btnAsigna.Enabled = False
    End Sub

    Private Sub CmbTipoElem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbTipoElem.SelectedIndexChanged
        If CmbTipoElem.SelectedIndex = 16 Then
            cbIPub.Enabled = True
            cbUsoSUelo.Enabled = False
            nudPisos.Enabled = False
        Else
            cbIPub.Enabled = False
            cbUsoSUelo.Enabled = True
            nudPisos.Enabled = True
        End If
        ChecaCombos()
    End Sub

    Public Sub ChecaCombos()
        If (CmbTipoElem.SelectedIndex = 16 And cbIPub.SelectedIndex >= 0) Then
            btnAsigna.Enabled = True
        End If
        If (CmbTipoElem.SelectedIndex >= 0) And (cbUsoSUelo.SelectedIndex >= 0) Then
            btnAsigna.Enabled = True
        End If
    End Sub

    Private Sub CbIPub_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbIPub.SelectedIndexChanged
        ChecaCombos()
    End Sub

    Private Sub BtnAsigna_Click(sender As Object, e As EventArgs) Handles btnAsigna.Click
        Dim tipo As String
        Dim ipub As String
        Dim uso As String
        Dim pisos As String
        Dim codigo As String
        Dim entidad As AcadEntity = Nothing

        appactivateAutoCAD()
        entidad = getEntidad("Seleccione objeto para asignar información")
        Me.Activate()
        If Not IsNothing(entidad) Then
            Try
                tipo = Convert.ToString(CmbTipoElem.SelectedIndex + 1)
                ipub = Convert.ToString(cbIPub.SelectedIndex + 1)
                uso = Convert.ToString(cbUsoSUelo.SelectedIndex + 1)
                pisos = Convert.ToString(nudPisos.Value)
                codigo = Convert.ToString(entidad.Handle)
                AsignaCampoEntidad(entidad, "TIPOPROPIEDAD", tipo)
                AsignaCampoEntidad(entidad, "IPUB", ipub)
                AsignaCampoEntidad(entidad, "USOSUELO", uso)
                AsignaCampoEntidad(entidad, "NUMEROPISOS", pisos)
                AsignaCampoEntidad(entidad, "CODIGO", codigo)
                Mensaje("Datos Asignados a objeto " & codigo)
                CmbTipoElem.SelectedIndex = -1
                cbUsoSUelo.SelectedIndex = -1
                cbIPub.SelectedIndex = -1
                nudPisos.Value = 0
                btnAsigna.Enabled = False
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            Mensaje("Se requiere seleccionar un objeto")
        End If
    End Sub

    Private Sub BtnRecupera_Click(sender As Object, e As EventArgs) Handles BtnRecupera.Click
        'Me.Visible = False
        'recuperarDiccionarioAEntidad()
        'Me.Visible = True
        Dim entidad As AcadEntity

        appactivateAutoCAD()

        entidad = getEntidad("Selecciona objeto")
        If Not IsNothing(entidad) Then
            Try

                Dim objdetalle As New DatosCiudadDetalle(entidad.Handle)
            objdetalle.ShowDialog(Me)
                Me.Activate()

            Catch ex As Exception

            End Try
        Else
            Mensaje("Debe de seleccionar un objeto")
        End If
    End Sub
End Class