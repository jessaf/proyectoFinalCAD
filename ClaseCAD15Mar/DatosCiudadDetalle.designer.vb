<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DatosCiudadDetalle
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.lbltipo = New System.Windows.Forms.Label()
        Me.lblinfpub = New System.Windows.Forms.Label()
        Me.lblpisos = New System.Windows.Forms.Label()
        Me.lbluso = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(37, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tipo"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(37, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Infraestructura Publica"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(37, 119)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Pisos"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(37, 162)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Uso de Suelo"
        '
        'btnCerrar
        '
        Me.btnCerrar.Location = New System.Drawing.Point(172, 226)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(100, 30)
        Me.btnCerrar.TabIndex = 7
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'lbltipo
        '
        Me.lbltipo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbltipo.Location = New System.Drawing.Point(197, 31)
        Me.lbltipo.Name = "lbltipo"
        Me.lbltipo.Size = New System.Drawing.Size(216, 20)
        Me.lbltipo.TabIndex = 8
        Me.lbltipo.Text = "TIPO"
        Me.lbltipo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblinfpub
        '
        Me.lblinfpub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblinfpub.Location = New System.Drawing.Point(197, 69)
        Me.lblinfpub.Name = "lblinfpub"
        Me.lblinfpub.Size = New System.Drawing.Size(216, 20)
        Me.lblinfpub.TabIndex = 9
        Me.lblinfpub.Text = "INF PUB"
        Me.lblinfpub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblpisos
        '
        Me.lblpisos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblpisos.Location = New System.Drawing.Point(197, 112)
        Me.lblpisos.Name = "lblpisos"
        Me.lblpisos.Size = New System.Drawing.Size(216, 20)
        Me.lblpisos.TabIndex = 10
        Me.lblpisos.Text = "PISOS"
        Me.lblpisos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbluso
        '
        Me.lbluso.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbluso.Location = New System.Drawing.Point(197, 155)
        Me.lbluso.Name = "lbluso"
        Me.lbluso.Size = New System.Drawing.Size(216, 20)
        Me.lbluso.TabIndex = 11
        Me.lbluso.Text = "USO SUELO"
        Me.lbluso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DatosCiudadDetalle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(444, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.lbluso)
        Me.Controls.Add(Me.lblpisos)
        Me.Controls.Add(Me.lblinfpub)
        Me.Controls.Add(Me.lbltipo)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DatosCiudadDetalle"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Información Objeto"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnCerrar As Button
    Friend WithEvents lbltipo As Label
    Friend WithEvents lblinfpub As Label
    Friend WithEvents lblpisos As Label
    Friend WithEvents lbluso As Label
End Class
