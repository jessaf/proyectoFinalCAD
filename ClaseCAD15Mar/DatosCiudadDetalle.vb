Public Class DatosCiudadDetalle
    Dim tipo(40) As String
    Dim uso(40) As String
    Dim infpub(40) As String
    Dim codigo As String
    Dim gasto As String

    Sub New(ByVal param As String)
        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        codigo = param
    End Sub

    Private Sub DatosCiudaDetalle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim vtipo As Integer
        Dim vuso As Integer
        Dim vinfpub As Integer
        Dim vpisos As Integer
        Dim gagua As Integer
        Dim val As String
        Dim entidad As AcadEntity

        Me.Activate()

        entidad = DOCUMENTO.HandleToObject(codigo)

        tipo(1) = "CASA HABITACION"
        tipo(2) = "CASA OFICINA"
        tipo(3) = "CASA COMERCIO"
        tipo(4) = "EDIFICIO HABITACIONAL"
        tipo(5) = "EDIFICIO OFICINAS"
        tipo(6) = "EDIFICIO COMERCIAL"
        tipo(7) = "EDIFICIO GUBERNAMENTAL"
        tipo(8) = "CENTRO COMERCIAL"
        tipo(9) = "SUPERMERCADO"
        tipo(10) = "MERCADO"
        tipo(11) = "HOSPITAL"
        tipo(12) = "CLINICA"
        tipo(13) = "JARDIN"
        tipo(14) = "PARQUE"
        tipo(15) = "5TERRENO BALDIO"
        tipo(16) = "T6ERRENO PRIVADO"
        tipo(17) = "INFRAESTRUCTURA PUBLICA"
        tipo(18) = "INFRAESTRUCTURA PRIVADA"
        tipo(19) = "TERRENO PUBLICO"
        tipo(20) = "OFICINA DE GOBIERNO"
        tipo(21) = "ZONA ECOLOGICA"
        tipo(22) = "ZONA PROTEGIDA"
        tipo(23) = "ZONA INDIGENA"

        infpub(1) = "LUMINARIA"
        infpub(2) = "REGISTRO AGUA"
        infpub(3) = "REGISTRO LUZ"
        infpub(4) = "VIALIDAD PRINCIPAL"
        infpub(5) = "VIALIDAD SECUNDARIA"
        infpub(6) = "CALLE"
        infpub(7) = "POSTE LUZ"
        infpub(8) = "DRENAJE"
        infpub(9) = "DUCTO AGUA"

        uso(1) = "URBANO HABITACIONAL"
        uso(2) = "URBANO COMERCIAL"
        uso(3) = "URBANO INDUSTRIAL"
        uso(4) = "URBANO TERRENO"
        uso(5) = "RURAL AGRICOLA"
        uso(6) = "RURAL COMERCIAL"
        uso(7) = "RURAL TERRENO"
        uso(8) = "RURAL TEMPORAL"

        getXdata(entidad, "TIPOPROPIEDAD", val)
        vtipo = CInt(val)
        getXdata(entidad, "IPUB", val)
        vinfpub = CInt(val)
        getXdata(entidad, "USOSUELO", val)
        vuso = CInt(val)
        getXdata(entidad, "NUMEROPISOS", val)
        vpisos = CInt(val)
        getXdata(entidad, "GASTOAGUA", val)
        gagua = CInt(val)

        lbltipo.Text = tipo(vtipo)
        lbluso.Text = uso(vuso)
        lblpisos.Text = CStr(vpisos)
        lblinfpub.Text = infpub(vinfpub)
        lblGastoAgua.Text = gagua.ToString
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub
End Class