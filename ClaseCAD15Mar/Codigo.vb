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
        'End [

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
    'PENDIENTE 5 : HAY UN PROBLEMA EN LA FUNCION QUE DETERMINA SI HAY INTERSECCION ENTRE EL CONTORNO Y LAS CALLES (DENTRO DE LA FUNCION QUE CREA LOS VEHICULOS) AHI EN LOS COMENTARIOS HAY MAS INFORMACION ACERCA DE ESTE PROBLEMA
    'PENDIENTE 4 : HAY UN PROBLEMA EN LA FUNCION DE GIRO, EN ALGUNOS CASOS LOS VEHICULOS NO GIRAN COMO DEBERIAN, EN LA MAYORIA LO HACEN BIEN, PERO NO EN TODOS LOS CASOS (ESTO DEJENLO AL FINAL POR FAVOR, CUANDO TODO LO DEMAS YA ESTE JALANDO")




    Public Function seguimientoCalle()

        'Para este ejemplo se tienen que crear las Layers en el Banner/Layers/LayerProperties y crear una que se llame LINEAS, CIRCULOS, OTROS
        'revisando todos los elementos del plano
        Dim conjunto As AcadSelectionSet = Nothing
        Dim conjuntoCalles As AcadSelectionSet = Nothing
        Dim conjuntoTemp As AcadSelectionSet = Nothing
        Dim conjuntoVehiculos As AcadSelectionSet = Nothing
        Dim conjuntoCallesEnBorde As AcadSelectionSet = Nothing
        Dim conjuntoSemaforos As AcadSelectionSet = Nothing
        Dim element As AcadEntity
        Dim vehiculo As AcadEntity
        Dim lines() As AcadEntity
        Dim calle As AcadEntity
        Dim temp(0) As AcadEntity
        Dim temp2(0) As AcadEntity
        Dim temp3(0) As AcadCircle
        Dim contorno As AcadEntity = Nothing
        Dim nextStreet As AcadEntity
        Dim lista As ListBox
        Dim FilterType(0) As Integer
        Dim FilterData(0) As Object
        Dim numMov As Integer = 200
        Dim currentPosition(0 To 2) As Double
        Dim nextPosition(0 To 2) As Double
        Dim prueba(2) As Object
        Dim str As String = Nothing
        Dim I As Integer, j As Integer, k As Integer
        Dim numCalles As Integer = Nothing
        Dim numCarros As Integer
        Dim strStart As String
        Dim valor As String
        Dim pasoactual As Integer
        Dim numsteps As Integer
        Dim nowtime As Integer
        Dim randomnumber As Integer
        Dim numeropasos As Integer
        '
        lista = Form3.ListBox1
        lista.Items.Clear()

        '===================================================================================
        'PASO 1 Encontrando calles, contornos y demas 
        '===================================================================================
        'Creamos los conjuntos con los cuales trabajaremos
        conjunto = conjunto_vacio(DOCUMENTO, "Plano")
        conjuntoCalles = conjunto_vacio(DOCUMENTO, "Calles")
        conjuntoTemp = conjunto_vacio(DOCUMENTO, "Temp")
        conjuntoVehiculos = conjunto_vacio(DOCUMENTO, "Vehiculos")
        conjuntoCallesEnBorde = conjunto_vacio(DOCUMENTO, "Borde")
        conjuntoSemaforos = conjunto_vacio(DOCUMENTO, "Semaforos")
        conjunto.Select(AcSelect.acSelectionSetAll)


        'Clasificamos en conjuntos los elementos encontrados en el plano
        For Each element In conjunto
            'Obtenemos el contorno
            If element.ObjectName = "AcDbPolyline" Then
                contorno = element
                MsgBox("Se encontro el contorno")
            End If
            'Obtenemos las calles
            If element.ObjectName = "AcDbLine" Then
                temp2(0) = element
                conjuntoCalles.AddItems(temp2)
                'MsgBox("Se ha agregado elemento a calles")
            End If
            'Obtenemos los semaforos
            If element.ObjectName = "AcDbCircle" Then
                temp3(0) = element
                conjuntoSemaforos.AddItems(temp3)
            End If
        Next

        '===================================================================================
        'PASO 2 Crear los vehiculos en las calles
        '===================================================================================
        'PRIMERO PROCEDEMOS A CREAR LOS VEHICULOS EN AQUELLAS INTERSECCIONES ENTRE LAS CALLES Y EL CONTORNO
        numCarros = CreandoVehiculos(conjuntoCalles, conjuntoVehiculos, conjuntoCallesEnBorde, contorno, lista)

        '=========================================================================================================
        ''Esta seccion es la que se tiene que tener para que los vehicuos se muevan al mismo tiempo y no uno despues del otro 
        'para realizar esto se consulta el diccionario y verifque en que paso esta el vehiculo
        'luego de un paso (un solo movimiento) y mmodifique los valores en el diccionario para acutalizar la informacion acerca del paso actual
        'De esta forma en el ciclo For Each aqui abajo, ira iterando por cada carro y por cada carro dara un paso y luego pasara al siguiente carro
        'con esto se lograra dar la sensacion de que todos avanzan simultaneamente 
        '=========================================================================================================

        'por los siglos de los siglos amen 
        'Do While 1
        addDictionaryToTrafficLights(conjuntoCalles, conjuntoSemaforos)

        For temporal = 1 To 300




            lista.Items.Add("========================= iteracion no " & temporal & "=========================") '
            nowtime = changeTraficLights(CStr(Now()), conjuntoSemaforos, lista)

            For Each vehiculo In conjuntoVehiculos




                'Obtenemos informacion sobre si el carro ya ha avanzado o es su primera vez
                strStart = GetDictionaryVehiculo(vehiculo, "START", valor)
                'Si el paso actual no es el inicial entonces estamos en un punto intermedio de la calle 
                If (strStart = "TRUE") Then
                    numeropasos = GetDictionaryVehiculo(vehiculo, "NUMSTEPS", valor)
                    'Recuperamos la informacion sobre la calle actual del vehiculo para seguirnos moviendo sobre esa misma.
                    nextStreet = getStreetByHandle(GetDictionaryVehiculo(vehiculo, "STREET", valor), conjuntoCalles)
                    'Ejectuamos el avance de un paso para el vehiculo sobre la calle antes recuperada.
                    moverVehiculo(nextStreet, vehiculo, numeropasos, lista)
                End If

                'Si la bandera strStart es false entonces estamos en el punto inicial de la calle o en un punto de interseccion donde varias calles hay por delante
                'En cuyo caso tenemos que obtener la siguiente calle, girar acorde a ella, y realizar el primer movimiento que nos sacara del inicio del camino 
                If strStart = "FALSE" Then
                    'Obtenemos la siguente calle para avenzar de entre las calles disponibles para este proposito
                    nextStreet = getNextStreet(getNextPosibleStreets(vehiculo, conjuntoCalles, conjuntoCallesEnBorde, conjuntoVehiculos, numCalles, lista), numCalles)
                    'Actualizamos la informacion de la calle en el diccionario que de aqui en adelante se pueda consultar e ir avanzando sobre esa misma calle 
                    UpdateDictionaryToVehiculoTextField(vehiculo, "STREET", nextStreet.Handle)
                    If Not IsNothing(nextStreet) Then

                        'La velocidad será aleatoria entre 40 y 20 
                        Randomize()
                        randomnumber = Int((40 - 20 + 1) * Rnd() + 20)
                        'Giramos segun la orientacion de la nueva calle
                        GirarVehiculoSegunOrientacionDeLaCalle(nextStreet, vehiculo, lista)
                        'pend definir la velocidad del vehiculo
                        UpdateDictionaryToVehiculoTextField(vehiculo, "START", "TRUE")
                        'Se tiene que guardar el numero de pasos para cada vehiculo
                        UpdateDictionaryToVehiculoTextField(vehiculo, "NUMSTEPS", CStr(randomnumber))
                    Else
                        MsgBox("Saltamos ese vehiculo por no encontrar calle siguiente")
                    End If
                End If

            Next
            '''


        Next
        'Loop

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
        Dim randomnumber As Integer

        'Dim prueba As Acad3DSolid
        'Dim simbolo As AcadBlockReference
        'con esto se tiene la propiedad de rotacion.

        Dim tempcontadorcallesencontorno As Integer

        contadorvehiculos = 0
        tempcontadorcallesencontorno = 0

        For Each calle In conjuntoCalles

            p1 = calle.startpoint
            p2 = calle.endpoint

            contadorCalles = contadorCalles + 1
            arrcontorno = Nothing
            'Pregunto si hay interseccion entre la calle y el contorno y si la hay guardo el punto de interseccion en el arreglo arrcontorno
            arrcontorno = contorno.IntersectWith(calle, AcExtendOption.acExtendNone)

            'Si el tamanio del arreglo es mayor a cero entonces tiene las coordenadas guardadas por lo que en efecto hubo interseccion
            'esto se puede intepretar como "Si la calle intersecta con el contorno entonces"
            'MsgBox("Se han revisado las calles que intesectan con el contorno ")
            If (arrcontorno.Count > 0) Then
                'La siguiente condicion es vital para que los vehiculos desaparezcan de un borde y aparezcan en otro, si no se tiene la siguiente 
                'condicion apareceran carros en medio del plano, esto no es deseable.

                'Si la calle esta intersectando al contorno en su punto inicial entonces aparecer automovil, else nope
                If Math.Round(p1(0), 3) = Math.Round(arrcontorno(0), 3) And Math.Round(p1(1), 3) = Math.Round(arrcontorno(1), 3) Then
                    tempcallesenborde(0) = calle
                    conjuntoCallesEnBorde.AddItems(tempcallesenborde)
                    tempcontadorcallesencontorno = tempcontadorcallesencontorno + 1

                    'CREAMOS EL VEHICULO EN EL PUNTO INICIAL DE LA CALLE 
                    vehiculo = DOCUMENTO.ModelSpace.AddBox(p1, 2, 1, 1)

                    'Le agregamos su respectivo diccionario3
                    AddDictionaryToVehiculo(vehiculo)
                    temp(0) = vehiculo
                    conjuntoVehiculos.AddItems(temp)
                    contadorvehiculos = contadorvehiculos + 1
                End If
            Else
                'este else se ejecuta cuando la calle en cuestion que se esta analizando no intersecta con el borde, de forma tal 
                'Que se puede decir que es una calle intermedia en el plano. 
                Randomize()
                randomnumber = CInt(Int((2 * Rnd()) + 0))
                If randomnumber > 0 Then
                    If Not isthereavehicle(calle, conjuntoVehiculos) Then
                        vehiculo = DOCUMENTO.ModelSpace.AddBox(p1, 2, 1, 2)
                        'Le agregamos su respectivo diccionario
                        AddDictionaryToVehiculo(vehiculo)
                        temp(0) = vehiculo
                        conjuntoVehiculos.AddItems(temp)
                        contadorvehiculos = contadorvehiculos + 1
                    End If
                End If

            End If



        Next
        MsgBox("el numero total de calles en el contorno es = " & tempcontadorcallesencontorno)
        'lista.Items.Add("el numero total de calles en el contorno es = " & tempcontadorcallesencontorno) '
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
        Dim linea As AcadLine
        Dim angledegrees As Double



        If Not IsNothing(nextStreet) Then
            'Calcularemos la pendiente de la siguiente calle
            p1 = nextStreet.startpoint
            p2 = nextStreet.endpoint
        Else
            MsgBox("No se encontro nextstreet en funcion de giro")
        End If

        linea = nextStreet
        pendiente = (p2(1) - p1(1)) / (p2(0) - p1(0))
        angle = linea.Angle
        angledegrees = convertRadtoAngd(angle)

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
        Dim pasoactual As String = Nothing

        Dim numsteps As Integer
        Dim startstreet(0 To 2) As Double
        Dim endstreet(0 To 2) As Double
        Dim valor As String = Nothing
        Dim prueba As Integer

        pasoactual = CInt(GetDictionaryVehiculo(vehiculo, "PASOACTUAL", valor))

        'If shouldCarMove(vehiculo) Then

        'Else
        '    MsgBox("Saltamos este paso para esperar que la vialidad permita el movimiento.")
        'End If

        If shouldCarMove(vehiculo, calle) Then
            'La primera vez que vamos a mover el vehiculo determinamos los parametros de movimiento. Punto inicial, punto final, numero de pasos a avanzar y paso actual.
            If pasoactual = 0 Then

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

                'la siguiente posicion consistira  en la posicion actual mas un incremento
                nextPosition(0) = currentPosition(0) + incX
                nextPosition(1) = currentPosition(1) + incY

                'scanCarNearObjects(currentPosition)
                vehiculo.Move(currentPosition, nextPosition)

                currentPosition(0) = nextPosition(0)
                currentPosition(1) = nextPosition(1)

                vehiculo.Update()

                'UpdateDictionaryToVehiculoTextField(vehiculo, "NUMSTEPS", Str(numMov))
                UpdateDictionaryToVehiculoTextField(vehiculo, "ORIGEN_X", Str(p1(0)))
                UpdateDictionaryToVehiculoTextField(vehiculo, "ORIGEN_Y", Str(p1(1)))
                UpdateDictionaryToVehiculoTextField(vehiculo, "DESTINO_X", Str(p2(0)))
                UpdateDictionaryToVehiculoTextField(vehiculo, "DESTINO_Y", Str(p2(1)))
                UpdateDictionaryToVehiculoTextField(vehiculo, "INCX", Str(incX))
                UpdateDictionaryToVehiculoTextField(vehiculo, "INCY", Str(incY))

                UpdateDictionaryToVehiculoTextField(vehiculo, "PASOACTUAL", Str(1))
                UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_X", Str(p1(0)))
                UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_Y", Str(p1(1)))

                'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & currentPosition(0) & " // " & currentPosition(1)) '
                'WasteTime(1)
                'PauseEvent(1)
            Else

                'Obtenemos los valores guardados en el diccionario
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

                If (pasoactual < numsteps) Then
                    'lista.Items.Add("incx " & incX & "//" & " incy " & incY) '
                    'la siguiente posicion consistira  en la posicion actual mas un incremento
                    nextPosition(0) = currentPosition(0) + incX
                    nextPosition(1) = currentPosition(1) + incY
                    'lista.Items.Add("paso actual = " & pasoactual & " VEHICULO AVANZARA DE " & currentPosition(0) & "//" & currentPosition(1) & " To " & nextPosition(0) & "//" & nextPosition(1)) '
                    vehiculo.Move(currentPosition, nextPosition)

                    currentPosition(0) = nextPosition(0)
                    currentPosition(1) = nextPosition(1)
                    pasoactual = pasoactual + 1
                End If
                If pasoactual = numsteps Then
                    'vehiculo.Move(currentPosition, endstreet)
                    'scanCarNearObjects(currentPosition)
                    vehiculo.Move(vehiculo.centroid, endstreet)
                    UpdateDictionaryToVehiculoTextField(vehiculo, "START", "FALSE")
                    pasoactual = 0
                    'lista.Items.Add("El LLEGO AL FIN DE LA CALLE") '
                    MsgBox("El LLEGO AL FIN DE LA CALLE, pasoactual = " & pasoactual)
                End If

                UpdateDictionaryToVehiculoTextField(vehiculo, "PASOACTUAL", Str(pasoactual))
                UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_X", Str(currentPosition(0)))
                UpdateDictionaryToVehiculoTextField(vehiculo, "POSICIONACTUAL_Y", Str(currentPosition(1)))

                'MsgBox("dado que start es true El vehiculo avanza sobre la calle")
                vehiculo.Update()

                'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & currentPosition(0) & " // " & currentPosition(1)) '
                'WasteTime(1)
                'PauseEvent(1)


            End If

        Else
            MsgBox("El carro no se mueve")
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


        'MsgBox("getnextstreet: RANDOM = " & randomnumber & "," & "tamofcollection = " & callesdeinterseccion.Count)
        If callesdeinterseccion.Count <> 0 Then
            Return callesdeinterseccion.Item(randomnumber)
            'MsgBox("Getnextstreet: se procede a ir a la calle numero " & randomnumber & ", de " & numCalles & " posibles")
        Else
            MsgBox("Aqui muere")
        End If

    End Function

    Public Function getNextPosibleStreets(vehiculo As AcadEntity, conjuntoCalles As AcadSelectionSet, conjuntoCallesEnBorde As AcadSelectionSet, conjuntoVehiculos As AcadSelectionSet, numCalles As Integer, lista As ListBox) As Collection
        'Esta funcion busca cuales son las calles que colindan con la interseccion en la que se encuentra nuestro vehiculo de forma tal que posteriormente se pueda decidir a cual de ellas ir 
        Dim calle As AcadEntity
        Dim p1calle() As Double = Nothing
        Dim p2calle() As Double = Nothing
        Dim centroide() As Double = Nothing
        Dim callesenlainterseccion As Collection = Nothing
        callesenlainterseccion = New Collection

        centroide = vehiculo.centroid
        numCalles = 0
        lista.Items.Add("-------------------------------------------------------------------------------------")
        lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & Math.Round(centroide(0), 3) & " // " & Math.Round(centroide(1), 3))
        'Obtenemos las calles que estan colindantes a la calle en la cual venia nuestro vehiculo
        For Each calle In conjuntoCalles
            p1calle = calle.startpoint
            p2calle = calle.endpoint
            'lista.Items.Add("handeler= " & vehiculo.Handle & " lastpos= " & centroide(0) & " // " & centroide(1))
            lista.Items.Add("calle= " & calle.Handle & " p1 = " & Math.Round(p1calle(0), 3) & " // " & Math.Round(p1calle(1), 3) & " p2 = " & Math.Round(p2calle(0), 3) & " // " & Math.Round(p2calle(1), 3))

            If Math.Round(centroide(0), 3) = Math.Round(p1calle(0), 3) And Math.Round(centroide(1), 3) = Math.Round(p1calle(1), 3) Then
                'If centroide(0) = p1calle(0) Or centroide(1) = p1calle(1) Then

                lista.Items.Add("@@@@@@@@@@@@@@@@@@@@Se ha encontrado nueva calle colindante con la actual para el vehiculo@@@@@@@@@@@@@@@@@@@@")
                ' MsgBox("Se ha encontrado nueva calle colindante con la actual para el vehiculo")
                callesenlainterseccion.Add(calle)
                numCalles = numCalles + 1
                ' MsgBox("Se encontraron posibles calles para avanzar #calles = " & callesenlainterseccion.Count)
            End If


        Next

        '''''QQQQQQQQQQQ
        'Cuando no existen calles que colinden con la calle actual significa que llegamos al borde por lo cual se tendra que reapacer el vehiculo
        If callesenlainterseccion.Count = 0 Then
            callesenlainterseccion.Clear()
            MsgBox("Se llego a una condicion de fin de calle, se procede a reaparecer el vehiculo")
            callesenlainterseccion.Add(reaparecerVehiculo(vehiculo, conjuntoCallesEnBorde, conjuntoVehiculos))
        End If

        Return callesenlainterseccion
    End Function

    Public Function reaparecerVehiculo(vehiculo As AcadEntity, conjuntoCallesEnBorde As AcadSelectionSet, conjuntoVehiculos As AcadSelectionSet) As AcadEntity

        'Cuando el vehiculo llega al borde de la calle ya no tiene mas calles para seguir avanzando, asi que se selecciona una calle, asi que debemos desaparecerlo de ahi y reaparecerlo en el comienzo de otra calle
        'de las que colindan con el borde aleatoriamente para desaparecer al vehiculo de donde esta (el punto sin avance) y aparecerlo
        'en la nueva calle, esta calle sera la siguiente calle en la cual circulara el vehiculo

        Dim numerodecalles As Integer
        Dim calle As AcadEntity = Nothing
        Dim randomnumber As Integer
        Dim p1() As Double = Nothing
        Dim contadordeescape As Integer
        Dim banderareaparecer As Boolean = True


        numerodecalles = conjuntoCallesEnBorde.Count
        'MsgBox("El numero de calles en el borde es = " & numerodecalles)

        If numerodecalles <> 0 Then
            'Hasta donde tengo entendido la formula de abajo me devuelve numeros de 1 a numerodecalles
            'por alguna razon si el random da un numero igual al numero de calles (es decir selecciona la ultima calle del conjunto)
            'me produce un error (outofbounding???) por lo que en ese caso es necesario volver a solicitar el numero aleatorio para impedir
            'una exceptcion en ese sentido. Sera que el ultimo elemento del conjunto se referencia en la posicion n-1???
            contadordeescape = 0
            While IsNothing(calle)
                Randomize()
                '[Mejorar] esto tiene que ir de 1 a 4 y por alguna razon va de 1 a 3
                randomnumber = Int(numerodecalles * Rnd() + 1)
                calle = conjuntoCallesEnBorde(randomnumber)

                If Not IsNothing(calle) Then
                    'El siguiente if nos sirve para evitar que dos vehiculos aparezcan en el mismo punto despues de haber llegado al fin del plano. 

                    If isthereavehicle(calle, conjuntoVehiculos, randomnumber) Then
                        calle = Nothing
                        'MsgBox("Se omite la calle " & randomnumber & " ,esta calle por que ahi ya hay un vehiculo")


                        contadordeescape = contadordeescape + 1
                    Else
                        MsgBox("La calle a la que se ira sera la siguiente = " & randomnumber)
                    End If
                End If
                'Defino una secuencia de escape por que en ocaciones el numero de carros que llego a un punto final del mapa es superior que el numero
                'De calles disponibles para que reaparezcan todos. por ejemplo se tienen 4 carros en un punto final del mapa y solo 3 calles en las cuales pueden
                'reaparecer, por lo cual 3 carros reaparecen pero como esos lugares ya estan ocupados el carro 4 se queda en el limbo por que se hace aqui un bucle
                'infinito buscando una calle disponible. En ese caso prefiero perder el carro y seguir con los que quedan. 
                If contadordeescape = 10 Then
                    banderareaparecer = False
                    Exit While
                End If
            End While

            If banderareaparecer = True Then
                p1 = calle.startpoint
                vehiculo.Move(vehiculo.centroid, p1)
                MsgBox("Se pudo reaparecer en la siguiente calle " & randomnumber)
                Return calle
            End If


        End If

    End Function
    Public Function isthereavehicle(calle As AcadEntity, conjuntoVehiculos As AcadSelectionSet, numberofcalle As Integer) As Boolean

        Dim contador As Integer = 0
        Dim startpoint() As Double
        Dim centroide() As Double

        For Each vehiculo In conjuntoVehiculos
            startpoint = calle.startpoint
            centroide = vehiculo.centroid
            'MsgBox("Calle= " & numberofcalle & vbCr & "Centroide(x) = " & centroide(0) & "   Centroide(Y) = " & centroide(1) & vbCr & "starpoint(x) = " & startpoint(0) & "   starpoint(y) = " & startpoint(1))
            'Math.Round(, 3)
            If Math.Round(startpoint(0), 3).Equals(Math.Round(centroide(0), 3)) And Math.Round(startpoint(1), 3).Equals(Math.Round(centroide(1), 3)) Then
                contador = contador + 1
                'MsgBox("Si hay un vehiculo en esa calle " & numberofcalle)
            End If
        Next

        If contador > 0 Then
            Return True
        Else
            Return False
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



    Public Function isthereavehicle(calle As AcadEntity, conjuntoVehiculos As AcadSelectionSet) As Boolean

        Dim contador As Integer = 0
        Dim startpoint() As Double
        Dim centroide() As Double



        For Each vehiculo In conjuntoVehiculos
            startpoint = calle.startpoint
            centroide = vehiculo.centroid
            'MsgBox("Centroide(x) = " & centroide(0) & "   Centroide(Y) = " & centroide(1) & "   starpoint(x) = " & startpoint(0) & "   starpoint(y) = " & startpoint(1))
            If startpoint(0).Equals(centroide(0)) Or startpoint(1).Equals(centroide(1)) Then
                contador = contador + 1
                'MsgBox("Si hay un vehiculo en esa calle ")
            End If
        Next

        If contador > 0 Then
            Return True
        Else
            Return False
        End If


    End Function


    Public Sub scanCarNearObjects(p() As Double)
        'dada una coordenada naliza que objetos se encuentras dentro de un rectangulo
        'el analisis se realiza en un delta definido

        Dim conjunto As AcadSelectionSet
        Dim delta As Double = 3
        Dim esquinas(11) As Double
        Dim lista As ListBox = Form3.ListBox1
        Dim perimetro As AcadPolyline = Nothing



        If Not IsNothing(p) Then
            'cada 3 indices representa una coordenada xyz
            esquinas(0) = p(0) + 0 : esquinas(1) = p(1) + delta : esquinas(2) = p(2)    'coord 1
            esquinas(3) = p(0) - delta : esquinas(4) = p(1) + delta : esquinas(5) = p(2)    'coord2
            esquinas(6) = p(0) - 0 : esquinas(7) = p(1) - delta : esquinas(8) = p(2)    'coord3
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
                    MsgBox("Intersecta con los siguientes objetos" & element.handle & " " & element.ObjectName)
                End If
            Next
            conjunto.Delete()
        End If

    End Sub

    Public Function changeTraficLights(nowtime As String, conjuntoSemaforos As AcadSelectionSet, lista As ListBox)

        Dim segundos As Integer
        Dim valor As String
        Dim numerocalles As Integer
        Dim handle As String
        Dim changer As Boolean
        Dim tempstring As String


        segundos = 2 'Int(Left(Right(nowtime, 5), 2))

        'Iteramos sobre el conjunto de semaforos
        For Each semaforo In conjuntoSemaforos

            tempstring = GetDictionarySemaforo(semaforo, "CALLESSEMAFORO", valor)

            'Este if origianalmente no iba pero se puso sangron el programa y lo tuve que poner jaja
            If tempstring <> "VACIO" Then
                numerocalles = CInt(tempstring)
                ''Obtenemos el numero de calles (endpoints) que colindan con el semaforo
                'numerocalles = CInt(GetDictionarySemaforo(semaforo, "CALLESSEMAFORO", valor))
                'Se iterara cada 30 segundos por lo cual hay dos ifs, uno para 0 a 30 y otro para 30 a 60
                'en cada caso se itera sobre las calles que colindan con el semaforo, de forma tal que a una le asigna un valor y a otra el valor contrario
                lista.Items.Add("segundo = " & segundos & "Semaforo= " & semaforo.handle & "  # calles en el semaforo = " & numerocalles) '
                If (segundos >= 0 And segundos <= 10) Or (segundos > 20 And segundos <= 30) Or (segundos > 40 And segundos <= 50) Then
                    For i = 1 To numerocalles
                        'Se toma el handle que correponde al numero de calle y con el se especifica si para ese handle (calle) el semaforo esta en rojo o en verde. 
                        handle = GetDictionarySemaforo(semaforo, CStr(i), valor)
                        If changer = True Then
                            changer = False
                            UpdateDictionaryToSemaforoTextField(semaforo, handle, "ROJO")
                        Else
                            changer = True
                            UpdateDictionaryToSemaforoTextField(semaforo, handle, "VERDE")
                        End If

                        lista.Items.Add("handler = " & handle & "Semaoforo = " & GetDictionarySemaforo(semaforo, handle, valor)) '

                    Next

                End If

                If (segundos > 10 And segundos <= 20) Or (segundos > 30 And segundos <= 40) Or (segundos > 50 And segundos <= 60) Then
                    For i = 1 To numerocalles
                        handle = GetDictionarySemaforo(semaforo, CStr(i), valor)
                        If changer = True Then
                            changer = False
                            UpdateDictionaryToSemaforoTextField(semaforo, handle, "VERDE")
                        Else
                            changer = True
                            UpdateDictionaryToSemaforoTextField(semaforo, handle, "ROJO")
                        End If
                        lista.Items.Add("handler = " & handle & "Semaoforo = " & GetDictionarySemaforo(semaforo, handle, valor)) '
                    Next

                End If

            End If


        Next

    End Function

    Public Function addDictionaryToTrafficLights(conjuntoCalles As AcadSelectionSet, conjuntoSemaforos As AcadSelectionSet)

        Dim contadorcalles As Integer = 0
        Dim puntofinal() As Double
        Dim contadorsemaforos As Integer = 0

        For Each semaforo In conjuntoSemaforos

            addXdata(semaforo, "CALLESSEMAFORO", "VACIO")
            contadorcalles = 0
            For Each calle In conjuntoCalles
                If Not IsNothing(calle) Then
                    puntofinal = calle.endpoint
                    If semaforo.center(0) = puntofinal(0) And semaforo.center(1) = puntofinal(1) Then
                        contadorcalles = contadorcalles + 1
                        contadorsemaforos = contadorsemaforos + 1
                        addXdata(semaforo, CStr(contadorcalles), CStr(calle.handle))
                        addXdata(semaforo, CStr(calle.handle), "STOPPED")
                        addXdata(semaforo, "CALLESSEMAFORO", Str(contadorcalles))
                    End If
                End If

            Next
        Next
        MsgBox("Se econtraron " & contadorsemaforos & " Semaforos")
    End Function

    Public Function GetDictionarySemaforo(semaforo As AcadEntity, llave As String, ByRef valor As String)
        'ESTA FUNCION SIMILAR A LA ANTERIOR PERO EN LUGAR DE MODIFICAR EL DATO DEL DICCIONARIO LO CONSULTA, SE SUELEN USAR JUNTAS AMBAS FUCIONESS
        Dim valorretorno As String = Nothing
        If Not IsNothing(semaforo) Then
            getXdata(semaforo, llave, valor)
            valorretorno = valor
        End If
        Return valorretorno
    End Function
    Public Sub UpdateDictionaryToSemaforoTextField(semaforo As AcadEntity, campo As String, valor As String)
        'ESTA FUNCION ES EXTREMADAMENTE UTIL POR QUE NOS PERMITE ACTUALIZAR EL VALOR DE UN DATO DEL DICCIONARIO, ESTA ACTUALIZACION SE USA POR EJEMPLO EN LA FUNCION DE GIRO AL ACTUALIZAR EL ANGULO DEL VEHICULO
        'ESTA FUNCION SERA NECESARIA PARA ACTUALIZAR LOS DATOS DURANTE CADA PASO DEL MOVIMIENTO. eSTOS PASOS NECESITAN SERA ACTUALIZADOS PARA QUE LA FUNCION DE MOVIMIENT SEPA EN QUE PASO ESTAMOS Y POR LO TANDO DAR EL SIGUIENTE
        If Not IsNothing(semaforo) Then
            'esto checa si ya tiene un diccionario y si no lo tiene se agrega, cada vez que se de una llave default se le debe dar un dato default
            addXdata(semaforo, campo, valor)
        End If
    End Sub

    Public Function shouldCarMove(vehiculo As AcadEntity, calle As AcadEntity) As Boolean

        Dim conjuntoEscaneo As AcadSelectionSet = Nothing
        Dim valor As String
        Dim numerocalles As Integer
        Dim handle As String
        Dim luz As String
        Dim tempstring As String

        conjuntoEscaneo = AnalizandoEntornoCircular(vehiculo)

        For Each element In conjuntoEscaneo
            'En este caso testea al semaforo para saber cual es la luz y saber si va a avanzar o no 
            If element.ObjectName = "AcDbCircle" Then
                tempstring = GetDictionarySemaforo(element, "CALLESSEMAFORO", valor)

                If tempstring <> "VACIO" Then
                    numerocalles = CInt(tempstring)
                    For i = 0 To numerocalles
                        handle = GetDictionarySemaforo(element, CStr(i), valor)
                        'Debido a que en el semaforo hay dos calles se cuestiona si la luz a testar es la misma que la de la calle en la que viaja el carro
                        If handle = calle.Handle Then
                            'Si de casualidad aparece en el cruce, entonces que se mueva para no estorbar
                            If vehiculo.centroid(0) = calle.startpoint(0) And vehiculo.centroid(1) = calle.startpoint(1) Then
                                Return True

                            Else
                                luz = GetDictionarySemaforo(element, handle, valor)
                                If luz = "ROJO" Then
                                    Return False
                                Else
                                    Return True
                                End If
                            End If

                        End If
                    Next
                Else
                    Return False
                End If


            ElseIf element.ObjectName = "AcDb3dSolid" And element.Handle <> vehiculo.Handle Then
                'En este caso encuentra un vehiculo al frente por lo cual frena para no chocar 
                Return False
            End If

        Next
        Return True

    End Function

    Public Function generaCoordenadasCirculos(p() As Double, radio As Double, angInicial As Double, angFinal As Double, avances As Integer) As Double()
        'grados estan Angulos esta en grados
        'Debe regresar un arreglo lineal donde cada 3 elementos son una coordenada

        Dim angulo As Double
        Dim anguloDeAvance As Double
        Dim pCirculo() As Double
        Dim pPolar() As Double
        Dim pN As Integer

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
        Return (anguloGrados * Math.PI / 180.0)
    End Function
    Public Function convertRadtoAngd(anguloRad As Double) As Double
        Return (anguloRad * (180.0 / Math.PI))
    End Function

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



    Public Function AnalizandoEntornoCircular(vehiculo As AcadEntity) As AcadSelectionSet
        '###  Funcion que analiza dentro de un entorno circular los objetos que se encuentran al rededor del vehículo
        'Esta funcion se manda a llamar cada vez que se actualiza la posicion del vehiculo

        Dim conjunto As AcadSelectionSet
        Dim esquinas(11) As Double
        Dim lista As ListBox = Form3.ListBox1 'para mostrar en un list box los objetos que fueron encontrados dentro del área analizada
        Dim perimetro As AcadPolyline = Nothing
        Dim radio As Double
        Dim angle As Double
        Dim p(0 To 2) As Double
        Dim x, y As Double
        Dim valor As String
        Dim centroide() As Double
        Dim angulo As Double
        Dim objetoaremover(0 To 1) As AcadEntity


        radio = 1.0
        centroide = vehiculo.centroid

        angle = GetDictionaryVehiculo(vehiculo, "ANGULOACTUAL", valor)
        angulo = CDbl(angle)

        x = Math.Cos(CDbl(angle))
        y = Math.Sin(CDbl(angle))

        p(0) = centroide(0) + 2 * x
        p(1) = centroide(1) + 2 * y

        If Not IsNothing(p) Then

            'Calculamos la posisicon de las esquinas en base a los parametros dados
            esquinas = generaCoordenadasCirculos(p, radio, 0, 360, 20)
            'Se traza el poligono 
            perimetro = drawPolygon(esquinas) 'trazando el poligono de busqueda

            conjunto = conjunto_vacio(DOCUMENTO, "ELEMENTOS")
            conjunto.SelectByPolygon(AcSelect.acSelectionSetCrossingPolygon, esquinas)

            lista.Items.Clear()
            If Not IsNothing(conjunto) Then

                For Each element In conjunto
                    'objetoaremover(0) = Nothing
                    'no reportamos el perimetro generado
                    If element.Handle = perimetro.Handle Then
                        objetoaremover(0) = element
                        'conjunto.RemoveItems(objetoaremover)

                    End If

                    'If element.ObjectName = "AcDb3dSolid" Then ' se exluye el auto 
                    '    objetoaremover(1) = element
                    '    'conjunto.RemoveItems(objetoaremover)
                    'End If

                    If element.ObjectName = "AcDbLine" Then
                        objetoaremover(1) = element
                        'conjunto.RemoveItems(objetoaremover)
                    End If

                Next
                If Not IsNothing(objetoaremover) Then
                    conjunto.RemoveItems(objetoaremover)

                End If
                'conjunto.Delete()
                perimetro.Delete()
                Return conjunto
            End If

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
'            MsgBox("Se encontro el contorno")

'        End If
'        'Obtenemos las calles
'        If element.ObjectName = "AcDbLine" Then
'            temp2(0) = element
'            conjuntoCalles.AddItems(temp2)
'            MsgBox("Se ha agregado elemento a calles")
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
'================================================================================================================================================


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

'        MsgBox("Se ha agregado un diccionario con valores VACIOS a la entidad")
'    End If
'End Sub

'================================================================================================================================================
'Public Function AnalizandoEntornoRectangular(vehiculo As AcadEntity) As Integer
'    'dada una coordenada naliza que objetos se encuentras dentro de un rectangulo
'    'el analisis se realiza en un delta definido

'    Dim conjunto As AcadSelectionSet
'    Dim delta As Double = 5
'    Dim esquinas(11) As Double
'    Dim lista As ListBox = Form3.ListBox1
'    Dim valor As String
'    Dim x, y, z, w, a, b, u, v As Double
'    Dim angle, beta As Double


'    Dim perimetro As AcadPolyline = Nothing
'    Dim cuantos As Integer

'    angle = GetDictionaryVehiculo(vehiculo, "ANGULOACTUAL", valor)

'    If Not IsNothing(vehiculo.centroid) Then

'        beta = 90 - CDbl(angle)
'        x = Math.Cos(CDbl(angle))
'        y = Math.Sin(CDbl(angle))
'        v = (0.5) * Math.Cos(beta)
'        u = (0.5) * Math.Sin(beta)
'        z = (0.5) * Math.Sin(beta)
'        w = (0.5) * Math.Cos(beta)


'        '0=1-2
'        '3=4=5
'        '6=7=8
'        '9=10=11

'        'cada 3 indices representa una coordenada xyz   
'        esquinas(0) = vehiculo.centroid(0) + x - (4 * w) : esquinas(1) = vehiculo.centroid(1) + y - z : esquinas(2) = 0                   'coord 1
'        esquinas(3) = vehiculo.centroid(0) + x + (4 * v) : esquinas(4) = vehiculo.centroid(1) + y + u : esquinas(5) = 0                   'coord 2
'        esquinas(9) = esquinas(0) + (1) * Math.Cos(CDbl(angle)) : esquinas(10) = esquinas(1) + (1) * Math.Sin(CDbl(angle)) : esquinas(11) = 0     'coord 3
'        esquinas(6) = esquinas(9) + (4) * Math.Cos(beta) : esquinas(7) = esquinas(10) + (1) * Math.Sin(beta) : esquinas(8) = 0     'coord 4


'        ''cada 3 indices representa una coordenada xyz
'        'esquinas(0) = p(0) + delta : esquinas(1) = p(1) + delta : esquinas(2) = p(2)    'coord 1
'        'esquinas(3) = p(0) - delta : esquinas(4) = p(1) + delta : esquinas(5) = p(2)    'coord2
'        'esquinas(6) = p(0) - delta : esquinas(7) = p(1) - delta : esquinas(8) = p(2)    'coord3
'        'esquinas(9) = p(0) + delta : esquinas(10) = p(1) - delta : esquinas(11) = p(2)  'coord4


'        'trazando el poligono de busqueda
'        perimetro = drawPolygon(esquinas)

'        conjunto = conjunto_vacio(DOCUMENTO, "deteccion")
'        conjunto.SelectByPolygon(AcSelect.acSelectionSetCrossingPolygon, esquinas)

'        lista.Items.Clear()

'        For Each element In conjunto
'            'no reportamos el perimetro generado
'            If element.handle = perimetro.Handle Then
'                lista.Items.Add(element.handle & " " & element.ObjectName)
'                MsgBox("Se econtro obstaculo para el vehiculo " & vehiculo.Handle)
'                'conjunto.RemoveItems(element)
'            End If
'        Next
'        cuantos = conjunto.Count
'        conjunto.Delete()
'    End If
'    Return cuantos
'End Function