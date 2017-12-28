<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtInputDirectory = New System.Windows.Forms.TextBox()
        Me.txtCounter = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnClearText = New System.Windows.Forms.Button()
        Me.txtStartStatus = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 11)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Port:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 300)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "ACK Text:"
        '
        'txtPort
        '
        Me.txtPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPort.Location = New System.Drawing.Point(63, 7)
        Me.txtPort.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.ReadOnly = True
        Me.txtPort.Size = New System.Drawing.Size(76, 26)
        Me.txtPort.TabIndex = 5
        Me.txtPort.Text = "2204"
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(16, 79)
        Me.txtLog.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(719, 309)
        Me.txtLog.TabIndex = 7
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(20, 43)
        Me.btnStart.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(100, 28)
        Me.btnStart.TabIndex = 8
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Enabled = False
        Me.btnStop.Location = New System.Drawing.Point(128, 43)
        Me.btnStop.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(100, 28)
        Me.btnStop.TabIndex = 9
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(261, 15)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 17)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Input Directory: "
        '
        'txtInputDirectory
        '
        Me.txtInputDirectory.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInputDirectory.Location = New System.Drawing.Point(373, 9)
        Me.txtInputDirectory.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtInputDirectory.Name = "txtInputDirectory"
        Me.txtInputDirectory.ReadOnly = True
        Me.txtInputDirectory.Size = New System.Drawing.Size(347, 26)
        Me.txtInputDirectory.TabIndex = 11
        '
        'txtCounter
        '
        Me.txtCounter.Location = New System.Drawing.Point(561, 46)
        Me.txtCounter.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtCounter.Name = "txtCounter"
        Me.txtCounter.Size = New System.Drawing.Size(151, 22)
        Me.txtCounter.TabIndex = 13
        Me.txtCounter.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(501, 49)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 17)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Count"
        '
        'btnClearText
        '
        Me.btnClearText.Location = New System.Drawing.Point(373, 46)
        Me.btnClearText.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClearText.Name = "btnClearText"
        Me.btnClearText.Size = New System.Drawing.Size(100, 28)
        Me.btnClearText.TabIndex = 15
        Me.btnClearText.Text = "Clear"
        Me.btnClearText.UseVisualStyleBackColor = True
        '
        'txtStartStatus
        '
        Me.txtStartStatus.Location = New System.Drawing.Point(16, 399)
        Me.txtStartStatus.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtStartStatus.Name = "txtStartStatus"
        Me.txtStartStatus.Size = New System.Drawing.Size(244, 22)
        Me.txtStartStatus.TabIndex = 16
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(760, 438)
        Me.Controls.Add(Me.txtStartStatus)
        Me.Controls.Add(Me.btnClearText)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtCounter)
        Me.Controls.Add(Me.txtInputDirectory)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "ULH Receiver"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtInputDirectory As System.Windows.Forms.TextBox
    Friend WithEvents txtCounter As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnClearText As System.Windows.Forms.Button
    Friend WithEvents txtStartStatus As System.Windows.Forms.TextBox

End Class
