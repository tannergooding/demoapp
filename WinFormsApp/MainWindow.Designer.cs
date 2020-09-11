namespace WinFormsApp
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._grid = new System.Windows.Forms.TableLayoutPanel();
            this._leftGrid = new System.Windows.Forms.TableLayoutPanel();
            this._sceneListBox = new System.Windows.Forms.ListBox();
            this._rotationGroupBox = new System.Windows.Forms.GroupBox();
            this._rotationGrid = new System.Windows.Forms.TableLayoutPanel();
            this._rotationXLabel = new System.Windows.Forms.Label();
            this._rotationXSlider = new System.Windows.Forms.TrackBar();
            this._rotationYLabel = new System.Windows.Forms.Label();
            this._rotationYSlider = new System.Windows.Forms.TrackBar();
            this._rotationZLabel = new System.Windows.Forms.Label();
            this._rotationZSlider = new System.Windows.Forms.TrackBar();
            this._lightPositionGroupBox = new System.Windows.Forms.GroupBox();
            this._lightPositionGrid = new System.Windows.Forms.TableLayoutPanel();
            this._lightPositionXLabel = new System.Windows.Forms.Label();
            this._lightPositionXSlider = new System.Windows.Forms.TrackBar();
            this._lightPositionYLabel = new System.Windows.Forms.Label();
            this._lightPositionYSlider = new System.Windows.Forms.TrackBar();
            this._lightPositionZLabel = new System.Windows.Forms.Label();
            this._lightPositionZSlider = new System.Windows.Forms.TrackBar();
            this._viewGroupBox = new System.Windows.Forms.GroupBox();
            this._viewGrid = new System.Windows.Forms.TableLayoutPanel();
            this._zoomLabel = new System.Windows.Forms.Label();
            this._zoomSlider = new System.Windows.Forms.TrackBar();
            this._optionsGroupBox = new System.Windows.Forms.GroupBox();
            this._optionsStackPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._displayDepthBufferCheckBox = new System.Windows.Forms.CheckBox();
            this._rotateModelCheckBox = new System.Windows.Forms.CheckBox();
            this._useHWIntrinsicsCheckBox = new System.Windows.Forms.CheckBox();
            this._wireframeCheckBox = new System.Windows.Forms.CheckBox();
            this._resetButton = new System.Windows.Forms.Button();
            this._displaySurface = new System.Windows.Forms.PictureBox();
            this._grid.SuspendLayout();
            this._leftGrid.SuspendLayout();
            this._rotationGroupBox.SuspendLayout();
            this._rotationGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rotationXSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._rotationYSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._rotationZSlider)).BeginInit();
            this._lightPositionGroupBox.SuspendLayout();
            this._lightPositionGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lightPositionXSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lightPositionYSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lightPositionZSlider)).BeginInit();
            this._viewGroupBox.SuspendLayout();
            this._viewGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._zoomSlider)).BeginInit();
            this._optionsGroupBox.SuspendLayout();
            this._optionsStackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._displaySurface)).BeginInit();
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.ColumnCount = 2;
            this._grid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._grid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this._grid.Controls.Add(this._leftGrid, 0, 0);
            this._grid.Controls.Add(this._displaySurface, 1, 0);
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.RowCount = 1;
            this._grid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._grid.Size = new System.Drawing.Size(784, 666);
            this._grid.TabIndex = 0;
            // 
            // _leftGrid
            // 
            this._leftGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._leftGrid.AutoSize = true;
            this._leftGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._leftGrid.ColumnCount = 1;
            this._leftGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._leftGrid.Controls.Add(this._sceneListBox, 0, 0);
            this._leftGrid.Controls.Add(this._rotationGroupBox, 0, 1);
            this._leftGrid.Controls.Add(this._lightPositionGroupBox, 0, 2);
            this._leftGrid.Controls.Add(this._viewGroupBox, 0, 3);
            this._leftGrid.Controls.Add(this._optionsGroupBox, 0, 4);
            this._leftGrid.Controls.Add(this._resetButton, 0, 5);
            this._leftGrid.Location = new System.Drawing.Point(3, 3);
            this._leftGrid.Name = "_leftGrid";
            this._leftGrid.RowCount = 6;
            this._leftGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._leftGrid.Size = new System.Drawing.Size(150, 655);
            this._leftGrid.TabIndex = 0;
            // 
            // _sceneListBox
            // 
            this._sceneListBox.FormattingEnabled = true;
            this._sceneListBox.ItemHeight = 15;
            this._sceneListBox.Location = new System.Drawing.Point(3, 3);
            this._sceneListBox.Name = "_sceneListBox";
            this._sceneListBox.Size = new System.Drawing.Size(144, 49);
            this._sceneListBox.TabIndex = 0;
            this._sceneListBox.SelectedIndexChanged += new System.EventHandler(this.SceneListBox_SelectionChanged);
            // 
            // _rotationGroupBox
            // 
            this._rotationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._rotationGroupBox.AutoSize = true;
            this._rotationGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rotationGroupBox.Controls.Add(this._rotationGrid);
            this._rotationGroupBox.Location = new System.Drawing.Point(3, 58);
            this._rotationGroupBox.Name = "_rotationGroupBox";
            this._rotationGroupBox.Size = new System.Drawing.Size(144, 175);
            this._rotationGroupBox.TabIndex = 1;
            this._rotationGroupBox.TabStop = false;
            this._rotationGroupBox.Text = "Rotation";
            // 
            // _rotationGrid
            // 
            this._rotationGrid.AutoSize = true;
            this._rotationGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rotationGrid.ColumnCount = 2;
            this._rotationGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._rotationGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._rotationGrid.Controls.Add(this._rotationXLabel, 0, 0);
            this._rotationGrid.Controls.Add(this._rotationXSlider, 1, 0);
            this._rotationGrid.Controls.Add(this._rotationYLabel, 0, 1);
            this._rotationGrid.Controls.Add(this._rotationYSlider, 1, 1);
            this._rotationGrid.Controls.Add(this._rotationZLabel, 0, 2);
            this._rotationGrid.Controls.Add(this._rotationZSlider, 1, 2);
            this._rotationGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationGrid.Location = new System.Drawing.Point(3, 19);
            this._rotationGrid.Name = "_rotationGrid";
            this._rotationGrid.RowCount = 3;
            this._rotationGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._rotationGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._rotationGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._rotationGrid.Size = new System.Drawing.Size(138, 153);
            this._rotationGrid.TabIndex = 0;
            // 
            // _rotationXLabel
            // 
            this._rotationXLabel.AutoSize = true;
            this._rotationXLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationXLabel.Location = new System.Drawing.Point(3, 0);
            this._rotationXLabel.Name = "_rotationXLabel";
            this._rotationXLabel.Size = new System.Drawing.Size(63, 50);
            this._rotationXLabel.TabIndex = 0;
            this._rotationXLabel.Text = "X (0):";
            this._rotationXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _rotationXSlider
            // 
            this._rotationXSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationXSlider.Location = new System.Drawing.Point(72, 3);
            this._rotationXSlider.Maximum = 25;
            this._rotationXSlider.Minimum = -25;
            this._rotationXSlider.Name = "_rotationXSlider";
            this._rotationXSlider.Size = new System.Drawing.Size(63, 44);
            this._rotationXSlider.TabIndex = 1;
            this._rotationXSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._rotationXSlider.Scroll += new System.EventHandler(this.OnRotationXChanged);
            // 
            // _rotationYLabel
            // 
            this._rotationYLabel.AutoSize = true;
            this._rotationYLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationYLabel.Location = new System.Drawing.Point(3, 50);
            this._rotationYLabel.Name = "_rotationYLabel";
            this._rotationYLabel.Size = new System.Drawing.Size(63, 50);
            this._rotationYLabel.TabIndex = 2;
            this._rotationYLabel.Text = "Y (0):";
            this._rotationYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _rotationYSlider
            // 
            this._rotationYSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationYSlider.Location = new System.Drawing.Point(72, 53);
            this._rotationYSlider.Maximum = 25;
            this._rotationYSlider.Minimum = -25;
            this._rotationYSlider.Name = "_rotationYSlider";
            this._rotationYSlider.Size = new System.Drawing.Size(63, 44);
            this._rotationYSlider.TabIndex = 3;
            this._rotationYSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._rotationYSlider.Scroll += new System.EventHandler(this.OnRotationYChanged);
            // 
            // _rotationZLabel
            // 
            this._rotationZLabel.AutoSize = true;
            this._rotationZLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationZLabel.Location = new System.Drawing.Point(3, 100);
            this._rotationZLabel.Name = "_rotationZLabel";
            this._rotationZLabel.Size = new System.Drawing.Size(63, 53);
            this._rotationZLabel.TabIndex = 4;
            this._rotationZLabel.Text = "Z (0):";
            this._rotationZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _rotationZSlider
            // 
            this._rotationZSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rotationZSlider.Location = new System.Drawing.Point(72, 103);
            this._rotationZSlider.Maximum = 25;
            this._rotationZSlider.Minimum = -25;
            this._rotationZSlider.Name = "_rotationZSlider";
            this._rotationZSlider.Size = new System.Drawing.Size(63, 47);
            this._rotationZSlider.TabIndex = 5;
            this._rotationZSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._rotationZSlider.Scroll += new System.EventHandler(this.OnRotationZChanged);
            // 
            // _lightPositionGroupBox
            // 
            this._lightPositionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._lightPositionGroupBox.AutoSize = true;
            this._lightPositionGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._lightPositionGroupBox.Controls.Add(this._lightPositionGrid);
            this._lightPositionGroupBox.Location = new System.Drawing.Point(3, 239);
            this._lightPositionGroupBox.Name = "_lightPositionGroupBox";
            this._lightPositionGroupBox.Size = new System.Drawing.Size(144, 175);
            this._lightPositionGroupBox.TabIndex = 2;
            this._lightPositionGroupBox.TabStop = false;
            this._lightPositionGroupBox.Text = "Light Position";
            // 
            // _lightPositionGrid
            // 
            this._lightPositionGrid.AutoSize = true;
            this._lightPositionGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._lightPositionGrid.ColumnCount = 2;
            this._lightPositionGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._lightPositionGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._lightPositionGrid.Controls.Add(this._lightPositionXLabel, 0, 0);
            this._lightPositionGrid.Controls.Add(this._lightPositionXSlider, 1, 0);
            this._lightPositionGrid.Controls.Add(this._lightPositionYLabel, 0, 1);
            this._lightPositionGrid.Controls.Add(this._lightPositionYSlider, 1, 1);
            this._lightPositionGrid.Controls.Add(this._lightPositionZLabel, 0, 2);
            this._lightPositionGrid.Controls.Add(this._lightPositionZSlider, 1, 2);
            this._lightPositionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionGrid.Location = new System.Drawing.Point(3, 19);
            this._lightPositionGrid.Name = "_lightPositionGrid";
            this._lightPositionGrid.RowCount = 3;
            this._lightPositionGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._lightPositionGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._lightPositionGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this._lightPositionGrid.Size = new System.Drawing.Size(138, 153);
            this._lightPositionGrid.TabIndex = 0;
            // 
            // _lightPositionXLabel
            // 
            this._lightPositionXLabel.AutoSize = true;
            this._lightPositionXLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionXLabel.Location = new System.Drawing.Point(3, 0);
            this._lightPositionXLabel.Name = "_lightPositionXLabel";
            this._lightPositionXLabel.Size = new System.Drawing.Size(63, 50);
            this._lightPositionXLabel.TabIndex = 0;
            this._lightPositionXLabel.Text = "X (0):";
            this._lightPositionXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lightPositionXSlider
            // 
            this._lightPositionXSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionXSlider.Location = new System.Drawing.Point(72, 3);
            this._lightPositionXSlider.Maximum = 1000;
            this._lightPositionXSlider.Minimum = -1000;
            this._lightPositionXSlider.Name = "_lightPositionXSlider";
            this._lightPositionXSlider.Size = new System.Drawing.Size(63, 44);
            this._lightPositionXSlider.TabIndex = 1;
            this._lightPositionXSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._lightPositionXSlider.Scroll += new System.EventHandler(this.OnLightPositionXChanged);
            // 
            // _lightPositionYLabel
            // 
            this._lightPositionYLabel.AutoSize = true;
            this._lightPositionYLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionYLabel.Location = new System.Drawing.Point(3, 50);
            this._lightPositionYLabel.Name = "_lightPositionYLabel";
            this._lightPositionYLabel.Size = new System.Drawing.Size(63, 50);
            this._lightPositionYLabel.TabIndex = 2;
            this._lightPositionYLabel.Text = "Y (0):";
            this._lightPositionYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lightPositionYSlider
            // 
            this._lightPositionYSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionYSlider.Location = new System.Drawing.Point(72, 53);
            this._lightPositionYSlider.Maximum = 1000;
            this._lightPositionYSlider.Minimum = -1000;
            this._lightPositionYSlider.Name = "_lightPositionYSlider";
            this._lightPositionYSlider.Size = new System.Drawing.Size(63, 44);
            this._lightPositionYSlider.TabIndex = 3;
            this._lightPositionYSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._lightPositionYSlider.Scroll += new System.EventHandler(this.OnLightPositionYChanged);
            // 
            // _lightPositionZLabel
            // 
            this._lightPositionZLabel.AutoSize = true;
            this._lightPositionZLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionZLabel.Location = new System.Drawing.Point(3, 100);
            this._lightPositionZLabel.Name = "_lightPositionZLabel";
            this._lightPositionZLabel.Size = new System.Drawing.Size(63, 53);
            this._lightPositionZLabel.TabIndex = 4;
            this._lightPositionZLabel.Text = "Z (0):";
            this._lightPositionZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lightPositionZSlider
            // 
            this._lightPositionZSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lightPositionZSlider.Location = new System.Drawing.Point(72, 103);
            this._lightPositionZSlider.Maximum = 1000;
            this._lightPositionZSlider.Minimum = -1000;
            this._lightPositionZSlider.Name = "_lightPositionZSlider";
            this._lightPositionZSlider.Size = new System.Drawing.Size(63, 47);
            this._lightPositionZSlider.TabIndex = 5;
            this._lightPositionZSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._lightPositionZSlider.Scroll += new System.EventHandler(this.OnLightPositionZChanged);
            // 
            // _viewGroupBox
            // 
            this._viewGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._viewGroupBox.AutoSize = true;
            this._viewGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._viewGroupBox.Controls.Add(this._viewGrid);
            this._viewGroupBox.Location = new System.Drawing.Point(3, 420);
            this._viewGroupBox.Name = "_viewGroupBox";
            this._viewGroupBox.Size = new System.Drawing.Size(144, 73);
            this._viewGroupBox.TabIndex = 3;
            this._viewGroupBox.TabStop = false;
            this._viewGroupBox.Text = "View";
            // 
            // _viewGrid
            // 
            this._viewGrid.AutoSize = true;
            this._viewGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._viewGrid.ColumnCount = 2;
            this._viewGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._viewGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._viewGrid.Controls.Add(this._zoomLabel, 0, 0);
            this._viewGrid.Controls.Add(this._zoomSlider, 1, 0);
            this._viewGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewGrid.Location = new System.Drawing.Point(3, 19);
            this._viewGrid.Name = "_viewGrid";
            this._viewGrid.RowCount = 1;
            this._viewGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._viewGrid.Size = new System.Drawing.Size(138, 51);
            this._viewGrid.TabIndex = 0;
            // 
            // _zoomLabel
            // 
            this._zoomLabel.AutoSize = true;
            this._zoomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._zoomLabel.Location = new System.Drawing.Point(3, 0);
            this._zoomLabel.Name = "_zoomLabel";
            this._zoomLabel.Size = new System.Drawing.Size(63, 51);
            this._zoomLabel.TabIndex = 0;
            this._zoomLabel.Text = "Zoom (0):";
            this._zoomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _zoomSlider
            // 
            this._zoomSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this._zoomSlider.Location = new System.Drawing.Point(72, 3);
            this._zoomSlider.Maximum = 1000;
            this._zoomSlider.Name = "_zoomSlider";
            this._zoomSlider.Size = new System.Drawing.Size(63, 45);
            this._zoomSlider.TabIndex = 1;
            this._zoomSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this._zoomSlider.Scroll += new System.EventHandler(this.OnZoomChanged);
            // 
            // _optionsGroupBox
            // 
            this._optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._optionsGroupBox.AutoSize = true;
            this._optionsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._optionsGroupBox.Controls.Add(this._optionsStackPanel);
            this._optionsGroupBox.Location = new System.Drawing.Point(3, 499);
            this._optionsGroupBox.Name = "_optionsGroupBox";
            this._optionsGroupBox.Size = new System.Drawing.Size(144, 122);
            this._optionsGroupBox.TabIndex = 4;
            this._optionsGroupBox.TabStop = false;
            this._optionsGroupBox.Text = "Options";
            // 
            // _optionsStackPanel
            // 
            this._optionsStackPanel.AutoSize = true;
            this._optionsStackPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._optionsStackPanel.Controls.Add(this._displayDepthBufferCheckBox);
            this._optionsStackPanel.Controls.Add(this._rotateModelCheckBox);
            this._optionsStackPanel.Controls.Add(this._useHWIntrinsicsCheckBox);
            this._optionsStackPanel.Controls.Add(this._wireframeCheckBox);
            this._optionsStackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsStackPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._optionsStackPanel.Location = new System.Drawing.Point(3, 19);
            this._optionsStackPanel.Name = "_optionsStackPanel";
            this._optionsStackPanel.Size = new System.Drawing.Size(138, 100);
            this._optionsStackPanel.TabIndex = 0;
            this._optionsStackPanel.WrapContents = false;
            // 
            // _displayDepthBufferCheckBox
            // 
            this._displayDepthBufferCheckBox.AutoSize = true;
            this._displayDepthBufferCheckBox.Location = new System.Drawing.Point(3, 3);
            this._displayDepthBufferCheckBox.Name = "_displayDepthBufferCheckBox";
            this._displayDepthBufferCheckBox.Size = new System.Drawing.Size(134, 19);
            this._displayDepthBufferCheckBox.TabIndex = 0;
            this._displayDepthBufferCheckBox.Text = "Display Depth Buffer";
            this._displayDepthBufferCheckBox.UseVisualStyleBackColor = true;
            this._displayDepthBufferCheckBox.CheckedChanged += new System.EventHandler(this.OnDisplayDepthBufferCheckedChanged);
            // 
            // _rotateModelCheckBox
            // 
            this._rotateModelCheckBox.AutoSize = true;
            this._rotateModelCheckBox.Checked = true;
            this._rotateModelCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._rotateModelCheckBox.Location = new System.Drawing.Point(3, 28);
            this._rotateModelCheckBox.Name = "_rotateModelCheckBox";
            this._rotateModelCheckBox.Size = new System.Drawing.Size(97, 19);
            this._rotateModelCheckBox.TabIndex = 1;
            this._rotateModelCheckBox.Text = "Rotate Model";
            this._rotateModelCheckBox.UseVisualStyleBackColor = true;
            this._rotateModelCheckBox.CheckedChanged += new System.EventHandler(this.OnRotateModelCheckedChanged);
            // 
            // _useHWIntrinsicsCheckBox
            // 
            this._useHWIntrinsicsCheckBox.AutoSize = true;
            this._useHWIntrinsicsCheckBox.Location = new System.Drawing.Point(3, 53);
            this._useHWIntrinsicsCheckBox.Name = "_useHWIntrinsicsCheckBox";
            this._useHWIntrinsicsCheckBox.Size = new System.Drawing.Size(115, 19);
            this._useHWIntrinsicsCheckBox.TabIndex = 2;
            this._useHWIntrinsicsCheckBox.Text = "Use HWIntrinsics";
            this._useHWIntrinsicsCheckBox.UseVisualStyleBackColor = true;
            this._useHWIntrinsicsCheckBox.CheckedChanged += new System.EventHandler(this.OnUseHWIntrinsicsCheckedChanged);
            // 
            // _wireframeCheckBox
            // 
            this._wireframeCheckBox.AutoSize = true;
            this._wireframeCheckBox.Checked = true;
            this._wireframeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._wireframeCheckBox.Location = new System.Drawing.Point(3, 78);
            this._wireframeCheckBox.Name = "_wireframeCheckBox";
            this._wireframeCheckBox.Size = new System.Drawing.Size(81, 19);
            this._wireframeCheckBox.TabIndex = 3;
            this._wireframeCheckBox.Text = "Wireframe";
            this._wireframeCheckBox.UseVisualStyleBackColor = true;
            this._wireframeCheckBox.CheckedChanged += new System.EventHandler(this.OnWireframeCheckedChanged);
            // 
            // _resetButton
            // 
            this._resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._resetButton.AutoSize = true;
            this._resetButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._resetButton.Location = new System.Drawing.Point(3, 627);
            this._resetButton.Name = "_resetButton";
            this._resetButton.Size = new System.Drawing.Size(144, 25);
            this._resetButton.TabIndex = 5;
            this._resetButton.Text = "Reset";
            this._resetButton.UseVisualStyleBackColor = true;
            this._resetButton.Click += new System.EventHandler(this.OnResetClicked);
            // 
            // _displaySurface
            // 
            this._displaySurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this._displaySurface.Location = new System.Drawing.Point(159, 3);
            this._displaySurface.Name = "_displaySurface";
            this._displaySurface.Size = new System.Drawing.Size(622, 660);
            this._displaySurface.TabIndex = 1;
            this._displaySurface.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 666);
            this.Controls.Add(this._grid);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this._grid.ResumeLayout(false);
            this._grid.PerformLayout();
            this._leftGrid.ResumeLayout(false);
            this._leftGrid.PerformLayout();
            this._rotationGroupBox.ResumeLayout(false);
            this._rotationGroupBox.PerformLayout();
            this._rotationGrid.ResumeLayout(false);
            this._rotationGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rotationXSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._rotationYSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._rotationZSlider)).EndInit();
            this._lightPositionGroupBox.ResumeLayout(false);
            this._lightPositionGroupBox.PerformLayout();
            this._lightPositionGrid.ResumeLayout(false);
            this._lightPositionGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lightPositionXSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lightPositionYSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lightPositionZSlider)).EndInit();
            this._viewGroupBox.ResumeLayout(false);
            this._viewGroupBox.PerformLayout();
            this._viewGrid.ResumeLayout(false);
            this._viewGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._zoomSlider)).EndInit();
            this._optionsGroupBox.ResumeLayout(false);
            this._optionsGroupBox.PerformLayout();
            this._optionsStackPanel.ResumeLayout(false);
            this._optionsStackPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._displaySurface)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _grid;
        private System.Windows.Forms.TableLayoutPanel _leftGrid;
        private System.Windows.Forms.ListBox _sceneListBox;
        private System.Windows.Forms.GroupBox _rotationGroupBox;
        private System.Windows.Forms.TableLayoutPanel _rotationGrid;
        private System.Windows.Forms.Label _rotationXLabel;
        private System.Windows.Forms.TrackBar _rotationXSlider;
        private System.Windows.Forms.Label _rotationYLabel;
        private System.Windows.Forms.TrackBar _rotationYSlider;
        private System.Windows.Forms.Label _rotationZLabel;
        private System.Windows.Forms.TrackBar _rotationZSlider;
        private System.Windows.Forms.GroupBox _lightPositionGroupBox;
        private System.Windows.Forms.TableLayoutPanel _lightPositionGrid;
        private System.Windows.Forms.Label _lightPositionXLabel;
        private System.Windows.Forms.TrackBar _lightPositionXSlider;
        private System.Windows.Forms.Label _lightPositionYLabel;
        private System.Windows.Forms.TrackBar _lightPositionYSlider;
        private System.Windows.Forms.Label _lightPositionZLabel;
        private System.Windows.Forms.TrackBar _lightPositionZSlider;
        private System.Windows.Forms.GroupBox _viewGroupBox;
        private System.Windows.Forms.TableLayoutPanel _viewGrid;
        private System.Windows.Forms.Label _zoomLabel;
        private System.Windows.Forms.TrackBar _zoomSlider;
        private System.Windows.Forms.GroupBox _optionsGroupBox;
        private System.Windows.Forms.FlowLayoutPanel _optionsStackPanel;
        private System.Windows.Forms.CheckBox _displayDepthBufferCheckBox;
        private System.Windows.Forms.CheckBox _rotateModelCheckBox;
        private System.Windows.Forms.CheckBox _useHWIntrinsicsCheckBox;
        private System.Windows.Forms.CheckBox _wireframeCheckBox;
        private System.Windows.Forms.Button _resetButton;
        private System.Windows.Forms.PictureBox _displaySurface;
    }
}

