<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ElementosCiudad
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CmbTipoElem = New System.Windows.Forms.ComboBox()
        Me.nudPisos = New System.Windows.Forms.NumericUpDown()
        Me.btnAsigna = New System.Windows.Forms.Button()
        Me.cbUsoSUelo = New System.Windows.Forms.ComboBox()
        Me.cbIPub = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnRecupera = New System.Windows.Forms.Button()
        CType(Me.nudPisos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CmbTipoElem
        '
        Me.CmbTipoElem.FormattingEnabled = True
        Me.CmbTipoElem.Location = New System.Drawing.Point(255, 12)
        Me.CmbTipoElem.Name = "CmbTipoElem"
        Me.CmbTipoElem.Size = New System.Drawing.Size(241, 21)
        Me.CmbTipoElem.TabIndex = 0
        '
        'nudPisos
        '
        Me.nudPisos.Location = New System.Drawing.Point(255, 101)
        Me.nudPisos.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudPisos.Name = "nudPisos"
        Me.nudPisos.Size = New System.Drawing.Size(37, 20)
        Me.nudPisos.TabIndex = 15
        '
        'btnAsigna
        '
        Me.btnAsigna.Location = New System.Drawing.Point(206, 208)
        Me.btnAsigna.Name = "btnAsigna"
        Me.btnAsigna.Size = New System.Drawing.Size(100, 30)
        Me.btnAsigna.TabIndex = 14
        Me.btnAsigna.Text = "Asignar Valores"
        Me.btnAsigna.UseVisualStyleBackColor = True
        '
        'cbUsoSUelo
        '
        Me.cbUsoSUelo.FormattingEnabled = True
        Me.cbUsoSUelo.Location = New System.Drawing.Point(255, 143)
        Me.cbUsoSUelo.Name = "cbUsoSUelo"
        Me.cbUsoSUelo.Size = New System.Drawing.Size(197, 21)
        Me.cbUsoSUelo.TabIndex = 13
        '
        'cbIPub
        '
        Me.cbIPub.FormattingEnabled = True
        Me.cbIPub.Location = New System.Drawing.Point(255, 57)
        Me.cbIPub.Name = "cbIPub"
        Me.cbIPub.Size = New System.Drawing.Size(197, 21)
        Me.cbIPub.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(93, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Uso de Suelo"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(93, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Pisos"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(93, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Infraestructura Publica"
        '
        'BtnRecupera
        '
        Me.BtnRecupera.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnRecupera.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.BtnRecupera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.BtnRecupera.Font = New System.Drawing.Font("Gabriola", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRecupera.Location = New System.Drawing.Point(358, 199)
        Me.BtnRecupera.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.BtnRecupera.Name = "BtnRecupera"
        Me.BtnRecupera.Size = New System.Drawing.Size(178, 43)
        Me.BtnRecupera.TabIndex = 16
        Me.BtnRecupera.Text = "Consulta datos"
        Me.BtnRecupera.UseVisualStyleBackColor = False
        '
        'ElementosCiudad
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(642, 255)
        Me.Controls.Add(Me.BtnRecupera)
        Me.Controls.Add(Me.nudPisos)
        Me.Controls.Add(Me.btnAsigna)
        Me.Controls.Add(Me.cbUsoSUelo)
        Me.Controls.Add(Me.cbIPub)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CmbTipoElem)
        Me.Name = "ElementosCiudad"
        Me.Text = "ElementosCiudad"
        CType(Me.nudPisos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CmbTipoElem As ComboBox
    Friend WithEvents nudPisos As NumericUpDown
    Friend WithEvents btnAsigna As Button
    Friend WithEvents cbUsoSUelo As ComboBox
    Friend WithEvents cbIPub As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnRecupera As Button
End Class
