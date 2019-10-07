using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace quickDBExplorer
{
	/// <summary>
	/// ServerSelectDialog の概要の説明です。
	/// </summary>
	[System.Runtime.InteropServices.ComVisible(false)]
	public class ServerSelectDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox serverListBox;
		private System.Windows.Forms.Button btnOk;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnCancel;
		private	ConditionRecorder	ServerList;

		/// <summary>
		/// 選択されたサーバー名
		/// </summary>
		private	string pSelectedServer;
        private Button btnDelete;
		/// <summary>
		/// 選択されたサーバー名
		/// </summary>
		public	string SelectedServer
		{
			get { return this.pSelectedServer; }
			set { this.pSelectedServer = value; }
		}
		/// <summary>
		/// 選択されたインスタンス名
		/// </summary>
		private  string pSelectedInstance;
		/// <summary>
		/// 選択されたインスタンス名
		/// </summary>
		public  string SelectedInstance
		{
			get { return this.pSelectedInstance; }
			set { this.pSelectedInstance = value; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="cl">過去の接続サーバー履歴情報</param>
		public ServerSelectDialog(ConditionRecorder cl)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			ServerList = cl;
			pSelectedServer = "";
			pSelectedInstance = "";
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerSelectDialog));
            this.serverListBox = new System.Windows.Forms.ListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverListBox
            // 
            this.serverListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serverListBox.ItemHeight = 12;
            this.serverListBox.Location = new System.Drawing.Point(8, 8);
            this.serverListBox.Name = "serverListBox";
            this.serverListBox.Size = new System.Drawing.Size(444, 244);
            this.serverListBox.TabIndex = 0;
            this.serverListBox.DoubleClick += new System.EventHandler(this.serverListBox_DoubleClick);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(8, 264);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 24);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "サーバー選択(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(332, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "キャンセル(&X)";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(163, 264);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "履歴削除(&D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ServerSelectDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(456, 293);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.serverListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerSelectDialog";
            this.ShowInTaskbar = false;
            this.Text = "サーバー選択";
            this.Load += new System.EventHandler(this.ServerSelectDialog_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if( this.serverListBox.SelectedItem != null )
			{
				string delimStr = ":";
				string []str = this.serverListBox.SelectedItem.ToString().Split(delimStr.ToCharArray(), 2);
				this.pSelectedServer = str[0];
				this.pSelectedInstance = str[1];
			}
			else
			{
				this.pSelectedServer = "";
				this.pSelectedInstance = "";
			}
		}

		private void ServerSelectDialog_Load(object sender, System.EventArgs e)
		{

            DispServerList();
			this.serverListBox.Refresh();
		}

        private void DispServerList()
        {
            this.serverListBox.SuspendLayout();
            try
            {
                this.serverListBox.Items.Clear();

                foreach (object sd in ServerList.PerServerData.Values)
                {
                    ServerData svd = (ServerData)sd;
                    this.serverListBox.Items.Add(svd.Servername + ":" + svd.InstanceName);
                }
                this.serverListBox.Sorted = true;
            }
            finally
            {
                this.serverListBox.ResumeLayout();
            }
        }

        private void serverListBox_DoubleClick(object sender, System.EventArgs e)
		{
			this.btnOk.PerformClick();
		}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if( this.serverListBox.SelectedItems.Count != 1 ){
                MessageBox.Show("削除対象のサーバーを選択して下さい");
                return;
            }

            if (MessageBox.Show("選択したサーバーの情報を削除してもよろしいですか？", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            DeleteSelectServer();
            DispServerList();
            this.serverListBox.Refresh();

        }

        private void DeleteSelectServer()
        {
            string delimStr = ":";
            string[] str = this.serverListBox.SelectedItem.ToString().Split(delimStr.ToCharArray(), 2);

            ServerData deltarget = null;
            foreach (object sd in ServerList.PerServerData.Values)
            {
                deltarget = (ServerData)sd;
                if (deltarget.Servername == str[0] &&
                    (
                        (string.IsNullOrEmpty(deltarget.InstanceName) && string.IsNullOrEmpty(str[1])) ||
                        deltarget.InstanceName == str[1]
                    )
                    )
                {
                    break;
                }
            }
            ServerList.PerServerData.Remove(deltarget.KeyName);
        }
	}
}
