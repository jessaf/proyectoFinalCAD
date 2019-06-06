Module Codigo
    Public AUTOCADAPP As AutoCAD.AcadApplication
    Public DOCUMENTO As AutoCAD.AcadDocument
    Public UTILITY As AutoCAD.AcadUtility

    'Public Declare PtrSafe Sub Sleep Lib "kernel32" (ByVal Milliseconds As LongPtr)
    'Public Declare Sub Sleep Lib "kernel32" Alias "Sleep" (ByVal dwMilliseconds As Long)

    ' Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long) 'For 32 Bit Systems  



    Public Sub inicializa_conexion(ByRef version As String)
        Dim R As String = ""

        If version = "2017" Then
            R = "Autocad.Application.21" 'R2019
        ElseIf version = "2018" Then
            R = "Autocad.Application.22" 'R2019
        ElseIf version = "2019" Then
            R = "Autocad.Application.23" 'R2019
        End If

        Try
            DOCUMENTO = Nothing
            AUTOCADAPP = GetObject(, R)                 'Aqui jala el proceso.
            DOCUMENTO = AUTOCADAPP.ActiveDocument       'El documento activo que se esta visualizando en autocad
            UTILITY = DOCUMENTO.Utility
            AUTOCADAPP.Visible = True                   ' Si no se esta visualizando autocad entonces forzamos la visualizacion.
        Catch er As Exception
            MsgBox(er.Message, MsgBoxStyle.Information, "CAD")
        End Try
        'DOCUMENTO es la referencia al documento de ACAD
        'UTILITY 

    End Sub

    Public Sub crearEntidadLinea()
        'Crear una linea en forma progrmada

        Dim linea As AcadLine
        Dim p1(2) As Double
        Dim p2(2) As Double
        'Creamos los puntos y los pasamos a la función que crea la linea AddLine
        p1(0) = 0 : p1(1) = 0 : p1(2) = 0
        p2(0) = 100 : p2(1) = 100 : p2(2) = 50

        linea = DOCUMENTO.ModelSpace.AddLine(p1, p2)

    End Sub

    Public Function getCoordenada(ByVal mensaje As String, Optional ByVal pBase() As Double = Nothing) As Double()
        'Regresa una coordenada o nothing
        'Dependiendo si el pBase no es nothing el metodo crea una rubberband
        'Puede servir para obtener una coordenada o dos
        Dim p() As Double = Nothing

        Try
            If IsNothing(pBase) Then
                'Se solicita la primera coordenada
                p = DOCUMENTO.Utility.GetPoint(, mensaje)
            Else
                'Se solicita la siguiente coordenada si ya se ingreso la primera y se le pasa la primera. 
                p = DOCUMENTO.Utility.GetPoint(pBase, mensaje)
            End If
        Catch e As Exception
            MsgBox(e.Message)
        End Try
        Return p

    End Function


    Public Sub appactivateAutoCAD()
        'activando AutoCAD para los select
        Dim AUTOCADWINDNAME As String
        Dim acadProcess() As Process = Process.GetProcessesByName("ACAD")
        Try
            'guaradando variables para activar autocad cuando sea necesario
            AUTOCADWINDNAME = acadProcess(0).MainWindowTitle
            AppActivate(AUTOCADWINDNAME)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub seleccionDeObjetos(metodo As String)

        Dim conjunto As AcadSelectionSet
        Dim entidad As AcadEntity = Nothing
        Dim esquinas(11) As Double
        Dim p() As Double = Nothing
        Dim p1() As Double
        Dim lista As ListBox
        lista = Form3.ListBox1
        lista.Items.Clear()

        Select Case metodo
            '=======================================================================================================
            'Case "A" ' Seleccion selectiva del plano 
            Case "A"
                'Seleccion selectiva del plano
                appactivateAutoCAD()
                conjunto = conjunto_vacio(DOCUMENTO, "IDLE") ' se crea un conj vacio
                If Not IsNothing(conjunto) Then
                    conjunto.SelectOnScreen()
                    If tieneElementos(conjunto) Then
                        MsgBox(conjunto.Count)
                    Else
                        MsgBox("No hay elementos en el conjunto")
                    End If
                End If
            '=======================================================================================================
            Case "B"
                p = getCoordenada("Esquina 1")
                If Not IsNothing(p) Then
                    p1 = getCoordenada("Esquina opuesta", p)
                    If Not IsNothing(p1) Then
                        'cada 3 indices representan una coordenada XYZ
                        esquinas(0) = p(0) : esquinas(1) = p(1) : esquinas(2) = 0   'coord1   x : y : z
                        esquinas(3) = p1(0) : esquinas(4) = p(1) : esquinas(5) = 0  'coord2
                        esquinas(6) = p1(0) : esquinas(7) = p1(1) : esquinas(8) = 0  'coord3
                        esquinas(9) = p(0) : esquinas(10) = p1(1) : esquinas(11) = 0 'coord4
                        'Ahora necesito un conjunto vacio sobre el cual operar
                        conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
                        If Not IsNothing(conjunto) Then
                            'Al conjunto vacio se le indica que seleccione usando selecion poligonal y dandole las esquinas
                            conjunto.SelectByPolygon(AcSelect.acSelectionSetCrossingPolygon, esquinas)

                            MsgBox(conjunto.Count)
                        End If
                    End If
                End If
            '=======================================================================================================
            Case "C"
                'Todos los elementos del plano
                'si quieres que te de un conjunto de objetos reservar un espacio en memoria en este caso idle
                'Ahora si todos los elementos los asigna a ese espacio. 

                conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
                If Not IsNothing(conjunto) Then
                    'Realizamos una seleccion de todos los objetos en el plano
                    conjunto.Select(AcSelect.acSelectionSetAll)
                    MsgBox(conjunto.Count)
                End If
            '=======================================================================================================
            Case "D"
                ' En este caso se selecciona una entidad a partir de un punto ???

                appactivateAutoCAD()
                Try
                    DOCUMENTO.Utility.GetEntity(entidad, p, "Selecciona una entidad")
                Catch
                    'Observe que estamos monitoriando todos los errores incluidos los COM 
                    'Esta fomra de chachar erro funciona para este caso de error
                    entidad = Nothing
                End Try
                If Not IsNothing(entidad) Then
                    'MsgBox(entidad.ObjectName)
                    DOCUMENTO.Utility.Prompt(entidad.ObjectName)
                    MsgBox(entidad.ObjectName)
                    'lista.Items.Add("hola")
                Else
                    'MsgBox("Se ha caido en el Else, lo que indica que no se ha seleccionado entidad")
                End If
                '=======================================================================================================
        End Select

    End Sub

    Public Function conjunto_vacio(ByRef DOCUMENTO As AcadDocument, ByRef nombre As String) As AcadSelectionSet

        'Esta funcion no reserva espacio en memoria para un conjunto vacio en el cual meter objetos.



        Dim indice As Integer
        Dim limite As Integer
        Dim cObjects As AcadSelectionSet = Nothing

        nombre = nombre.Trim.ToUpper ' los conjuntos deben ser en mayuscula.
        'conjunto_vacio = Nothing

        Try
            'Autocad tiene conjuntos, aqui hace referencia a los conjuntos de seleccion en el documento acutal. 
            limite = DOCUMENTO.SelectionSets.Count
            limite = limite - 1
            For indice = 0 To limite
                If DOCUMENTO.SelectionSets.Item(indice).Name = nombre Then
                    DOCUMENTO.SelectionSets.Item(indice).Delete()
                    Exit For
                End If
            Next
            cObjects = DOCUMENTO.SelectionSets.Add(nombre)

        Catch ex As ApplicationException
            MsgBox(ex.Message, MsgBoxStyle.Information, "CAD")
        End Try

        Return cObjects  'regresa siempre conj limpio de elementos
    End Function

    Public Function tieneElementos(ByRef conjunto As AcadSelectionSet) As Boolean
        'Nos indica si un conjunto existente tiene elementos
        tieneElementos = False
        Try
            If conjunto.Count > 0 Then
                tieneElementos = True
            End If
        Catch ex As ApplicationException
            MsgBox(ex.Message, MsgBoxStyle.Information, "CAD")
        End Try

        Return tieneElementos
    End Function

    Public Function generaCorrdenadasCirculo(p() As Double, radio As Double, angInicial As Double, avances As Integer) As Double()
        'grados estan Angulos esta en grados
        'Debe regresar un arreglo lineal donde cada 3 elementos son una coordenada

        Dim angulo As Double
        Dim anguloDeAvance As Double
        Dim pCirculo() As Double
        Dim pPolar() As Double
        Dim pN As Integer
        Dim angFinal As Double

        pN = 0

        anguloDeAvance = convertAngtoRad(angFinal - angInicial) / avances 'radianes
        angulo = convertAngtoRad(angInicial)
        For i = 1 To avances
            pPolar = DOCUMENTO.Utility.PolarPoint(p, angulo, radio)
            'Una coordenada polar requiere estos datos. Se avanca el numero de grados dividido entre el numero de puntos que tengo,. Esto se pudo hacer por seno y coseno pero se prefirio hacerlo así. 
            ReDim Preserve pCirculo(pN + 2)
            pCirculo(pN) = pPolar(0) : pCirculo(pN + 1) = pPolar(1) : pCirculo(pN + 2) = 0
            'Esto es un arreglo dinamico de coordenadas. 
            angulo = angulo + anguloDeAvance
            pN = pN + 3
        Next
        Return pCirculo

    End Function

    Public Function convertAngtoRad(anguloGrados As Double) As Double
        Return (anguloGrados * 3.1416 / 180.0)
    End Function

    Public Sub analizandoTodos()
        'Para este ejemplo se tienen que crear las Layers en el Banner/Layers/LayerProperties y crear una que se llame LINEAS, CIRCULOS, OTROS
        'revisando todos los elementos del plano
        Dim conjunto As AcadSelectionSet
        Dim element As AcadEntity
        Dim lista As ListBox
        Dim p1() As Double
        Dim p2() As Double

        ' todos los elemntos del plano
        lista = Form3.ListBox1
        lista.Items.Clear()


        conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
        conjunto.Select(AcSelect.acSelectionSetAll)

        For Each element In conjunto
            lista.Items.Add(element.ObjectName & " " & "Handler=" & element.Handle) '
            Select Case element.ObjectName
                Case "AcDbCircle"
                    lista.Items.Add(">>>>>Radio=" & element.radius)
                    element.Layer = "CIRCULOS"
                Case "AcDbLine"
                    element.Layer = "LINEAS"
                    p1 = element.startpoint
                    p2 = element.endpoint
                    'lista.Items.Add(">>>>>Longitud = " & element.length)
                    'lista.Items.Add(">>>>>Coordenada inciial = " & p1(0) & "," & p1(1) & "," & p1(2))
                    'lista.Items.Add(">>>>>Coordenada final = " & p2(0) & "," & p2(1) & "," & p2(2))
                Case "AcDbArc"
                    element.Layer = "OTROS"
                    'lista.Items.Add(">>>>>Angulo inicial ( radianes) = " & element.startangle)
                Case "AcDbText", "AcDbMText"
                    element.Layer = "OTROS"
                    'lista.Items.Add(">>>>>Texto = " & element.textstring)

            End Select
        Next
        conjunto.Delete()
    End Sub

    Public Sub AnalizandoEntornoRectangular()
        'dada una coordenada naliza que objetos se encuentras dentro de un rectangulo
        'el analisis se realiza en un delta definido

        Dim conjunto As AcadSelectionSet
        Dim delta As Double = 500
        Dim esquinas(11) As Double
        Dim lista As ListBox = Form3.ListBox1
        Dim p() As Double = Nothing
        Dim perimetro As AcadPolyline = Nothing

        appactivateAutoCAD()
        p = getCoordenada("indica una coordenada central ")

        If Not IsNothing(p) Then
            'cada 3 indices representa una coordenada xyz
            esquinas(0) = p(0) + delta : esquinas(1) = p(1) + delta : esquinas(2) = p(2)    'coord 1
            esquinas(3) = p(0) - delta : esquinas(4) = p(1) + delta : esquinas(5) = p(2)    'coord2
            esquinas(6) = p(0) - delta : esquinas(7) = p(1) - delta : esquinas(8) = p(2)    'coord3
            esquinas(9) = p(0) + delta : esquinas(10) = p(1) - delta : esquinas(11) = p(2)  'coord4


            'trazando el poligono de busqueda
            perimetro = drawPolygon(esquinas)

            conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
            conjunto.SelectByPolygon(AcSelect.acSelectionSetCrossingPolygon, esquinas)

            lista.Items.Clear()

            For Each element In conjunto
                'no reportamos el perimetro generado
                If element.handle <> perimetro.Handle Then
                    lista.Items.Add(element.handle & " " & element.ObjectName)
                End If
            Next
            conjunto.Delete()
        End If

    End Sub

    Public Function drawPolygon(ByVal coordenadas() As Double) As AcadPolyline
        'genera un poligono en el modelspace

        Dim perimetro As AcadPolyline
        Dim uB As Integer
        Dim uI As Integer

        uI = coordenadas.GetUpperBound(0)

        'redimensionando el arreglo dinamico para que acepte una coordenada adicional
        uB = uI + 3
        ReDim Preserve coordenadas(uB)

        'agregagndo nuevaente la primera coordenada para generar un poligono cerrado
        coordenadas(uI + 1) = coordenadas(0)
        coordenadas(uI + 2) = coordenadas(1)
        coordenadas(uI + 3) = coordenadas(2)

        'crando el poligono cerrado
        perimetro = DOCUMENTO.ModelSpace.AddPolyline(coordenadas)
        perimetro.Update()

        Return perimetro


    End Function


    Public Function OrganizandoCirculosYLineas()

        Dim element As AcadEntity
        Dim entidadLineaCentral As AcadEntity = Nothing
        Dim esquinas(11) As Double
        Dim pm As Double = Nothing
        Dim vertical As Boolean = False
        Dim lineacentral As AcadLine = Nothing
        Dim conjunto As AcadSelectionSet
        Dim p1() As Double
        Dim p2() As Double
        Dim p3() As Double = Nothing
        Dim p4() As Double
        Dim p5() As Double
        Dim p() As Double
        Dim lista As ListBox
        Dim centro() As Double
        Dim nuevocentro() As Double = Nothing
        Dim circulo As AcadCircle


        lista = Form3.ListBox1
        lista.Items.Clear()

        conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
        conjunto.Select(AcSelect.acSelectionSetAll)
        For Each element In conjunto
            lista.Items.Add(element.ObjectName & " " & "Handler=" & element.Handle) '
        Next


        'conjunto = conjunto_vacio(DOCUMENTO, "IDLE") ' se crea un conj vacio
        'appactivateAutoCAD()
        '===============================================================================================
        '                               PASO 1 OBTENIENDO LINEA CENTRAL
        '===============================================================================================
        Try
            DOCUMENTO.Utility.GetEntity(entidadLineaCentral, p, "Selecciona una entidad")
        Catch
            lineacentral = Nothing
        End Try

        If Not IsNothing(entidadLineaCentral) Then
            'MsgBox(entidadLineaCentral.)
            If entidadLineaCentral.ObjectName = "AcDbLine" Then
                p1 = entidadLineaCentral.startpoint
                p2 = entidadLineaCentral.endpoint
                'MsgBox(p1(0) & "=" & p2(0))
                If p1(0).ToString = p2(0).ToString Then
                    MsgBox("Si es vertical, Handler= " & entidadLineaCentral.Handle)
                    'Cambiar el proceso de autocad.
                    p3 = p2
                    p3(1) = p1(1) + (p2(1) - p1(1)) / 2
                End If
            End If
        Else
            MsgBox("Se produjo un error al obtener la linea central")
        End If
        lista.Items.Clear()

        '===============================================================================================
        '                               PASO 2 OBTENIENDO TODOS LOS DEMAS ELEMENTOS
        '===============================================================================================

        'conjunto.RemoveItems(entidadLineaCentral)
        'For Each element In conjunto
        'lista.Items.Add(element.ObjectName & " " & "Handler=" & element.Handle) '
        'Next




        'Esto esta al comienzo de la funcion, al tener todo funcionando mover ese codigo aqui






        '===============================================================================================
        '                               PASO 3 MOVIENDO CIRCULOS Y LINEAS
        '===============================================================================================

        For Each element In conjunto

            Select Case element.ObjectName
                Case "AcDbCircle"
                    'lista.Items.Add(">>>>>Radio=" & element.radius)
                    centro = element.center
                    'nuevocentro = centro
                    nuevocentro(0) = entidadLineaCentral.startpoint(0)
                    nuevocentro(1) = centro(1)
                    nuevocentro(2) = centro(2)
                    element.Move(centro, nuevocentro)
                    lista.Items.Add(element.ObjectName & " " & "Handler= " & element.Handle & " CentroOriginal= " & centro(0).ToString & " NuevoCentro= " & nuevocentro(0).ToString)
                Case "AcDbLine"
                    If (element.Handle <> entidadLineaCentral.Handle) Then
                        p4 = element.startpoint
                        p5 = element.endpoint
                        element.Move(p4, p3)
                        lista.Items.Add(element.ObjectName & " " & "Handler=" & element.Handle & " PuntoOriginal= " & p4(0).ToString & " NuevoPunto = " & p3(0).ToString)
                    End If


            End Select
        Next
        conjunto.Delete()

        '===============================================================================================
        '                               PASO 4 DESTRASLAPANDO CIRCULOS
        '===============================================================================================

    End Function

    Public Sub agregarDiccionarioEntidad()
        'AGREGANDO UN DICCIONARIO A UNA ENTIDAD.
        Dim entidad As AcadEntity
        appactivateAutoCAD()
        entidad = getEntidad("Selecciona una entidad") 'entidad para agregar el dato
        If Not IsNothing(entidad) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(entidad, "MATERIAL", "ACERO")
            MsgBox("Se ha agregado un diccionario a la entidad")

        End If
    End Sub
    Public Sub cambiarDiccionarioEntidad()
        'AGREGANDO UN DICCIONARIO A UNA ENTIDAD.
        Dim entidad As AcadEntity
        appactivateAutoCAD()
        entidad = getEntidad("Selecciona una entidad") 'entidad para agregar el dato
        If Not IsNothing(entidad) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(entidad, "MATERIAL", "MADERA")
            'MsgBox("Se ha agregado un diccionario a la entidad")

        End If
    End Sub

    Public Sub recuperarDiccionarioAEntidad()
        'agregando un diccionario a una entidad
        Dim entidad As AcadEntity
        Dim material As String = Nothing


        appactivateAutoCAD()
        entidad = getEntidad("selecciona una entidad")
        If Not IsNothing(entidad) Then
            getXdata(entidad, "MATERIAL", material)
            If material Is Nothing Then
                DOCUMENTO.Utility.Prompt("el objeto no tiene material")
            Else
                DOCUMENTO.Utility.Prompt("El material del objeto es " & material)
            End If
        End If
    End Sub


    Public Sub addXdata(entidad As AcadEntity, nameXrecord As String, valor As String)
        'Agrega un Xrecord y un solo valor
        'Agregar el diccionario al objeto
        'Agregar un registro
        'Ponerle un vlaor
        Dim dictASTI As AcadDictionary
        Dim astiXRec As AcadXRecord
        Dim keyCode() As Short 'Obligadorio que sea short. integer envia  error en el setrecordadata 'llave
        Dim cptData() As Object 'Obligatorio object                         'dato

        Dim getKey As Object = Nothing
        Dim getData As Object = Nothing

        ReDim keyCode(0)
        ReDim cptData(0)

        'Este meto recregsa un objeto de tipo diccionario 
        'Ya teniendo el dicicionario se usa addxrecod y ese record se le pone valores y se le asignana valores al registro. se hace con SetXRecordData
        dictASTI = entidad.GetExtensionDictionary                  'accediendo al dicc del elemento
        astiXRec = dictASTI.AddXRecord(nameXrecord.ToUpper.Trim)   'agrega el registro astiXRec es el registro
        'el codigo 100 es de un string
        keyCode(0) = 100 : cptData(0) = valor     'poner valores a los registros
        astiXRec.SetXRecordData(keyCode, cptData) 'asignar los valores a los registros


        'astiXRec.GetXRecordData(getKey, getData)
        'If Not IsNothing(getData) Then
        '    valor = getData(0) 'recuperando el valor del XRecord
        '    MsgBox("El valor es=" & valor.ToString)

        'Else
        '    MsgBox("Dentro de addxdata no se encontro el el valor")
        'End If

    End Sub

    Public Sub getXdata(entidad As AcadEntity, nameXrecord As String, ByRef valor As String)
        'extrayendo datos
        'estamos considerando que un Xrecord solo tiene un dato lo cual..
        'Entro al diccionario 
        'Tengo que saber si el diccionario tiene el registro. 

        'agrga un Xrecord y un solo valor
        'Regresa los object como arreglos dinamicos con getxrecorddata.
        Dim astiXRec As AcadXRecord = Nothing
        Dim dictASTI As AcadDictionary
        Dim getKey As Object = Nothing
        Dim getData As Object = Nothing

        valor = Nothing
        dictASTI = entidad.GetExtensionDictionary
        Try
            astiXRec = dictASTI.Item(nameXrecord.ToUpper.Trim) ' Revisando si existe el Xrecord

        Catch ex As Exception
            'No existe el Xrecord
            'MsgBox("No existe el xrecrod")
        End Try

        If Not IsNothing(astiXRec) Then
            'MsgBox("Si existe el xrecrod")
            astiXRec.GetXRecordData(getKey, getData)
            If Not IsNothing(getData) Then
                valor = getData(0) 'recuperando el valor del XRecord
                'MsgBox("Si existe el valor del xrecord y es =" & valor.ToString)
            End If
        End If
    End Sub

    Public Function getEntidad(mensaje As String) As AcadEntity
        'nos regresa una ntidad de AutoCAD o Noghitn si no se seleccioa nada
        Dim entidad As AcadEntity = Nothing
        Dim p() As Double = Nothing
        Try
            DOCUMENTO.Utility.GetEntity(entidad, p, mensaje)
        Catch ex As Exception

        End Try
        Return entidad
    End Function



    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


    Public Function seguimientoCalle()

        'Para este ejemplo se tienen que crear las Layers en el Banner/Layers/LayerProperties y crear una que se llame LINEAS, CIRCULOS, OTROS
        'revisando todos los elementos del plano
        Dim conjunto As AcadSelectionSet = Nothing
        Dim conjuntoCalles As AcadSelectionSet = Nothing
        Dim conjuntoTemp As AcadSelectionSet = Nothing
        Dim conjuntoVehiculos As AcadSelectionSet = Nothing
        Dim conjuntoCallesEnBorde As AcadSelectionSet = Nothing
        Dim element As AcadEntity
        Dim vehiculo As AcadEntity
        Dim lines() As AcadEntity
        Dim calle As AcadEntity
        Dim temp(0) As AcadEntity
        Dim temp2(0) As AcadEntity
        Dim contorno As AcadEntity = Nothing
        Dim nextStreet As AcadEntity
        Dim lista As ListBox
        Dim FilterType(0) As Integer
        Dim FilterData(0) As Object
        Dim p1() As Double
        Dim p2() As Double
        Dim numMov As Integer = 200
        Dim incX, incY As Double
        Dim currentPosition(0 To 2) As Double
        Dim nextPosition(0 To 2) As Double
        Dim prueba(2) As Object
        Dim intersectionpoints As VariantType
        Dim str As String = Nothing
        Dim I As Integer, j As Integer, k As Integer
        Dim numCalles As Integer = Nothing
        Dim fileName As String, textData As String, textRow As String, fileNo As Integer
        Dim numCarros As Integer
        'deprecaded:
        'Dim point1Auto() As Double
        'Dim point2Auto() As Double
        'Dim point3Auto() As Double
        'Dim point4Auto() As Double
        'Dim conjuntoPolilineas As AcadSelectionSet
        'Dim automovil As AcadSolid
        'fileName = "C:\test.txt"
        'fileNo = FreeFile()

        'Open fileName For Output As #fileNo

        lista = Form3.ListBox1
        lista.Items.Clear()


        'El siguiente codigo deberia seleccionar solo las lineas del dibujo pero no esta funcionando
        'conjuntoCalles.Select(AcSelect.acSelectionSetAll, FilterType, FilterData)


        '===================================================================================
        'PASO 1 Encontrando calles, contornos y demas 
        '===================================================================================

        'ObteniendoInfoDelPlano(conjuntoCalles, conjuntoVehiculos, conjuntoTemp, conjunto, contorno)
        'Creamos los conjuntos con los cuales trabajaremos
        conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
        conjuntoCalles = conjunto_vacio(DOCUMENTO, "Calles")
        conjuntoTemp = conjunto_vacio(DOCUMENTO, "Temp")
        conjuntoVehiculos = conjunto_vacio(DOCUMENTO, "Vehiculos")
        conjuntoCallesEnBorde = conjunto_vacio(DOCUMENTO, "Borde")
        conjunto.Select(AcSelect.acSelectionSetAll)


        'Clasificamos en conjuntos los elementos encontrados en el plano
        For Each element In conjunto
            'Obtenemos el contorno
            If element.ObjectName = "AcDbPolyline" Then
                contorno = element
                'MsgBox("Se encontro el contorno")

            End If
            'Obtenemos las calles
            If element.ObjectName = "AcDbLine" Then
                temp2(0) = element
                conjuntoCalles.AddItems(temp2)
                'MsgBox("Se ha agregado elemento a calles")
            End If
        Next

        '===================================================================================
        'PASO 2 Crear los vehiculos en las calles
        '===================================================================================

        numCarros = CreandoVehiculos(conjuntoCalles, conjuntoVehiculos, conjuntoCallesEnBorde, contorno, lista)

        For Each vehiculo In conjuntoVehiculos
            nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, numCalles, lista), numCalles)
            GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
            moverVehiculo(nextStreet, vehiculo, 200, lista)
        Next

        For Each vehiculo In conjuntoVehiculos
            nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, numCalles, lista), numCalles)
            GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
            moverVehiculo(nextStreet, vehiculo, 200, lista)
        Next





        'conjunto.Delete()
    End Function
    Public Function CreandoVehiculos(conjuntoCalles As AcadSelectionSet, conjuntoVehiculos As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, contorno As AcadEntity, lista As ListBox) As Integer

        Dim calle As AcadEntity
        Dim contadorvehiculos As Integer
        Dim contadorCalles As Integer
        Dim arrcontorno() As Double
        Dim p1() As Double
        Dim p2() As Double
        Dim vehiculo As AcadEntity
        Dim temp(0) As AcadEntity
        Dim tempcallesenborde(0) As AcadEntity

        contadorvehiculos = 0

        For Each calle In conjuntoCalles

            'lista.Items.Add(calle.ObjectName & " " & "Handler=" & calle.Handle) '
            'element.Layer = "LINEAS"
            p1 = calle.startpoint
            p2 = calle.endpoint

            contadorCalles = contadorCalles + 1

            arrcontorno = Nothing
            'Pregunto si hay interseccion entre la calle y el contorno y si la hay guardo el punto de interseccion en el arreglo arrcontorno
            arrcontorno = contorno.IntersectWith(calle, AcExtendOption.acExtendNone)
            'Si el tamanio del arreglo es mayor a cero entonces tiene las coordenadas guardadas por lo que en efecto hubo interseccion
            If (UBound(arrcontorno) > 0) Then
                If (p1(0) = arrcontorno(0) And p1(1) = arrcontorno(1)) Then
                    tempcallesenborde(0) = calle
                    conjuntoCallesEnBorde.AddItems(tempcallesenborde)
                End If

                'Necesito saber si intersecta en el punto inicial o en el punto final de linea
                'Luego necesito leer el diccionario para saber la direccion de la calle 
                'En base a eso iria una anidacion de ifs del siguiente estilo
                'if (intersecta en punto final)
                '   if (direccion de la calle = izq)
                '       aparecer carro

                'if (intersecta en punto inicial)
                '   if (direccion de la calle = der)
                '       aparecer carro


                vehiculo = DOCUMENTO.ModelSpace.AddBox(p1, 2, 1, 2)
                'Le agregamos su respectivo diccionario
                AddDictionaryToVehiculo(vehiculo)
                temp(0) = vehiculo
                conjuntoVehiculos.AddItems(temp)
                contadorvehiculos = contadorvehiculos + 1


            End If
            '[PEND} agregar un else con un random que de igual manaera agrege vehiculos 


        Next
        Return contadorvehiculos

    End Function

    Public Sub GirarVehiculoSegunOrientacionDeLaCalle(nextStreet As AcadEntity, vehiculo As AcadEntity, lista As ListBox)

        Dim pendiente As Double
        Dim p1() As Double
        Dim p2() As Double
        Dim ca, co, hip, angle As Double
        Dim valor As String = Nothing
        Dim centroideVehiculo() As Double = Nothing



        'Calcularemos la pendiente de la siguiente calle
        p1 = nextStreet.startpoint
        p2 = nextStreet.endpoint

        pendiente = (p2(1) - p1(1)) / (p2(0) - p1(0))
        'Hacemos trigonometria para obtener el angulo 
        ca = (p2(0) - p1(0))
        co = (p2(1) - p1(1))
        hip = Math.Sqrt(Math.Pow(ca, 2) + Math.Pow(co, 2))
        angle = Math.Acos(ca / hip)
        'Ajustamos angle para dar el giro correcto dependiendo de la pendiente de la calle 
        If pendiente < 0 Then
            angle = angle * -1
        End If

        'Obtenemos el valor del augulo anterior para poder determinar como girar
        'Notese que no importa si el angulo anterior fue negativo o positivo, se ajustara hacia la direccion contraria 
        'gracias a la multiplicacion por -1
        GetDictionaryVehiculo(vehiculo, "ANGULOACTUAL", valor)
        If valor <> "VACIO" Then
            'anguloactual = CDbl(valor)
            vehiculo.Rotate(p1, CDbl(valor) * -1)
        End If

        vehiculo.Rotate(p1, angle)
        UpdateDictionaryToVehiculo(vehiculo, angle)

    End Sub

    Public Function moverVehiculo(calle As AcadEntity, vehiculo As AcadEntity, numMov As Integer, lista As ListBox)

        Dim p1(), p2() As Double
        Dim incX, incY As Double
        Dim currentPosition(0 To 2) As Double
        Dim nextPosition(0 To 2) As Double
        Dim centroideVehiculo() As Double = Nothing

        p1 = calle.startpoint
        p2 = calle.endpoint

        'Se establece un incremento a partir de el numero de movimientos que se deseen para el vehiculo
        'Esto tiene el inconveniente de que entre mas grande sea la calle mas grande seran los saltos que vaya
        'dando el vehiculo. 
        incX = (p2(0) - p1(0)) / numMov
        incY = (p2(1) - p1(1)) / numMov


        'Se guarda la posicion actual
        currentPosition(0) = p1(0)
        currentPosition(1) = p1(1)

        For count = 1 To numMov


            'Se mueve el vehiculo a la siguiente posicion
            If (count = numMov) Then
                vehiculo.Move(currentPosition, p2)
            Else
                'la siguiente posicion consistira  en la posicion actual mas un incremento
                nextPosition(0) = currentPosition(0) + incX
                nextPosition(1) = currentPosition(1) + incY

                vehiculo.Move(currentPosition, nextPosition)

                currentPosition(0) = nextPosition(0)
                currentPosition(1) = nextPosition(1)

            End If
            vehiculo.Update()

            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & currentPosition(0) & " // " & currentPosition(1)) '
            'WasteTime(1)
            'PauseEvent(1)
        Next

    End Function

    Public Sub AddDictionaryToVehiculo(vehiculo As AcadEntity, origin As Double(), destiny As Double(), steps As Integer, handle As Integer, angle As Double, position As Double())
        If Not IsNothing(vehiculo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ORIGEN_X", Str(origin(0)))
            addXdata(vehiculo, "ORIGEN_Y", Str(origin(1)))
            addXdata(vehiculo, "DESTINO_X", Str(destiny(0)))
            addXdata(vehiculo, "DESTINO_Y", Str(destiny(1)))
            addXdata(vehiculo, "NUMSTEPS", Str(steps))
            addXdata(vehiculo, "HANDLE", Str(handle))
            addXdata(vehiculo, "ANGULOACTUAL", Str(angle))
            addXdata(vehiculo, "POSICIONACTUAL_X", Str(position(0)))
            addXdata(vehiculo, "POSICIONACTUAL_Y", Str(position(1)))
            addXdata(vehiculo, "START", "FALSE")
            'MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")
        End If
    End Sub

    Public Sub AddDictionaryToVehiculo(vehiculo As AcadEntity)
        If Not IsNothing(vehiculo) Then

            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ORIGEN_X", "VACIO")
            addXdata(vehiculo, "ORIGEN_Y", "VACIO")
            addXdata(vehiculo, "DESTINO_X", "VACIO")
            addXdata(vehiculo, "DESTINO_Y", "VACIO")
            addXdata(vehiculo, "NUMSTEPS", "VACIO")
            addXdata(vehiculo, "HANDLE", "VACIO")
            addXdata(vehiculo, "ANGULOACTUAL", "VACIO")
            addXdata(vehiculo, "POSICIONACTUAL_X", "VACIO")
            addXdata(vehiculo, "POSICIONACTUAL_Y", "VACIO")
            addXdata(vehiculo, "START", "TRUE")

            'MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")

        End If
    End Sub
    Public Sub UpdateDictionaryToVehiculo(vehiculo As AcadEntity, angle As Double)
        If Not IsNothing(vehiculo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ANGULOACTUAL", Str(angle))
        End If
    End Sub

    Public Sub GetDictionaryVehiculo(vehiculo As AcadEntity, llave As String, ByRef valor As String)
        If Not IsNothing(vehiculo) Then
            getXdata(vehiculo, llave, valor)
        End If
    End Sub

    Public Function getNextStreet(callesdeinterseccion As Collection, numCalles As Integer) As AcadEntity
        'Esta funcion toma todas las calles de interseccion, y aleatoriamente selecciona una para ser la siguiente calle en la trayectoria
        Dim randomnumber As Integer
        Randomize()
        randomnumber = Int(numCalles * Rnd()) + 1

        MsgBox("RANDOM = " & randomnumber & "," & "tamofcollection = " & callesdeinterseccion.Count)
        If callesdeinterseccion.Count <> 0 Then
            Return callesdeinterseccion.Item(randomnumber)
            MsgBox("se procede a ir a la calle numero " & randomnumber & ", de " & numCalles & " posibles")
        Else
            MsgBox("Aqui muere")
        End If

    End Function

    Public Function getNextPosibleStreets(vehiculo As AcadEntity, conjuntoCalles As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, numCalles As Integer, lista As ListBox) As Collection
        Dim calle As AcadEntity
        Dim p1calle() As Double
        Dim p2calle() As Double
        Dim centroide() As Double
        Dim callesenlainterseccion As Collection = Nothing
        callesenlainterseccion = New Collection

        centroide = vehiculo.centroid
        numCalles = 0

        For Each calle In conjuntoCalles
            p1calle = calle.startpoint
            p2calle = calle.endpoint
            lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & centroide(0) & " // " & centroide(1))
            lista.Items.Add("calle= " & calle.Handle & " p1 = " & p1calle(0) & " // " & p1calle(1) & " p2 = " & p2calle(0) & " // " & p2calle(1))
            If centroide(0) = p1calle(0) And centroide(1) = p1calle(1) Then
                callesenlainterseccion.Add(calle)
                numCalles = numCalles + 1
                MsgBox("Se encontraron posibles calles para avanzar #calles = " & callesenlainterseccion.Count)
            End If
        Next

        If callesenlainterseccion.Count = 0 Then
            MsgBox("Se llego a una condicion de fin de calle, se procede a reaparecer el vehiculo")
            callesenlainterseccion.Add(reaparecerVehiculo(vehiculo, conjuntoCallesEnBorde))
        End If

        Return callesenlainterseccion
    End Function

    Public Function reaparecerVehiculo(vehiculo As AcadEntity, conjuntoCallesEnBorde As AcadSelectionSet) As AcadEntity
        'Cuando el vehiculo llega al borde de la calle ya no tiene mas calles para seguir avanzando, asi que se selecciona una calle
        'de las que colindan con el borde aleatoriamente para desaparecer al vehiculo de donde esta (el punto sin avance) y aparecerlo
        'en la nueva calle, esta calle sera la siguiente calle en la cual circulara el vehiculo

        Dim numerodecalles As Integer
        Dim calle As AcadEntity
        Dim randomnumber As Integer
        Dim p1() As Double = Nothing


        numerodecalles = conjuntoCallesEnBorde.Count

        If numerodecalles > 0 Then
            Randomize()
            randomnumber = Int(numerodecalles * Rnd()) + 1
            calle = conjuntoCallesEnBorde(randomnumber)
            p1 = calle.startpoint
            vehiculo.Move(vehiculo.centroid, p1)
            MsgBox("Se pudo reaparece en la siguiente calle")
            Return calle
        Else
            MsgBox("No se pudo reaparecer en la siguiente calle")
            'Lo siguiente no esta probado, no se sabe como reaccionara el resto del codigo\
            'Si se llega hasta aqui lo mas probable es que el programa crashee por una exepcion 
            'conjuntoCallesEnBorde.RemoveItems(vehiculo)
            'vehiculo.Delete()

        End If

    End Function
End Module
'(entget(car(entsel))'("*"))






















'================================================================================================================================================
'                                                           CODIGO QUE YA NO SE USA 
'================================================================================================================================================


'================================================================================================================================================
'Public Function ObteniendoInfoDelPlano(conjuntoCalles As AcadSelectionSet, conjuntoVehiculos As AcadSelectionSet, conjuntoTemp As AcadSelectionSet, conjunto As AcadSelectionSet, contorno As AcadEntity)
'    'Esta funcion no funciono pero se deja aqui para futura referencia
'    Dim temp2(0) As AcadEntity

'    'Creamos los conjuntos con los cuales trabajaremos
'    conjunto = conjunto_vacio(DOCUMENTO, "IDLE")
'    conjuntoCalles = conjunto_vacio(DOCUMENTO, "Calles")
'    conjuntoTemp = conjunto_vacio(DOCUMENTO, "Temp")
'    conjuntoVehiculos = conjunto_vacio(DOCUMENTO, "Vehiculos")
'    conjunto.Select(AcSelect.acSelectionSetAll)


'    'Clasificamos en conjuntos los elementos encontrados en el plano
'    For Each element In conjunto
'        'Obtenemos el contorno
'        If element.ObjectName = "AcDbPolyline" Then
'            contorno = element
'            'MsgBox("Se encontro el contorno")

'        End If
'        'Obtenemos las calles
'        If element.ObjectName = "AcDbLine" Then
'            temp2(0) = element
'            conjuntoCalles.AddItems(temp2)
'            'MsgBox("Se ha agregado elemento a calles")
'        End If
'    Next
'End Function
'================================================================================================================================================
''Public Function AnalizarInterseccion(conjuntoCalles As AcadSelectionSet, calleOrigen As AcadEntity, numCalles As Integer) As Collection

''    'Esta funcion analiza cuantas calles estan conectadas con la calle de donde viene el automovil
''    Dim calle As AcadEntity
''    Dim p2Origen As Double()
''    Dim p1Destino As Double()
''    Dim callesenlainterseccion As Collection = Nothing
''    callesenlainterseccion = New Collection

''    numCalles = 0
''    p2Origen = calleOrigen.endpoint

''    For Each calle In conjuntoCalles
''        p1Destino = calle.startpoint
''        If p2Origen(0) = p1Destino(0) And p2Origen(1) = p1Destino(1) Then
''            callesenlainterseccion.Add(calle)
''            numCalles = numCalles + 1
''        End If
''    Next
''    Return callesenlainterseccion
''End Function
'================================================================================================================================================
'Public Function WhereIsMyCar(conjuntoCalles As AcadSelectionSet, vehiculo As AcadEntity)
'    Dim p1() As Double
'    Dim p2() As Double
'    Dim centroide() As Double
'    Dim calle As AcadEntity

'    centroide = vehiculo.centroid

'    If (centroide(0) = p1(0) And centroide(1) = p1(1)) Or (centroide(0) - p2(0) And centroide(1) = p2(1)) Then

'    End If



'End Function

