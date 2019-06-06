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


    ' EL ALGORITMO QUE SE IMPLEMENTO AQUI FUNCIONA DE LA SIGUIENTE MANERA: APARECEN CARROS EN AQUELLAS CALLES (LINEAS) QUE ESTAN TOCANDO AL CONTORNO (POLILINEA)
    ' LUEGO SE BUSCA PARA CADA VEHICULO CUALES CALLES COLINDAN CON EL PUNTO EN EL QUE APARECIO Y TOMA UNA DESICION (ALEATORIA) SOBRE CUAL CALLE SEGUIRA. EN LA PRIMERA EJECUCION LOS VEHICULOS
    ' APARECEN EN LAS CALLES QUE COLINDAN CON EL CONTORNO, POR LO CUAL SOLO HABRA UNA CALLE DISPONIBLE PARA AVANZAR (AQUELLA EN LA QUE APARECIO), PERO EN LAS SUBSECUENTES, EL VEHICULO SE ENCONTRARA EN CRUCES DE CALLES DONDE HABRA MAS DE UNA CALLE A ESCOGER COMO SIGUIENTE DESTINO, EN ESOS CASOS
    ' HACE UNA SELECCION ALEATORIA Y SE VA POR ESA NUEVA CALLE.
    ' PARA IR AVANZANDO POR LA CALLE TOMA EL PUNTO INICIAL, Y EL PUNTO FINAL DE LA CALLA (STARTPOINT,ENDPOINT) Y HACE UNA DIVISION ENTRE EL NUMERO DE PASOS INDICADO PARA DIVIDIR EL CAMINO EN PEQUENIOS PASOS
    ' VA MOVIENDOSE EN ESTOS PASOS HASTA LLEGAR AL FINAL DE LA CALLE DONDE VUELVE A INFERIR QUE CALLES HAY DISPONIBLES Y VUELVE A HACER UNA SELECCION ALEATORIA. 
    ' EN CADA CRUCE Y DESPUES DE HABER SELECCIONADO LA NUEVA CALLE POR LA QUE AVANZARA, HACE UN CALCULO TRIGONOMETRICO PARA GIRAR Y ALINEARSE AL ANGULO QUE TENGA LA CALLE 


    ' NOTESE ALGO SUMAMENTE IMPORTANTE, DEBIDO A QUE AVANZA EN LA CALLE DESDE EL PUNTO INICAL (STARTPOINT), HASTA EL PUNTO FINAL (ENDPOINT) DE LA MISMA, EL ORDEN EN COMO FUERON TRAZADAS LAS CALLES(LINEAS)
    ' ES IMPORTANTE POR QUE ESTE ORDEN MARCA LA DIRECCION DE LA CALLE, ES DECIR, SI LA CALLE VA DE IZQUIERDA A DERECHA, EN ESE ORDEN DEBIO SER DIBUJADA EN EL PLANO. PARA CADA SEGMENTO DE LA CALLE, SE SEGUIRA SU
    ' SENTIDO DE TRAZO. PARA QUE ESTO FUNCIONE EN CADA CRUCE DE CALLES DEL PLANO, DEBERA HABER SEGMENTOS DE LINEA CONECTADOS, ES DECIR SI HAY UNA CALLE CON CURCE TIPO (T) ( ES DECIR UNA CALLE QUE LLEGA A OTRA PERO
    ' QUE NO PUEDE CONTINUAR Y FORZOCAMENTE DEBE GIRAR PARA UNIRSE A LA NUEVA CALLE A LA QUE LLEG]O, ENTONCES CADA LINEA DE LA (T) DEBERA SER UN SEGMENTO DE LINEA (3 EN TOTAL) PARA QUE DE ESTA FORMA AL LLEGAR
    ' EL VEHICULO A ESE PUNTO DE INTERSECCION, PUEDA RECONOCER QUE HAY UN ENDPOINT (DE LA LINEA IZQUIERDA DE LA T) Y UN STARPOINT (DE LA LINEA DERECHA DE LA T) Y PUEDA IR EN SENTIDO DE LA CALLE UNIENDOSE AL 
    ' STARTPOINT Y YENDO HACIA EL ENPOINT DE ESE SEGMENTO DE LINEA. 





    'PENDIENTE 0 : SEGMENTAR LAS CALLES SEGUN LO EXPLICADO ARRIBA EN LE PLANO 
    'PENDIENTE 1 : mODIFICAR EL CODIGO DE LA FUNCION DE MOVIMIENTO PARA AVANCE PASO A PASO EL VEHICULO, LEERA EL DICCIONARIO PARA SABER EL PASO ACTUAL, AVANZARA UN PASO, Y MODIFICARA EL DICCIOANRIO PARA ACTUALIZARLO CON LA INFORMACION DEL NUEVO PASO ACTUAL. AHI EN LOS COMENTARIOS DE LA FUNCION seguimientoCalle() HAY UNA PEQUENIA EXPLICACION 
    'PENDIENTE 2 : BUSCAR UNA FUNCION O FORMA DE SABER SI DOS OBJETOS ESTAN CERCA UNO DEL OTRO O SE ESTAN TOCANDO (COMPARTEN AL MENOS UN PUNTO EN COMUN), CON ESTA INFORMACION EVITAR EL TRASLAPE DE LOS VEHICULOS EN SU MOVIMEINTOE
    'PENDIENTE 3 : CON LA INFORMACION DE INVESTIGADA EN EL PASO ANTERIOR TAMBIEN INTRODUCIR LOS SEMAFOROS
    'PENDIENTE 4 : HAY UN PROBLEMA EN LA FUNCION DE GIRO, EN ALGUNOS CASOS LOS VEHICULOS NO GIRAN COMO DEBERIAN, EN LA MAYORIA LO HACEN BIEN, PERO NO EN TODOS LOS CASOS (ESTO DEJENLO AL FINAL POR FAVOR, CUANDO TODO LO DEMAS YA ESTE JALANDO")
    'PENDIENTE 5 : HAY UN PROBLEMA EN LA FUNCION QUE DETERMINA SI HAY INTERSECCION ENTRE EL CONTORNO Y LAS CALLES (DENTRO DE LA FUNCION QUE CREA LOS VEHICULOS) AHI EN LOS COMENTARIOS HAY MAS INFORMACION ACERCA DE ESTE PROBLEMA



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
        Dim strStart As String
        Dim valor As String
        Dim pasoactual As Integer
        Dim numsteps As Integer
        '
        lista = Form3.ListBox1
        lista.Items.Clear()




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


        'PRIMERO PROCEDEMOS A CREAR LOS VEHICULOS EN AQUELLAS INTERSECCIONES ENTRE LAS CALLES Y EL CONTORNO
        numCarros = CreandoVehiculos(conjuntoCalles, conjuntoVehiculos, conjuntoCallesEnBorde, contorno, lista)







        '=========================================================================================================
        ''Esta seccion es la que se tiene que tener para que los vehicuos se muevan al mismo tiempo y no uno despues del otro 
        'para que funcone se tiene que modificar la funcion de movimiento para que consulte el diccionario y verifque en que paso esta el vehiculo
        'luego de un paso (un solo movimiento) y mmodifique los valores en el diccionario para acutalizar la informacion acerca del paso actual

        'De esta forma en el ciclo For Each aqui abajo, ira iterando por cada carro y por cada carro dara un paso y luego pasara al siguiente carro
        'con esto se lograra dar la sensacion de que todos avanzan simultaneamente 
        '=========================================================================================================

        'por los siglos de los siglos amen 
        'Do While 1
        For temporal = 1 To 100

            lista.Items.Add("========================= iteracion no " & temporal & "=========================") '

            For Each vehiculo In conjuntoVehiculos

                'Obtenemos informacion sobre si el carro ya ha avanzado o es su primera vez
                strStart = GetDictionaryVehiculo(vehiculo, "START", valor)
                lista.Items.Add("Vehiculo " & vehiculo.Handle & " Start? = " & strStart) '
                'MsgBox("Start? = " & strStart)


                'Si el paso actual no es el inicial entonces estamos en un punto intermedio de la calle 
                If (strStart = "TRUE") Then
                    'Recuperamos la informacion sobre la calle actual del vehiculo para seguirnos moviendo sobre esa misma.
                    nextStreet = getStreetByHandle(GetDictionaryVehiculo(vehiculo, "STREET", valor), conjuntoCalles)
                    'Ejectuamos el avance de un paso para el vehiculo sobre la calle antes recuperada.
                    moverVehiculo(nextStreet, vehiculo, 200, lista)
                End If
                'Si la bandera strStart es false entonces estamos en el punto inicial de la calle o en un punto de interseccion donde varias calles hay por delante
                'En cuyo caso tenemos que obtener la siguiente calle, girar acorde a ella, y realizar el primer movimiento que nos sacara del inicio del camino 
                If strStart = "FALSE" Then
                    'Obtenemos la siguente calle para avenzar de entre las calles disponibles para este proposito
                    nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, numCalles, lista), numCalles)
                    'Actualizamos la informacion de la calle en el diccionario que de aqui en adelante se pueda consultar e ir avanzando sobre esa misma calle 
                    lista.Items.Add("dado que es false El handle de la sig calle es " & nextStreet.Handle) '
                    'MsgBox("dado que es false El handle de la sig calle es " & nextStreet.Handle)
                    UpdateDictionaryToVehiculoTextField(vehiculo, "STREET", nextStreet.Handle)
                    If Not IsNothing(nextStreet) Then
                        'Giramos segun la orientacion de la nueva calle
                        GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
                        'Ejecutamos el avance del primer paso
                        moverVehiculo(nextStreet, vehiculo, 200, lista)
                    Else
                        MsgBox("Saltamos ese vehiculo por no encontrar calle siguiente")
                    End If
                End If

            Next
        Next
        'Loop












        ''=========================================================================================================
        ''Esta seccion funciona actualmente 
        ''=========================================================================================================

        'For Each vehiculo In conjuntoVehiculos
        '    nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, numCalles, lista), numCalles)
        '    If Not IsNothing(nextStreet) Then
        '        GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
        '        moverVehiculo(nextStreet, vehiculo, 200, lista)
        '    Else
        '        MsgBox("Saltamos ese vehiculo por no encontrar calle siguiente")
        '    End If

        'Next





        'conjunto.Delete()
    End Function
    Public Function CreandoVehiculos(conjuntoCalles As AcadSelectionSet, conjuntoVehiculos As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, contorno As AcadEntity, lista As ListBox) As Integer
        'eSTA FUNCION CREA LOS VEHICULOS EN LAS INTERSECCIONES CON EL CONTORNO (POLILINEA)
        Dim calle As AcadEntity
        Dim contadorvehiculos As Integer
        Dim contadorCalles As Integer
        Dim arrcontorno() As Double
        Dim p1() As Double
        Dim p2() As Double
        Dim vehiculo As AcadEntity
        Dim temp(0) As AcadEntity
        Dim tempcallesenborde(0) As AcadEntity
        Dim tempcontadorcallesencontorno As Integer

        contadorvehiculos = 0
        tempcontadorcallesencontorno = 0

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
            'MsgBox("Se han revisado las calles que intesectan con el contorno ")
            If (arrcontorno.Count > 0) Then
                'La siguiente condicion es vital para que los vehiculos desaparezcan de un borde y aparezcan en otro, si no se tiene la siguiente 
                'condicion apareceran carros en medio del plano, esto no es deseable, sin embargo por algun motivo ocurre algo muy raro:
                'En algunos casos La primer condicion se cumple, lo cual significa que se detecta una interseccion entre el contorno y la calle
                'sin embargo cuando en la siguiente condicion se intenta comparar coordenadas, se obtienen coordenadas diferentes (A pesar de que la condicion anterior nos dice que si interceptan)
                'y por lo tanto no detecta bien cuales son las calles que intersectan.
                'Es observable que la diferencia entre las coordenadas de la interseccion y las coordenadas del punto de la linea no son muy grandes, por lo que esto
                'se podria resolver haciendo una resta y poniendo una condicion que testee si la resta es minima. 
                'MsgBox("El tamani del arreglo que guarda las coordenadas es " & arrcontorno.Count & ", " & arrcontorno(0) & "//" & arrcontorno(1) & "                                    Coordenadas de la linea: X=" & p1(0) & "//" & p1(1) & "," & "Y=" & p2(0) & "//" & p2(1))
                'If (((p1(0) = arrcontorno(0) Or p1(0) = arrcontorno(1)) And ((p1(1) = arrcontorno(1)) Or p1(1) = arrcontorno(0))) Or ((p2(0) = arrcontorno(0) Or p2(0) = arrcontorno(1)) And ((p2(1) = arrcontorno(1)) Or p2(1) = arrcontorno(0)))) Then
                tempcallesenborde(0) = calle
                conjuntoCallesEnBorde.AddItems(tempcallesenborde)
                tempcontadorcallesencontorno = tempcontadorcallesencontorno + 1
                'MsgBox("Se encontro nueva calle en el contorno")



                ''''''''''''''Necesito saber si intersecta en el punto inicial o en el punto final de linea
                ''''''''''''''Luego necesito leer el diccionario para saber la direccion de la calle 
                ''''''''''''''En base a eso iria una anidacion de ifs del siguiente estilo
                ''''''''''''''if (intersecta en punto final)
                ''''''''''''''   if (direccion de la calle = izq)
                ''''''''''''''       aparecer carro

                ''''''''''''''if (intersecta en punto inicial)
                ''''''''''''''   if (direccion de la calle = der)
                ''''''''''''''       aparecer carro

                'CREAMOS EL VEHICULO EN EL PUNTO INICIAL DE LA CALLE 
                vehiculo = DOCUMENTO.ModelSpace.AddBox(p1, 2, 1, 2)
                'Le agregamos su respectivo diccionario
                AddDictionaryToVehiculo(vehiculo)
                temp(0) = vehiculo
                conjuntoVehiculos.AddItems(temp)
                contadorvehiculos = contadorvehiculos + 1
                'End If



            End If
            '[PEND} agregar un else con un random que de igual manaera agrege vehiculos aleatoriamente en la parte central del plano 


        Next
        'MsgBox("el numero total de calles en el contorno es = " & tempcontadorcallesencontorno)
        lista.Items.Add("el numero total de calles en el contorno es = " & tempcontadorcallesencontorno) '
        Return contadorvehiculos

    End Function

    Public Sub GirarVehiculoSegunOrientacionDeLaCalle(nextStreet As AcadEntity, vehiculo As AcadEntity, lista As ListBox)

        'Le pasamos la nueva calle en la que se encuntra el vehiculo y el vehiculo en cuestion para que haga trigonometria y calcule el angulo que debe tener el 
        'vehiculo para alinearse al angulo de la calle. 

        Dim pendiente As Double
        Dim p1() As Double
        Dim p2() As Double
        Dim ca, co, hip, angle As Double
        Dim valor As String = Nothing
        Dim centroideVehiculo() As Double = Nothing


        '
        If Not IsNothing(nextStreet) Then
            'Calcularemos la pendiente de la siguiente calle
            p1 = nextStreet.startpoint
            p2 = nextStreet.endpoint
        Else
            MsgBox("No se encontro nextstreet en funcion de giro")
        End If


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

        'Esta funcion es la que se tiene que modificar para ir avanzando paso a paso en concordancia con el codigo comentado de la funcion seguimientoCalle()

        Dim p1(), p2() As Double
        Dim incX, incY As Double
        Dim currentPosition(0 To 2) As Double
        Dim nextPosition(0 To 2) As Double
        Dim centroideVehiculo() As Double = Nothing
        Dim start As String = Nothing
        Dim pasoactual As Integer
        Dim numsteps As Integer
        Dim startstreet(0 To 2) As Double
        Dim endstreet(0 To 2) As Double
        Dim valor As String = Nothing



        start = GetDictionaryVehiculo(vehiculo, "START", valor)

        'La primera vez que vamos a mover el vehiculo determinamos los parametros de movimiento. Punto inicial, punto final, numero de pasos a avanzar y paso actual.
        If start = "FALSE" Then

            'Indicamos que el vehiculo ha comenzado su viaje a traves de la calle
            UpdateDictionaryToVehiculoTextField(vehiculo, "START", "TRUE")
            lista.Items.Add("El vehiculo da el primer paso") '
            'MsgBox("El vehiculo da el primer paso")

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
            pasoactual = 1

            UpdateDictionaryToVehiculoTextField(vehiculo, "NUMSTEPS", Str(numMov))
            UpdateDictionaryToVehiculoTextField(vehiculo, "ORIGEN_X", Str(p1(0)))
            UpdateDictionaryToVehiculoTextField(vehiculo, "ORIGEN_Y", Str(p1(1)))
            UpdateDictionaryToVehiculoTextField(vehiculo, "DESTINO_X", Str(p2(0)))
            UpdateDictionaryToVehiculoTextField(vehiculo, "DESTINO_Y", Str(p2(1)))
            UpdateDictionaryToVehiculoTextField(vehiculo, "INCX", Str(incX))
            UpdateDictionaryToVehiculoTextField(vehiculo, "INCY", Str(incY))

            UpdateDictionaryToVehiculoTextField(vehiculo, "PASOACTUAL", Str(1))
            UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_X", Str(p1(0)))
            UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_Y", Str(p1(1)))


            'la siguiente posicion consistira  en la posicion actual mas un incremento
            nextPosition(0) = currentPosition(0) + incX
            nextPosition(1) = currentPosition(1) + incY

            vehiculo.Move(currentPosition, nextPosition)

            currentPosition(0) = nextPosition(0)
            currentPosition(1) = nextPosition(1)


            vehiculo.Update()


            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & currentPosition(0) & " // " & currentPosition(1)) '
            'WasteTime(1)
            'PauseEvent(1)


        End If
        '==========================================================================================================
        'En los subsecuentes pasos solo consultamos esta informacion y la vamos actualizando.
        If start = "TRUE" Then

            'Obtenemos los valores guardados en el diccionario
            If Not IsNothing(vehiculo) Then
                startstreet(0) = CDbl(GetDictionaryVehiculo(vehiculo, "ORIGEN_X", valor))
                startstreet(1) = CDbl(GetDictionaryVehiculo(vehiculo, "ORIGEN_Y", valor))
                currentPosition(0) = CDbl(GetDictionaryVehiculo(vehiculo, "POSICIONACTUAL_X", valor))
                currentPosition(1) = CDbl(GetDictionaryVehiculo(vehiculo, "POSICIONACTUAL_Y", valor))
                pasoactual = CInt(GetDictionaryVehiculo(vehiculo, "PASOACTUAL", valor))
                numsteps = CInt(GetDictionaryVehiculo(vehiculo, "NUMSTEPS", valor))
                endstreet(0) = CDbl(GetDictionaryVehiculo(vehiculo, "DESTINO_X", valor))
                endstreet(1) = CDbl(GetDictionaryVehiculo(vehiculo, "DESTINO_Y", valor))
                incX = CDbl(GetDictionaryVehiculo(vehiculo, "INCX", valor))
                incY = CDbl(GetDictionaryVehiculo(vehiculo, "INCY", valor))
            Else
                MsgBox("gameover")
            End If




            'Se mueve el vehiculo a la siguiente posicion
            If (pasoactual = numsteps) Then
                vehiculo.Move(currentPosition, endstreet)
                UpdateDictionaryToVehiculoTextField(vehiculo, "START", "FALSE")
                lista.Items.Add("El LLEGO AL FIN DE LA CALLE") '
            Else
                lista.Items.Add("incx " & incX & "//" & " incy " & incY) '
                'la siguiente posicion consistira  en la posicion actual mas un incremento
                nextPosition(0) = currentPosition(0) + incX
                nextPosition(1) = currentPosition(1) + incY
                lista.Items.Add("VEHICULO AVANZARA DE " & currentPosition(0) & "//" & currentPosition(1) & " To " & nextPosition(0) & "//" & nextPosition(1)) '
                vehiculo.Move(currentPosition, nextPosition)

                currentPosition(0) = nextPosition(0)
                currentPosition(1) = nextPosition(1)
                pasoactual = pasoactual + 1

            End If

            UpdateDictionaryToVehiculoTextField(vehiculo, "PASOACTUAL", Str(pasoactual + 1))
            UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_X", Str(currentPosition(0)))
            UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_Y", Str(currentPosition(1)))

            'MsgBox("dado que start es true El vehiculo avanza sobre la calle")
            vehiculo.Update()

            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & currentPosition(0) & " // " & currentPosition(1)) '
            'WasteTime(1)
            'PauseEvent(1)


        End If

    End Function



    Public Sub AddDictionaryToVehiculo(vehiculo As AcadEntity)
        'Esta funcion inicializa el diccionario del vehiculo con valores vacios
        'NOTESE QUE HAY VALORES EN DONDE SE PUEDE GUARDAR LA X Y Y ACTUALES, EL NUMERO DE PASO ACUTAL, EL NUMERO DE PASOS A SEGUIR Y EL DESTINO, CON ESOS DATOS SE PUEDE IR 
        'AVANZANDO PASO A PASO DESPUES DE MODIFICAR LA FUNCION DE MOVIMIENTO SEGUN LO QUE SE INDICA EN LOS COMENTARIOS DE LA FUNCION seguimientoCalles()

        If Not IsNothing(vehiculo) Then

            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ORIGEN_X", "VACIO")
            addXdata(vehiculo, "ORIGEN_Y", "VACIO")
            addXdata(vehiculo, "DESTINO_X", "VACIO")
            addXdata(vehiculo, "DESTINO_Y", "VACIO")
            addXdata(vehiculo, "NUMSTEPS", "0")
            addXdata(vehiculo, "HANDLE", "VACIO")
            addXdata(vehiculo, "ANGULOACTUAL", "VACIO")
            addXdata(vehiculo, "POSICIONACTUAL_X", "VACIO")
            addXdata(vehiculo, "POSICIONACTUAL_Y", "VACIO")
            addXdata(vehiculo, "START", "FALSE")
            addXdata(vehiculo, "PASOACTUAL", "0")
            addXdata(vehiculo, "X_ACTUAL", "VACIO")
            addXdata(vehiculo, "Y_ACTUAL", "VACIO")
            addXdata(vehiculo, "INCX", "VACIO")
            addXdata(vehiculo, "INCY", "VACIO")
            addXdata(vehiculo, "STREET", "VACIO")


            'MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")

        End If
    End Sub
    Public Sub UpdateDictionaryToVehiculo(vehiculo As AcadEntity, angle As Double)
        'ESTA FUNCION ES EXTREMADAMENTE UTIL POR QUE NOS PERMITE ACTUALIZAR EL VALOR DE UN DATO DEL DICCIONARIO, ESTA ACTUALIZACION SE USA POR EJEMPLO EN LA FUNCION DE GIRO AL ACTUALIZAR EL ANGULO DEL VEHICULO
        'ESTA FUNCION SERA NECESARIA PARA ACTUALIZAR LOS DATOS DURANTE CADA PASO DEL MOVIMIENTO. eSTOS PASOS NECESITAN SERA ACTUALIZADOS PARA QUE LA FUNCION DE MOVIMIENT SEPA EN QUE PASO ESTAMOS Y POR LO TANDO DAR EL SIGUIENTE
        If Not IsNothing(vehiculo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, "ANGULOACTUAL", Str(angle))
        End If
    End Sub

    Public Function GetDictionaryVehiculo(vehiculo As AcadEntity, llave As String, ByRef valor As String)
        'ESTA FUNCION SIMILAR A LA ANTERIOR PERO EN LUGAR DE MODIFICAR EL DATO DEL DICCIONARIO LO CONSULTA, SE SUELEN USAR JUNTAS AMBAS FUCIONESS
        Dim valorretorno As String = Nothing
        If Not IsNothing(vehiculo) Then
            getXdata(vehiculo, llave, valor)
            valorretorno = valor
        End If
        Return valorretorno
    End Function

    Public Function getNextStreet(callesdeinterseccion As Collection, numCalles As Integer) As AcadEntity

        'Esta funcion toma todas las calles de interseccion, y aleatoriamente selecciona una para ser la siguiente calle en la trayectoria
        'LAS CALLES DE LA INTERSECCION SE OBTIENEN DE LA FUNCION SIGUIENTE getNextPosibleStreets.
        Dim randomnumber As Integer
        Randomize()
        randomnumber = Int((callesdeinterseccion.Count - 1 + 1) * Rnd() + 1)


        MsgBox("getnextstreet: RANDOM = " & randomnumber & "," & "tamofcollection = " & callesdeinterseccion.Count)
        If callesdeinterseccion.Count <> 0 Then
            Return callesdeinterseccion.Item(randomnumber)
            'MsgBox("Getnextstreet: se procede a ir a la calle numero " & randomnumber & ", de " & numCalles & " posibles")
        Else
            MsgBox("Aqui muere")
        End If

    End Function

    Public Function getNextPosibleStreets(vehiculo As AcadEntity, conjuntoCalles As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, numCalles As Integer, lista As ListBox) As Collection
        'Esta funcion busca cuales son las calles que colindan con la interseccion en la que se encuentra nuestro vehiculo de forma tal que posteriormente se pueda decidir a cual de ellas ir 
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
            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & centroide(0) & " // " & centroide(1))
            'lista.Items.Add("calle= " & calle.Handle & " p1 = " & p1calle(0) & " // " & p1calle(1) & " p2 = " & p2calle(0) & " // " & p2calle(1))
            If centroide(0) = p1calle(0) And centroide(1) = p1calle(1) Then
                callesenlainterseccion.Add(calle)
                numCalles = numCalles + 1
                'MsgBox("Se encontraron posibles calles para avanzar #calles = " & callesenlainterseccion.Count)
            End If
        Next

        If callesenlainterseccion.Count = 0 Then
            callesenlainterseccion.Clear()
            'MsgBox("Se llego a una condicion de fin de calle, se procede a reaparecer el vehiculo")
            callesenlainterseccion.Add(reaparecerVehiculo(vehiculo, conjuntoCallesEnBorde))
        End If

        Return callesenlainterseccion
    End Function

    Public Function reaparecerVehiculo(vehiculo As AcadEntity, conjuntoCallesEnBorde As AcadSelectionSet) As AcadEntity

        'Cuando el vehiculo llega al borde de la calle ya no tiene mas calles para seguir avanzando, asi que se selecciona una calle, asi que debemos desaparecerlo de ahi y reaparecerlo en el comienzo de otra calle
        'de las que colindan con el borde aleatoriamente para desaparecer al vehiculo de donde esta (el punto sin avance) y aparecerlo
        'en la nueva calle, esta calle sera la siguiente calle en la cual circulara el vehiculo

        Dim numerodecalles As Integer
        Dim calle As AcadEntity
        Dim randomnumber As Integer
        Dim p1() As Double = Nothing


        numerodecalles = conjuntoCallesEnBorde.Count
        MsgBox("El numero de calles en el borde es = " & numerodecalles)

        If numerodecalles <> 0 Then
            Randomize()
            randomnumber = Int((numerodecalles - 1 + 1) * Rnd() + 1)
            MsgBox("La calle a la que se ira sera la siguiente = " & randomnumber)
            calle = conjuntoCallesEnBorde(randomnumber)
            If Not IsNothing(calle) Then
                p1 = calle.startpoint
                vehiculo.Move(vehiculo.centroid, p1)
                MsgBox("Se pudo reaparecer en la siguiente calle")
            Else
                MsgBox("No se recupero calle")
            End If


            Return calle
        Else
            MsgBox("No se pudo reaparecer en la siguiente calle")
            'Lo siguiente no esta probado, no se sabe como reaccionara el resto del codigo\
            'Si se llega hasta aqui lo mas probable es que el programa crashee por una exepcion 
            'conjuntoCallesEnBorde.RemoveItems(vehiculo)
            'vehiculo.Delete()

        End If

    End Function


    Public Sub UpdateDictionaryToVehiculoTextField(vehiculo As AcadEntity, campo As String, valor As String)
        'ESTA FUNCION ES EXTREMADAMENTE UTIL POR QUE NOS PERMITE ACTUALIZAR EL VALOR DE UN DATO DEL DICCIONARIO, ESTA ACTUALIZACION SE USA POR EJEMPLO EN LA FUNCION DE GIRO AL ACTUALIZAR EL ANGULO DEL VEHICULO
        'ESTA FUNCION SERA NECESARIA PARA ACTUALIZAR LOS DATOS DURANTE CADA PASO DEL MOVIMIENTO. eSTOS PASOS NECESITAN SERA ACTUALIZADOS PARA QUE LA FUNCION DE MOVIMIENT SEPA EN QUE PASO ESTAMOS Y POR LO TANDO DAR EL SIGUIENTE
        If Not IsNothing(vehiculo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(vehiculo, campo, valor)
        End If
    End Sub

    Public Function getStreetByHandle(handle As String, conjuntoCalles As AcadSelectionSet) As AcadEntity

        For Each calle In conjuntoCalles
            If calle.Handle = handle Then
                Return calle
            End If
        Next
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


'Public Sub AddDictionaryToVehiculo(vehiculo As AcadEntity, origin As Double(), destiny As Double(), steps As Integer, handle As Integer, angle As Double, position As Double())
'    'Esta funcion se disenio para actualizar todos los datos del diccionario del vehiculo pero hasta el momento no ha surgido la necesidad de actualiza todos los datos simultaneamente
'    If Not IsNothing(vehiculo) Then
'        'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
'        addXdata(vehiculo, "ORIGEN_X", Str(origin(0)))
'        addXdata(vehiculo, "ORIGEN_Y", Str(origin(1)))
'        addXdata(vehiculo, "DESTINO_X", Str(destiny(0)))
'        addXdata(vehiculo, "DESTINO_Y", Str(destiny(1)))
'        addXdata(vehiculo, "NUMSTEPS", Str(steps))
'        addXdata(vehiculo, "HANDLE", Str(handle))
'        addXdata(vehiculo, "ANGULOACTUAL", Str(angle))
'        addXdata(vehiculo, "POSICIONACTUAL_X", Str(position(0)))
'        addXdata(vehiculo, "POSICIONACTUAL_Y", Str(position(1)))
'        addXdata(vehiculo, "START", "FALSE")

'        'MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")
'    End If
'End Sub