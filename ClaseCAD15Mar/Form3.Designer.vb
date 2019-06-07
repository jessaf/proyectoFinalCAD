<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.dwgActual = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ConexionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProyectoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CarroSiguiendoLineaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SimuladorRedesAguaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SimuladorDeVientoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AyudaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AcercaDeTenochtitlanCityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.AgregarDatosAMapaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.dwgActual})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 508)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
        Me.StatusStrip1.ShowItemToolTips = True
        Me.StatusStrip1.Size = New System.Drawing.Size(730, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'dwgActual
        '
        Me.dwgActual.AutoToolTip = True
        Me.dwgActual.BackColor = System.Drawing.Color.Red
        Me.dwgActual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.dwgActual.Name = "dwgActual"
        Me.dwgActual.Size = New System.Drawing.Size(34, 17)
        Me.dwgActual.Text = "   ...   "
        Me.dwgActual.ToolTipText = "Esperando Conexión"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MenuStrip1.Font = New System.Drawing.Font("Century Schoolbook", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConexionToolStripMenuItem, Me.ProyectoToolStripMenuItem, Me.SimuladorRedesAguaToolStripMenuItem, Me.SimuladorDeVientoToolStripMenuItem, Me.AyudaToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(730, 27)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ConexionToolStripMenuItem
        '
        Me.ConexionToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ConexionToolStripMenuItem.Font = New System.Drawing.Font("Century Schoolbook", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ConexionToolStripMenuItem.Name = "ConexionToolStripMenuItem"
        Me.ConexionToolStripMenuItem.Size = New System.Drawing.Size(94, 23)
        Me.ConexionToolStripMenuItem.Text = "Conexión"
        '
        'ProyectoToolStripMenuItem
        '
        Me.ProyectoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CarroSiguiendoLineaToolStripMenuItem})
        Me.ProyectoToolStripMenuItem.Name = "ProyectoToolStripMenuItem"
        Me.ProyectoToolStripMenuItem.Size = New System.Drawing.Size(136, 23)
        Me.ProyectoToolStripMenuItem.Text = "Simulador Tráfico"
        '
        'CarroSiguiendoLineaToolStripMenuItem
        '
        Me.CarroSiguiendoLineaToolStripMenuItem.Name = "CarroSiguiendoLineaToolStripMenuItem"
        Me.CarroSiguiendoLineaToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.CarroSiguiendoLineaToolStripMenuItem.Text = "Carro siguiendo linea"
        '
        'SimuladorRedesAguaToolStripMenuItem
        '
        Me.SimuladorRedesAguaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AgregarDatosAMapaToolStripMenuItem})
        Me.SimuladorRedesAguaToolStripMenuItem.Name = "SimuladorRedesAguaToolStripMenuItem"
        Me.SimuladorRedesAguaToolStripMenuItem.Size = New System.Drawing.Size(168, 23)
        Me.SimuladorRedesAguaToolStripMenuItem.Text = "Simulador Redes Agua"
        '
        'SimuladorDeVientoToolStripMenuItem
        '
        Me.SimuladorDeVientoToolStripMenuItem.Name = "SimuladorDeVientoToolStripMenuItem"
        Me.SimuladorDeVientoToolStripMenuItem.Size = New System.Drawing.Size(152, 23)
        Me.SimuladorDeVientoToolStripMenuItem.Text = "Simulador de Viento"
        '
        'AyudaToolStripMenuItem
        '
        Me.AyudaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AcercaDeTenochtitlanCityToolStripMenuItem})
        Me.AyudaToolStripMenuItem.Name = "AyudaToolStripMenuItem"
        Me.AyudaToolStripMenuItem.Size = New System.Drawing.Size(63, 23)
        Me.AyudaToolStripMenuItem.Text = "Ayuda"
        '
        'AcercaDeTenochtitlanCityToolStripMenuItem
        '
        Me.AcercaDeTenochtitlanCityToolStripMenuItem.Name = "AcercaDeTenochtitlanCityToolStripMenuItem"
        Me.AcercaDeTenochtitlanCityToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.AcercaDeTenochtitlanCityToolStripMenuItem.Text = "Acerca de Tenochtitlan City"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 27)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackgroundImage = Global.ClaseCAD15Mar.My.Resources.Resources.image
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListBox1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Splitter1)
        Me.SplitContainer1.Size = New System.Drawing.Size(730, 481)
        Me.SplitContainer1.SplitterDistance = 171
        Me.SplitContainer1.SplitterWidth = 3
        Me.SplitContainer1.TabIndex = 4
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.BackColor = System.Drawing.Color.White
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(130, 0)
        Me.ListBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(601, 290)
        Me.ListBox1.TabIndex = 1
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(0, 0)
        Me.Splitter1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(129, 307)
        Me.Splitter1.TabIndex = 0
        Me.Splitter1.TabStop = False
        '
        'AgregarDatosAMapaToolStripMenuItem
        '
        Me.AgregarDatosAMapaToolStripMenuItem.Name = "AgregarDatosAMapaToolStripMenuItem"
        Me.AgregarDatosAMapaToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.AgregarDatosAMapaToolStripMenuItem.Text = "Agregar datos a Mapa"
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 530)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "Form3"
        Me.Text = "Tenochtitlan City"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ConexionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProyectoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CarroSiguiendoLineaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents dwgActual As ToolStripStatusLabel
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents SimuladorRedesAguaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SimuladorDeVientoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AyudaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AcercaDeTenochtitlanCityToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AgregarDatosAMapaToolStripMenuItem As ToolStripMenuItem
End Class
