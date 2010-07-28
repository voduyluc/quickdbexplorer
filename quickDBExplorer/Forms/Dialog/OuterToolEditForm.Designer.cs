﻿namespace quickDBExplorer.Forms.Dialog
{
    partial class OuterToolEditForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.EditPanelContainer = new System.Windows.Forms.SplitContainer();
            this.ListPanel = new System.Windows.Forms.Panel();
            this.toolList = new quickDBExplorer.qdbeListView();
            this.NameColumn = new System.Windows.Forms.ColumnHeader();
            this.commandColumn = new System.Windows.Forms.ColumnHeader();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.btnInsertMacro = new System.Windows.Forms.Button();
            this.macroList = new quickDBExplorer.qdbeListView();
            this.macroName = new System.Windows.Forms.ColumnHeader();
            this.macroValue = new System.Windows.Forms.ColumnHeader();
            this.txtName = new quickDBExplorer.quickDBExplorerTextBox();
            this.txtCommand = new quickDBExplorer.quickDBExplorerTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EditPanelContainer.Panel1.SuspendLayout();
            this.EditPanelContainer.Panel2.SuspendLayout();
            this.EditPanelContainer.SuspendLayout();
            this.ListPanel.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MsgArea
            // 
            this.MsgArea.Location = new System.Drawing.Point(160, 478);
            this.MsgArea.Size = new System.Drawing.Size(491, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(685, 478);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "閉じる(&X)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(12, 477);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "決定(&O)";
            // 
            // EditPanelContainer
            // 
            this.EditPanelContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.EditPanelContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EditPanelContainer.Location = new System.Drawing.Point(12, 12);
            this.EditPanelContainer.Name = "EditPanelContainer";
            // 
            // EditPanelContainer.Panel1
            // 
            this.EditPanelContainer.Panel1.Controls.Add(this.ListPanel);
            // 
            // EditPanelContainer.Panel2
            // 
            this.EditPanelContainer.Panel2.Controls.Add(this.infoPanel);
            this.EditPanelContainer.Size = new System.Drawing.Size(748, 459);
            this.EditPanelContainer.SplitterDistance = 347;
            this.EditPanelContainer.TabIndex = 0;
            // 
            // ListPanel
            // 
            this.ListPanel.Controls.Add(this.toolList);
            this.ListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListPanel.Location = new System.Drawing.Point(0, 0);
            this.ListPanel.Name = "ListPanel";
            this.ListPanel.Size = new System.Drawing.Size(343, 455);
            this.ListPanel.TabIndex = 0;
            // 
            // toolList
            // 
            this.toolList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.commandColumn});
            this.toolList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolList.FullRowSelect = true;
            this.toolList.IsAutoColumSort = true;
            this.toolList.Location = new System.Drawing.Point(0, 0);
            this.toolList.Name = "toolList";
            this.toolList.Size = new System.Drawing.Size(343, 455);
            this.toolList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.toolList.TabIndex = 0;
            this.toolList.UseCompatibleStateImageBehavior = false;
            this.toolList.View = System.Windows.Forms.View.Details;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "名称";
            this.NameColumn.Width = 135;
            // 
            // commandColumn
            // 
            this.commandColumn.Text = "コマンド";
            this.commandColumn.Width = 178;
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.btnInsertMacro);
            this.infoPanel.Controls.Add(this.macroList);
            this.infoPanel.Controls.Add(this.txtName);
            this.infoPanel.Controls.Add(this.txtCommand);
            this.infoPanel.Controls.Add(this.label3);
            this.infoPanel.Controls.Add(this.label1);
            this.infoPanel.Controls.Add(this.label2);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoPanel.Location = new System.Drawing.Point(0, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(393, 455);
            this.infoPanel.TabIndex = 0;
            // 
            // btnInsertMacro
            // 
            this.btnInsertMacro.Location = new System.Drawing.Point(65, 172);
            this.btnInsertMacro.Name = "btnInsertMacro";
            this.btnInsertMacro.Size = new System.Drawing.Size(92, 23);
            this.btnInsertMacro.TabIndex = 4;
            this.btnInsertMacro.Text = "マクロを挿入(&I)";
            this.btnInsertMacro.UseVisualStyleBackColor = true;
            // 
            // macroList
            // 
            this.macroList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.macroName,
            this.macroValue});
            this.macroList.IsAutoColumSort = true;
            this.macroList.Location = new System.Drawing.Point(65, 201);
            this.macroList.Name = "macroList";
            this.macroList.Size = new System.Drawing.Size(312, 242);
            this.macroList.TabIndex = 6;
            this.macroList.UseCompatibleStateImageBehavior = false;
            this.macroList.View = System.Windows.Forms.View.Details;
            // 
            // macroName
            // 
            this.macroName.Text = "マクロ名";
            this.macroName.Width = 99;
            // 
            // macroValue
            // 
            this.macroValue.Text = "値（サンプル）";
            this.macroValue.Width = 166;
            // 
            // txtName
            // 
            this.txtName.CanCtrlDelete = true;
            this.txtName.IsDigitOnly = false;
            this.txtName.IsShowZoom = false;
            this.txtName.Location = new System.Drawing.Point(65, 13);
            this.txtName.Name = "txtName";
            this.txtName.PdHistory = null;
            this.txtName.Size = new System.Drawing.Size(312, 19);
            this.txtName.TabIndex = 1;
            // 
            // txtCommand
            // 
            this.txtCommand.CanCtrlDelete = true;
            this.txtCommand.IsDigitOnly = false;
            this.txtCommand.IsShowZoom = false;
            this.txtCommand.Location = new System.Drawing.Point(65, 40);
            this.txtCommand.Multiline = true;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.PdHistory = null;
            this.txtCommand.Size = new System.Drawing.Size(312, 121);
            this.txtCommand.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "マクロ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "コマンド";
            // 
            // OuterToolEditForm
            // 
            this.ClientSize = new System.Drawing.Size(772, 507);
            this.Controls.Add(this.EditPanelContainer);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "OuterToolEditForm";
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.MsgArea, 0);
            this.Controls.SetChildIndex(this.EditPanelContainer, 0);
            this.EditPanelContainer.Panel1.ResumeLayout(false);
            this.EditPanelContainer.Panel2.ResumeLayout(false);
            this.EditPanelContainer.ResumeLayout(false);
            this.ListPanel.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer EditPanelContainer;
        private System.Windows.Forms.Panel ListPanel;
        private qdbeListView toolList;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader commandColumn;
        private quickDBExplorerTextBox txtCommand;
        private qdbeListView macroList;
        private System.Windows.Forms.ColumnHeader macroName;
        private System.Windows.Forms.ColumnHeader macroValue;
        private quickDBExplorerTextBox txtName;
        private System.Windows.Forms.Button btnInsertMacro;
    }
}