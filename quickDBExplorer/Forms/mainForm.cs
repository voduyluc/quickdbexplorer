using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using quickDBExplorer.Forms;
using quickDBExplorer.Forms.Events;

namespace quickDBExplorer
{
	/// <summary>
	/// メインとなる画面
	/// DBの選択、オーナーの選択、オブジェクトの選択、処理の選択などのメインとなる処理を全て実装している
	/// </summary>
	[System.Runtime.InteropServices.ComVisible(false)]
	internal class MainForm : quickDBExplorerBaseForm
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnCSV;
		private System.Windows.Forms.Button btnDDL;
		private System.Windows.Forms.Button btnDataEdit;
		private System.Windows.Forms.Button btnDataUpdate;
		private System.Windows.Forms.Button btnFieldList;
		private System.Windows.Forms.Button btnGridFormat;
		private System.Windows.Forms.Button btnIndex;
		private System.Windows.Forms.Button btnInsert;
		private System.Windows.Forms.Button btnQuerySelect;
		private System.Windows.Forms.Button btnRedisp;
		private System.Windows.Forms.Button btnReference;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnTmpAllDisp;
		private System.Windows.Forms.CheckBox chkDispData;
		private System.Windows.Forms.CheckBox chkDispFieldAttr;
		private System.Windows.Forms.ContextMenu fldContextMenu;
		private System.Windows.Forms.DataGrid dbGrid;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.GroupBox grpCharaSet;
		private System.Windows.Forms.GroupBox grpDataDispMode;
		private System.Windows.Forms.GroupBox grpOutputMode;
		private System.Windows.Forms.GroupBox grpSortMode;
		private System.Windows.Forms.GroupBox grpSysUserMode;
		private System.Windows.Forms.GroupBox grpViewMode;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelDB;
		private System.Windows.Forms.Label labelSchema;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private quickDBExplorer.qdbeListBox dbList;
		private quickDBExplorer.qdbeListBox fieldListbox;
		private quickDBExplorer.qdbeListBox ownerListbox;
		private quickDBExplorer.ObjectListView objectList;
		private System.Windows.Forms.MenuItem fldmenuCopy;
		private System.Windows.Forms.MenuItem fldmenuCopyNoCRLF;
		private System.Windows.Forms.MenuItem fldmenuCopyNoCRLFNoComma;
		private System.Windows.Forms.MenuItem fldmenuCopyNoComma;
		private System.Windows.Forms.RadioButton rdoDispSysUser;
		private System.Windows.Forms.RadioButton rdoNotDispSysUser;
		private System.Windows.Forms.RadioButton rdoDispView;
		private System.Windows.Forms.RadioButton rdoNotDispView;
		private System.Windows.Forms.RadioButton rdoClipboard;
		private System.Windows.Forms.RadioButton rdoOutFile;
		private System.Windows.Forms.RadioButton rdoOutFolder;
		private System.Windows.Forms.RadioButton rdoSortOwnerTable;
		private System.Windows.Forms.RadioButton rdoSortTable;
		private System.Windows.Forms.RadioButton rdoSjis;
		private System.Windows.Forms.RadioButton rdoUnicode;
		private System.Windows.Forms.RadioButton rdoUtf8;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private quickDBExplorerTextBox txtDispCount;
		private quickDBExplorerTextBox txtOutput;
		private quickDBExplorerTextBox txtSort;
        private quickDBExplorerTextBox txtWhere;

		private QueryDialog Sqldlg2 = new QueryDialog();
		private QueryDialogSelect Sqldlg = new QueryDialogSelect();
		private	CmdInputDialog	cmdDialog = new CmdInputDialog();
		private DataSet dspdt = new DataSet();
		private ServerData svdata;
		private	Font	gfont;
		private	Color	gcolor;
		private Color	btnBackColor;
		private Color	btnForeColor;
		private	IndexViewDialog indexdlg = null;
		private WhereDialog wheredlg = null;
		private string NumFormat;
		private	string	FloatFormat;
		private	string	DateFormat;
		private string aliasText;


		#region 公開メンバ
		private ISqlInterface	pSqlDriver = null;
		/// <summary>
		/// SQL文を処理するクラス
		/// </summary>
		public	ISqlInterface	SqlDriver
		{
			get { return this.pSqlDriver; }
			set { this.pSqlDriver = value; }
		}


		/// <summary>
		///  接続先のサーバー名。表示用にのみ利用
		/// </summary>
		private string pServerName = "";
		/// <summary>
		///  接続先のサーバー名。表示用にのみ利用
		/// </summary>
		public string ServerName
		{
			get { return this.pServerName; }
			set { this.pServerName = value; }
		}


        /// <summary>
        ///  接続先のサーバー名。表示用にのみ利用
        /// </summary>
        public string ServerInstanceName
        {
            get {
                if (this.InstanceName == null || this.InstanceName.Length == 0)
                {
                    return this.pServerName;
                }
                else
                {
                    return this.pServerName + @"\" + this.InstanceName;
                }
            }
        }

		/// <summary>
		/// 接続先サーバーの本当の名前。インスタンス名を含まない
		/// </summary>
		private string pServerRealName = "";
		/// <summary>
		/// 接続先サーバーの本当の名前。インスタンス名を含まない
		/// </summary>
		public string ServerRealName 
		{
			get { return this.pServerRealName; }
			set { this.pServerRealName = value; }
		}

		/// <summary>
		/// 接続先サーバーのインスタンス名
		/// </summary>
		private string pInstanceName = "";
		/// <summary>
		/// 接続先サーバーのインスタンス名
		/// </summary>
		public string InstanceName 
		{
			get { return this.pInstanceName; }
			set { this.pInstanceName = value; }
		}

		/// <summary>
		/// ログインID
		/// </summary>
		private string pLogOnUid = "";
		/// <summary>
		/// ログインID
		/// </summary>
		public string LogOnUid 
		{
			get { return this.pLogOnUid; }
			set { this.pLogOnUid = value; }
		}

		/// <summary>
		/// ログイン用パスワード
		/// </summary>
		private string pLogOnPassword = "";
		/// <summary>
		/// ログイン用パスワード
		/// </summary>
		public string LogOnPassword 
		{
			get { return this.pLogOnPassword; }
			set { this.pLogOnPassword = value; }
		}

		/// <summary>
		/// 信頼関係接続を利用するか否か
		/// </summary>
		private bool pIsUseTrust = false;
		/// <summary>
		/// 信頼関係接続を利用するか否か
		/// </summary>
		public bool IsUseTruse 
		{
			get { return this.pIsUseTrust; }
			set { this.pIsUseTrust = value; }
		}

		/// <summary>
		/// スレッドの稼動状態を表す
		/// 処理中=1 中断された、または未処理 = 0
		/// </summary>
		private int isThreadAlive = 0;

		/// <summary>
		/// Table/View リストの選択履歴
		/// Max10件を想定
		/// </summary>
		private List<string> selectedTables = new List<string>();


		/// <summary>
		/// Table/View リストの選択履歴 MAX件数
		/// </summary>
		private const int MaxTableHistory = 10;

		/// <summary>
		///  コンボボックスのイベント処理中か否か
		/// </summary>
		private bool isInCmbEvent = false;

        /// <summary>
        /// クエリ実行タイムアウト値の規定値
        /// </summary>
        public static int DefaultSqlTimeOut = 300;

		/// <summary>
		/// SQL のクエリ実行タイムアウト値
		/// </summary>
        private int pSqlTimeout = DefaultSqlTimeOut;
		/// <summary>
		/// SQL のクエリ実行タイムアウト値
		/// </summary>
		public int SqlTimeout 
		{
			get { return this.pSqlTimeout; }
			set { 
				this.pSqlTimeout = value; 
				this.SqlDriver.SetTimeout(this.pSqlTimeout);
			}
		}

		/// <summary>
		/// where 句の入力履歴情報
		/// </summary>
		private TextHistoryDataSet  whereHistory = new TextHistoryDataSet();

		/// <summary>
		/// order by 句の入力履歴情報
		/// </summary>
		private TextHistoryDataSet  sortHistory = new TextHistoryDataSet();

		/// <summary>
		/// alias 句の入力履歴情報
		/// </summary>
		private TextHistoryDataSet  aliasHistory = new TextHistoryDataSet();

		/// <summary>
		/// select 実行履歴情報
		/// </summary>
		private TextHistoryDataSet  selectHistory = new TextHistoryDataSet();

		/// <summary>
		/// select 以外のクエリ実行履歴
		/// </summary>
		private TextHistoryDataSet  DMLHistory = new TextHistoryDataSet();

		/// <summary>
		/// 各種コマンドの実行履歴
		/// </summary>
		private TextHistoryDataSet  cmdHistory = new TextHistoryDataSet();

		/// <summary>
		/// 検索の実行履歴
		/// </summary>
		private TextHistoryDataSet  searchHistory = new TextHistoryDataSet();

		/// <summary>
		/// 接続した先のSQL Serverのバージョン
		/// </summary>
		private SqlVersion pSqlVersion = null;
		/// <summary>
		/// 接続した先のSQL Serverのバージョン
		/// </summary>
		public	SqlVersion		ConnectSqlVersion
		{
			get { return this.pSqlVersion; }
			set { this.pSqlVersion = value; }
		}

        public ConnectionInfo ConnectionArg { get; private set; }

        #endregion

		private System.Windows.Forms.Button btnEtc;
		private System.Windows.Forms.MenuItem menuISQLW;
		private System.Windows.Forms.Button btnWhereZoom;
		private System.Windows.Forms.Button btnOrderZoom;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox useCheckBox;
		private System.Windows.Forms.Label labelObject;
		private System.Windows.Forms.ContextMenu dbGridMenu;
		private System.Windows.Forms.MenuItem copyDbGridMenu;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label label11;
        private quickDBExplorer.quickDBExplorerTextBox txtAlias;
		private System.Windows.Forms.MenuItem menuFieldAliasCopy;
		/// <summary>
		/// コマンド入力ダイアログ
		/// </summary>
		private System.Windows.Forms.ComboBox cmbHistory;
		private System.Windows.Forms.ColumnHeader ColTVSType;
		private System.Windows.Forms.ColumnHeader ColOwner;
		private System.Windows.Forms.ColumnHeader ColObjName;
		private System.Windows.Forms.MenuItem allSelectDbGridMenu;
		private System.Windows.Forms.MenuItem allUnSelectDbGridMenu;
		private System.Windows.Forms.MenuItem readCsvDbGridMenu;
		private System.Windows.Forms.MenuItem readTsvDbGridMenu;
		private System.Windows.Forms.MenuItem readCsvDQDbGridMenu;
		private System.Windows.Forms.MenuItem readTsvDQDbGridMenu;
		private System.Windows.Forms.MenuItem copySelectedDbGridMenu;
		private System.Windows.Forms.MenuItem menuFieldMakeWhere;
		private SplitContainer ObjFieldSplitter;
		private SplitContainer MainSplitter;
		private SplitContainer UpDownSplitter;
        private Label labelFilter;
        private quickDBExplorerTextBox txtObjFilter;
        private ContextMenuStrip dbMenu;
        private ToolStripMenuItem menuTimeoutChange;
        private ToolStripMenuItem DBReloadMenu;
        private ToolTip commTooltip;
		private System.Windows.Forms.ColumnHeader ColCreateDate;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="mdiparent">親MDIフォーム</param>
		/// <param name="conn">接続情報</param>
		public MainForm(Form mdiparent, ConnectionInfo conn)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

            SetParams(mdiparent, conn);

            InitSubDialog();
            InitHistory();

			// 右クリックメニューや、ボタンポップアップメニューを初期化する
			InitPopupMenu();
		}

        private void InitSubDialog()
        {
            Sqldlg.SelectSql = "";
            Sqldlg2.SelectSql = "";
            cmdDialog.SelectSql = "";
        }

        private void InitHistory()
        {
            this.whereHistory = svdata.WhereHistory;
            this.txtWhere.PdHistory = this.whereHistory;

            this.sortHistory = svdata.SortHistory;
            this.txtSort.PdHistory = this.sortHistory;

            this.aliasHistory = svdata.AliasHistory;
            this.txtAlias.PdHistory = this.aliasHistory;

            this.selectHistory = svdata.SelectHistory;

            this.DMLHistory = svdata.DMLHistory;
            this.cmdHistory = svdata.CmdHistory;
            this.searchHistory = svdata.SearchHistory;
        }

        private void SetParams(Form mdiparent, ConnectionInfo conn)
        {
            this.MdiParent = mdiparent;
            if (conn.InstanceName.Length != 0)
            {
                this.ServerName = conn.ServerName + "@" + conn.InstanceName;
            }
            else
            {
                this.ServerName = conn.ServerName;
            }
            this.ServerRealName = conn.ServerRealName;
            this.InstanceName = conn.InstanceName;
            this.LogOnUid = conn.LogOnUid;
            this.LogOnPassword = conn.LogOnPassword;
            this.IsUseTruse = conn.IsUseTruse;

            this.SqlDriver = conn.SqlDriver;
            this.ConnectSqlVersion = conn.SqlVersionInfo;
            this.svdata = conn.ServerDataInfo;
            this.ConnectionArg = conn;
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

		/// <summary>
		/// 終了処理
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosed(EventArgs e)
		{
			// SQLServerに対する接続を閉じる
			this.SqlDriver.CloseConnection();

			base.OnClosed (e);
		}


		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dbList = new quickDBExplorer.qdbeListBox();
            this.objectList = new quickDBExplorer.ObjectListView();
            this.ColTVSType = new System.Windows.Forms.ColumnHeader();
            this.ColOwner = new System.Windows.Forms.ColumnHeader();
            this.ColObjName = new System.Windows.Forms.ColumnHeader();
            this.ColCreateDate = new System.Windows.Forms.ColumnHeader();
            this.menuISQLW = new System.Windows.Forms.MenuItem();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnFieldList = new System.Windows.Forms.Button();
            this.btnCSV = new System.Windows.Forms.Button();
            this.rdoDispView = new System.Windows.Forms.RadioButton();
            this.grpViewMode = new System.Windows.Forms.GroupBox();
            this.rdoNotDispView = new System.Windows.Forms.RadioButton();
            this.grpSortMode = new System.Windows.Forms.GroupBox();
            this.rdoSortOwnerTable = new System.Windows.Forms.RadioButton();
            this.rdoSortTable = new System.Windows.Forms.RadioButton();
            this.txtWhere = new quickDBExplorer.quickDBExplorerTextBox();
            this.txtSort = new quickDBExplorer.quickDBExplorerTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.ownerListbox = new quickDBExplorer.qdbeListBox();
            this.btnDDL = new System.Windows.Forms.Button();
            this.dbGrid = new System.Windows.Forms.DataGrid();
            this.dbGridMenu = new System.Windows.Forms.ContextMenu();
            this.copyDbGridMenu = new System.Windows.Forms.MenuItem();
            this.copySelectedDbGridMenu = new System.Windows.Forms.MenuItem();
            this.allSelectDbGridMenu = new System.Windows.Forms.MenuItem();
            this.allUnSelectDbGridMenu = new System.Windows.Forms.MenuItem();
            this.readCsvDbGridMenu = new System.Windows.Forms.MenuItem();
            this.readCsvDQDbGridMenu = new System.Windows.Forms.MenuItem();
            this.readTsvDbGridMenu = new System.Windows.Forms.MenuItem();
            this.readTsvDQDbGridMenu = new System.Windows.Forms.MenuItem();
            this.chkDispData = new System.Windows.Forms.CheckBox();
            this.grpDataDispMode = new System.Windows.Forms.GroupBox();
            this.txtDispCount = new quickDBExplorer.quickDBExplorerTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpSysUserMode = new System.Windows.Forms.GroupBox();
            this.rdoNotDispSysUser = new System.Windows.Forms.RadioButton();
            this.rdoDispSysUser = new System.Windows.Forms.RadioButton();
            this.grpOutputMode = new System.Windows.Forms.GroupBox();
            this.btnReference = new System.Windows.Forms.Button();
            this.txtOutput = new quickDBExplorer.quickDBExplorerTextBox();
            this.rdoOutFile = new System.Windows.Forms.RadioButton();
            this.rdoClipboard = new System.Windows.Forms.RadioButton();
            this.rdoOutFolder = new System.Windows.Forms.RadioButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.labelDB = new System.Windows.Forms.Label();
            this.labelSchema = new System.Windows.Forms.Label();
            this.fieldListbox = new quickDBExplorer.qdbeListBox();
            this.fldContextMenu = new System.Windows.Forms.ContextMenu();
            this.fldmenuCopy = new System.Windows.Forms.MenuItem();
            this.fldmenuCopyNoCRLF = new System.Windows.Forms.MenuItem();
            this.fldmenuCopyNoComma = new System.Windows.Forms.MenuItem();
            this.fldmenuCopyNoCRLFNoComma = new System.Windows.Forms.MenuItem();
            this.menuFieldAliasCopy = new System.Windows.Forms.MenuItem();
            this.menuFieldMakeWhere = new System.Windows.Forms.MenuItem();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.grpCharaSet = new System.Windows.Forms.GroupBox();
            this.rdoUtf8 = new System.Windows.Forms.RadioButton();
            this.rdoSjis = new System.Windows.Forms.RadioButton();
            this.rdoUnicode = new System.Windows.Forms.RadioButton();
            this.chkDispFieldAttr = new System.Windows.Forms.CheckBox();
            this.btnQuerySelect = new System.Windows.Forms.Button();
            this.btnDataUpdate = new System.Windows.Forms.Button();
            this.btnDataEdit = new System.Windows.Forms.Button();
            this.btnGridFormat = new System.Windows.Forms.Button();
            this.btnIndex = new System.Windows.Forms.Button();
            this.btnRedisp = new System.Windows.Forms.Button();
            this.btnTmpAllDisp = new System.Windows.Forms.Button();
            this.btnEtc = new System.Windows.Forms.Button();
            this.btnWhereZoom = new System.Windows.Forms.Button();
            this.btnOrderZoom = new System.Windows.Forms.Button();
            this.useCheckBox = new System.Windows.Forms.CheckBox();
            this.labelObject = new System.Windows.Forms.Label();
            this.cmbHistory = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAlias = new quickDBExplorer.quickDBExplorerTextBox();
            this.ObjFieldSplitter = new System.Windows.Forms.SplitContainer();
            this.labelFilter = new System.Windows.Forms.Label();
            this.txtObjFilter = new quickDBExplorer.quickDBExplorerTextBox();
            this.MainSplitter = new System.Windows.Forms.SplitContainer();
            this.UpDownSplitter = new System.Windows.Forms.SplitContainer();
            this.dbMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuTimeoutChange = new System.Windows.Forms.ToolStripMenuItem();
            this.DBReloadMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.commTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.grpViewMode.SuspendLayout();
            this.grpSortMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).BeginInit();
            this.grpDataDispMode.SuspendLayout();
            this.grpSysUserMode.SuspendLayout();
            this.grpOutputMode.SuspendLayout();
            this.grpCharaSet.SuspendLayout();
            this.ObjFieldSplitter.Panel1.SuspendLayout();
            this.ObjFieldSplitter.Panel2.SuspendLayout();
            this.ObjFieldSplitter.SuspendLayout();
            this.MainSplitter.Panel1.SuspendLayout();
            this.MainSplitter.Panel2.SuspendLayout();
            this.MainSplitter.SuspendLayout();
            this.UpDownSplitter.Panel1.SuspendLayout();
            this.UpDownSplitter.Panel2.SuspendLayout();
            this.UpDownSplitter.SuspendLayout();
            this.dbMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MsgArea
            // 
            this.MsgArea.Location = new System.Drawing.Point(304, 660);
            this.MsgArea.Size = new System.Drawing.Size(709, 16);
            this.MsgArea.TabIndex = 42;
            // 
            // dbList
            // 
            this.dbList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dbList.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dbList.HorizontalScrollbar = true;
            this.dbList.ItemHeight = 12;
            this.dbList.Location = new System.Drawing.Point(60, 8);
            this.dbList.Name = "dbList";
            this.dbList.Size = new System.Drawing.Size(164, 52);
            this.dbList.TabIndex = 1;
            this.dbList.CopyData += new quickDBExplorer.CopyDataEventHandler(this.dbList_CopyData);
            this.dbList.SelectedIndexChanged += new System.EventHandler(this.dbList_SelectedIndexChanged);
            // 
            // objectList
            // 
            this.objectList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.objectList.AllowDrop = true;
            this.objectList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColTVSType,
            this.ColOwner,
            this.ColObjName,
            this.ColCreateDate});
            this.objectList.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objectList.FullRowSelect = true;
            this.objectList.GridLines = true;
            this.objectList.HideSelection = false;
            this.objectList.IsAutoColumSort = true;
            this.objectList.Location = new System.Drawing.Point(3, 26);
            this.objectList.Name = "objectList";
            this.objectList.Size = new System.Drawing.Size(328, 297);
            this.objectList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.objectList.TabIndex = 23;
            this.objectList.UseCompatibleStateImageBehavior = false;
            this.objectList.View = System.Windows.Forms.View.Details;
            this.objectList.CopyData += new quickDBExplorer.qdbeListView.CopyDataEventHandler(this.objectList_CopyData);
            this.objectList.SelectedIndexChanged += new System.EventHandler(this.objectList_SelectedIndexChanged);
            this.objectList.DoubleClick += new System.EventHandler(this.InsertMake);
            this.objectList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.objectList_ColumnClick);
            // 
            // ColTVSType
            // 
            this.ColTVSType.Text = "";
            this.ColTVSType.Width = 12;
            // 
            // ColOwner
            // 
            this.ColOwner.Text = "Owner";
            this.ColOwner.Width = 50;
            // 
            // ColObjName
            // 
            this.ColObjName.Text = "名称";
            this.ColObjName.Width = 226;
            // 
            // ColCreateDate
            // 
            this.ColCreateDate.Text = "作成日";
            this.ColCreateDate.Width = 130;
            // 
            // menuISQLW
            // 
            this.menuISQLW.Index = -1;
            this.menuISQLW.Text = "クエリアナライザ起動";
            this.menuISQLW.Click += new System.EventHandler(this.CallISQLW);
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Location = new System.Drawing.Point(337, 26);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(136, 24);
            this.btnInsert.TabIndex = 24;
            this.btnInsert.Text = "INSERT文作成(&I)";
            // 
            // btnFieldList
            // 
            this.btnFieldList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFieldList.Location = new System.Drawing.Point(337, 54);
            this.btnFieldList.Name = "btnFieldList";
            this.btnFieldList.Size = new System.Drawing.Size(136, 24);
            this.btnFieldList.TabIndex = 25;
            this.btnFieldList.Text = "フィールドリスト作成(&F)";
            // 
            // btnCSV
            // 
            this.btnCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCSV.Location = new System.Drawing.Point(337, 138);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(136, 24);
            this.btnCSV.TabIndex = 28;
            this.btnCSV.Text = "CSV等作成・読込(&K)";
            // 
            // rdoDispView
            // 
            this.rdoDispView.Location = new System.Drawing.Point(8, 16);
            this.rdoDispView.Name = "rdoDispView";
            this.rdoDispView.Size = new System.Drawing.Size(88, 16);
            this.rdoDispView.TabIndex = 0;
            this.rdoDispView.Text = "表示する";
            this.rdoDispView.CheckedChanged += new System.EventHandler(this.rdoDispView_CheckedChanged);
            // 
            // grpViewMode
            // 
            this.grpViewMode.Controls.Add(this.rdoNotDispView);
            this.grpViewMode.Controls.Add(this.rdoDispView);
            this.grpViewMode.Location = new System.Drawing.Point(8, 212);
            this.grpViewMode.Name = "grpViewMode";
            this.grpViewMode.Size = new System.Drawing.Size(216, 40);
            this.grpViewMode.TabIndex = 6;
            this.grpViewMode.TabStop = false;
            this.grpViewMode.Text = "VIEWを一覧に";
            // 
            // rdoNotDispView
            // 
            this.rdoNotDispView.Location = new System.Drawing.Point(112, 16);
            this.rdoNotDispView.Name = "rdoNotDispView";
            this.rdoNotDispView.Size = new System.Drawing.Size(88, 16);
            this.rdoNotDispView.TabIndex = 1;
            this.rdoNotDispView.Text = "表示しない";
            // 
            // grpSortMode
            // 
            this.grpSortMode.Controls.Add(this.rdoSortOwnerTable);
            this.grpSortMode.Controls.Add(this.rdoSortTable);
            this.grpSortMode.Location = new System.Drawing.Point(8, 256);
            this.grpSortMode.Name = "grpSortMode";
            this.grpSortMode.Size = new System.Drawing.Size(216, 52);
            this.grpSortMode.TabIndex = 7;
            this.grpSortMode.TabStop = false;
            this.grpSortMode.Text = "ソート順";
            // 
            // rdoSortOwnerTable
            // 
            this.rdoSortOwnerTable.Location = new System.Drawing.Point(112, 16);
            this.rdoSortOwnerTable.Name = "rdoSortOwnerTable";
            this.rdoSortOwnerTable.Size = new System.Drawing.Size(96, 32);
            this.rdoSortOwnerTable.TabIndex = 1;
            this.rdoSortOwnerTable.Text = "オーナー名・オブジェクト名";
            // 
            // rdoSortTable
            // 
            this.rdoSortTable.Location = new System.Drawing.Point(8, 16);
            this.rdoSortTable.Name = "rdoSortTable";
            this.rdoSortTable.Size = new System.Drawing.Size(96, 32);
            this.rdoSortTable.TabIndex = 0;
            this.rdoSortTable.Text = "オブジェクト名のみ";
            this.rdoSortTable.CheckedChanged += new System.EventHandler(this.rdoSortTable_CheckedChanged);
            // 
            // txtWhere
            // 
            this.txtWhere.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWhere.CanCtrlDelete = true;
            this.txtWhere.IsDigitOnly = false;
            this.txtWhere.IsShowZoom = true;
            this.txtWhere.Location = new System.Drawing.Point(72, 480);
            this.txtWhere.Name = "txtWhere";
            this.txtWhere.PdHistory = null;
            this.txtWhere.Size = new System.Drawing.Size(117, 19);
            this.txtWhere.TabIndex = 11;
            this.txtWhere.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWhere_KeyDown);
            this.txtWhere.ShowHistory += new quickDBExplorer.ShowHistoryEventHandler(this.txtWhere_ShowHistory);
            this.txtWhere.Leave += new System.EventHandler(this.txtWhere_Leave);
            this.txtWhere.ShowZoom += new quickDBExplorer.ShowZoomEventHandler(this.txtWhere_ShowZoom);
            // 
            // txtSort
            // 
            this.txtSort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSort.CanCtrlDelete = true;
            this.txtSort.IsDigitOnly = false;
            this.txtSort.IsShowZoom = true;
            this.txtSort.Location = new System.Drawing.Point(72, 508);
            this.txtSort.Name = "txtSort";
            this.txtSort.PdHistory = null;
            this.txtSort.Size = new System.Drawing.Size(117, 19);
            this.txtSort.TabIndex = 14;
            this.txtSort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSort_KeyDown);
            this.txtSort.ShowHistory += new quickDBExplorer.ShowHistoryEventHandler(this.txtSort_ShowHistory);
            this.txtSort.Leave += new System.EventHandler(this.txtSort_Leave);
            this.txtSort.ShowZoom += new quickDBExplorer.ShowZoomEventHandler(this.txtSort_ShowZoom);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "where(&P)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 508);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "order by(&S)";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(337, 82);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(136, 24);
            this.btnSelect.TabIndex = 26;
            this.btnSelect.Text = "Select 文生成(&X)";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // ownerListbox
            // 
            this.ownerListbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ownerListbox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ownerListbox.HorizontalScrollbar = true;
            this.ownerListbox.ItemHeight = 12;
            this.ownerListbox.Location = new System.Drawing.Point(60, 72);
            this.ownerListbox.Name = "ownerListbox";
            this.ownerListbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ownerListbox.Size = new System.Drawing.Size(164, 88);
            this.ownerListbox.TabIndex = 4;
            this.ownerListbox.CopyData += new quickDBExplorer.CopyDataEventHandler(this.ownerListbox_CopyData);
            this.ownerListbox.SelectedIndexChanged += new System.EventHandler(this.ownerListbox_SelectedIndexChanged);
            // 
            // btnDDL
            // 
            this.btnDDL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDDL.Location = new System.Drawing.Point(337, 110);
            this.btnDDL.Name = "btnDDL";
            this.btnDDL.Size = new System.Drawing.Size(136, 24);
            this.btnDDL.TabIndex = 27;
            this.btnDDL.Text = "簡易定義文生成(&D)";
            // 
            // dbGrid
            // 
            this.dbGrid.AlternatingBackColor = System.Drawing.Color.Silver;
            this.dbGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dbGrid.BackColor = System.Drawing.Color.White;
            this.dbGrid.CaptionBackColor = System.Drawing.Color.Gainsboro;
            this.dbGrid.CaptionFont = new System.Drawing.Font("Tahoma", 8F);
            this.dbGrid.CaptionForeColor = System.Drawing.Color.White;
            this.dbGrid.CaptionVisible = false;
            this.dbGrid.ContextMenu = this.dbGridMenu;
            this.dbGrid.DataMember = "";
            this.dbGrid.Font = new System.Drawing.Font("ＭＳ ゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dbGrid.ForeColor = System.Drawing.Color.Black;
            this.dbGrid.GridLineColor = System.Drawing.Color.Silver;
            this.dbGrid.HeaderBackColor = System.Drawing.Color.Silver;
            this.dbGrid.HeaderFont = new System.Drawing.Font("ＭＳ ゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbGrid.HeaderForeColor = System.Drawing.Color.Black;
            this.dbGrid.LinkColor = System.Drawing.Color.Maroon;
            this.dbGrid.Location = new System.Drawing.Point(4, 64);
            this.dbGrid.Name = "dbGrid";
            this.dbGrid.ParentRowsBackColor = System.Drawing.Color.Silver;
            this.dbGrid.ParentRowsForeColor = System.Drawing.Color.Black;
            this.dbGrid.RowHeaderWidth = 40;
            this.dbGrid.SelectionBackColor = System.Drawing.Color.Maroon;
            this.dbGrid.SelectionForeColor = System.Drawing.Color.White;
            this.dbGrid.Size = new System.Drawing.Size(764, 240);
            this.dbGrid.TabIndex = 41;
            this.dbGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.dbGrid_Paint);
            // 
            // dbGridMenu
            // 
            this.dbGridMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.copyDbGridMenu,
            this.copySelectedDbGridMenu,
            this.allSelectDbGridMenu,
            this.allUnSelectDbGridMenu,
            this.readCsvDbGridMenu,
            this.readCsvDQDbGridMenu,
            this.readTsvDbGridMenu,
            this.readTsvDQDbGridMenu});
            this.dbGridMenu.Popup += new System.EventHandler(this.dbGridMenu_Popup);
            // 
            // copyDbGridMenu
            // 
            this.copyDbGridMenu.Index = 0;
            this.copyDbGridMenu.Text = "全ての行をクリップボードにコピー";
            this.copyDbGridMenu.Click += new System.EventHandler(this.copyDbGridMenu_Click);
            // 
            // copySelectedDbGridMenu
            // 
            this.copySelectedDbGridMenu.Index = 1;
            this.copySelectedDbGridMenu.Text = "選択行のみクリップボードにコピー";
            this.copySelectedDbGridMenu.Click += new System.EventHandler(this.copySelectedDbGridMenu_Click);
            // 
            // allSelectDbGridMenu
            // 
            this.allSelectDbGridMenu.Index = 2;
            this.allSelectDbGridMenu.Text = "全行選択";
            this.allSelectDbGridMenu.Click += new System.EventHandler(this.allSelectDbGridMenu_Click);
            // 
            // allUnSelectDbGridMenu
            // 
            this.allUnSelectDbGridMenu.Index = 3;
            this.allUnSelectDbGridMenu.Text = "全行選択解除";
            this.allUnSelectDbGridMenu.Click += new System.EventHandler(this.allUnSelectDbGridMenu_Click);
            // 
            // readCsvDbGridMenu
            // 
            this.readCsvDbGridMenu.Index = 4;
            this.readCsvDbGridMenu.Text = "データ取込(CSV)";
            this.readCsvDbGridMenu.Click += new System.EventHandler(this.readCsvDbGridMenu_Click);
            // 
            // readCsvDQDbGridMenu
            // 
            this.readCsvDQDbGridMenu.Index = 5;
            this.readCsvDQDbGridMenu.Text = "データ取込(CSV)(\"付き)";
            this.readCsvDQDbGridMenu.Click += new System.EventHandler(this.readCsvDQDbGridMenu_Click);
            // 
            // readTsvDbGridMenu
            // 
            this.readTsvDbGridMenu.Index = 6;
            this.readTsvDbGridMenu.Text = "データ取込(TAB)";
            this.readTsvDbGridMenu.Click += new System.EventHandler(this.readTsvDbGridMenu_Click);
            // 
            // readTsvDQDbGridMenu
            // 
            this.readTsvDQDbGridMenu.Index = 7;
            this.readTsvDQDbGridMenu.Text = "データ取込(TAB)(\"付き)";
            this.readTsvDQDbGridMenu.Click += new System.EventHandler(this.readTsvDQDbGridMenu_Click);
            // 
            // chkDispData
            // 
            this.chkDispData.Location = new System.Drawing.Point(14, 14);
            this.chkDispData.Name = "chkDispData";
            this.chkDispData.Size = new System.Drawing.Size(52, 24);
            this.chkDispData.TabIndex = 2;
            this.chkDispData.Text = "表示";
            this.chkDispData.CheckedChanged += new System.EventHandler(this.chkDispData_CheckedChanged);
            // 
            // grpDataDispMode
            // 
            this.grpDataDispMode.Controls.Add(this.txtDispCount);
            this.grpDataDispMode.Controls.Add(this.label3);
            this.grpDataDispMode.Controls.Add(this.chkDispData);
            this.grpDataDispMode.Location = new System.Drawing.Point(8, 557);
            this.grpDataDispMode.Name = "grpDataDispMode";
            this.grpDataDispMode.Size = new System.Drawing.Size(216, 44);
            this.grpDataDispMode.TabIndex = 18;
            this.grpDataDispMode.TabStop = false;
            this.grpDataDispMode.Text = "データグリッド";
            // 
            // txtDispCount
            // 
            this.txtDispCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDispCount.CanCtrlDelete = true;
            this.txtDispCount.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtDispCount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDispCount.IsDigitOnly = true;
            this.txtDispCount.IsShowZoom = false;
            this.txtDispCount.Location = new System.Drawing.Point(132, 16);
            this.txtDispCount.MaxLength = 300;
            this.txtDispCount.Name = "txtDispCount";
            this.txtDispCount.PdHistory = null;
            this.txtDispCount.Size = new System.Drawing.Size(72, 19);
            this.txtDispCount.TabIndex = 1;
            this.txtDispCount.Text = "1000";
            this.txtDispCount.Leave += new System.EventHandler(this.txtDispCount_Leave);
            this.txtDispCount.TextChanged += new System.EventHandler(this.txtDispCount_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(72, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "表示件数";
            // 
            // grpSysUserMode
            // 
            this.grpSysUserMode.Controls.Add(this.rdoNotDispSysUser);
            this.grpSysUserMode.Controls.Add(this.rdoDispSysUser);
            this.grpSysUserMode.Location = new System.Drawing.Point(8, 168);
            this.grpSysUserMode.Name = "grpSysUserMode";
            this.grpSysUserMode.Size = new System.Drawing.Size(216, 40);
            this.grpSysUserMode.TabIndex = 5;
            this.grpSysUserMode.TabStop = false;
            this.grpSysUserMode.Text = "システムユーザーを";
            // 
            // rdoNotDispSysUser
            // 
            this.rdoNotDispSysUser.Checked = true;
            this.rdoNotDispSysUser.Location = new System.Drawing.Point(112, 16);
            this.rdoNotDispSysUser.Name = "rdoNotDispSysUser";
            this.rdoNotDispSysUser.Size = new System.Drawing.Size(88, 16);
            this.rdoNotDispSysUser.TabIndex = 1;
            this.rdoNotDispSysUser.TabStop = true;
            this.rdoNotDispSysUser.Text = "表示しない";
            this.rdoNotDispSysUser.CheckedChanged += new System.EventHandler(this.rdoNotDispSysUser_CheckedChanged);
            // 
            // rdoDispSysUser
            // 
            this.rdoDispSysUser.Location = new System.Drawing.Point(8, 16);
            this.rdoDispSysUser.Name = "rdoDispSysUser";
            this.rdoDispSysUser.Size = new System.Drawing.Size(88, 16);
            this.rdoDispSysUser.TabIndex = 0;
            this.rdoDispSysUser.Text = "表示する";
            this.rdoDispSysUser.CheckedChanged += new System.EventHandler(this.rdoDispSysUser_CheckedChanged);
            // 
            // grpOutputMode
            // 
            this.grpOutputMode.Controls.Add(this.btnReference);
            this.grpOutputMode.Controls.Add(this.txtOutput);
            this.grpOutputMode.Controls.Add(this.rdoOutFile);
            this.grpOutputMode.Controls.Add(this.rdoClipboard);
            this.grpOutputMode.Controls.Add(this.rdoOutFolder);
            this.grpOutputMode.Location = new System.Drawing.Point(8, 316);
            this.grpOutputMode.Name = "grpOutputMode";
            this.grpOutputMode.Size = new System.Drawing.Size(216, 84);
            this.grpOutputMode.TabIndex = 8;
            this.grpOutputMode.TabStop = false;
            this.grpOutputMode.Text = "出力先";
            // 
            // btnReference
            // 
            this.btnReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReference.Location = new System.Drawing.Point(168, 52);
            this.btnReference.Name = "btnReference";
            this.btnReference.Size = new System.Drawing.Size(40, 20);
            this.btnReference.TabIndex = 4;
            this.btnReference.Text = "参照(&R)";
            this.btnReference.Click += new System.EventHandler(this.btnReference_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.CanCtrlDelete = true;
            this.txtOutput.IsDigitOnly = false;
            this.txtOutput.IsShowZoom = false;
            this.txtOutput.Location = new System.Drawing.Point(8, 52);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.PdHistory = null;
            this.txtOutput.Size = new System.Drawing.Size(160, 19);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // rdoOutFile
            // 
            this.rdoOutFile.Location = new System.Drawing.Point(8, 32);
            this.rdoOutFile.Name = "rdoOutFile";
            this.rdoOutFile.Size = new System.Drawing.Size(88, 16);
            this.rdoOutFile.TabIndex = 1;
            this.rdoOutFile.Text = "単独ファイル";
            this.rdoOutFile.CheckedChanged += new System.EventHandler(this.rdoOutFile_CheckedChanged);
            // 
            // rdoClipboard
            // 
            this.rdoClipboard.Location = new System.Drawing.Point(8, 12);
            this.rdoClipboard.Name = "rdoClipboard";
            this.rdoClipboard.Size = new System.Drawing.Size(88, 16);
            this.rdoClipboard.TabIndex = 0;
            this.rdoClipboard.Text = "クリップボード";
            this.rdoClipboard.CheckedChanged += new System.EventHandler(this.rdoClipboard_CheckedChanged);
            // 
            // rdoOutFolder
            // 
            this.rdoOutFolder.Location = new System.Drawing.Point(104, 32);
            this.rdoOutFolder.Name = "rdoOutFolder";
            this.rdoOutFolder.Size = new System.Drawing.Size(88, 16);
            this.rdoOutFolder.TabIndex = 2;
            this.rdoOutFolder.Text = "複数ファイル";
            this.rdoOutFolder.CheckedChanged += new System.EventHandler(this.rdoOutFolder_CheckedChanged);
            // 
            // labelDB
            // 
            this.labelDB.Location = new System.Drawing.Point(8, 20);
            this.labelDB.Name = "labelDB";
            this.labelDB.Size = new System.Drawing.Size(48, 32);
            this.labelDB.TabIndex = 0;
            this.labelDB.Text = "DB(&B)";
            this.labelDB.DoubleClick += new System.EventHandler(this.labelDB_DoubleClick);
            this.labelDB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.labelDB_MouseClick);
            // 
            // labelSchema
            // 
            this.labelSchema.Location = new System.Drawing.Point(4, 72);
            this.labelSchema.Name = "labelSchema";
            this.labelSchema.Size = new System.Drawing.Size(48, 32);
            this.labelSchema.TabIndex = 3;
            this.labelSchema.Text = "owner/Role(&O)";
            this.labelSchema.DoubleClick += new System.EventHandler(this.labelSchema_DoubleClick);
            // 
            // fieldListbox
            // 
            this.fieldListbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldListbox.ContextMenu = this.fldContextMenu;
            this.fieldListbox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fieldListbox.HorizontalScrollbar = true;
            this.fieldListbox.ItemHeight = 12;
            this.fieldListbox.Location = new System.Drawing.Point(1, 26);
            this.fieldListbox.Name = "fieldListbox";
            this.fieldListbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fieldListbox.Size = new System.Drawing.Size(292, 292);
            this.fieldListbox.TabIndex = 35;
            this.fieldListbox.CopyData += new quickDBExplorer.CopyDataEventHandler(this.fieldListbox_CopyData);
            this.fieldListbox.ExtendedCopyData += new quickDBExplorer.ExtendedCopyDataEventHandler(this.fieldListbox_ExtendedCopyData);
            // 
            // fldContextMenu
            // 
            this.fldContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fldmenuCopy,
            this.fldmenuCopyNoCRLF,
            this.fldmenuCopyNoComma,
            this.fldmenuCopyNoCRLFNoComma,
            this.menuFieldAliasCopy,
            this.menuFieldMakeWhere});
            // 
            // fldmenuCopy
            // 
            this.fldmenuCopy.Index = 0;
            this.fldmenuCopy.Text = "コピー";
            this.fldmenuCopy.Click += new System.EventHandler(this.fldmenuCopy_Click);
            // 
            // fldmenuCopyNoCRLF
            // 
            this.fldmenuCopyNoCRLF.Index = 1;
            this.fldmenuCopyNoCRLF.Text = "改行なしコピー";
            this.fldmenuCopyNoCRLF.Click += new System.EventHandler(this.fldmenuCopyNoCRLF_Click);
            // 
            // fldmenuCopyNoComma
            // 
            this.fldmenuCopyNoComma.Index = 2;
            this.fldmenuCopyNoComma.Text = "コピーカンマなし";
            this.fldmenuCopyNoComma.Click += new System.EventHandler(this.fldmenuCopyNoComma_Click);
            // 
            // fldmenuCopyNoCRLFNoComma
            // 
            this.fldmenuCopyNoCRLFNoComma.Index = 3;
            this.fldmenuCopyNoCRLFNoComma.Text = "コピー改行・カンマなし";
            this.fldmenuCopyNoCRLFNoComma.Click += new System.EventHandler(this.fldmenuCopyNoCRLFNoComma_Click);
            // 
            // menuFieldAliasCopy
            // 
            this.menuFieldAliasCopy.Index = 4;
            this.menuFieldAliasCopy.Text = "条件指定コピー";
            this.menuFieldAliasCopy.Click += new System.EventHandler(this.menuFieldAliasCopy_Click);
            // 
            // menuFieldMakeWhere
            // 
            this.menuFieldMakeWhere.Index = 5;
            this.menuFieldMakeWhere.Text = "where 句生成";
            this.menuFieldMakeWhere.Click += new System.EventHandler(this.menuFieldMakeWhere_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(280, 16);
            this.label9.TabIndex = 37;
            this.label9.Text = "複数行にわたる文字列はとき色(ピンク)に着色されます。";
            this.commTooltip.SetToolTip(this.label9, "セル背景色: 水色 ⇒ NULL値 : とき色(ピンク) ⇒ 複数行にわたる文字列 : コバルトグリーン ⇒ バイナリデータ");
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(368, 16);
            this.label7.TabIndex = 35;
            this.label7.Text = "見出しに★がある列はNULL可です。NULLのセルは水色に着色されます。";
            this.commTooltip.SetToolTip(this.label7, "セル背景色: 水色 ⇒ NULL値 : とき色(ピンク) ⇒ 複数行にわたる文字列 : コバルトグリーン ⇒ バイナリデータ");
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(500, 16);
            this.label8.TabIndex = 36;
            this.label8.Text = "NULLを入力するにはCtrl+1 を、空文字列を入力するにはCtrl+2を押下します。Ctrl+3で値拡大表示。";
            this.commTooltip.SetToolTip(this.label8, "セル背景色: 水色 ⇒ NULL値 : とき色(ピンク) ⇒ 複数行にわたる文字列 : コバルトグリーン ⇒ バイナリデータ");
            // 
            // grpCharaSet
            // 
            this.grpCharaSet.Controls.Add(this.rdoUtf8);
            this.grpCharaSet.Controls.Add(this.rdoSjis);
            this.grpCharaSet.Controls.Add(this.rdoUnicode);
            this.grpCharaSet.Location = new System.Drawing.Point(8, 404);
            this.grpCharaSet.Name = "grpCharaSet";
            this.grpCharaSet.Size = new System.Drawing.Size(216, 60);
            this.grpCharaSet.TabIndex = 9;
            this.grpCharaSet.TabStop = false;
            this.grpCharaSet.Text = "出力文字コード";
            // 
            // rdoUtf8
            // 
            this.rdoUtf8.Location = new System.Drawing.Point(8, 36);
            this.rdoUtf8.Name = "rdoUtf8";
            this.rdoUtf8.Size = new System.Drawing.Size(80, 16);
            this.rdoUtf8.TabIndex = 2;
            this.rdoUtf8.Text = "UTF-8";
            this.rdoUtf8.CheckedChanged += new System.EventHandler(this.rdoUtf8_CheckedChanged);
            // 
            // rdoSjis
            // 
            this.rdoSjis.Location = new System.Drawing.Point(104, 16);
            this.rdoSjis.Name = "rdoSjis";
            this.rdoSjis.Size = new System.Drawing.Size(80, 16);
            this.rdoSjis.TabIndex = 1;
            this.rdoSjis.Text = "ShiftJIS";
            this.rdoSjis.CheckedChanged += new System.EventHandler(this.rdoSjis_CheckedChanged);
            // 
            // rdoUnicode
            // 
            this.rdoUnicode.Location = new System.Drawing.Point(8, 16);
            this.rdoUnicode.Name = "rdoUnicode";
            this.rdoUnicode.Size = new System.Drawing.Size(92, 16);
            this.rdoUnicode.TabIndex = 0;
            this.rdoUnicode.Text = "UNICODE";
            this.rdoUnicode.CheckedChanged += new System.EventHandler(this.rdoUnicode_CheckedChanged);
            // 
            // chkDispFieldAttr
            // 
            this.chkDispFieldAttr.Location = new System.Drawing.Point(3, 6);
            this.chkDispFieldAttr.Name = "chkDispFieldAttr";
            this.chkDispFieldAttr.Size = new System.Drawing.Size(161, 20);
            this.chkDispFieldAttr.TabIndex = 34;
            this.chkDispFieldAttr.Text = "フィールド属性を表示(&Z)";
            this.chkDispFieldAttr.CheckedChanged += new System.EventHandler(this.chkDispFieldAttr_CheckedChanged);
            // 
            // btnQuerySelect
            // 
            this.btnQuerySelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuerySelect.Location = new System.Drawing.Point(337, 222);
            this.btnQuerySelect.Name = "btnQuerySelect";
            this.btnQuerySelect.Size = new System.Drawing.Size(136, 24);
            this.btnQuerySelect.TabIndex = 31;
            this.btnQuerySelect.Text = "クエリ指定結果表示(&J)";
            this.btnQuerySelect.Click += new System.EventHandler(this.btnQuerySelect_Click);
            // 
            // btnDataUpdate
            // 
            this.btnDataUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataUpdate.Location = new System.Drawing.Point(337, 286);
            this.btnDataUpdate.Name = "btnDataUpdate";
            this.btnDataUpdate.Size = new System.Drawing.Size(132, 24);
            this.btnDataUpdate.TabIndex = 33;
            this.btnDataUpdate.Text = "データ更新(&U)";
            this.btnDataUpdate.Click += new System.EventHandler(this.btnDataUpdate_Click);
            // 
            // btnDataEdit
            // 
            this.btnDataEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDataEdit.Location = new System.Drawing.Point(337, 258);
            this.btnDataEdit.Name = "btnDataEdit";
            this.btnDataEdit.Size = new System.Drawing.Size(132, 24);
            this.btnDataEdit.TabIndex = 32;
            this.btnDataEdit.Text = "データ編集(&T)";
            this.btnDataEdit.Click += new System.EventHandler(this.btnDataEdit_Click);
            // 
            // btnGridFormat
            // 
            this.btnGridFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGridFormat.Location = new System.Drawing.Point(609, 16);
            this.btnGridFormat.Name = "btnGridFormat";
            this.btnGridFormat.Size = new System.Drawing.Size(156, 20);
            this.btnGridFormat.TabIndex = 39;
            this.btnGridFormat.Text = "グリッド表示書式指定(&G)";
            this.btnGridFormat.Click += new System.EventHandler(this.btnGridFormat_Click);
            // 
            // btnIndex
            // 
            this.btnIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIndex.Location = new System.Drawing.Point(337, 166);
            this.btnIndex.Name = "btnIndex";
            this.btnIndex.Size = new System.Drawing.Size(136, 23);
            this.btnIndex.TabIndex = 29;
            this.btnIndex.Text = "INDEX情報表示(&N)";
            this.btnIndex.Click += new System.EventHandler(this.btnIndex_Click);
            // 
            // btnRedisp
            // 
            this.btnRedisp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedisp.Location = new System.Drawing.Point(497, 40);
            this.btnRedisp.Name = "btnRedisp";
            this.btnRedisp.Size = new System.Drawing.Size(108, 20);
            this.btnRedisp.TabIndex = 38;
            this.btnRedisp.Text = "グリッド再描画(&L)";
            this.btnRedisp.Click += new System.EventHandler(this.Redisp_Click);
            // 
            // btnTmpAllDisp
            // 
            this.btnTmpAllDisp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTmpAllDisp.Location = new System.Drawing.Point(609, 40);
            this.btnTmpAllDisp.Name = "btnTmpAllDisp";
            this.btnTmpAllDisp.Size = new System.Drawing.Size(156, 20);
            this.btnTmpAllDisp.TabIndex = 40;
            this.btnTmpAllDisp.Text = "一時的に全データを表示(&A)";
            this.btnTmpAllDisp.Click += new System.EventHandler(this.btnTmpAllDisp_Click);
            // 
            // btnEtc
            // 
            this.btnEtc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEtc.Location = new System.Drawing.Point(337, 194);
            this.btnEtc.Name = "btnEtc";
            this.btnEtc.Size = new System.Drawing.Size(136, 23);
            this.btnEtc.TabIndex = 30;
            this.btnEtc.Text = "その他(&E)";
            // 
            // btnWhereZoom
            // 
            this.btnWhereZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWhereZoom.Location = new System.Drawing.Point(189, 480);
            this.btnWhereZoom.Name = "btnWhereZoom";
            this.btnWhereZoom.Size = new System.Drawing.Size(16, 20);
            this.btnWhereZoom.TabIndex = 12;
            this.btnWhereZoom.Click += new System.EventHandler(this.btnWhereZoom_Click);
            // 
            // btnOrderZoom
            // 
            this.btnOrderZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrderZoom.Location = new System.Drawing.Point(189, 508);
            this.btnOrderZoom.Name = "btnOrderZoom";
            this.btnOrderZoom.Size = new System.Drawing.Size(16, 20);
            this.btnOrderZoom.TabIndex = 15;
            this.btnOrderZoom.Click += new System.EventHandler(this.btnOrderZoom_Click);
            // 
            // useCheckBox
            // 
            this.useCheckBox.Location = new System.Drawing.Point(20, 116);
            this.useCheckBox.Name = "useCheckBox";
            this.useCheckBox.Size = new System.Drawing.Size(36, 36);
            this.useCheckBox.TabIndex = 2;
            this.useCheckBox.Text = "CheckList";
            this.useCheckBox.Visible = false;
            // 
            // labelObject
            // 
            this.labelObject.Location = new System.Drawing.Point(3, 6);
            this.labelObject.Name = "labelObject";
            this.labelObject.Size = new System.Drawing.Size(152, 16);
            this.labelObject.TabIndex = 1;
            this.labelObject.Text = "オブジェクト(&V)";
            this.labelObject.DoubleClick += new System.EventHandler(this.labelObject_DoubleClick);
            // 
            // cmbHistory
            // 
            this.cmbHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHistory.DropDownWidth = 300;
            this.cmbHistory.Location = new System.Drawing.Point(80, 3);
            this.cmbHistory.MaxDropDownItems = 11;
            this.cmbHistory.Name = "cmbHistory";
            this.cmbHistory.Size = new System.Drawing.Size(176, 20);
            this.cmbHistory.TabIndex = 2;
            this.cmbHistory.SelectionChangeCommitted += new System.EventHandler(this.cmbHistory_SelectionChangeCommitted);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(8, 536);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 16);
            this.label11.TabIndex = 16;
            this.label11.Text = "Alias(&M)";
            this.label11.DoubleClick += new System.EventHandler(this.label11_DoubleClick);
            // 
            // txtAlias
            // 
            this.txtAlias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAlias.CanCtrlDelete = true;
            this.txtAlias.IsDigitOnly = false;
            this.txtAlias.IsShowZoom = true;
            this.txtAlias.Location = new System.Drawing.Point(72, 532);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.PdHistory = null;
            this.txtAlias.Size = new System.Drawing.Size(137, 19);
            this.txtAlias.TabIndex = 17;
            this.commTooltip.SetToolTip(this.txtAlias, "選択したオブジェクトに別名(Alias)をつけることができます");
            this.txtAlias.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAlias_KeyDown);
            this.txtAlias.ShowHistory += new quickDBExplorer.ShowHistoryEventHandler(this.txtAlias_ShowHistory);
            this.txtAlias.Leave += new System.EventHandler(this.txtAlias_Leave);
            this.txtAlias.Enter += new System.EventHandler(this.txtAlias_Enter);
            this.txtAlias.ShowZoom += new quickDBExplorer.ShowZoomEventHandler(this.txtAlias_ShowZoom);
            // 
            // ObjFieldSplitter
            // 
            this.ObjFieldSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ObjFieldSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjFieldSplitter.Location = new System.Drawing.Point(0, 0);
            this.ObjFieldSplitter.Name = "ObjFieldSplitter";
            // 
            // ObjFieldSplitter.Panel1
            // 
            this.ObjFieldSplitter.Panel1.AutoScroll = true;
            this.ObjFieldSplitter.Panel1.Controls.Add(this.labelFilter);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.cmbHistory);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.txtObjFilter);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.labelObject);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.objectList);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnInsert);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnFieldList);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnCSV);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnEtc);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnSelect);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnDDL);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnQuerySelect);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnIndex);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnDataUpdate);
            this.ObjFieldSplitter.Panel1.Controls.Add(this.btnDataEdit);
            // 
            // ObjFieldSplitter.Panel2
            // 
            this.ObjFieldSplitter.Panel2.AutoScroll = true;
            this.ObjFieldSplitter.Panel2.Controls.Add(this.chkDispFieldAttr);
            this.ObjFieldSplitter.Panel2.Controls.Add(this.fieldListbox);
            this.ObjFieldSplitter.Size = new System.Drawing.Size(779, 340);
            this.ObjFieldSplitter.SplitterDistance = 480;
            this.ObjFieldSplitter.SplitterWidth = 2;
            this.ObjFieldSplitter.TabIndex = 19;
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(262, 9);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(54, 12);
            this.labelFilter.TabIndex = 3;
            this.labelFilter.Text = "フィルタ(&A)";
            this.labelFilter.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtObjFilter
            // 
            this.txtObjFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObjFilter.CanCtrlDelete = true;
            this.txtObjFilter.IsDigitOnly = false;
            this.txtObjFilter.IsShowZoom = false;
            this.txtObjFilter.Location = new System.Drawing.Point(319, 3);
            this.txtObjFilter.Name = "txtObjFilter";
            this.txtObjFilter.PdHistory = null;
            this.txtObjFilter.Size = new System.Drawing.Size(153, 19);
            this.txtObjFilter.TabIndex = 22;
            this.txtObjFilter.TextChanged += new System.EventHandler(this.txtObjFilter_TextChanged);
            // 
            // MainSplitter
            // 
            this.MainSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSplitter.Location = new System.Drawing.Point(0, 0);
            this.MainSplitter.Name = "MainSplitter";
            // 
            // MainSplitter.Panel1
            // 
            this.MainSplitter.Panel1.AutoScroll = true;
            this.MainSplitter.Panel1.Controls.Add(this.dbList);
            this.MainSplitter.Panel1.Controls.Add(this.txtAlias);
            this.MainSplitter.Panel1.Controls.Add(this.useCheckBox);
            this.MainSplitter.Panel1.Controls.Add(this.label11);
            this.MainSplitter.Panel1.Controls.Add(this.btnOrderZoom);
            this.MainSplitter.Panel1.Controls.Add(this.labelSchema);
            this.MainSplitter.Panel1.Controls.Add(this.btnWhereZoom);
            this.MainSplitter.Panel1.Controls.Add(this.grpDataDispMode);
            this.MainSplitter.Panel1.Controls.Add(this.grpSysUserMode);
            this.MainSplitter.Panel1.Controls.Add(this.grpViewMode);
            this.MainSplitter.Panel1.Controls.Add(this.grpSortMode);
            this.MainSplitter.Panel1.Controls.Add(this.txtWhere);
            this.MainSplitter.Panel1.Controls.Add(this.txtSort);
            this.MainSplitter.Panel1.Controls.Add(this.grpCharaSet);
            this.MainSplitter.Panel1.Controls.Add(this.label1);
            this.MainSplitter.Panel1.Controls.Add(this.labelDB);
            this.MainSplitter.Panel1.Controls.Add(this.label2);
            this.MainSplitter.Panel1.Controls.Add(this.grpOutputMode);
            this.MainSplitter.Panel1.Controls.Add(this.ownerListbox);
            // 
            // MainSplitter.Panel2
            // 
            this.MainSplitter.Panel2.Controls.Add(this.UpDownSplitter);
            this.MainSplitter.Size = new System.Drawing.Size(1032, 660);
            this.MainSplitter.SplitterDistance = 251;
            this.MainSplitter.SplitterWidth = 2;
            this.MainSplitter.TabIndex = 0;
            // 
            // UpDownSplitter
            // 
            this.UpDownSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UpDownSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpDownSplitter.Location = new System.Drawing.Point(0, 0);
            this.UpDownSplitter.Name = "UpDownSplitter";
            this.UpDownSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // UpDownSplitter.Panel1
            // 
            this.UpDownSplitter.Panel1.AutoScroll = true;
            this.UpDownSplitter.Panel1.Controls.Add(this.ObjFieldSplitter);
            // 
            // UpDownSplitter.Panel2
            // 
            this.UpDownSplitter.Panel2.AutoScroll = true;
            this.UpDownSplitter.Panel2.Controls.Add(this.dbGrid);
            this.UpDownSplitter.Panel2.Controls.Add(this.label9);
            this.UpDownSplitter.Panel2.Controls.Add(this.btnGridFormat);
            this.UpDownSplitter.Panel2.Controls.Add(this.label8);
            this.UpDownSplitter.Panel2.Controls.Add(this.btnRedisp);
            this.UpDownSplitter.Panel2.Controls.Add(this.label7);
            this.UpDownSplitter.Panel2.Controls.Add(this.btnTmpAllDisp);
            this.UpDownSplitter.Size = new System.Drawing.Size(779, 660);
            this.UpDownSplitter.SplitterDistance = 340;
            this.UpDownSplitter.SplitterWidth = 2;
            this.UpDownSplitter.TabIndex = 0;
            // 
            // dbMenu
            // 
            this.dbMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTimeoutChange,
            this.DBReloadMenu});
            this.dbMenu.Name = "dbMenu";
            this.dbMenu.Size = new System.Drawing.Size(161, 48);
            // 
            // menuTimeoutChange
            // 
            this.menuTimeoutChange.Name = "menuTimeoutChange";
            this.menuTimeoutChange.Size = new System.Drawing.Size(160, 22);
            this.menuTimeoutChange.Text = "タイムアウト変更(&t)";
            this.menuTimeoutChange.Click += new System.EventHandler(this.menuTimeoutChange_Click);
            // 
            // DBReloadMenu
            // 
            this.DBReloadMenu.Name = "DBReloadMenu";
            this.DBReloadMenu.Size = new System.Drawing.Size(160, 22);
            this.DBReloadMenu.Text = "DB再読み込み(&R)";
            this.DBReloadMenu.Click += new System.EventHandler(this.DBReloadMenu_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1033, 677);
            this.Controls.Add(this.MainSplitter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Database選択";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Controls.SetChildIndex(this.MsgArea, 0);
            this.Controls.SetChildIndex(this.MainSplitter, 0);
            this.grpViewMode.ResumeLayout(false);
            this.grpSortMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dbGrid)).EndInit();
            this.grpDataDispMode.ResumeLayout(false);
            this.grpDataDispMode.PerformLayout();
            this.grpSysUserMode.ResumeLayout(false);
            this.grpOutputMode.ResumeLayout(false);
            this.grpOutputMode.PerformLayout();
            this.grpCharaSet.ResumeLayout(false);
            this.ObjFieldSplitter.Panel1.ResumeLayout(false);
            this.ObjFieldSplitter.Panel1.PerformLayout();
            this.ObjFieldSplitter.Panel2.ResumeLayout(false);
            this.ObjFieldSplitter.ResumeLayout(false);
            this.MainSplitter.Panel1.ResumeLayout(false);
            this.MainSplitter.Panel1.PerformLayout();
            this.MainSplitter.Panel2.ResumeLayout(false);
            this.MainSplitter.ResumeLayout(false);
            this.UpDownSplitter.Panel1.ResumeLayout(false);
            this.UpDownSplitter.Panel2.ResumeLayout(false);
            this.UpDownSplitter.ResumeLayout(false);
            this.dbMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}




		#endregion

		#region ボタンメニュー関連処理
#if false
		// 現在未使用
		private void DispButtonMenu(object sender, System.EventArgs e, MenuItem[] list)
		{
			ContextMenu tmpmenu = new System.Windows.Forms.ContextMenu();
			MenuItem[] cplist = new MenuItem[list.Length];
			// メニューの各項目の先頭に、(1) のようなアクセラレーターキーを追加する
			int j = 1;
			for( int i = 0; i<list.Length; i++ )
			{
				cplist[i] = list[i].CloneMenu();
				if( list[i].Text == "-" )
				{
					// セパレーターはショートカットは無用
					continue;
				}
				cplist[i].Text = string.Format(System.Globalization.CultureInfo.CurrentCulture,"(&{0}) {1}",j,cplist[i].Text );
				j++;
			}
			tmpmenu.MenuItems.AddRange(cplist);

			tmpmenu.Show((Control)sender,new Point(0,0));
		}
#endif

		/// <summary>
		/// ポップアップ系メニューを初期化する
		/// </summary>
		public	void	InitPopupMenu()
		{
            List<qdbeMenuItem> menuAr = SetInitPopupMenu();


            ContextMenu objMenu = CreatePopupMenuItem(menuAr);

			this.objectList.ContextMenu = objMenu;

            SetButtonPopup(menuAr);
		}

        private void SetButtonPopup(List<qdbeMenuItem> menuAr)
        {
            List<Control> btnAr = new List<Control>();
            btnAr.Add(this.btnCSV);
            btnAr.Add(this.btnDDL);
            btnAr.Add(this.btnEtc);
            btnAr.Add(this.btnFieldList);
            btnAr.Add(this.btnInsert);

            foreach (Button btn in btnAr)
            {
                ContextMenu btnMenu = new ContextMenu();
                int i = 0;
                int j = 0;

                foreach (qdbeMenuItem itm in menuAr)
                {
                    if (itm.CallBtnName == btn.Name)
                    {
                        if (itm.IsSeparater == true)
                        {
                            btnMenu.MenuItems.Add(itm.CreateMenuItem(i, 0));
                        }
                        else
                        {
                            btnMenu.MenuItems.Add(itm.CreateMenuItem(i, j + 1));
                            j++;
                        }
                        i++;
                    }
                }
                if (btnMenu.MenuItems.Count > 0)
                {
                    btn.Tag = btnMenu;
                    btn.Click += new EventHandler(btn_Click);
                }
            }
        }

        private ContextMenu CreatePopupMenuItem(List<qdbeMenuItem> menuAr)
        {
            ContextMenu objMenu = new System.Windows.Forms.ContextMenu();
            int idx = 0;
            foreach (qdbeMenuItem it in menuAr)
            {
                if (it.IsObjTarget == false)
                {
                    continue;
                }
                objMenu.MenuItems.Add(it.CreateMenuItem(idx, 0));
                idx++;
            }
            return objMenu;
        }

        private List<qdbeMenuItem> SetInitPopupMenu()
        {
            List<qdbeMenuItem> menuAr = new List<qdbeMenuItem>();
            menuAr.Add(new qdbeMenuItem(false, true, null, "オブジェクト名コピー", new EventHandler(this.menuTableCopy_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, null, "オブジェクト名コピー カンマ付き", new EventHandler(this.menuTableCopyCsv_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, null, "指定オブジェクト選択", new EventHandler(this.menuTableSelect_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, null, "オブジェクト情報再読込", new EventHandler(this.TableInfoUpdate)));
            menuAr.Add(new qdbeMenuItem(true, true, null, "-", null));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnInsert.Name, "INSERT文作成", new EventHandler(this.InsertMake)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnInsert.Name, "INSERT文作成(DELETE文付き)", new EventHandler(this.InsertMakeDelete)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnInsert.Name, "INSERT文作成(フィールドリストなし)", new EventHandler(this.InsertMakeNoField)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnInsert.Name, "INSERT文作成(フィールドリストなし　DELETE文付き)", new EventHandler(this.InsertMakeNoFieldDelete)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnInsert.Name, "INSERT文作成(DELETE文付き、退避付き)", new EventHandler(this.menuInsertDeleteTaihi_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnInsert.Name, "INSERT文作成(フィールドなし DELETE文付き 退避付き)", new EventHandler(this.menuInsertNoFldDeleteTaihi_Click)));
            menuAr.Add(new qdbeMenuItem(true, true, null, "-", null));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnFieldList.Name, "フィールドリスト作成", new EventHandler(this.makefldlist)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnFieldList.Name, "フィールドリスト改行作成", new EventHandler(this.makefldListLF)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnFieldList.Name, "フィールドリストカンマなし作成", new EventHandler(this.makefldListNoComma)));
            menuAr.Add(new qdbeMenuItem(true, true, null, "-", null));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnDDL.Name, "簡易定義文生成", new EventHandler(this.makeDDL)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnDDL.Name, "簡易定義文生成 DROP文付き", new EventHandler(this.makeDDLDrop)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnDDL.Name, "簡易定義文生成([]付き)", new EventHandler(this.makeDDLPare)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnDDL.Name, "簡易定義文生成( DROP []付き)", new EventHandler(this.makeDDLDropPare)));
            menuAr.Add(new qdbeMenuItem(true, true, null, "-", null));
            menuAr.Add(new qdbeMenuItem(false, true, null, "Select文生成", new EventHandler(this.btnSelect_Click)));
            menuAr.Add(new qdbeMenuItem(true, true, null, "-", null));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "CSV作成", new EventHandler(this.makeCSV)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "CSV作成(”付き)", new EventHandler(this.makeCSVQuote)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "Tab区切出力", new EventHandler(this.menuMakeTab_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "Tab区切出力(\"付き)", new EventHandler(this.menuMakeTabDQ_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "CSV読込", new EventHandler(this.menuCSVRead_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "CSV読込(\"付き)", new EventHandler(this.menuCSVReadDQ_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "Tab区切読込", new EventHandler(this.menuTabRead_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnCSV.Name, "Tab区切読込（”付き)", new EventHandler(this.menuTabReadDQ_Click)));
            menuAr.Add(new qdbeMenuItem(true, false, null, "-", null));
            menuAr.Add(new qdbeMenuItem(false, false, this.btnEtc.Name, "簡易クエリ実行（Select以外）", new EventHandler(this.btnQueryNonSelect_Click)));
            menuAr.Add(new qdbeMenuItem(true, false, this.btnEtc.Name, "-", null));
            if (this.ConnectSqlVersion.CanUseQueryAnalyzer == true)
            {
                menuAr.Add(new qdbeMenuItem(false, false, this.btnEtc.Name, "クエリアナライザ起動", new EventHandler(this.CallISQLW)));
            }
            menuAr.Add(new qdbeMenuItem(false, false, this.btnEtc.Name, "プロファイラ起動", new EventHandler(this.CallProfile)));
            if (this.ConnectSqlVersion.IsManagementStudio == true)
            {
                menuAr.Add(new qdbeMenuItem(false, false, this.btnEtc.Name, "Management Studio起動", new EventHandler(this.CallEPM)));
            }
            else
            {
                menuAr.Add(new qdbeMenuItem(false, false, this.btnEtc.Name, "Enterprise Manager起動", new EventHandler(this.CallEPM)));
            }
            menuAr.Add(new qdbeMenuItem(true, true, this.btnEtc.Name, "-", null));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "依存関係出力", new EventHandler(this.DependOutPut)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "データ件数出力", new EventHandler(this.RecordCountOutPut)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "データ件数表示", new EventHandler(this.menuRecordCountDisp_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "統計情報更新", new EventHandler(this.menuUpdateStaticsMain_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "各種コマンド実行", new EventHandler(this.menuDoQuery_Click)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "オブジェクト情報表示", new EventHandler(this.DispObjectInfo)));
            menuAr.Add(new qdbeMenuItem(false, true, this.btnEtc.Name, "オブジェクト検索", new EventHandler(this.ObjectSearch)));
            return menuAr;
        }

		private void btn_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			ContextMenu btnMenu = (ContextMenu)btn.Tag;
			btnMenu.Show((Control)sender,new Point(0,0));
		}

		#endregion

		#region メニュー関連ボタンイベントハンドラ

		private void TableInfoUpdate(object sender, System.EventArgs e)
		{
			this.objectList.ReloadSelectObjectInfo();
			if( this.objectList.SelectedItems.Count == 1 )
			{
				// 一件だけの選択ならフィールドのリストを更新する
				this.DispFldList(this.objectList.GetSelectObject(0));
			}
		}
		
		private void InsertMake(object sender, System.EventArgs e)
		{
			this.CreInsert(true,false,false);
		}

		private void makefldlist(object sender, System.EventArgs e)
		{
			CreateFldList(false,true);
		}

		private void makefldListLF(object sender, System.EventArgs e)
		{
			CreateFldList(true,true);
		}

		private void makefldListNoComma(object sender, System.EventArgs e)
		{
			CreateFldList(true,false);
		}

		private void makeCSV(object sender, System.EventArgs e)
		{
			CreateTCsvText(false, ",");
		}

		private void makeCSVQuote(object sender, System.EventArgs e)
		{
			CreateTCsvText(true,",");
		}

		private void menuMakeTab_Click(object sender, System.EventArgs e)
		{
			CreateTCsvText(false,"	");
		}

		private void menuMakeTabDQ_Click(object sender, System.EventArgs e)
		{
			CreateTCsvText(true,"	");
		}

		private void InsertMakeDelete(object sender, System.EventArgs e)
		{
			this.CreInsert( true, true,false );
		}

		private void InsertMakeNoField(object sender, System.EventArgs e)
		{
			this.CreInsert(false,false,false );
		}

		private void InsertMakeNoFieldDelete(object sender, System.EventArgs e)
		{
			this.CreInsert(false,true,false);
		}

		private void makeDDL(object sender, System.EventArgs e)
		{
			this.CreDDL(false, false);
		}

		private void makeDDLDrop(object sender, System.EventArgs e)
		{
			this.CreDDL(true, false);
		}
		
		private void makeDDLPare(object sender, System.EventArgs e)
		{
			this.CreDDL(false, true);
		
		}

		private void makeDDLDropPare(object sender, System.EventArgs e)
		{
			this.CreDDL(true, true);
		}

		private void menuTableCopy_Click(object sender, System.EventArgs e)
		{
			CopyTableName(false);
		}

		private void menuTableCopyCsv_Click(object sender, System.EventArgs e)
		{
			CopyTableName(true);
		}


		private void fldmenuCopy_Click(object sender, System.EventArgs e)
		{
			CopyFldList(true,true);
		}

		private void fldmenuCopyNoCRLF_Click(object sender, System.EventArgs e)
		{
			CopyFldList(false,true);
		}

		private void fldmenuCopyNoComma_Click(object sender, System.EventArgs e)
		{
			CopyFldList(true,false);
		}

		private void fldmenuCopyNoCRLFNoComma_Click(object sender, System.EventArgs e)
		{
			CopyFldList(false,false);
		}

		private void rdoUnicode_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.rdoUnicode.Checked == true )
			{
				svdata.TxtEncode[svdata.LastDatabase] = 0;
			}
		}

		private void rdoSjis_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.rdoSjis.Checked == true )
			{
				svdata.TxtEncode[svdata.LastDatabase] = 1;
			}
		}

		private void rdoUtf8_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.rdoUtf8.Checked == true )
			{
				svdata.TxtEncode[svdata.LastDatabase] = 2;
			}
		}


		private void menuInsertDeleteTaihi_Click(object sender, System.EventArgs e)
		{
			this.CreInsert(true,true,true);
		}

		private void menuInsertNoFldDeleteTaihi_Click(object sender, System.EventArgs e)
		{
			this.CreInsert(false,true,true);
		}

		private void menuCSVRead_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Data(true,false);
		}

		private void menuCSVReadDQ_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Data(true,true);
		}

		private void menuTabRead_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Data(false,false);
		}

		private void menuTabReadDQ_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Data(false,true);
		}

		private void menuUpdateStaticsMain_Click(object sender, System.EventArgs e)
		{
			this.menuStasticUpdate_Click(sender,e);
		}

		private void menuFieldAliasCopy_Click(object sender, System.EventArgs e)
		{
			fieldListbox_ExtendedCopyData(sender, new System.EventArgs());
		}

		/// <summary>
		/// フィールドリストのコピーメニュー選択時処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fieldListbox_CopyData(object sender, System.EventArgs e)
		{
			CopyFldList(true,true);
		}

		/// <summary>
		/// オブジェクト一覧のヘッダカラムクリック時処理
		/// </summary>
		/// <param name="sender">-</param>
		/// <param name="e">-</param>
		private void objectList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			this.objectList.SetColumClick(e.Column);
		}

		#endregion

		#region 画面制御
		/// <summary>
		/// 初期表示処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Load(object sender, System.EventArgs e)
		{

			this.InitErrMessage();

			try
			{
				// ラベル・ボタンの設定
				this.labelSchema.Text = this.SqlDriver.GetOwnerLabel1();
				this.rdoSortOwnerTable.Text = this.SqlDriver.GetOwnerLabel2();
				//this.ColObjName.Text = this.SqlDriver.GetTableListColumnName();

				this.Text = pServerName;

				// DB一覧の表示を実行
                DBLoad();
			}
			catch ( System.Data.SqlClient.SqlException se )
			{
				this.SetErrorMessage(se);
			}
            LoadInitialSetting();
		}

        private void LoadInitialSetting()
        {
            if (svdata.IsShowsysuser == 0)
            {
                this.rdoNotDispSysUser.Checked = true;
                this.rdoDispSysUser.Checked = false;
            }
            else
            {
                this.rdoNotDispSysUser.Checked = false;
                this.rdoDispSysUser.Checked = true;
            }
            if (svdata.SortKey == 0)
            {
                this.rdoSortTable.Checked = false;
                this.rdoSortOwnerTable.Checked = true;
                this.objectList.SetSortSchemaObject();
            }
            else
            {
                this.rdoSortTable.Checked = true;
                this.rdoSortOwnerTable.Checked = false;
                this.objectList.SetSortObjectOnly();
            }
            if (svdata.ShowView == 0)
            {
                this.rdoDispView.Checked = false;
                this.rdoNotDispView.Checked = true;
            }
            else
            {
                this.rdoDispView.Checked = true;
                this.rdoNotDispView.Checked = false;
            }

            // 前回の値を元にDB先を変更する
            if (svdata.LastDatabase != null && svdata.LastDatabase != "")
            {
                for (int i = 0; i < this.dbList.Items.Count; i++)
                {
                    if ((string)this.dbList.Items[i] == svdata.LastDatabase)
                    {
                        this.dbList.SetSelected(i, true);
                        this.dbList.Focus();
                        break;
                    }
                }
            }
            else
            {
                this.dbList.SelectedIndex = 0;
            }
            gfont = this.dbGrid.Font;
            gcolor = this.dbGrid.ForeColor;

            // ボタンの表示色を記憶しておく
            this.btnBackColor = this.btnDataEdit.BackColor;
            this.btnForeColor = this.btnDataEdit.ForeColor;
        }

        private void DBLoad()
        {
            DbDataAdapter da = this.SqlDriver.NewDataAdapter();
            IDbCommand cmd = this.SqlDriver.NewSqlCommand(this.SqlDriver.GetDBSelect());
            this.SqlDriver.SetSelectCmd(da, cmd);
            DataSet ds = new DataSet();
            ds.CaseSensitive = true;
            ds.Locale = System.Globalization.CultureInfo.CurrentCulture;
            da.Fill(ds, "sysdatabases");

            this.dbList.Items.Clear();
            foreach (DataRow row in ds.Tables["sysdatabases"].Rows)
            {
                this.dbList.Items.Add(row["name"]);
            }
        }

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            SaveSetting();
		}

        private void SaveSetting()
        {
            if (this.rdoNotDispSysUser.Checked == true)
            {
                svdata.IsShowsysuser = 0;
            }
            else
            {
                svdata.IsShowsysuser = 1;
            }

            if (this.rdoSortOwnerTable.Checked == true)
            {
                svdata.SortKey = 0;
            }
            else
            {
                svdata.SortKey = 1;
            }
            if (this.rdoDispView.Checked == false)
            {
                svdata.ShowView = 0;
            }
            else
            {
                svdata.ShowView = 1;
            }

            if (this.rdoClipboard.Checked == true)
            {
                svdata.OutDest[svdata.LastDatabase] = 0;
            }
            if (this.rdoOutFile.Checked == true)
            {
                svdata.OutDest[svdata.LastDatabase] = 1;
            }
            if (this.rdoOutFolder.Checked == true)
            {
                svdata.OutDest[svdata.LastDatabase] = 2;
            }
            svdata.OutFile[svdata.LastDatabase] = this.txtOutput.Text;
            if (this.chkDispData.CheckState == CheckState.Checked)
            {
                svdata.ShowGrid[svdata.LastDatabase] = 1;
            }
            else
            {
                svdata.ShowGrid[svdata.LastDatabase] = 0;
            }
            if (this.rdoUnicode.Checked == true)
            {
                svdata.TxtEncode[svdata.LastDatabase] = 0;
            }
            if (this.rdoSjis.Checked == true)
            {
                svdata.TxtEncode[svdata.LastDatabase] = 1;
            }
            if (this.rdoUtf8.Checked == true)
            {
                svdata.TxtEncode[svdata.LastDatabase] = 2;
            }
            svdata.GridDispCnt[svdata.LastDatabase] = this.txtDispCount.Text;
        }


		#endregion

		#region 画面コントロールのイベント処理(処理ロジックは除く)

		private void ResetOwnerList()
		{
			// 対象となるオブジェクト一覧の表示
			DispObjectList();
			// 対象となる owner/role/schema の表示
			DispListOwner();
			if (svdata.Dbopt[svdata.LastDatabase] != null)
			{
				if (svdata.Dbopt[svdata.LastDatabase] is ArrayList)
				{

					ArrayList saveownerlist = (ArrayList)svdata.Dbopt[svdata.LastDatabase];

					// 該当DBの最後に選択したユーザーを選択する
					string[] olist = (string[])saveownerlist.ToArray(typeof(string));
                    ownerListbox.SetSelectedItems(olist);
					this.ownerListbox.Focus();
				}
				else
				{
					svdata.Dbopt[svdata.LastDatabase] = null;
				}
			}

			if (this.ownerListbox.SelectedItems.Count == 0)
			{
				// 選択がない場合、一番最初をディフォルトで選択する
				this.ownerListbox.SetSelected(0, true);
			}
		}


		/// <summary>
		/// DB選択が変更になった場合のイベントハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dbList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( this.dbList.SelectedItems.Count != 0 )
			{
				svdata.LastDatabase = (string)this.dbList.SelectedItem;
			}
			// オブジェクトの選択履歴をクリア
			this.selectedTables.Clear();
            this.txtObjFilter.Text = string.Empty;
            this.cmbHistory.DataSource = null;
			this.cmbHistory.DataSource = this.selectedTables;
			this.cmbHistory.Refresh();

			// Owner のリストを再読み込みし表示する
			ResetOwnerList();

            // 設定値を保存したデータから呼び戻す
            LoadPreviousSetting();

            this.commTooltip.SetToolTip(this.dbList, (string)this.dbList.SelectedItem);

			this.Text = pServerName + "@" + (string)this.dbList.SelectedItem;
			this.dbList.Focus();	//フォーカスを元に戻す
		}

        /// <summary>
        /// 設定値を保存したデータから呼び戻す
        /// </summary>
        private void LoadPreviousSetting()
        {
            if (svdata.OutDest[svdata.LastDatabase] != null)
            {
                // 該当DBの最後の出力先をセットする
                switch ((int)svdata.OutDest[svdata.LastDatabase])
                {
                    case 0:
                        //クリップボード
                        this.rdoClipboard.Checked = true;
                        this.rdoOutFile.Checked = false;
                        this.rdoOutFolder.Checked = false;
                        break;
                    case 1:
                        this.rdoClipboard.Checked = false;
                        this.rdoOutFile.Checked = true;
                        this.rdoOutFolder.Checked = false;
                        break;
                    case 2:
                        this.rdoClipboard.Checked = false;
                        this.rdoOutFile.Checked = false;
                        this.rdoOutFolder.Checked = true;
                        break;
                }
            }
            else
            {
                //標準はクリップボード
                this.rdoClipboard.Checked = true;
                this.rdoOutFile.Checked = false;
                this.rdoOutFolder.Checked = false;
            }

            if (svdata.OutFile[svdata.LastDatabase] != null)
            {
                this.txtOutput.Text = (string)svdata.OutFile[svdata.LastDatabase];
            }
            else
            {
                this.txtOutput.Text = "";
            }


            if (svdata.ShowGrid[svdata.LastDatabase] != null)
            {
                if ((int)svdata.ShowGrid[svdata.LastDatabase] == 0)
                {
                    this.chkDispData.CheckState = CheckState.Unchecked;
                }
                else
                {
                    this.chkDispData.CheckState = CheckState.Checked;
                }
            }
            else
            {
                this.chkDispData.CheckState = CheckState.Checked;
            }

            if (svdata.GridDispCnt[svdata.LastDatabase] != null)
            {
                if ((string)svdata.GridDispCnt[svdata.LastDatabase] != "")
                {
                    this.txtDispCount.Text = (string)svdata.GridDispCnt[svdata.LastDatabase];
                }
                else
                {
                    this.txtDispCount.Text = "";
                }
            }
            else
            {
                this.txtDispCount.Text = "1000";
            }

            if (svdata.TxtEncode[svdata.LastDatabase] != null)
            {
                if ((int)svdata.TxtEncode[svdata.LastDatabase] == 0)
                {
                    this.rdoUnicode.Checked = true;
                }
                else if ((int)svdata.TxtEncode[svdata.LastDatabase] == 1)
                {
                    this.rdoSjis.Checked = true;
                }
                else
                {
                    this.rdoUtf8.Checked = true;
                }
            }
            else
            {
                this.rdoUnicode.Checked = true;
            }
        }

		private void objectList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1 )
			{
				// 1件のみ選択されている場合

				if( isInCmbEvent == false )
				{
					// 選択されたTable/View を記憶する
					if( this.selectedTables.Contains(this.objectList.GetSelectOneObjectName()) == false )
					{
						if( this.selectedTables.Count > MaxTableHistory )
						{
							this.selectedTables.RemoveAt(0);
						}
					}
					else
					{
						this.selectedTables.Remove(this.objectList.GetSelectOneObjectName());

					}
					this.selectedTables.Add(this.objectList.GetSelectOneObjectName());
					this.cmbHistory.DataSource = null;
					this.cmbHistory.DataSource = this.selectedTables;
					int i = this.cmbHistory.FindStringExact(this.objectList.GetSelectOneObjectName());
					this.cmbHistory.SelectedIndex = i;
					this.cmbHistory.Refresh();
				}


				qdbeUtil.SetNewHistory(this.objectList.GetSelectOneObjectFormalName(),this.txtWhere.Text,this.whereHistory);
				qdbeUtil.SetNewHistory(this.objectList.GetSelectOneObjectFormalName(),this.txtSort.Text,this.sortHistory);
				qdbeUtil.SetNewHistory(this.objectList.GetSelectOneObjectFormalName(),this.txtAlias.Text,this.aliasHistory);
				// データ表示部に、該当オブジェクトのデータを表示する
				DispData(this.objectList.GetSelectObject(0));
			}
			else
			{
				DispData(null);
			}
			if( this.objectList.SelectedItems.Count == 1 )
			{
				DispFldList(this.objectList.GetSelectObject(0));
			}
			else
			{
				DispFldList(null);
			}
			if( indexdlg != null && indexdlg.Visible == true )
			{
				if( this.objectList.SelectedItems.Count == 1 )
				{
					indexdlg.SetDisplayTable(this.objectList.GetSelectObject(0));
				}
				else
				{
					indexdlg.SetDisplayTable(null);
				}
				indexdlg.Show();
			}

			setWhereDialog(true,false);
		}

		private void rdoDispView_CheckedChanged(object sender, System.EventArgs e)
		{
			// オブジェクトの選択履歴をクリア
			this.selectedTables.Clear();
            this.txtObjFilter.Text = string.Empty;
            this.cmbHistory.DataSource = null;
			this.cmbHistory.DataSource = this.selectedTables;
			this.cmbHistory.Refresh();
			

			DispObjectList();
		}

		private void rdoSortTable_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.rdoSortTable.Checked == true )
			{
                this.objectList.SetSortObjectOnly();
			}
			else
			{
                this.objectList.SetSortSchemaObject();
			}
			this.objectList.Sort();
				 
			//DispObjectList();
		}

		private void chkDispData_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1 )
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
				DispData(this.objectList.GetSelectObject(0));
			}
			else
			{
				DispData(null);
			}
			if( this.chkDispData.CheckState == CheckState.Checked )
			{
				svdata.ShowGrid[svdata.LastDatabase] = 1;
			}
			else
			{
				svdata.ShowGrid[svdata.LastDatabase] = 0;
			}
		}

		private void txtDispCount_Leave(object sender, System.EventArgs e)
		{
			if (((quickDBExplorerTextBox)sender).IsTextChanged == false)
			{
				return;
			}
			if (this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1 )
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
				DispData(this.objectList.GetSelectObject(0));
			}
			else
			{
				DispData(null);
			}
		}

		private void txtWhere_Leave(object sender, System.EventArgs e)
		{
			if (((quickDBExplorerTextBox)sender).IsTextChanged == false)
			{
				return;
			}
			string tbname = "";
			if (this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1)
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
				tbname = this.objectList.GetSelectObject(0).FormalName;
				DispData(this.objectList.GetSelectObject(0));
				// 履歴に現在の値を記録 TODO
				qdbeUtil.SetNewHistory(tbname, this.txtWhere.Text, this.whereHistory);
			}
			else
			{
				DispData(null);
			}
		}

		private void txtSort_Leave(object sender, System.EventArgs e)
		{
			if (((quickDBExplorerTextBox)sender).IsTextChanged == false)
			{
				return;
			}
			string tbname = "";
			if( this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1 )
			{
					// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
					tbname = this.objectList.GetSelectObject(0).FormalName;
					DispData(this.objectList.GetSelectObject(0));
					// 履歴に現在の値を記録 TODO
					qdbeUtil.SetNewHistory(tbname, this.txtSort.Text, this.sortHistory);
			}
			else
			{
				DispData(null);
			}
		}

		private void rdoDispSysUser_CheckedChanged(object sender, System.EventArgs e)
		{
			// オブジェクトの選択履歴をクリア
			this.selectedTables.Clear();
            this.txtObjFilter.Text = string.Empty;
            this.cmbHistory.DataSource = null;
			this.cmbHistory.DataSource = this.selectedTables;
			this.cmbHistory.Refresh();

			DispObjectList();
			DispListOwner();
		}

		private void ownerListbox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( this.ownerListbox.IsAllSelecting == true )
			{
				return;
			}
			if( this.ownerListbox.SelectedItem != null )
			{
				// 選択したDBの最終オーナーを記録する
				ArrayList saveownerlist;
				if( svdata.Dbopt[svdata.LastDatabase] == null )
				{
					saveownerlist = new ArrayList();
					svdata.Dbopt[svdata.LastDatabase] = saveownerlist;
				}
				else
				{
					saveownerlist = (ArrayList)svdata.Dbopt[svdata.LastDatabase];
				}
				saveownerlist.Clear();
				foreach( string itm in this.ownerListbox.SelectedItems )
				{
					saveownerlist.Add(itm);
				}
			}
			// オブジェクトの選択履歴をクリア
			this.selectedTables.Clear();
            this.txtObjFilter.Text = string.Empty;
			this.cmbHistory.DataSource = null;
			this.cmbHistory.DataSource = this.selectedTables;
			this.cmbHistory.Refresh();
            if( ownerListbox.SelectedItems.Count == 1 )
            {
                this.commTooltip.SetToolTip(this.ownerListbox, ownerListbox.SelectedItem.ToString());
            }
			

			DispObjectList();
		}

		private void rdoNotDispSysUser_CheckedChanged(object sender, System.EventArgs e)
		{
			// オブジェクトの選択履歴をクリア
			this.selectedTables.Clear();
            this.txtObjFilter.Text = string.Empty;
			this.cmbHistory.DataSource = null;
			this.cmbHistory.DataSource = this.selectedTables;
			this.cmbHistory.Refresh();

			DispObjectList();
			DispListOwner();
		}

		private void rdoClipboard_CheckedChanged(object sender, System.EventArgs e)
		{
			if( rdoClipboard.Checked == true )
			{
				this.txtOutput.Enabled = false;
				this.btnReference.Enabled = false;
				svdata.OutDest[svdata.LastDatabase] = 0;
				this.rdoUnicode.Enabled = false;
				this.rdoSjis.Enabled = false;
				this.rdoUtf8.Enabled = false;
			}
		}

		private void rdoOutFolder_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.rdoOutFolder.Checked == true )
			{
				this.txtOutput.Enabled = true;
				this.btnReference.Enabled = true;
				svdata.OutDest[svdata.LastDatabase] = 2;
				this.rdoUnicode.Enabled = true;
				this.rdoSjis.Enabled = true;
				this.rdoUtf8.Enabled = true;
			}
		}

		private void rdoOutFile_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.rdoOutFile.Checked == true )
			{
				this.txtOutput.Enabled = true;
				this.btnReference.Enabled = true;
				svdata.OutDest[svdata.LastDatabase] = 1;
				this.rdoUnicode.Enabled = true;
				this.rdoSjis.Enabled = true;
				this.rdoUtf8.Enabled = true;
			}
		}

		private void btnReference_Click(object sender, System.EventArgs e)
		{
			if( this.rdoOutFile.Checked == true )
			{
				if( this.txtOutput.Text != "" )
				{
					DirectoryInfo d = new DirectoryInfo(this.txtOutput.Text);
					if( d.Exists )
					{
						this.saveFileDialog1.InitialDirectory = this.txtOutput.Text;
						this.saveFileDialog1.FileName = "";
					}
					else
					{
						this.saveFileDialog1.FileName = this.txtOutput.Text;
					}
				}
				// 単独ファイルの参照指定
				
				this.saveFileDialog1.CreatePrompt = true;
				this.saveFileDialog1.Filter = "SQL|*.sql|csv|*.csv|txt|*.txt|全て|*.*";
				DialogResult ret = this.saveFileDialog1.ShowDialog();
				if( ret == DialogResult.OK )
				{
					this.txtOutput.Text = this.saveFileDialog1.FileName;
				}
			}
			else
			{
				// 複数ファイルのディレクトリ参照指定
				if( this.txtOutput.Text != "" )
				{
					FileInfo f = new FileInfo(this.txtOutput.Text);
					if( f.Exists &&
						( f.Attributes & FileAttributes.Directory ) == FileAttributes.Directory)
					{
						this.folderBrowserDialog1.SelectedPath = this.txtOutput.Text;
					}
					else if( f.Exists && (f.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
					{
						this.folderBrowserDialog1.SelectedPath = f.Directory.FullName;
					}
					else if( !f.Exists )
					{
						this.folderBrowserDialog1.SelectedPath = this.txtOutput.Text;
					}
					else
					{
						this.folderBrowserDialog1.SelectedPath = "";
					}
				}
				else
				{
					this.folderBrowserDialog1.SelectedPath = "";
				}
				
				this.folderBrowserDialog1.ShowNewFolderButton = true;
				DialogResult ret = this.folderBrowserDialog1.ShowDialog();
				if( ret == DialogResult.OK )
				{
					this.txtOutput.Text = this.folderBrowserDialog1.SelectedPath;
				}
			}
		}

		private void txtOutput_TextChanged(object sender, System.EventArgs e)
		{
			this.commTooltip.SetToolTip(this.txtOutput,this.txtOutput.Text);
			svdata.OutFile[svdata.LastDatabase] = this.txtOutput.Text;
		}


		private void txtDispCount_TextChanged(object sender, System.EventArgs e)
		{
			svdata.GridDispCnt[svdata.LastDatabase] = this.txtDispCount.Text;
		}

		private void chkDispFieldAttr_CheckedChanged(object sender, System.EventArgs e)
		{
			if( this.objectList.SelectedItems.Count == 1 )
			{
				DispFldList(this.objectList.GetSelectObject(0));
			}
			else
			{
				DispFldList(null);
			}
		}

		private void btnDataUpdate_Click(object sender, System.EventArgs e)
		{
			IDbTransaction tran	= null;
			try
			{
				this.InitErrMessage();

				this.dbGrid.EndEdit(this.dbGrid.TableStyles[0].GridColumnStyles[this.dbGrid.CurrentCell.ColumnNumber], this.dbGrid.CurrentCell.RowNumber, false);

				if (this.chkDispData.CheckState == CheckState.Checked &&
					this.objectList.SelectedItems.Count == 1 &&
					this.dspdt.GetChanges() != null &&
					this.dspdt.GetChanges().Tables[0].Rows.Count > 0 &&
					MessageBox.Show("本当に更新してよろしいですか", "", MessageBoxButtons.YesNo) == DialogResult.Yes
					)
				{
					// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
					DBObjectInfo dboInfo = this.objectList.GetSelectObject(0);
					string stSql;
					stSql = "select ";
					int maxlines;
					if (this.txtDispCount.Text != "")
					{
						maxlines = int.Parse(this.txtDispCount.Text, System.Globalization.CultureInfo.CurrentCulture);
					}
					else
					{
						maxlines = 0;
					}
					if (maxlines != 0)
					{
						stSql += " TOP " + this.txtDispCount.Text;
					}

					stSql += string.Format(System.Globalization.CultureInfo.CurrentCulture, " * from {0}", dboInfo.GetAliasName(this.GetAlias()));
					if (this.txtWhere.Text.Trim() != "")
					{
						stSql += " where " + this.txtWhere.Text.Trim();
					}
					if (this.txtSort.Text.Trim() != "")
					{
						stSql += " order by " + this.txtSort.Text.Trim();
					}

					DbDataAdapter da = this.SqlDriver.NewDataAdapter();
					IDbCommand cmd = this.SqlDriver.NewSqlCommand(stSql);
					this.SqlDriver.SetSelectCmd(da, cmd);

					tran = this.SqlDriver.SetTransaction(cmd);
					this.SqlDriver.SetCommandBuilder(da);
					da.Update(dspdt, dspdt.Tables[0].TableName);
					tran.Commit();

					this.dbGrid.SetDataBinding(dspdt, dspdt.Tables[0].TableName);
				}
			}
			catch (System.Data.DBConcurrencyException exp)
			{
				if (tran != null)
				{
					tran.Rollback();
				}
				string emsg = string.Format("{0}行のデータが既に他の処理により更新されている可能性があります。\r\n処理を中断しました。\r\n再度データを読み込みしなおして再度編集して下さい",
					exp.RowCount);
				MessageBox.Show(emsg);
			}
			catch (Exception exp)
			{
				this.SetErrorMessage(exp);
				if (tran != null)
				{
					tran.Rollback();
				}
			}

		}

		private void btnDataEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.InitErrMessage();

				if( this.dbGrid.ReadOnly == true )
				{
					// 編集可にする
					this.dbGrid.ReadOnly = false;
					this.btnDataEdit.Text = "データ編集終了(&T)";
					this.btnDataEdit.ForeColor = Color.WhiteSmoke;
					this.btnDataEdit.BackColor = Color.Navy;
				}
				else
				{
					// 編集不可にする
					if( this.dspdt.Tables[0].GetChanges() == null ||
						this.dspdt.Tables[0].GetChanges().Rows.Count == 0 )
					{
						this.dbGrid.ReadOnly = true;
						this.btnDataEdit.Text = "データ編集(&T)";
						this.btnDataEdit.BackColor = this.btnBackColor;
						this.btnDataEdit.ForeColor = this.btnForeColor;
					}
					else
					{
						// 変更があった
						if( MessageBox.Show("変更を破棄してもよろしいですか?","",MessageBoxButtons.YesNo) == DialogResult.Yes )
						{
							this.dspdt.Tables[0].RejectChanges();
							this.dbGrid.SetDataBinding(dspdt, dspdt.Tables[0].TableName);
							this.dbGrid.Show();
							this.dbGrid.ReadOnly = true;
							this.btnDataEdit.Text = "データ編集(&T)";
							this.btnDataEdit.BackColor = this.btnBackColor;
							this.btnDataEdit.ForeColor = this.btnForeColor;
						}
					}
				}
			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
			}
		}

		private void dbGrid_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if( this.dspdt == null ||
				this.dspdt.Tables.Count == 0 ||
				this.dspdt.Tables[0].Rows.Count == 0 )
			{
				return;
			}

			int row=0;
			int yDelta = 0;

			int headerHeight = this.dbGrid.PreferredRowHeight + 3;

			CurrencyManager cm = (CurrencyManager) this.BindingContext[dbGrid.DataSource, dbGrid.DataMember];
			int dispRow = 0;
			Rectangle rect = Rectangle.Empty;


			int firstCheckHeight = this.dbGrid.PreferredRowHeight * 2;
			int firstCheckCol = this.dbGrid.RowHeaderWidth + 3;

			DataGrid.HitTestInfo hinfo = null;
			bool isFound = false;
			while( firstCheckHeight > 0 && isFound == false)
			{
				hinfo = this.dbGrid.HitTest(firstCheckCol,firstCheckHeight);
				switch( hinfo.Type )
				{
					case(System.Windows.Forms.DataGrid.HitTestType.Cell):
						isFound = true;
						break;
       
					case(System.Windows.Forms.DataGrid.HitTestType.Caption):
						break;
       
					case(System.Windows.Forms.DataGrid.HitTestType.ColumnHeader):
						firstCheckHeight++;
						break;
          
					case(System.Windows.Forms.DataGrid.HitTestType.ColumnResize):
						firstCheckHeight++;
						break;
          
					case(System.Windows.Forms.DataGrid.HitTestType.ParentRows):
						isFound = true;
						break;
          
					case(System.Windows.Forms.DataGrid.HitTestType.RowHeader):
						firstCheckHeight++;
						break;
          
					case(System.Windows.Forms.DataGrid.HitTestType.RowResize):
						firstCheckHeight++;
						break;
          
					case(System.Windows.Forms.DataGrid.HitTestType.None):
						isFound = true;
						break;

					default:
						break;
				}
			}

			// 最初に表示される可能性のある行を探し出す
			row = hinfo.Row;
			if( row < 0 )
			{
				row = 0;
			}
			else
			{
				for(;row > 0; row-- )
				{
					rect = dbGrid.GetCellBounds(row, 0);
					if( rect.Top < 0 )
					{
						break;
					}
				}
			}

			while(row < cm.Count && yDelta < this.dbGrid.Height)
			{
				rect = dbGrid.GetCellBounds(row, 0);
				if( ( rect.Top + 1 ) <= headerHeight )
				{
					row++;
					continue;
				}
				//get & draw the header text...
				yDelta = dbGrid.GetCellBounds(row, 0).Top;
				string text = string.Format(System.Globalization.CultureInfo.CurrentCulture,"{0}", row+1);
				e.Graphics.DrawString(text, dbGrid.Font, new SolidBrush(Color.Black), 10, yDelta+2);
				row++;
				dispRow++;
			}
		}

		private void btnGridFormat_Click(object sender, System.EventArgs e)
		{
			GridFormatDialog dlg = new GridFormatDialog();
			dlg.Gfont = gfont;
			dlg.Gcolor = gcolor;
			dlg.NumFormat = this.NumFormat;
			dlg.FloatFormat = this.FloatFormat;
			dlg.DateFormat = this.DateFormat;
			
			if( dlg.ShowDialog() == DialogResult.OK )
			{
				this.gfont = dlg.Gfont;
				this.gcolor = dlg.Gcolor;
				this.dbGrid.Font = this.gfont;
				this.dbGrid.ForeColor = this.gcolor;
				this.NumFormat = dlg.NumFormat;
				this.FloatFormat = dlg.FloatFormat;
				this.DateFormat = dlg.DateFormat;
			}
		}

		private void Redisp_Click(object sender, System.EventArgs e)
		{
			if (this.dbGrid.Visible == true)
			{
				DispData(null, false, true);
			}
			////再描画ボタン押下
			//if( this.chkDispData.CheckState == CheckState.Checked &&
			//    this.objectList.SelectedItems.Count == 1 )
			//{
			//    // 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
			//    DispData(this.objectList.GetSelectObject(0));
			//}
			//else
			//{
			//    DispData(null);
			//}
		}

		private void btnTmpAllDisp_Click(object sender, System.EventArgs e)
		{
			//再描画ボタン押下
			if( this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1 )
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
				if (btnTmpAllDisp.BackColor == Color.Navy)
				{
					// 現在全表示なので...
					DispData(this.objectList.GetSelectObject(0), false, false);
					this.btnTmpAllDisp.ForeColor = this.ForeColor;
					this.btnTmpAllDisp.BackColor = this.BackColor;
					this.btnTmpAllDisp.Enabled = true;
				}
				else
				{
					DispData(this.objectList.GetSelectObject(0), true, false);
					this.btnTmpAllDisp.ForeColor = Color.WhiteSmoke;
					this.btnTmpAllDisp.BackColor = Color.Navy;
					this.btnTmpAllDisp.Enabled = true;
				}
			}
			else
			{
				DispData(null);
			}
		}

		private void btnWhereZoom_Click(object sender, System.EventArgs e)
		{
			setWhereDialog(false,true);
		}

		private void dlgWhereZoom_Click(object sender, System.EventArgs e)
		{
			this.txtWhere.Text = ((ZoomDialog)sender).EditText;
			string tbname = this.objectList.GetSelectObject(0).FormalName;
			// 履歴に現在の値を記録
			qdbeUtil.SetNewHistory(tbname, this.txtWhere.Text, this.whereHistory);

			if (this.objectList.SelectedItems.Count == 1)
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する

				DispData(this.objectList.GetSelectObject(0));
			}
			else
			{
				tbname = "";
				DispData(null);
			}
//			DispData(this.objectList.GetSelectObject(0));
		}

		private void btnOrderZoom_Click(object sender, System.EventArgs e)
		{
			this.txtSort.DoShowZoom("order by 指定",new System.EventHandler(this.dlgSortZoom_Click));
		}

		private void dlgSortZoom_Click(object sender, System.EventArgs e)
		{
			this.txtSort.Text = ((ZoomDialog)sender).EditText;
			string tbname = this.objectList.GetSelectObject(0).FormalName;
			// 履歴に現在の値を記録
			qdbeUtil.SetNewHistory(tbname, this.txtSort.Text, this.sortHistory);
			if (this.objectList.SelectedItems.Count == 1)
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する

				DispData(this.objectList.GetSelectObject(0));
			}
			else
			{
				tbname = "";
				DispData(null);
			}
		}

		private void dlgAliasZoom_Click(object sender, System.EventArgs e)
		{
			this.txtAlias.Text = ((ZoomDialog)sender).EditText;
			string tbname = this.objectList.GetSelectObject(0).FormalName;
			// 履歴に現在の値を記録
			qdbeUtil.SetNewHistory(tbname, this.txtAlias.Text, this.aliasHistory);

			if (this.objectList.SelectedItems.Count == 1)
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する

				DispData(this.objectList.GetSelectObject(0));
			}
			else
			{
				tbname = "";
				DispData(null);
			}
		}

		/// <summary>
		/// Alias のフォーカスイン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtAlias_Enter(object sender, EventArgs e)
		{
			this.aliasText = this.txtAlias.Text;
		}

		/// <summary>
		/// Alias のフォーカスアウト
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtAlias_Leave(object sender, System.EventArgs e)
		{
			TextBox			senderText = sender as TextBox;
			string tbname = "";
			if( this.chkDispData.CheckState == CheckState.Checked &&
				this.objectList.SelectedItems.Count == 1 )
			{
				// 1件のみ選択されている場合、データ表示部に、該当オブジェクトのデータを表示する
				tbname = this.objectList.GetSelectOneObjectFormalName();
				// 履歴に現在の値を記録 TODO
				qdbeUtil.SetNewHistory(tbname, senderText.Text, this.aliasHistory);
			}
			else
			{
				tbname = "";
			}
			if (this.txtAlias.Text != this.aliasText)
			{
				setWhereDialog(false,false);
			}
			this.aliasText = this.txtAlias.Text;
		}

		void txtAlias_ShowZoom(object sender, EventArgs e)
		{
			this.txtAlias.DoShowZoom("Alias 指定", new System.EventHandler(this.dlgAliasZoom_Click));
		}

		private void txtAlias_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			TextBox			senderText = sender as TextBox;

			if( e.KeyCode == Keys.Return ||
				e.KeyCode == Keys.Enter )
			{
				if( this.objectList.SelectedItems.Count == 1 )
				{
					qdbeUtil.SetNewHistory(this.objectList.GetSelectOneObjectFormalName(),senderText.Text,this.aliasHistory);
				}
				DispData(this.objectList.GetSelectObject(0));
				if (this.txtAlias.Text != this.aliasText)
				{
					setWhereDialog(false, false);
				}
				this.aliasText = this.txtAlias.Text;
			}
		}


		/// <summary>
		/// キー押下時処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if( e.Control == true && e.Shift == false && e.Alt == false && e.KeyCode == Keys.G )
			{
				ThreadEndIfAlive();
			}
			if( e.Control == true && e.Shift == true && e.Alt == true && e.KeyCode == Keys.T )
			{
                ChangeTimeout();
			}
		}

        private void ChangeTimeout()
        {
            if (this.SqlTimeout == 0)
            {
                this.SqlTimeout = 300;
            }
            else
            {
                this.SqlTimeout = 0;
            }
            MessageBox.Show("SQL Timeout値を " + this.SqlTimeout.ToString(System.Globalization.CultureInfo.CurrentCulture) + "秒に設定しました");
        }

		private void txtWhere_ShowHistory(object sender, EventArgs e)
		{
			// Ctrl + S
			// 入力履歴を表示する
			string targetTable = "";
			if (this.objectList.SelectedItems.Count == 1)
			{
				targetTable = this.objectList.GetSelectOneObjectFormalName();
			}
			if (this.txtWhere.DoShowHistory(targetTable))
			{
				DispData(this.objectList.GetSelectObject(0));
				setWhereDialog(false, false);
			}
		}

		void txtWhere_ShowZoom(object sender, EventArgs e)
		{
			// Ctrl + W
			// 値の拡大表示を行う
			setWhereDialog(false, true);
		}

		/// <summary>
		/// where 入力テキストボックスでの特殊キー押下ハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtWhere_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if( e.KeyCode == Keys.Return ||
				e.KeyCode == Keys.Enter )
			{
				// Enter(Return) では、入力を確定させて、グリッド表示に反映させる
				if( this.objectList.SelectedItems.Count == 1 )
				{
					qdbeUtil.SetNewHistory(this.objectList.GetSelectOneObjectFormalName(),this.txtWhere.Text,this.whereHistory);
				}
				DispData(this.objectList.GetSelectObject(0));
			}
		}



		/// <summary>
		/// order by 指定で Ctrl+Wが押下された場合に値拡大表示を行う
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtSort_ShowZoom(object sender, EventArgs e)
		{
			this.txtSort.DoShowZoom("order by 指定", new System.EventHandler(this.dlgSortZoom_Click));
		}

		private void txtSort_ShowHistory(object sender, EventArgs e)
		{
			string targetTable = "";
			targetTable = this.objectList.GetSelectOneObjectFormalName();
			if (this.txtSort.DoShowHistory(targetTable))
			{
				DispData(this.objectList.GetSelectObject(0));
			}
		}

		private void txtSort_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if( e.KeyCode == Keys.Return ||
				e.KeyCode == Keys.Enter )
			{
				if( this.objectList.SelectedItems.Count == 1 )
				{
					qdbeUtil.SetNewHistory(this.objectList.GetSelectOneObjectFormalName(),this.txtSort.Text,this.sortHistory);
				}
				DispData(this.objectList.GetSelectObject(0));
			}
		
		}

		/// <summary>
		/// 指定されたオブジェクトのリストを元に、オブジェクトの一覧の選択状態を変更する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTableSelect_Click(object sender, System.EventArgs e)
		{
			tableSelect(string.Empty);
		}

		/// <summary>
		/// 指定されたオブジェクトのリストを元に、オブジェクトの一覧の選択状態を変更する
		/// </summary>
		/// <param name="tableList">初期値とセットする一覧値</param>
		private void tableSelect(string tableList)
		{
			TableSelectDialog dlg = new TableSelectDialog();
			dlg.ResultStr = tableList;
			if( dlg.ShowDialog() == DialogResult.OK && dlg.ResultStr != "")
			{
				string tabs = dlg.ResultStr;
				string []objectLists = tabs.Replace("\r\n","\r").Split("\r\n".ToCharArray());
				this.objectList.BeginUpdate();
				this.objectList.ClearSelected();
				int startPosition = -1;
				for( int i = 0; i < objectLists.Length; i++ )
				{
					string []separatedStr = objectLists[i].Split("\t,:;".ToCharArray());
					int x = this.objectList.FindStringExact(separatedStr[0]);
					if( x > 0 )
					{
						this.objectList.Items[x].Selected = true;
						if( startPosition < x )
						{
							startPosition = x;
						}
					}
				}
				if( startPosition >= 0 )
				{
					this.objectList.EnsureVisible(startPosition);
				}
				this.objectList.EndUpdate();
			}
		}

		private void labelDB_DoubleClick(object sender, System.EventArgs e)
		{
            ChangeTimeout();
		}


		#endregion

		#region 実処理関連

		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			// select 文の作成

			this.InitErrMessage();

			try
			{
				if( this.objectList.SelectedItems.Count == 0 )
				{
					return;
				}
				if( CheckFileSpec() == false )
				{
					return;
				}

				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false, GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
				}

				string alias = this.GetAlias();

				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					DBObjectInfo dboInfo = this.objectList.GetSelectObject(ti);

					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + 
							dboInfo.ToString()
							+ ".sql",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
						fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql\r\n");
					}
					// get id 

					wr.Write("select {0}",wr.NewLine);
                    int i = 0;
                    foreach(DBFieldInfo each in dboInfo.FieldInfo)
                    {
						if( i != 0 )
						{
							wr.Write(",{0}", wr.NewLine);
						}

						wr.Write("\t");
						if( alias != string.Empty )
						{
							wr.Write(alias + ".");
						}


						wr.Write("{0}", each.Name);

                        i++;
					}
					wr.Write(wr.NewLine);
					wr.Write(" from {0}{1}", dboInfo.GetAliasName(this.GetAlias()),wr.NewLine);
					if( this.txtWhere.Text.Trim() != "" )
					{
						wr.Write(" where {0}{1}", this.txtWhere.Text.Trim(),wr.NewLine);
					}
					if( this.txtSort.Text.Trim() != "" )
					{
						wr.Write(" order by {0}{1}", this.txtSort.Text.Trim(),wr.NewLine);
					}
					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
					}
				}
				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Close();
				}

				if( this.rdoClipboard.Checked == true ) 
				{
					Clipboard.SetDataObject(strline.ToString(),true );
				}
				else
				{
					Clipboard.SetDataObject(fname.ToString(),true );
				}

				MessageBox.Show( "処理を終了しました" );
			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
			}
		}


		private void btnIndex_Click(object sender, System.EventArgs e)
		{
			if( indexdlg == null )
			{
				indexdlg = new IndexViewDialog();

				indexdlg.SqlDriver = this.SqlDriver;
				indexdlg.DisplayObj = this.objectList.GetSelectObject(0);

				indexdlg.Show();
			}
			else
			{
				indexdlg.SetDisplayTable(this.objectList.GetSelectObject(0));
				indexdlg.Show();
				indexdlg.BringToFront();
			}
		}

		/// <summary>
		/// 選択されたオブジェクトに関する依存関係を出力する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DependOutPut(object sender, System.EventArgs e)
		{
			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}
			if( CheckFileSpec() == false )
			{
				return;
			}
			
			this.InitErrMessage();

			try
			{
				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
					wr.Write("オブジェクト名");
					wr.Write("\t依存関係先名称");
					wr.Write("\t種類");
					wr.Write("\t更新あり");
					wr.Write("\tselectでの利用");
					wr.Write("\t従属性が存在する列またはパラメータ");
					wr.Write(wr.NewLine);
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false, GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
					wr.Write("オブジェクト名");
					wr.Write("\t依存関係先名称");
					wr.Write("\t種類");
					wr.Write("\t更新あり");
					wr.Write("\tselectでの利用");
					wr.Write("\t従属性が存在する列またはパラメータ");
					wr.Write(wr.NewLine);
				}

				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);
					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
						fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql\r\n");
						wr.Write("オブジェクト名");
						wr.Write("\t依存関係先名称");
						wr.Write("\t種類");
						wr.Write("\t更新あり");
						wr.Write("\tselectでの利用");
						wr.Write("\t従属性が存在する列またはパラメータ");
						wr.Write(wr.NewLine);
					}

					// 依存関係の情報を取得し、

					// get id 
					string stSql;

					stSql = string.Format(System.Globalization.CultureInfo.CurrentCulture,"sp_depends N'{0}'", dboInfo.FormalName );

					DbDataAdapter da = this.SqlDriver.NewDataAdapter();
					IDbCommand cmd = this.SqlDriver.NewSqlCommand(stSql);
					this.SqlDriver.SetSelectCmd(da,cmd);

					DataSet ds = new DataSet();
					ds.CaseSensitive = true;
					ds.Locale = System.Globalization.CultureInfo.CurrentCulture;
					da.Fill(ds,dboInfo.ToString());

					if(	ds.Tables.Count != 0 &&
						ds.Tables[dboInfo.ToString()].Rows != null &&
						ds.Tables[dboInfo.ToString()].Rows.Count != 0)
					{
						foreach(DataRow dr in ds.Tables[dboInfo.ToString()].Rows)
						{
							// オブジェクト名
							wr.Write(dboInfo.FormalName);
							foreach( DataColumn col in ds.Tables[dboInfo.ToString()].Columns)
							{
								wr.Write("\t");
								wr.Write(dr[col.ColumnName].ToString());
							}
							wr.Write(wr.NewLine);
						}
					}

					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
					}
				}
				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Close();
				}
				if( this.rdoClipboard.Checked == true ) 
				{
					Clipboard.SetDataObject(strline.ToString(),true );
				}
				else
				{
					Clipboard.SetDataObject(fname.ToString(),true );
				}
				MessageBox.Show("処理を完了しました");
			}
			catch ( Exception se )
			{
				this.SetErrorMessage(se);
			}
		}

		/// <summary>
		/// 選択されたオブジェクトに関するデータ件数を出力する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RecordCountOutPut(object sender, System.EventArgs e)
		{
			IDataReader  dr = null;
			IDbCommand cm = this.SqlDriver.NewSqlCommand();

			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}
			if( this.objectList.SelectedItems.Count > 1 &&
				this.txtWhere.Text != null &&
				this.txtWhere.Text.Trim() != "" )
			{
				if( MessageBox.Show("複数オブジェクトに同一の where 句を適用しますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
					== System.Windows.Forms.DialogResult.No )
				{
					return;
				}
			}

			if( CheckFileSpec() == false )
			{
				return;
			}

			this.InitErrMessage();

			int			rowcount = 0;
			int			trow = 0;
			try
			{

				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
					wr.WriteLine("オブジェクト名,データ件数");
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false, GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
					wr.WriteLine("オブジェクト名,データ件数");
				}


				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);

					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv.tmp",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
						wr.WriteLine("オブジェクト名,データ件数");
					}
					trow = 0;
					string stSql;
					stSql = string.Format(System.Globalization.CultureInfo.CurrentCulture,"select  count(1) from {0} ",dboInfo.GetAliasName(this.GetAlias()));
					if( this.txtWhere.Text.Trim() != "" )
					{
						stSql += " where " + this.txtWhere.Text.Trim();
					}

					cm.CommandText = stSql;

					dr = cm.ExecuteReader();

					// データの書き出し
					while (dr.Read())
					{
						rowcount++;
						trow++;
						wr.Write(dboInfo.FormalName);
						wr.Write(",");
						if( dr.IsDBNull(0) )
						{
							wr.WriteLine( "0" );
						}
						else
						{
							wr.WriteLine( dr.GetValue(0).ToString() );
						}
					}
					if( dr != null && dr.IsClosed == false )
					{
						dr.Close();
					}
					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
						File.Delete(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv");
						if( trow > 0 )
						{
							fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv\r\n");
							// ファイルをリネームする
							File.Move(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv.tmp", 
								this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv");
						}
						else
						{
							File.Delete(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv.tmp");
						}
					}
				}
				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Close();
				}
				if( rowcount == 0 )
				{
					MessageBox.Show("対象データがありませんでした");
				}
				else
				{
					if( this.rdoClipboard.Checked == true ) 
					{
						Clipboard.SetDataObject(strline.ToString(),true );
					}
					else
					{
						Clipboard.SetDataObject(fname.ToString(),true );
					}
					MessageBox.Show("処理を完了しました");
				}
			}
			catch ( System.Data.SqlClient.SqlException se )
			{
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
				return;
			}
			catch( Exception se )
			{
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
			}
			finally 
			{
				if( cm != null )
				{
					cm.Dispose();
				}
			}

			// set datas to clipboard
		}

		/// <summary>
		/// 現在接続先のDBを初期値として、クエリアナライザを起動する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CallISQLW(object sender, System.EventArgs e)
		{
			this.SqlDriver.CallISQL(this.pServerRealName,this.pInstanceName, this.pIsUseTrust, (string)this.dbList.SelectedItem,this.pLogOnUid, this.pLogOnPassword );
		}

		/// <summary>
		/// 現在接続先のDBを初期値として、クエリアナライザを起動する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CallProfile(object sender, System.EventArgs e)
		{
			this.SqlDriver.CallProfile(this.pServerRealName,this.pInstanceName, this.pIsUseTrust, (string)this.dbList.SelectedItem,this.pLogOnUid, this.pLogOnPassword );
		}

		/// <summary>
		/// エンタープライズマネージャーを起動する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CallEPM(object sender, System.EventArgs e)
		{
			this.SqlDriver.CallEPM(this.pServerRealName,this.pInstanceName, this.pIsUseTrust, (string)this.dbList.SelectedItem,this.pLogOnUid, this.pLogOnPassword );
		}

		private void menuRecordCountDisp_Click(object sender, System.EventArgs e)
		{
			IDataReader dr = null;
			IDbCommand	cm = this.SqlDriver.NewSqlCommand();

			DataTable	rcdt = new DataTable("RecordCount");
			rcdt.CaseSensitive = true;
			rcdt.Locale = System.Globalization.CultureInfo.CurrentCulture;

			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}
			if( this.objectList.SelectedItems.Count > 1 &&
				this.txtWhere.Text != null &&
				this.txtWhere.Text.Trim() != "" )
			{
				if( MessageBox.Show("複数オブジェクトに同一の where 句を適用しますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
					== System.Windows.Forms.DialogResult.No )
				{
					return;
				}
			}

			this.InitErrMessage();

			int			rowcount = 0;
			int			trow = 0;
			rcdt.Columns.Add("オブジェクト名");
			rcdt.Columns.Add("データ件数",typeof(int));

			try
			{

				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);

					trow = 0;
					string stSql;
					stSql = string.Format(System.Globalization.CultureInfo.CurrentCulture,"select  count(1) from {0} ",dboInfo.GetAliasName(this.GetAlias()));
					if( this.txtWhere.Text.Trim() != "" )
					{
						stSql += " where " + this.txtWhere.Text.Trim();
					}

					cm.CommandText = stSql;

					dr = cm.ExecuteReader();

					// データの書き出し
					while (dr.Read())
					{
						rowcount++;
						trow++;
						DataRow addrow = rcdt.NewRow();
						addrow[0] = dboInfo.FormalName;
						if( dr.IsDBNull(0) )
						{
							addrow[1] = 0;
						}
						else
						{
							addrow[1] = dr.GetValue(0);
						}
						rcdt.Rows.Add(addrow);
					}
					if( dr != null && dr.IsClosed == false )
					{
						dr.Close();
					}
				}
				if( rowcount == 0 )
				{
					MessageBox.Show("対象データがありませんでした");
				}
				else
				{
					DataGridViewBase dlg = new DataGridViewBase(rcdt,"データ件数");
					dlg.ShowDialog();
				}
			}
			catch ( System.Data.SqlClient.SqlException se )
			{
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
				return;
			}
			catch( Exception se )
			{
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
			}
			finally 
			{
				if( cm != null )
				{
					cm.Dispose();
				}
			}
		}

		/// <summary>
		/// すべての行をクリップボードにコピーする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void copyDbGridMenu_Click(object sender, System.EventArgs e)
		{
			copyDbGrid(false);
		}

		/// <summary>
		/// 選択行のみクリップボードにコピーする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void copySelectedDbGridMenu_Click(object sender, System.EventArgs e)
		{
			copyDbGrid(true);
		}

		/// <summary>
		/// 選択行のみクリップボードにコピーする
		/// </summary>
		/// <param name="isSelectOnly">選択行のみコピーするか否か</param>
		private void copyDbGrid(bool isSelectOnly)
		{
			if( this.dbGrid.Visible == false )
			{
				return;
			}
			if( this.dbGrid.DataSource == null )
			{
				return;
			}
			DataTable dt = null;
			if( this.dbGrid.DataSource is DataSet )
			{
				dt = ((DataSet)this.dbGrid.DataSource).Tables[0];
			}
			else if( this.dbGrid.DataSource is DataTable )
			{
				dt = (DataTable)this.dbGrid.DataSource;
			}
			if( dt == null ||
				dt.Rows.Count == 0 )
			{
				return;
			}
			StringBuilder strline = new StringBuilder();
			StringWriter wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
			// header 
			int cnt = 0;
			foreach( DataColumn col in dt.Columns )
			{
				if( cnt != 0 )
				{
					wr.Write("\t");
				}
				wr.Write(col.ColumnName);
				cnt++;
			}
			wr.Write(wr.NewLine);

			CurrencyManager cm = (CurrencyManager) this.BindingContext[this.dbGrid.DataSource, this.dbGrid.DataMember];

			for( int y = 0; y < cm.Count; y++ )
			{
				if( isSelectOnly == true &&
					this.dbGrid.IsSelected(y) == false )
				{
					continue;
				}
				for( int x = 0; x < dt.Columns.Count; x++ )
				{
					if( x != 0 )
					{
						wr.Write("\t");
					}
					if( this.dbGrid[y,x] != DBNull.Value )
					{
						wr.Write(this.dbGrid[y,x].ToString());
					}
				}
				wr.Write(wr.NewLine);
			}
			Clipboard.SetDataObject(strline.ToString(),true );
			MessageBox.Show("処理を完了しました");
		}

		private void allSelectDbGridMenu_Click(object sender, System.EventArgs e)
		{
			if( this.dbGrid.Visible == false )
			{
				return;
			}
			if( this.dbGrid.DataSource == null )
			{
				return;
			}
			CurrencyManager cm = (CurrencyManager) this.BindingContext[this.dbGrid.DataSource, this.dbGrid.DataMember];
			int maxCnt = cm.Count;
			this.dbGrid.SuspendLayout();
			for( int i = 0; i < maxCnt; i++ )
			{
				this.dbGrid.Select(i);
			}
			this.dbGrid.ResumeLayout();
		}

		private void allUnSelectDbGridMenu_Click(object sender, System.EventArgs e)
		{
			if( this.dbGrid.Visible == false )
			{
				return;
			}
			if( this.dbGrid.DataSource == null )
			{
				return;
			}
			CurrencyManager cm = (CurrencyManager) this.BindingContext[this.dbGrid.DataSource, this.dbGrid.DataMember];
			int maxCnt = cm.Count;
			this.dbGrid.SuspendLayout();
			for( int i = 0; i < maxCnt; i++ )
			{
				this.dbGrid.UnSelect(i);
			}
			this.dbGrid.ResumeLayout();
		}

		/// <summary>
		/// データグリッドへの CSVの取込
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void readCsvDbGridMenu_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Grid(true,false);
		}

		private void readTsvDbGridMenu_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Grid(false,false);
		}

		private void readCsvDQDbGridMenu_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Grid(true,true);
		}

		private void readTsvDQDbGridMenu_Click(object sender, System.EventArgs e)
		{
			this.LoadFile2Grid(false,true);
		}


		private void btnQuerySelect_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.InitErrMessage();

				Sqldlg.DHistory = this.selectHistory;

				if( Sqldlg.ShowDialog() == DialogResult.OK )
				{
					DbDataAdapter da = this.SqlDriver.NewDataAdapter();
					IDbCommand cmd = this.SqlDriver.NewSqlCommand(Sqldlg.SelectSql);
					this.SqlDriver.SetSelectCmd(da,cmd);

					da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
					dspdt = new DataSet();
					dspdt.CaseSensitive = true;
					dspdt.Locale = System.Globalization.CultureInfo.CurrentCulture;

					da.Fill(dspdt,"aaaa");

					//新しいDataGridTableStyleの作成
					DataGridTableStyle ts = new DataGridTableStyle();
					//マップ名を指定する
					ts.MappingName = "aaaa";

					QdbeDataGridTextBoxColumn cs;
					foreach( DataColumn col in dspdt.Tables[0].Columns )
					{
						//列スタイルにQdbeDataGridTextBoxColumnを使う
						cs = new QdbeDataGridTextBoxColumn(this.dbGrid,col, 
							GetFormat(this.NumFormat),
							GetFormat(this.FloatFormat),
							GetFormat(this.DateFormat)
							);

						//DataGridTableStyleに追加する
						ts.GridColumnStyles.Add(cs);
					}

					//オブジェクトスタイルをDataGridに追加する
					this.dbGrid.TableStyles.Clear();
					this.dbGrid.TableStyles.Add(ts);

					this.dbGrid.ReadOnly = true;
					this.btnDataEdit.BackColor = this.btnBackColor;
					this.btnDataEdit.ForeColor = this.btnForeColor;
					this.btnTmpAllDisp.BackColor = this.btnBackColor;
					this.btnTmpAllDisp.ForeColor = this.btnForeColor;
					this.btnDataEdit.Text = "データ編集(&T)";
					this.btnDataUpdate.Enabled = true;
					this.btnDataEdit.Enabled = true;
					this.btnGridFormat.Enabled = true;
					this.chkDispData.Checked = true;
					this.dbGrid.AllowSorting = true;
					this.commTooltip.SetToolTip(this.dbGrid,Sqldlg.SelectSql.Replace("\r\n"," ").Replace("\t"," "));
					this.dbGrid.SetDataBinding(dspdt,dspdt.Tables[0].TableName);
					this.dbGrid.Tag = Sqldlg.SelectSql;
					this.dbGrid.Show();
				}
			}
			catch( Exception exp)
			{
				this.SetErrorMessage(exp);
			}
		}

		private void btnQueryNonSelect_Click(object sender, System.EventArgs e)
		{
			IDbTransaction tran	= null;
			try
			{
				this.InitErrMessage();

				Sqldlg2.DHistory = this.DMLHistory;

				if( Sqldlg2.ShowDialog() == DialogResult.OK )
				{
					IDbCommand cm = this.SqlDriver.NewSqlCommand(Sqldlg2.SelectSql);
					tran = this.SqlDriver.SetTransaction(cm);

					string msg = "";
					if( Sqldlg2.HasReturn == true )
					{
						object ret = cm.ExecuteScalar();
						tran.Commit();
						msg = string.Format(System.Globalization.CultureInfo.CurrentCulture,"処理が終了しました。\r\nリターン値は [{0}] です", ret.ToString() );
					}
					else
					{
						int cnt = cm.ExecuteNonQuery();
						tran.Commit();
						msg = string.Format(System.Globalization.CultureInfo.CurrentCulture,"処理が終了しました。\r\n影響した件数は {0} 件です", cnt );
					}
					MessageBox.Show(msg);
				}
			}
			catch( Exception exp)
			{
				if( tran != null )
				{
					tran.Rollback();
				}
				this.SetErrorMessage(exp);
			}
		}

		/// <summary>
		/// 選択されたオブジェクトの統計情報を更新する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuStasticUpdate_Click(object sender, System.EventArgs e)
		{
			// SQL 的には、UPDATE STATISTICS table を実施する
			IDbCommand cm = this.SqlDriver.NewSqlCommand();

			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}
		
			this.InitErrMessage();

			try
			{

				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);

					string stSql;


					if( dboInfo.CanStatistics ==  false )
					{
						continue;
					}

					stSql = "update STATISTICS " + dboInfo.RealObjName;
					cm.CommandText = stSql;
					cm.ExecuteNonQuery();
				}
				MessageBox.Show("処理を完了しました");
			}
			catch ( System.Data.SqlClient.SqlException se )
			{
				this.SetErrorMessage(se);
			}
			catch ( Exception se )
			{
				this.SetErrorMessage(se);
			}
			finally 
			{
				if( cm != null )
				{
					cm.Dispose();
				}
			}
		}

		private void menuDoQuery_Click(object sender, System.EventArgs e)
		{
			// オブジェクト名称を引数として、各種クエリの実行を可能にする

			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}

			DbDataAdapter da = this.SqlDriver.NewDataAdapter();
			IDbCommand cm = this.SqlDriver.NewSqlCommand();
			this.SqlDriver.SetSelectCmd(da,cm);

			try
			{
				this.InitErrMessage();

				this.cmdDialog.SelectSql = " {0} ";
				this.cmdDialog.DHistory = this.cmdHistory;

				if( cmdDialog.ShowDialog() == DialogResult.OK )
				{
					DataSet	ds = new DataSet("retData");
					ds.CaseSensitive = true;
					ds.Locale = System.Globalization.CultureInfo.CurrentCulture;

					
					DBObjectInfo dboInfo;
					for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
					{
						dboInfo = this.objectList.GetSelectObject(ti);
						cm.CommandText = string.Format(System.Globalization.CultureInfo.CurrentCulture,this.cmdDialog.SelectSql,
							dboInfo.FormalName);

						if( cmdDialog.HasReturn == true )
						{
							// 戻り値あり
							this.SqlDriver.SetSelectCmd(da,cm);
							da.Fill(ds,"retdata");
						}
						else
						{
							cm.ExecuteNonQuery();
						}
					}
					if( cmdDialog.HasReturn == true )
					{
						this.dbGrid.SetDataBinding(ds,"retdata");
						this.dbGrid.Show();
						this.dbGrid.ReadOnly = true;
						this.btnDataEdit.Text = "データ編集(&T)";
						this.btnDataEdit.BackColor = this.btnBackColor;
						this.btnDataEdit.ForeColor = this.btnForeColor;
					}
					MessageBox.Show("処理を完了しました");
				}
			}
			catch( Exception exp)
			{
				this.SetErrorMessage(exp);
			}
			finally
			{
				if( cm != null )
				{
					cm.Dispose();
				}
			}
		}

		/// <summary>
		/// 選択されたオブジェクトの基礎情報を表示する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DispObjectInfo(object sender, System.EventArgs e)
		{
			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}

			DataTable	objTable = new DataTable("ObjectInfo");
			objTable.CaseSensitive = true;
			objTable.Locale = System.Globalization.CultureInfo.CurrentCulture;

			this.InitErrMessage();
			this.SqlDriver.InitObjTable(objTable);

			try
			{
				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);
					this.SqlDriver.AddObjectInfo(dboInfo,objTable);
				}

				DataGridViewBase dlg = new DataGridViewBase(objTable,"オブジェクト情報");
				dlg.ShowDialog();
			}
			catch( Exception se )
			{
				this.SetErrorMessage(se);
			}
		}

		/// <summary>
		/// 選択されたオブジェクトの基礎情報を表示する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ObjectSearch(object sender, System.EventArgs e)
		{
			this.InitErrMessage();

			SearchConditionDlg dlg = new SearchConditionDlg(this.ConnectSqlVersion);
			dlg.DHistory = this.searchHistory;
			if( dlg.ShowDialog() == DialogResult.OK )
			{
				//ここまでは条件が入力されただけ

				// ここから検索する
				DataSet ds = this.ObjectSearchSub(
					dlg.SelectSql,
					dlg.SearchType,
					dlg.IsCaseSensitive,
					dlg.IsSchemaOnly,
					dlg.IsSearchField,
					dlg.IsSearchTable,
					dlg.IsSearchView,
					dlg.IsSearchSynonym,
					dlg.IsSearchFunction,
					dlg.IsSearchProcedure
					);

				if( ds.Tables[0].Rows.Count == 0 )
				{
					MessageBox.Show("検索した結果、何も見つかりませんでした");
					return;
				}

				StringBuilder sb = new StringBuilder();
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendFormat("[{0}].[{1}]",
						dr[0].ToString(),
						dr[1].ToString()
						);
					if( dr[2] != DBNull.Value )
					{
						sb.AppendFormat("\t[{0}]",
							dr[2].ToString()
							);
					}
					sb.Append("\r\n");
				}
				Clipboard.SetDataObject(sb.ToString(),true );
				if( dlg.IsShowTableSelect == true )
				{
					this.tableSelect(sb.ToString());
				}
			}
		}

		/// <summary>
		/// 条件に応じてオブジェクトを検索する
		/// </summary>
		/// <param name="searchCondition">検索文字</param>
		/// <param name="searchType">検索条件</param>
		/// <param name="isCaseSensitive">大文字小文字を区別するか否か</param>
		/// <param name="isLimitSchema">対象スキーマを現在表示中のもののみに絞り込むか否か</param>
		/// <param name="isField">フィールドを検索するか否か</param>
		/// <param name="isTable">テーブルを検索するか否か</param>
		/// <param name="isView">Viewを検索するか否か</param>
		/// <param name="isSynonym">シノニムを検索するか否か</param>
		/// <param name="isFunction">ファンクションを検索するか否か</param>
		/// <param name="isProcedure">ストアドプロシージャーを検索するか否か</param>
		/// <returns></returns>
		private DataSet ObjectSearchSub(
			string searchCondition,
			SearchType searchType,
			bool isCaseSensitive,
			bool isLimitSchema,
			bool isField,
			bool isTable,
			bool isView,
			bool isSynonym,
			bool isFunction,
			bool isProcedure
			)
		{
			DbDataAdapter da = this.SqlDriver.NewDataAdapter();
			IDbCommand cm = this.SqlDriver.NewSqlCommand();
			this.SqlDriver.SetSelectCmd(da,cm);


			DataSet ds = new DataSet();
			ds.CaseSensitive = true;
			ds.Locale = System.Globalization.CultureInfo.CurrentCulture;
			da.MissingSchemaAction = MissingSchemaAction.Ignore;

			ds.Tables.Add("SearchResult");
			ds.Tables["SearchResult"].Columns.Add("UserName");
			ds.Tables["SearchResult"].Columns.Add("ObjName");
			ds.Tables["SearchResult"].Columns.Add("FieldName");

			List<string> limitSchema = new List<string>();
			if( isLimitSchema == true )
			{
				foreach( string itm in this.ownerListbox.SelectedItems )
				{
					if( itm.ToString() == "全て")
					{
						limitSchema.Clear();
						break;
					}
					limitSchema.Add(itm.ToString());
				}
			}

			if( isField == true )
			{
				cm.CommandText = this.SqlDriver.GetSearchFieldSql(searchCondition,searchType, isCaseSensitive,limitSchema);
				da.Fill(ds, "SearchResult");
			}

			if (isTable == true || isView == true || isSynonym == true || isFunction == true || isProcedure == true)
			{
				cm.CommandText = this.SqlDriver.GetSearchObjectSql(searchCondition,searchType, isCaseSensitive,limitSchema, isTable,isView,isSynonym,isFunction,isProcedure);
				da.Fill(ds, "SearchResult");
			}

			return ds;
		}

		private void fieldListbox_ExtendedCopyData(object sender, System.EventArgs e)
		{
			// フィールド一覧で Ctrl + F が押下された場合の処理
			// 別ダイアログを表示してエイリアス等の指定を可能にする
			if( this.objectList.SelectedItems.Count != 1 )
			{
				return;
			}

			FieldGetDialog dlg = new FieldGetDialog();
			dlg.BaseTableName = this.objectList.GetSelectOneObjectName();
			if( dlg.ShowDialog(this) != DialogResult.OK )
			{
				return;
			}
			StringBuilder str = new StringBuilder();
			for( int i=0; i < this.fieldListbox.SelectedItems.Count; i++ )
			{
				if( i != 0 )
				{
					if( dlg.RetCrlf == true )
					{
						if( dlg.RetComma ) 
						{
							str.Append(",\r\n");
						}
						else
						{
							str.Append("\r\n");
						}
					}
					else
					{
						if( dlg.RetComma )
						{
							str.Append(",");
						}
						else
						{
							str.Append("\t");
						}
					}
				}
				str.Append(dlg.RetTableAccessor+".");
				str.Append((string)this.fieldListbox.SelectedItems[i].ToString());
			}
			if( str.Length != 0 )
			{
				Clipboard.SetDataObject(str.ToString(),true );
			}		
		}

		private void label11_DoubleClick(object sender, System.EventArgs e)
		{
			if( this.objectList.SelectedItems.Count != 1 )
			{
				return;
			}
			this.txtAlias.Text = this.objectList.GetSelectOneObjectName();
		
		}


		/// <summary>
		/// DBリストのコピーメニュー選択時処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dbList_CopyData(object sender, System.EventArgs e)
		{
			if( this.dbList.SelectedItems.Count > 0 )
			{
				StringBuilder strline =  new StringBuilder();
				foreach( string name in dbList.SelectedItems )
				{
					if( strline.Length != 0 )
					{
						strline.Append(",");
						strline.Append("\r\n");
					}
					strline.Append(name);
				}
				Clipboard.SetDataObject(strline.ToString(),true );
			}
		}

		/// <summary>
		/// Owner/Role/Schemaのコピーメニュー選択時処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ownerListbox_CopyData(object sender, System.EventArgs e)
		{
			// Owner/Role/Schema名のコピー
			if( this.ownerListbox.SelectedItems.Count > 0 )
			{
				StringBuilder strline =  new StringBuilder();
				foreach( string name in ownerListbox.SelectedItems )
				{
					if( strline.Length != 0 )
					{
						strline.Append(",");
						strline.Append("\r\n");
					}
					strline.Append(name);
				}
				Clipboard.SetDataObject(strline.ToString(),true );
			}
		}

		private void objectList_CopyData(object sender, System.EventArgs e)
		{
			CopyTableName(false);
		}

		private void cmbHistory_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if( this.cmbHistory.SelectedIndex < 0 )
			{
				return;
			}
			string tablename = (string)this.cmbHistory.SelectedItem;

			int setidx = this.objectList.FindStringExact(tablename);
            if (setidx < 0)
            {
                return;
            }
            isInCmbEvent = true;
            this.objectList.ClearSelected();
			this.objectList.Items[setidx].Selected = true;
			this.objectList.EnsureVisible(setidx);
			isInCmbEvent = false;
			this.objectList.Focus();
		}



		private void DispFldList(DBObjectInfo dboInfo)
		{
			try
			{
				this.fieldListbox.Items.Clear();
				if( dboInfo == null )
				{
					return;
				}

				bool	dodsp;
				if( this.chkDispFieldAttr.Checked == true )
				{
					dodsp = true;
				}
				else
				{
					dodsp = false;
				}

				string	istr;

				foreach(DBFieldInfo fi in dboInfo.FieldInfo)
				{
					if( dodsp == false )
					{
						istr = fi.Name + " ";
					}
					else
					{
                        istr = fi.Name + fi.GetFieldTypeString();
					}
					this.fieldListbox.Items.Add(new FieldListItem(istr,fi));
				}
			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
			}
		}

		/// <summary>
		/// オブジェクト一覧の表示
		/// </summary>
		private void DispObjectList()
		{
			IDataReader dr = null;
			IDbCommand cm = this.SqlDriver.NewSqlCommand();

			this.InitErrMessage();

			try 
			{
				if( this.dbList.SelectedItem == null )
				{
					return ;
				}
				this.SqlDriver.SetDatabase((String)this.dbList.SelectedItem);
				
				// listbox2 にオブジェクト一覧を表示

				string sortkey;
				if( this.rdoSortTable.Checked == true )
				{
					sortkey = " order by 1 ";
				}
				else
				{
					sortkey = " order by 2,1 ";
				}

				string ownerlist = "";
				if( this.ownerListbox.SelectedItem != null )
				{
					// 選択があれば、そのOWNERのみのオブジェクトを表示する
					foreach( String owname in this.ownerListbox.SelectedItems )
					{
						if( owname == "全て" )
						{
							ownerlist = "";
							break;
						}
						if( ownerlist != "" )
						{
							ownerlist += ",";
						}
						ownerlist += "'" + owname + "'";
					}
				}
				cm.CommandText = this.SqlDriver.GetDisplayObjList(
					true,
					this.rdoDispView.Checked,
					true,
					false,
					false,
					ownerlist
					);

				cm.CommandText += sortkey;

				dr = cm.ExecuteReader();

                this.objectList.SetObjectListItem(dr, this.SqlDriver);

				dr.Close();
			}
			catch ( System.Data.SqlClient.SqlException se )
			{
				if( dr != null )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
			}
			catch ( Exception se )
			{
				if( dr != null )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
			}
			finally 
			{
				cm.Dispose();
			}
		
		}

		private void DispListOwner()
		{
			IDataReader dr = null;
			IDbCommand 	cm = this.SqlDriver.NewSqlCommand();

			this.InitErrMessage();

			try 
			{
				cm.CommandText = this.SqlDriver.GetOwnerList(rdoDispSysUser.Checked);

				dr = cm.ExecuteReader();

				this.ownerListbox.Items.Clear();
				this.ownerListbox.Items.Add("全て");
				while ( dr.Read())
				{
					this.ownerListbox.Items.Add(dr["name"]);
				}
				dr.Close();
			
			}
			catch ( Exception se )
			{
				if( dr != null )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
			}
			finally 
			{
				cm.Dispose();
			}
		}


		private void CreInsert(bool fieldlst, bool deletefrom, bool isTaihi)
		{
			try
			{
				this.InitErrMessage();
				// insert 文の作成
				if( this.objectList.SelectedItems.Count == 0 )
				{
					return;
				}
				if( this.objectList.SelectedItems.Count > 1 &&
					this.txtWhere.Text != null &&
					this.txtWhere.Text.Trim() != "" )
				{
					if( MessageBox.Show("複数オブジェクトに同一の where 句を適用しますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
						== System.Windows.Forms.DialogResult.No )
					{
						return;
					}
				}
				if( this.objectList.SelectedItems.Count > 1 &&
					this.txtSort.Text != null &&
					this.txtSort.Text.Trim() != "" )
				{
					if( MessageBox.Show("複数オブジェクトに同一の order by 句を適用しますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
						== System.Windows.Forms.DialogResult.No )
					{
						return;
					}
				}
			
				if( CheckFileSpec() == false )
				{
					return;
				}
			
				int			rowcount = 0;
				int			trow	= 0;
				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false,GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
				}
				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Write("SET NOCOUNT ON{0}GO{0}{0}",wr.NewLine);
				}

				IDataReader dr = null;
				IDbCommand 	cm = this.SqlDriver.NewSqlCommand();

				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);
					
					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql.tmp",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
						wr.Write("SET NOCOUNT ON{0}GO{0}{0}",wr.NewLine);
					}

					// get id 
					string stSql;
					if( dboInfo.IsUseAssemblyType == true )
					{
						// アセンブリを利用している場合、フィールド名＋.ToString()をつける必要あり
						StringBuilder fieldStr = new StringBuilder();
						fieldStr.Append("select ");
						int loop = 0;
						foreach(DBFieldInfo fi in dboInfo.FieldInfo)
						{
							if( loop != 0 )
							{
								fieldStr.Append(",");
							}
							fieldStr.Append(fi.Name);
							if( fi.IsAssembly == true )
							{
								fieldStr.Append(".ToString() as ").Append(fi.Name);
							}
							loop++;
						}
						fieldStr.AppendFormat(" from {0} ",dboInfo.GetAliasName(this.GetAlias()));
						stSql = fieldStr.ToString();
					}
					else
					{
						stSql = string.Format(System.Globalization.CultureInfo.CurrentCulture,"select  * from {0} ",dboInfo.GetAliasName(this.GetAlias()));
					}
					if( this.txtWhere.Text.Trim() != "" )
					{
						stSql += " where " + this.txtWhere.Text.Trim();
					}
					if( this.txtSort.Text.Trim() != "" )
					{
						stSql += " order by " + this.txtSort.Text.Trim();
					}
					cm.CommandText = stSql;

					dr = cm.ExecuteReader();

					List<string> fldname = new List<string>();
					List<Type> strint = new List<Type>();
					int			maxcol;
	
					fldname.Clear();
					strint.Clear();

					maxcol = dr.FieldCount;
					for( int j=0 ; j < maxcol; j++ )
					{
						fldname.Add( dr.GetName(j) );
						strint.Add( dr.GetFieldType(j) );
					}

					bool IsIdentity = false;
					if( dboInfo.IsUseIdentity == true )
					{
						// Identity 列がある場合、SET IDENTITY_INSERT table on をつける
						string addidinsert = string.Format(System.Globalization.CultureInfo.CurrentCulture,"SET IDENTITY_INSERT {0} on ",dboInfo.FormalName);
						wr.WriteLine(addidinsert);
						wr.Write(wr.NewLine);
						IsIdentity = true;
					}

					if( isTaihi == true )
					{
						string taihistr = 
							String.Format(System.Globalization.CultureInfo.CurrentCulture,"select * into {1} from {0} ",
							dboInfo.GetAliasName(this.GetAlias()),
							dboInfo.GetNameAdd(DateTime.Now.ToString("yyyyMMdd",System.Globalization.CultureInfo.CurrentCulture))
							);
						if( this.txtWhere.Text.Trim() != "" )
						{
							taihistr += string.Format(System.Globalization.CultureInfo.CurrentCulture," where {0}", this.txtWhere.Text.Trim() );
						}
						wr.Write("{0}GO{0}",wr.NewLine );
						wr.Write(taihistr);
						wr.Write("{0}GO{0}",wr.NewLine );

					}

					trow	= 0;
					string flds = "";
					if( fieldlst == true )
					{
						StringBuilder sb = new StringBuilder();
						sb.Append(" ( ");
						for( int i = 0 ; i < maxcol; i++ )
						{
							if( i != 0 )
							{
								sb.Append(",");
							}
							sb.Append("[").Append(fldname[i]).Append("]");
						}
						sb.Append( " ) " );
						flds = sb.ToString();
					}
					while(dr.Read())
					{
						if( deletefrom == true && trow == 0)
						{
							wr.Write("delete from  ");
							wr.Write(dboInfo.FormalName);
							if( this.txtWhere.Text.Trim() != "" )
							{
								wr.Write( " where {0}", this.txtWhere.Text.Trim() );

							}
							wr.Write("{0}GO{0}",wr.NewLine );
						}
						if( trow != 0 && ( trow % 1000 == 0 ) )
						{
							wr.Write("GO{0}",wr.NewLine);
						}
						trow++;
						rowcount ++;
						wr.Write("insert into {0} {1} values ( ", dboInfo.FormalName, flds );

                        int i = 0 ;
                        foreach(DBFieldInfo each in dboInfo.FieldInfo)
                        {
							if( i != 0 )
							{
								wr.Write( ", " );
							}
							wr.Write(each.ConvData(dr, i, "'","N",true));
                            i++;
						}
						wr.Write( " ) {0}",wr.NewLine );
					}
					if( trow > 0 )
					{
						wr.Write("GO{0}{0}",wr.NewLine );
					}
					if( IsIdentity == true )
					{
						// Identity 列がある場合、SET IDENTITY_INSERT table off をつける
						string addidinsert = string.Format(System.Globalization.CultureInfo.CurrentCulture,"SET IDENTITY_INSERT {0} off",dboInfo.FormalName);
						wr.WriteLine(addidinsert);
						wr.WriteLine("GO");
						wr.Write(wr.NewLine);
					}

					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
						File.Delete(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql");
						if( trow > 0 )
						{
							fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql\r\n");
							// ファイルをリネームする
							File.Move(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql.tmp", 
								this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql");
						}
						else
						{
							File.Delete(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql.tmp");
						}

					}
					if( dr != null && dr.IsClosed == false )
					{
						dr.Close();
					}
				}
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}


				// set datas to clipboard
				if( rowcount == 0 )
				{
					MessageBox.Show("対象データがありませんでした");
				}
				else
				{
					if( this.rdoOutFolder.Checked == false ) 
					{
						wr.Close();
					}
					if( this.rdoClipboard.Checked == true ) 
					{
						Clipboard.SetDataObject(strline.ToString(),true );
					}
					else
					{
						Clipboard.SetDataObject(fname.ToString(),true );
					}
					MessageBox.Show("処理を完了しました");
				}
			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
			}
		}

		private void CreateFldList(bool isLF, bool iscomma)
		{
			try
			{
				this.InitErrMessage();

				if( this.objectList.SelectedItems.Count == 0 )
				{
					return;
				}

				if( CheckFileSpec() == false )
				{
					return;
				}

				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false, GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
				}

				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);

					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
						fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql\r\n");
					}

					// get id 
					wr.Write(dboInfo.FormalName);
					wr.Write(":");
					string	alias = this.GetAlias();
                    int i = 0;
                    foreach( DBFieldInfo each in dboInfo.FieldInfo)
					{
						if( i != 0 && iscomma )
						{
							wr.Write(",");
						}
						if( isLF )
						{
							wr.Write("{0}\t",wr.NewLine);
						}

						if( alias != string.Empty )
						{
							wr.Write(alias + "." );
						}
						wr.Write(each.Name);
                        i++;
					}
					wr.Write(wr.NewLine);

					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
					}
				}

				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Close();
				}
				if( this.rdoClipboard.Checked == true ) 
				{
					Clipboard.SetDataObject(strline.ToString(),true );
				}
				else
				{
					Clipboard.SetDataObject(fname.ToString(),true );
				}

				MessageBox.Show( "処理を終了しました" );
			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
			}
		}

		// CSVもしくはTSVを生成する
		private void CreateTCsvText(bool isdquote, string separater)
		{
			IDataReader dr = null;
			IDbCommand 	cm = this.SqlDriver.NewSqlCommand();

			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}
			if( this.objectList.SelectedItems.Count > 1 &&
				this.txtWhere.Text != null &&
				this.txtWhere.Text.Trim() != "" )
			{
				if( MessageBox.Show("複数オブジェクトに同一の where 句を適用しますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
					== System.Windows.Forms.DialogResult.No )
				{
					return;
				}
			}
			if( this.objectList.SelectedItems.Count > 1 &&
				this.txtSort.Text != null &&
				this.txtSort.Text.Trim() != "" )
			{
				if( MessageBox.Show("複数オブジェクトに同一の order by 句を適用しますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
					== System.Windows.Forms.DialogResult.No )
				{
					return;
				}
			}

			if( CheckFileSpec() == false )
			{
				return;
			}

			this.InitErrMessage();

			int			rowcount = 0;
			int			trow = 0;
			try
			{

				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false, GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
				}

				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);

					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv.tmp",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
					}
					trow = 0;
					string stSql;
					if( dboInfo.IsUseAssemblyType == true )
					{
						// アセンブリを利用している場合、フィールド名＋.ToString()をつける必要あり
						StringBuilder fieldStr = new StringBuilder();
						fieldStr.Append("select ");
						int loop = 0;
						foreach(DBFieldInfo fi in dboInfo.FieldInfo)
						{
							if( loop != 0 )
							{
								fieldStr.Append(",");
							}
							fieldStr.Append(fi.Name);
							if( fi.IsAssembly == true )
							{
								fieldStr.Append(".ToString() as ").Append(fi.Name);
							}
							loop++;
						}
						fieldStr.AppendFormat(" from {0} ",dboInfo.GetAliasName(this.GetAlias()));
						stSql = fieldStr.ToString();
					}
					else
					{
						stSql = string.Format(System.Globalization.CultureInfo.CurrentCulture,"select  * from {0} ",dboInfo.GetAliasName(this.GetAlias()));
					}
					if( this.txtWhere.Text.Trim() != "" )
					{
						stSql += " where " + this.txtWhere.Text.Trim();
					}
					if( this.txtSort.Text.Trim() != "" )
					{
						stSql += " order by " + this.txtSort.Text.Trim();
					}
					cm.CommandText = stSql;

					dr = cm.ExecuteReader();

					List<string> fldname = new List<string>();
					int			maxcol;
	
					fldname.Clear();

					maxcol = dr.FieldCount;
					for( int j=0 ; j < maxcol; j++ )
					{
						fldname.Add( dr.GetName(j) );
					}

					// 先頭行は列見出し
					for( int i = 0 ; i < maxcol; i++ )
					{
						if( i != 0 )
						{
							wr.Write(separater);
						}
						wr.Write( fldname[i] );
					}
					wr.Write( wr.NewLine );

					// データの書き出し
					while (dr.Read())
					{
						rowcount++;
						trow++;
                        int i = 0;
                        foreach(DBFieldInfo each in dboInfo.FieldInfo)
                        {
							if( i != 0 )
							{
								wr.Write( separater );
							}
							if ( isdquote == false )
							{
								wr.Write(each.ConvData(dr, i, "","",false));
							}
							else
							{
								wr.Write(each.ConvData(dr, i, "\"","",false));
							}
                            i++;
						}
						wr.Write( wr.NewLine );
					}
					if( dr != null && dr.IsClosed == false )
					{
						dr.Close();
					}
					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
						File.Delete(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv");
						if( trow > 0 )
						{
							fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv\r\n");
							// ファイルをリネームする
							File.Move(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv.tmp", 
								this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv");
						}
						else
						{
							File.Delete(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".csv.tmp");
						}
					}
				}
				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Close();
				}
				if( rowcount == 0 )
				{
					MessageBox.Show("対象データがありませんでした");
				}
				else
				{
					if( this.rdoClipboard.Checked == true ) 
					{
						Clipboard.SetDataObject(strline.ToString(),true );
					}
					else
					{
						Clipboard.SetDataObject(fname.ToString(),true );
					}
					MessageBox.Show("処理を完了しました");
				}
			}
			catch ( System.Data.SqlClient.SqlException se )
			{
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
				return;
			}
			catch( Exception se )
			{
				if( dr != null && dr.IsClosed == false )
				{
					dr.Close();
				}
				this.SetErrorMessage(se);
			}
			finally 
			{
				if( cm != null )
				{
					cm.Dispose();
				}
			}

			// set datas to clipboard
		}

		/// <summary>
		/// 現在の画面上のDB、Owner から、オブジェクト一覧を表示する
		/// </summary>
		private void CreDDL(bool bDrop, bool useParentheses)
		{	
			if( this.objectList.SelectedItems.Count == 0 )
			{
				return;
			}
			if( CheckFileSpec() == false )
			{
				return;
			}
			
			this.InitErrMessage();

			try
			{
				StringBuilder strline =  new StringBuilder();
				TextWriter	wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				StringBuilder fname = new StringBuilder();

				if( this.rdoClipboard.Checked == true) 
				{
					wr = new StringWriter(strline,System.Globalization.CultureInfo.CurrentCulture);
				}
				else if( this.rdoOutFile.Checked == true ) 
				{
					StreamWriter sw = new StreamWriter(this.txtOutput.Text,false, GetEncode());
					sw.AutoFlush = false;
					wr = sw;
					fname.Append(this.txtOutput.Text);
				}


				DBObjectInfo	dboInfo;
				for( int ti = 0; ti < this.objectList.SelectedItems.Count; ti++ )
				{
					dboInfo = this.objectList.GetSelectObject(ti);

					if( this.rdoOutFolder.Checked == true ) 
					{
						StreamWriter sw = new StreamWriter(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql",false, GetEncode());
						sw.AutoFlush = false;
						wr = sw;
						fname.Append(this.txtOutput.Text + "\\" + dboInfo.ToString() + ".sql\r\n");
					}

					if( dboInfo.IsSynonym )
					{
						// Synonym 
						if( bDrop )
						{
							wr.Write( "DROP SYNONYM " );
							wr.Write("{0}{1}", dboInfo.FormalName,wr.NewLine);
							wr.Write( "GO{0}",wr.NewLine);
						}
						wr.Write( string.Format(System.Globalization.CultureInfo.CurrentCulture,"create synonym {0} for {1}",
							dboInfo.FormalName,
							dboInfo.SynonymBase )
							);
						wr.Write("{0}Go{0}{0}",wr.NewLine);

					}

					if( bDrop )
					{
						wr.Write(this.SqlDriver.GetDDLDropStr(dboInfo));
					}

					wr.Write(this.SqlDriver.GetDdlCreateString(dboInfo, useParentheses));

					if( this.rdoOutFolder.Checked == true ) 
					{
						wr.Close();
					}
				}
				if( this.rdoOutFolder.Checked == false ) 
				{
					wr.Close();
				}
				if( this.rdoClipboard.Checked == true ) 
				{
					Clipboard.SetDataObject(strline.ToString(),true );
				}
				else
				{
					Clipboard.SetDataObject(fname.ToString(),true );
				}
				MessageBox.Show("処理を完了しました");
			}
			catch ( Exception se )
			{
				this.SetErrorMessage(se);
			}
		}

		/// <summary>
		/// 指定されたオブジェクトの情報を表示する
		/// </summary>
		protected void DispData(DBObjectInfo dboInfo)
		{
			DispData(dboInfo,false,false);
		}
		
		/// <summary>
		/// 指定されたオブジェクトの情報を表示する
		/// </summary>
		/// <param name="dboInfo">表示するオブジェクトの情報</param>
		/// <param name="isAllDisp">全て表示するか否かの指定
		/// true: 全て表示する
		/// false; 全て表示しない</param>
		/// <param name="usePreSql">前回表示時のクエリの結果をもう一度取得しなおすか否か</param>
		protected void DispData(DBObjectInfo dboInfo, bool isAllDisp, bool usePreSql)
		{
			try
			{
				this.InitErrMessage();

				// 編集中の可能性があるので、これをキャンセルする
				DataGridCell curcell = this.dbGrid.CurrentCell;
				//int rownum = curcell.RowNumber;
				int colnum  = curcell.ColumnNumber;
				DataSet ds = (DataSet)this.dbGrid.DataSource;
				if( ds != null )
				{
					if( ds.Tables[0].Rows.Count > 0 )
					{
						((QdbeDataGridTextBoxColumn)this.dbGrid.TableStyles[0].GridColumnStyles[colnum]).CancelEdit();
					}
				}

				// オブジェクト名が指定されていない場合は何も表示せず、グリッドを隠す
				if (( dboInfo == null && usePreSql == false ) ||
					usePreSql == true && this.dbGrid.Tag == null )
				{
					this.dbGrid.Hide();
					this.dbGrid.Tag = null;
					this.btnDataUpdate.Enabled = false;
					this.btnDataEdit.Enabled = false;
					this.btnGridFormat.Enabled = false;
					return;
				}
				// データの内容を取得し、表示する
				string stSql;
				string stSqlDisp;

				int maxlines;
				int maxGetLines;

				ProcCondition procCond = null;
				if (usePreSql == true)
				{
					procCond = GetProcCondition(null);
				}
				else
				{
					procCond = GetProcCondition(dboInfo.FormalName);
				}
				procCond.IsAllDisp = isAllDisp;

				maxlines = procCond.MaxCount;

				if (procCond.IsAllDisp == true)
				{
					maxlines = 0;
				}

				stSql = "select ";
				stSqlDisp = "select ";

				maxGetLines = 0;
				if (maxlines != 0)
				{
					maxGetLines = maxlines + 1;
					stSql += " TOP " + maxGetLines.ToString(System.Globalization.CultureInfo.CurrentCulture);
					stSqlDisp += " TOP " + maxlines.ToString(System.Globalization.CultureInfo.CurrentCulture);
				}

				if (dboInfo != null)
				{
					stSql += string.Format(System.Globalization.CultureInfo.CurrentCulture, " * from {0}", dboInfo.GetAliasName(procCond.AliasStr));
					stSqlDisp += string.Format(System.Globalization.CultureInfo.CurrentCulture, " * from {0}", dboInfo.GetAliasName(procCond.AliasStr));
				}
				if (procCond.WhereStr != "")
				{
					stSql += " where " + procCond.WhereStr;
					stSqlDisp += " where " + procCond.WhereStr;
				}
				if (procCond.OrderStr != "")
				{
					stSql += " order by " + procCond.OrderStr;
					stSqlDisp += " order by " + procCond.OrderStr;
				}
				DbDataAdapter da = this.SqlDriver.NewDataAdapter();
				IDbCommand cm = null;
				if (usePreSql == true)
				{
					cm = this.SqlDriver.NewSqlCommand((string)this.dbGrid.Tag);
				}
				else
				{
					cm = this.SqlDriver.NewSqlCommand(stSql);
				}
				this.SqlDriver.SetSelectCmd(da,cm);

				dspdt = new DataSet();
				dspdt.CaseSensitive = true;
				dspdt.Locale = System.Globalization.CultureInfo.CurrentCulture;
				da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				da.Fill(dspdt, "aaaa");

				if( maxlines != 0 )
				{
					if( dspdt.Tables[0].Rows.Count == maxGetLines )
					{
						this.btnTmpAllDisp.Enabled = true;
						dspdt.Tables[0].Rows[maxGetLines-1].Delete();
						dspdt.Tables[0].AcceptChanges();
					}
					else
					{
						// 取得したデータが表示件数に満たない為、一時的に全データを表示(&A) ボタンは無効でよい
						this.btnTmpAllDisp.Enabled = false;
					}
				}
				else
				{
					// 全件表示する為、一時的に全データを表示(&A) ボタンは無効でよい
					this.btnTmpAllDisp.Enabled = false;
				}

				//新しいDataGridTableStyleの作成
				DataGridTableStyle ts = new DataGridTableStyle();
				//マップ名を指定する
				ts.MappingName = "aaaa";

				QdbeDataGridTextBoxColumn cs;
				foreach( DataColumn col in dspdt.Tables[0].Columns )
				{
					//列スタイルにQdbeDataGridTextBoxColumnを使う
					cs = new QdbeDataGridTextBoxColumn(this.dbGrid,col, 
						GetFormat(this.NumFormat),
						GetFormat(this.FloatFormat),
						GetFormat(this.DateFormat)
						);
					
					//DataGridTableStyleに追加する
					ts.GridColumnStyles.Add(cs);
				}

				//テーブルスタイルをDataGridに追加する
				this.dbGrid.TableStyles.Clear();
				this.dbGrid.TableStyles.Add(ts);



				this.dbGrid.AllowSorting = true;
				this.dbGrid.SetDataBinding(dspdt, dspdt.Tables[0].TableName);
				if (usePreSql == false)
				{
					this.commTooltip.SetToolTip(this.dbGrid, stSqlDisp);
					this.dbGrid.Tag = stSql;
				}
				this.dbGrid.Show();
				this.btnDataEdit.Text = "データ編集(&T)";
				if( dspdt.Tables[0].PrimaryKey.Length == 0 )
				{
					// Primary Key の設定がないと編集できない
					this.btnDataUpdate.Enabled = false;
					this.btnDataEdit.Enabled = false;
				}
				else
				{
					this.btnDataUpdate.Enabled = true;
					this.btnDataEdit.Enabled = true;
				}
				this.dbGrid.ReadOnly = true;
				this.btnDataEdit.BackColor = this.btnBackColor;
				this.btnDataEdit.ForeColor = this.btnForeColor;
				this.btnTmpAllDisp.BackColor = this.btnBackColor;
				this.btnTmpAllDisp.ForeColor = this.btnForeColor;
				this.btnGridFormat.Enabled = true;
			}
			catch( Exception exp)
			{
				this.SetErrorMessage(exp);
			}
		}

		private void CopyTableName(bool addcomma)
		{
			if( this.objectList.SelectedItems.Count > 0 )
			{
				StringBuilder strline =  new StringBuilder();

				string name = "";
				for( int i = 0; i < this.objectList.SelectedItems.Count; i++ )
				{
					name = this.objectList.GetSelectObjectName(i);

					if( strline.Length != 0 )
					{
						if( addcomma )
						{
							strline.Append(",");
						}
						strline.Append("\r\n");
					}
					strline.Append(name);
				}
				Clipboard.SetDataObject(strline.ToString(),true );
			}
		}

		/// <summary>
		/// フィールドリスト情報のコピー
		/// </summary>
		/// <param name="lf">改行をつけるか否か</param>
		/// <param name="docomma">カンマをつけるか否か</param>
		protected void CopyFldList(bool lf, bool docomma)
		{
			StringBuilder str = new StringBuilder();
			for( int i=0; i < this.fieldListbox.SelectedItems.Count; i++ )
			{
				if( i != 0 )
				{
					if( lf == true )
					{
						if( docomma ) 
						{
							str.Append(",\r\n");
						}
						else
						{
							str.Append("\r\n");
						}
					}
					else
					{
						if( docomma )
						{
							str.Append(",");
						}
						else
						{
							str.Append("\t");
						}
					}
				}
				str.Append((string)this.fieldListbox.SelectedItems[i].ToString());
			}
			if( str.Length != 0 )
			{
				Clipboard.SetDataObject(str.ToString(),true );
			}
		}

		/// <summary>
		/// ファイルからデータを読み込みDBへ登録する
		/// CSV,TSVの指定、また ”の利用を指定可能
		/// </summary>
		/// <param name="isCsv">true: CSVでの読み込み false : TSV での読み込み</param>
		/// <param name="isUseDQ">文字列にダブルクォートをつけているか
		/// true: 付加されている false: 付加されていない</param>
		protected void  LoadFile2Data( bool isCsv, bool isUseDQ )
		{
			if( this.objectList.SelectedItems.Count != 1 )
			{
				MessageBox.Show("対象オブジェクトは単独で指定してください");
				return;
			}

			DBObjectInfo dboInfo = this.objectList.GetSelectObject(0);
			if (dboInfo.IsUseAssemblyType == true)
			{
				MessageBox.Show("CLR型の列を含むオブジェクトは指定できません");
				return;
			}

			DbDataAdapter da = this.SqlDriver.NewDataAdapter();
			IDbCommand cm = this.SqlDriver.NewSqlCommand();
			this.SqlDriver.SetSelectCmd(da,cm);
			IDbTransaction tran	= null;
			TextReader	wr = null;

			try
			{
				this.InitErrMessage();
				// insert 文の作成
				if( ( this.txtWhere.Text != null &&
					this.txtWhere.Text.Trim() != "" ) ||
					( this.txtSort.Text != null &&
					this.txtSort.Text.Trim() != "" ) )
				{
					if( MessageBox.Show("データ取込の場合、where, order by は無視されます","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
						== System.Windows.Forms.DialogResult.No )
					{
						return;
					}
				}
			
				if( MessageBox.Show("クリップボードから読み込みますか?","確認",System.Windows.Forms.MessageBoxButtons.YesNo) ==
					DialogResult.Yes)
				{
					object clipobj = Clipboard.GetDataObject().GetData(typeof(System.String));
					if (clipobj == null)
					{
						MessageBox.Show("クリップボードには読み込み可能な値がありません");
						return;
					}
					String str = clipobj.ToString();
					wr = new StringReader(str);
				}
				else
				{
					this.openFileDialog1.CheckFileExists = true;
					this.openFileDialog1.CheckPathExists = true;
					this.openFileDialog1.Filter = "csv|*.csv|txt|*.txt|全て|*.*";
					this.openFileDialog1.Multiselect = false;
					this.openFileDialog1.RestoreDirectory = false;
					if( this.openFileDialog1.ShowDialog(this) != DialogResult.OK )
					{
						return;
					}
					Stream fsw = this.openFileDialog1.OpenFile();
			
					wr = new StreamReader(fsw,true);
				}


				// get id 
				string stSql;
				stSql = string.Format(System.Globalization.CultureInfo.CurrentCulture,"select  * from {0} ",dboInfo.GetAliasName(this.GetAlias()) );
				cm.CommandText = stSql;

				this.SqlDriver.SetSelectCmd(da,cm);
                DataTable dt = new DataTable();
                dt.CaseSensitive = true;
				dt.Locale = System.Globalization.CultureInfo.CurrentCulture;
				da.FillSchema(dt,SchemaType.Mapped);

				if( dt.Columns.Count == 0 )
				{
					return ;
				}

                ArrayList drAr = this.LoadFile2DataTable(dt, dboInfo, wr, isCsv, isUseDQ);

				// データベースへ更新する
				if( drAr.Count > 0 )
				{
					foreach(DataRow addRow in drAr)
					{
						dt.Rows.Add(addRow);
					}
					if( MessageBox.Show(drAr.Count.ToString(System.Globalization.CultureInfo.CurrentCulture) + "件のデータを読み込みますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) == DialogResult.Yes )
					{

						DbDataAdapter uda = this.SqlDriver.NewDataAdapter();
						IDbCommand ucm = this.SqlDriver.NewSqlCommand(stSql);
						this.SqlDriver.SetSelectCmd(uda,ucm);
										
						tran = this.SqlDriver.SetTransaction(ucm);
						this.SqlDriver.SetCommandBuilder(uda);
						uda.Update(dt);
						tran.Commit();

						MessageBox.Show("読込を完了しました");
					}
				}

			}
			catch( Exception exp )
			{
				if( tran != null )
				{
					tran.Rollback();
				}
				this.SetErrorMessage(exp);
			}
			finally
			{
				if( wr != null )
				{
					wr.Close();
				}
			}
		}

		/// <summary>
		/// ファイルからデータを読み込みDBへ登録する
		/// CSV,TSVの指定、また ”の利用を指定可能
		/// </summary>
		/// <param name="isCsv">true: CSVでの読み込み false : TSV での読み込み</param>
		/// <param name="isUseDQ">文字列にダブルクォートをつけているか
		/// true: 付加されている false: 付加されていない</param>
		protected void  LoadFile2Grid( bool isCsv, bool isUseDQ )
		{
			if( this.dbGrid.Visible != true )
			{
				return;
			}
			if( this.dbGrid.ReadOnly == true )
			{
				return;
			}
			if( this.dbGrid.DataSource == null )
			{
				return;
			}
			#region CLR型対応暫定処置
			// 現時点では　グリッドへはクエリ実行時には必ず ReadOnly になっているのが前提
			// であるため、ここへ来るのは テーブル値の表示時のみという想定である
			// クエリ実行時にも編集可能とする場合には
			// 当機能も同様に対応しなければならない

			DBObjectInfo dboInfo = this.objectList.GetSelectObject(0);
			if (dboInfo.IsUseAssemblyType == true)
			{
				MessageBox.Show("CLR型の列を含むオブジェクトは指定できません");
				return;
			}
			#endregion

			TextReader	wr = null;

			try
			{
				this.InitErrMessage();
				// insert 文の作成
				if( ( this.txtWhere.Text != null &&
					this.txtWhere.Text.Trim() != "" ) ||
					( this.txtSort.Text != null &&
					this.txtSort.Text.Trim() != "" ) )
				{
					if( MessageBox.Show("データ取込の場合、where, order by は無視されます","確認",System.Windows.Forms.MessageBoxButtons.YesNo) 
						== System.Windows.Forms.DialogResult.No )
					{
						return;
					}
				}
			
				if( MessageBox.Show("クリップボードから読み込みますか?","確認",System.Windows.Forms.MessageBoxButtons.YesNo) ==
					DialogResult.Yes)
				{
					String str = Clipboard.GetDataObject().GetData(typeof(System.String)).ToString();
					wr = new StringReader(str);
				}
				else
				{
					this.openFileDialog1.CheckFileExists = true;
					this.openFileDialog1.CheckPathExists = true;
					this.openFileDialog1.Filter = "csv|*.csv|txt|*.txt|全て|*.*";
					this.openFileDialog1.Multiselect = false;
					this.openFileDialog1.RestoreDirectory = false;
					if( this.openFileDialog1.ShowDialog(this) != DialogResult.OK )
					{
						return;
					}
					Stream fsw = this.openFileDialog1.OpenFile();
			
					wr = new StreamReader(fsw,true);
				}


				DataTable dt = null;
				dt = this.dspdt.Tables[0];

				if( dt.Columns.Count == 0 )
				{
					return ;
				}

                ArrayList drAr = this.LoadFile2DataTable(dt, dboInfo, wr, isCsv, isUseDQ);

				// グリッドのテーブルへと更新する
				// 実際のDBへの反映は行わない
				if( drAr.Count > 0 )
				{
					if( MessageBox.Show(drAr.Count.ToString(System.Globalization.CultureInfo.CurrentCulture) + "件のデータを読み込みますか？","確認",System.Windows.Forms.MessageBoxButtons.YesNo) == DialogResult.Yes )
					{
						foreach(DataRow addRow in drAr)
						{
							dt.Rows.Add(addRow);
						}
						this.dbGrid.Invalidate();
						MessageBox.Show("読込を完了しました");
					}
				}

			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
			}
			finally
			{
				if( wr != null )
				{
					wr.Close();
				}
			}
		}


		/// <summary>
		/// ファイルからデータを読み込み
		/// CSV,TSVの指定、また ”の利用を指定可能
		/// </summary>
        /// <param name="targetDt">読込先のDataTable</param>
        /// <param name="dbobj">データオブジェクトの情報</param>
		/// <param name="wr">読み込むテキストのストリーム</param>
		/// <param name="isCsv">true: CSVでの読み込み false : TSV での読み込み</param>
		/// <param name="isUseDQ">文字列にダブルクォートをつけているか
		/// true: 付加されている false: 付加されていない</param>
		/// <returns>読み込みに成功したか否か 成功した場合、その件数
		/// 1以上 -- 成功した
		/// 0以下 -- 何かしらの理由で失敗した</returns>
        protected ArrayList LoadFile2DataTable(DataTable targetDt, DBObjectInfo dbobj, TextReader wr, bool isCsv, bool isUseDQ)
		{
			bool isSetAll = true;
			int linecount = 0;
			ArrayList drAr = new ArrayList();
			bool isSkipFirstLine = false;

			try
			{
				// ファイルのチェックを実施する

				string readstr = "";
				string Separator = "";
				if( isCsv == true )
				{
					Separator = ",";
				}
				else
				{
					Separator = "\t";
				}

                List<string> readData = new List<string>();
                //ArrayList readData = new ArrayList();

				if( MessageBox.Show("先頭行は見出しですか?","確認",System.Windows.Forms.MessageBoxButtons.YesNo) ==
					DialogResult.Yes)
				{
					// 一行読み飛ばし
					isSkipFirstLine = true;
				}

				for(;;)
				{
					readstr = wr.ReadLine();
					if( readstr == null )
					{
						break;
					}
					linecount++;
					string []firstsplit = readstr.Split(Separator.ToCharArray());
					readData.Clear();
					if( !isUseDQ )
					{
						if( linecount == 1 && isSkipFirstLine == true )
						{
							// 先頭行は読み飛ばし
							continue;
						}
						readData.AddRange(firstsplit);
					}
					else
					{
						if( linecount == 1 && isSkipFirstLine == true )
						{
							// 先頭行は読み飛ばし
							continue;
						}
						// ダブルクォートが指定されている為、文字の途中で切れている可能性がある
						string addstr = "";
						for( int j = 0; j < firstsplit.Length; j++ )
						{
							if( firstsplit[j].StartsWith("\"") == true &&
								firstsplit[j].EndsWith("\"") == true 
								)
							{
								// 前後の " を排除した値を配置
								readData.Add(firstsplit[j].Substring(1,firstsplit[j].Length-2));
							}
							else if( firstsplit[j].StartsWith("\"") == true )
							{
								// 最初の文字がダブルクォートで始まっているので、順次ダブルクォートが出てくるまでを検索する
								int k = j;
								for( ; k < firstsplit.Length; k ++ )
								{
									if( firstsplit[k].EndsWith("\"") == true )
									{
										addstr += firstsplit[k].Substring(0,firstsplit[k].Length-1);
										break;
									}
									else
									{
										if( j == k )
										{
											addstr += firstsplit[k].Substring(1);
										}
										else
										{
											addstr += firstsplit[k];
										}
									}
								}
								readData.Add(addstr);
								j = k;
							}
							else
							{
								readData.Add(firstsplit[j]);
							}
						}
					}

					// 一行のデータが ar に配置されたので、dt と比較する

					if( readData.Count != targetDt.Columns.Count )
					{
						MessageBox.Show("項目数が違います 行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture) );
						isSetAll = false;
						break;
					}

                    if (targetDt.Columns.Count != dbobj.FieldCount)
                    {
                        MessageBox.Show("フィールド情報が古い可能性があります。オブジェクト情報再読込を行って下さい");
						isSetAll = false;
						break;
                    }

					foreach(DBFieldInfo each in dbobj.FieldInfo )
					{
						if( each.RealTypeName == "timestamp" )
						{
                            MessageBox.Show("バイナリデータがあるオブジェクトは指定できません:列" + each.Name);
							isSetAll = false;
							break;
						}
					}
					if( isSetAll == false )
					{
						break;
					}

					DataRow dr = targetDt.NewRow();
					foreach(DBFieldInfo eachField in dbobj.FieldInfo)
					{
						if( eachField.IsNullable == false &&
							(string)readData[eachField.Colid] == string.Empty )
						{
							MessageBox.Show("項目 " + eachField.Name + "には値の指定が必要です。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
							isSetAll = false;
							break;
						}
						if( eachField.IsIdentity == true && (string)readData[eachField.Colid] != string.Empty)
						{
							MessageBox.Show("項目 " + eachField.Name + "は自動採番されるので値は指定できません。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
							isSetAll = false;
							break;
						}
						// {"System.Int16"}
						if( targetDt.Columns[eachField.Name].DataType == typeof(System.Int16) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = Int16.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Int16 の整数を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// {"System.Int32"}
                        else if (targetDt.Columns[eachField.Name].DataType == typeof(System.Int32))
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = Int32.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Int32の整数を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// {"System.Int64"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Int64) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = Int64.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Int64の整数を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// {"System.String"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.String) )
						{
							if( eachField.Length < readData[eachField.Colid].ToString().Length )
							{
								MessageBox.Show("項目 " + eachField.Name + "には " + eachField.Length + "桁以上の値は指定できません。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
							dr[eachField.Name] = readData[eachField.Colid];
						}
						// {"System.Boolean"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Boolean) )
						{
							try
							{
								dr[eachField.Name] = Boolean.Parse(readData[eachField.Colid].ToString());
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Boolean値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// {"System.DateTime"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.DateTime) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = DateTime.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には DateTimeを表す値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// SQL SERVER 2008 の time 型の場合、ここにくるはず
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.TimeSpan) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = TimeSpan.Parse(readData[eachField.Colid].ToString());
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には DateTimeを表す値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
							// {"System.Decimal"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Decimal) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = Decimal.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Decimal値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// {"System.Double"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Double) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = Double.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Double値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						// {"System.Single"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Single) )
						{
							try
							{
								if( readData[eachField.Colid].ToString() == "" )
								{
									dr[eachField.Name] = DBNull.Value;
								}
								else
								{
									dr[eachField.Name] = Single.Parse(readData[eachField.Colid].ToString(),System.Globalization.CultureInfo.CurrentCulture);
								}
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Single値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						//{"System.Object"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Object) )
						{
							try
							{
								dr[eachField.Name] = readData[eachField.Colid].ToString();
							}
							catch ( Exception e )
							{
								MessageBox.Show("項目 " + eachField.Name + "には 指定された値は指定できません。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture) + "\r\n" + e.ToString());
								isSetAll = false;
								break;
							}
						}
						//{"System.Byte"}	
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Byte) )
						{
							try
							{
                                if (eachField.RealTypeName == "timestamp")
                                {
                                    // タイムスタンプの場合はスキップする
                                }
                                else
                                {
                                    // Base64でdecodeする？
                                    dr[eachField.Name] = Byte.Parse(readData[eachField.Colid].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                }
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Byte値を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
						//{"System.Guid"}
						else if( targetDt.Columns[eachField.Name].DataType == typeof(System.Guid) )
						{
							try
							{
								dr[eachField.Name] = readData[eachField.Colid].ToString();
							}
							catch
							{
								MessageBox.Show("項目 " + eachField.Name + "には Guidを表す文字列を指定してください。行:" + linecount.ToString(System.Globalization.CultureInfo.CurrentCulture));
								isSetAll = false;
								break;
							}
						}
                        else if (targetDt.Columns[eachField.Name].DataType.IsClass == true)
                        {
                            // アセンブリである

                        }
                        else
						{
							// 想定外の型の場合、文字列扱いにする
							// 桁数の制限もなにもわからないので、チェックなし
							dr[eachField.Name] = readData[eachField.Colid];
						}
					}
					if( isSetAll == true )
					{
						drAr.Add(dr);
					}
					else
					{
						drAr.Clear();
						break;
					}
				}
			}
			catch( Exception exp )
			{
				this.SetErrorMessage(exp);
				linecount = -1;
			}
			return drAr;
		}

		#endregion


		#region クラス内ユーティリティ関連
		private bool CheckFileSpec()
		{
			if( this.rdoOutFile.Checked == true ) 
			{
				if( this.txtOutput.Text == "" )
				{
					this.saveFileDialog1.CreatePrompt = true;
					this.saveFileDialog1.Filter = "SQL|*.sql|csv|*.csv|txt|*.txt|全て|*.*";
					DialogResult ret = this.saveFileDialog1.ShowDialog();
					if( ret == DialogResult.OK )
					{
						this.txtOutput.Text = this.saveFileDialog1.FileName;
					}
				}
				else
				{
					DirectoryInfo d = new DirectoryInfo(this.txtOutput.Text);
					if( d.Exists )
					{
						MessageBox.Show("指定されたファイル名はフォルダを指しています。ファイル名を指定してください。処理を中断します");
						return false;
					}
				}
				if( this.txtOutput.Text == "" )
				{
					MessageBox.Show("ファイル名が指定されていないので、処理を中断します");
					return false;
				}
			}
			if( this.rdoOutFolder.Checked == true )
			{

				if( this.txtOutput.Text == "" )
				{
					this.folderBrowserDialog1.SelectedPath = "";
					this.folderBrowserDialog1.ShowNewFolderButton = true;
					DialogResult ret = this.folderBrowserDialog1.ShowDialog();
					if( ret == DialogResult.OK )
					{
						this.txtOutput.Text = this.folderBrowserDialog1.SelectedPath;
					}
				}

				if( this.txtOutput.Text != "" )
				{
					DirectoryInfo d = new DirectoryInfo(this.txtOutput.Text);
					FileInfo	f = new FileInfo(this.txtOutput.Text);
					if( f.Exists )
					{
						MessageBox.Show("フォルダ名は指定されていますが、フォルダではありません。処理を中断します");
						return false;
					}
					else if( !d.Exists )
					{
						Directory.CreateDirectory(this.txtOutput.Text);
						DirectoryInfo ff = new DirectoryInfo(this.txtOutput.Text);
						if( !ff.Exists )
						{
							MessageBox.Show("フォルダ名は指定されていますが、作成できませんでした。処理を中断します");
							return false;
						}
					}
				}
				else
				{
					MessageBox.Show("フォルダ名が指定されませんでした。処理を中断します");
					return false;
				}
				
			}
			return true;
		}


		private System.Text.Encoding GetEncode()
		{
			if( this.rdoUnicode.Checked == true )
			{
				// UNICODE
				return new System.Text.UnicodeEncoding();
			}
			else if( this.rdoSjis.Checked == true )
			{
				// (MS932)ShiftJIS
				return Encoding.GetEncoding("shift-jis");
			}
			else
			{
				// UTF-8
				return new System.Text.UTF8Encoding();
			}
		}

		/// <summary>
		/// from 句で利用するためのオブジェクト修飾子を取得する
		/// </summary>
		/// <returns>オブジェクト修飾子</returns>
		protected string GetAlias()
		{
			return this.txtAlias.Text;
		}

		/// <summary>
		/// フォーマット文字列を取得する
		/// </summary>
		/// <param name="fstr">基の表示書式</param>
		/// <returns>表示書式文字列</returns>
		protected string GetFormat(string fstr)
		{
			if(fstr == null)
			{
				return "";
			}
			int termp = fstr.IndexOf("	");
			if( termp == -1 )
			{
				return fstr;
			}
			return fstr.Substring(0,termp);
		}


		/// <summary>
		/// 新規スレッドを開始する
		/// </summary>
		/// <param name="callb">終了時に呼び出されるCallBack関数</param>
		/// <param name="tags">CallBack時に引き渡される情報</param>
		/// <returns>スレッドが正常に開始できたか否か</returns>
		protected bool	BeginNewThread(WaitCallback callb, object tags)
		{
			if( this.isThreadAlive > 0 )
			{
				// 他のスレッドがあれば、スレッドを開始しない
				return false;
			}
			try
			{
				Interlocked.Increment(ref this.isThreadAlive);
				ThreadPool.QueueUserWorkItem(callb,tags);
				return true;
			}
			catch(Exception e)
			{
				this.SetErrorMessage(e);
				if( this.isThreadAlive != 0 )
				{
					Interlocked.Decrement(ref this.isThreadAlive);
				}
				return false;
			}
		}

		/// <summary>
		/// スレッドを終了させる
		/// </summary>
		protected void	ThreadEndIfAlive()
		{
			if( this.isThreadAlive == 0 )
			{
				return;
			}
			try
			{
				Interlocked.Decrement(ref this.isThreadAlive);
			}
			catch
			{
				;
			}
		}

		/// <summary>
		/// スレッドが中止されたか否か
		/// </summary>
		/// <returns></returns>
		protected bool IsProcCancel()
		{
			if( this.isThreadAlive == 0 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// オブジェクトに対する処理用情報をセットする
		/// </summary>
		/// <param name="tbname"></param>
		/// <returns></returns>
		protected ProcCondition GetProcCondition(string tbname)
		{
			ProcCondition procCond = new ProcCondition();
			procCond.AliasStr = this.txtAlias.Text;


			if( this.rdoSjis.Checked == true )
			{
				procCond.Encoding = System.Text.Encoding.GetEncoding("shift-jis");
			}
			else if( this.rdoUnicode.Checked == true )
			{
				procCond.Encoding = System.Text.Encoding.Unicode;
			}
			else 
			{
				procCond.Encoding = System.Text.Encoding.UTF8;
			}

			procCond.IsDisp = this.chkDispData.Checked;

			procCond.MaxStr = this.txtDispCount.Text;

			procCond.OrderStr = this.txtSort.Text;

			if( this.rdoClipboard.Checked == true )
			{
				procCond.OutputDestination = 1;
			}
			else if( this.rdoOutFile.Checked == true )
			{
				procCond.OutputDestination = 2;
			}
			else
			{
				procCond.OutputDestination = 3;
			}

			procCond.OutputFile = this.txtOutput.Text;

			if( tbname != null )
			{
				procCond.Tbname.Add(tbname);
			}
			else
			{
				
				object obj;
				for( int i = 0; i < this.objectList.SelectedItems.Count; i++ )
				{
					obj = this.objectList.GetSelectObjectName(i);
					procCond.Tbname.Add((string)obj);
				}
			}

			procCond.WhereStr = this.txtWhere.Text;

			return procCond;
		}

		#endregion

		private void dbGridMenu_Popup(object sender, System.EventArgs e)
		{
			ArrayList controlTarget = new ArrayList();
			controlTarget.Add(this.readCsvDbGridMenu);
			controlTarget.Add(this.readCsvDQDbGridMenu);
			controlTarget.Add(this.readTsvDbGridMenu);
			controlTarget.Add(this.readTsvDQDbGridMenu);


			if( this.dbGrid.Visible == true &&
				this.dbGrid.ReadOnly == false &&
				this.dbGrid.DataSource != null )
			{
				// 編集可能
				foreach(MenuItem mi in controlTarget)
				{
					mi.Visible = true;
				}
			}
			else
			{
				// 編集不可
				foreach(MenuItem mi in controlTarget)
				{
					mi.Visible = false;
				}
			}

		}

		/// <summary>
		/// フィールドリストから where 句を生成する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuFieldMakeWhere_Click(object sender, System.EventArgs e)
		{
			// フィールドリストから where 句を生成する
			// 別ダイアログを表示してエイリアス等の指定を可能にする
			if (this.objectList.SelectedItems.Count != 1)
			{
				return;
			}
			if (this.fieldListbox.SelectedItems.Count == 0)
			{
				return;
			}

			MakeFieldWhereDlg dlg = new MakeFieldWhereDlg();
			dlg.ObjectInfo = this.objectList.GetSelectObject(0);
			StringCollection fcol = new StringCollection();
			for (int i = 0; i < this.fieldListbox.SelectedItems.Count; i++)
			{
				DBFieldInfo fi = (DBFieldInfo)((FieldListItem)this.fieldListbox.SelectedItems[i]).BackObj;
				fcol.Add(fi.Name);
			}
			dlg.FieldList = fcol;

			dlg.ShowDialog(this);
		}

		/// <summary>
		/// where 句指定ダイアログを設定しなおす
		/// </summary>
		/// <param name="isReset">フィールド情報などをリセットしなおすか否か</param>
		/// <param name="isShow">非表示の場合にダイアログを表示するか否か</param>
		private void setWhereDialog(bool isReset, bool isShow)
		{
			bool isInit = false;
			if (wheredlg == null)
			{
				this.wheredlg = new WhereDialog();
				isInit = true;
				isReset = false;
			}
			if (wheredlg.Visible == true)
			{
				if (this.objectList.SelectedItems.Count == 1)
				{
					wheredlg.TargetObject = this.objectList.GetSelectObject(0);
					wheredlg.AliasName = this.txtAlias.Text;
//					wheredlg.ResetTarget();
				}
				else
				{
					wheredlg.TargetObject = null;
//					wheredlg.ResetTarget();
				}
				if (isShow == true)
				{
					wheredlg.EditText = this.txtWhere.Text;
					wheredlg.Show();
					wheredlg.BringToFront();
				}
			}
			else
			{
				WhereDialog dlg = this.wheredlg;
				if (isInit == true)
				{
					dlg.Enter += new System.EventHandler(this.dlgWhereZoom_Click);
				}
				dlg.LableName = "where 指定";
				if (this.objectList.SelectedItems.Count == 1)
				{
					dlg.TargetObject = this.objectList.GetSelectObject(0);
				}
				dlg.AliasName = this.txtAlias.Text;
				if (isShow == true)
				{
					dlg.Show();
					dlg.BringToFront();
					dlg.Focus();
				}
				if (isReset == true)
				{
					dlg.ResetTarget();
				}
				dlg.SetFieldCondResult();
				if (isShow == true)
				{
					dlg.EditText = this.txtWhere.Text;
				}

			}
		}

		private void txtAlias_ShowHistory(object sender, EventArgs e)
		{
			DBObjectInfo dboInfo = null;
			string targetTable = "";
			if (this.objectList.SelectedItems.Count == 1)
			{
				targetTable = this.objectList.GetSelectOneObjectFormalName();
				dboInfo = this.objectList.GetSelectObject(0);
			}
			if (this.txtAlias.DoShowHistory(targetTable))
			{
				DispData(dboInfo);
				setWhereDialog(false, false);
			}
		}

		/// <summary>
		/// owner/Role がダブルクリックされた場合、もう一度 owner/Roleのリストを読み込み直す
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void labelSchema_DoubleClick(object sender, EventArgs e)
		{
			ResetOwnerList();
		}

		/// <summary>
		/// オブジェクトリストのラベルがダブルクリックされた場合、オブジェクトリストを再読み込みする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void labelObject_DoubleClick(object sender, EventArgs e)
		{
			DispObjectList();
		}

        private void txtObjFilter_TextChanged(object sender, EventArgs e)
        {
            this.objectList.FilterObjectList(this.txtObjFilter.Text);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.txtObjFilter.Text = string.Empty;
        }

        private void DBReloadMenu_Click(object sender, EventArgs e)
        {
            DBLoad();
            LoadInitialSetting();
        }

        private void labelDB_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.dbMenu.Show((Control)this.labelDB, new Point(0, 0));
            }
        }

        private void menuTimeoutChange_Click(object sender, EventArgs e)
        {
            ChangeTimeout();
        }

        public BookmarkInfo GetCurrentBookmarkInfo()
        {
            return new BookmarkInfo(this.dbList.SelectedItem.ToString(),
                this.ownerListbox.GetSelectedItemAsStringArray(),
                this.objectList.GetSelectObjects()
                );
        }

        public void LoadBookmark(BookmarkInfo bookmark)
        {
            int idx = this.dbList.FindString(bookmark.DBName);
            if (idx >= 0)
            {
                this.dbList.SelectedIndex = idx;
            }

            this.ownerListbox.SetSelectedItems(bookmark.Schema);
            this.objectList.SetSelectedObjects(bookmark.Objects);
        }

        public manager.MacroArgInfo CreateMacroArg()
        {
            if (this.ownerListbox.SelectedItems.Count == 1)
            {
                return new manager.MacroArgInfo(this.ConnectionArg,
                    this.dbList.SelectedItem.ToString(),
                    this.ownerListbox.SelectedItem.ToString());
            }
            else
            {
                return new manager.MacroArgInfo(this.ConnectionArg, this.dbList.SelectedItem.ToString(),
                    string.Empty);
            }
        }

	}

	/// <summary>
	/// 処理開始時の情報を記憶する為のクラス
	/// </summary>
	public class ProcCondition
	{
		/// <summary>
		/// 出力先の指定
		/// </summary>
		private int pOutputDestination;
		/// <summary>
		/// 出力先の指定
		/// </summary>
		public int OutputDestination
		{
			get { return this.pOutputDestination; }
			set { this.pOutputDestination = value; }
		}

		/// <summary>
		/// 出力先ファイルの指定
		/// </summary>
		private string pOutputFile;
		/// <summary>
		/// 出力先ファイルの指定
		/// </summary>
		public string OutputFile
		{
			get { return this.pOutputFile; }
			set { this.pOutputFile = value; }
		}

		/// <summary>
		/// 文字エンコーディングの指定
		/// </summary>
		private System.Text.Encoding pEncoding;
		/// <summary>
		/// 文字エンコーディングの指定
		/// </summary>
		public System.Text.Encoding Encoding
		{
			get { return this.pEncoding; }
			set { this.pEncoding = value; }
		}

		/// <summary>
		/// 選択されたオブジェクトの一覧
		/// </summary>
		private ArrayList	pTableName;
		/// <summary>
		/// 選択されたオブジェクトの一覧
		/// </summary>
		public ArrayList	Tbname
		{
			get { return this.pTableName; }
			set { this.pTableName = value; }
		}

		/// <summary>
		/// where 句の指定
		/// </summary>
		private string		pWhereStr;
		/// <summary>
		/// where 句の指定
		/// </summary>
		public string		WhereStr
		{
			get { return this.pWhereStr; }
			set { 
				if( value != null )
				{
					this.pWhereStr = value.Trim(); 
				}
				else
				{
					this.pWhereStr = value;
				}
			}
		}
		/// <summary>
		/// Order by 句の指定
		/// </summary>
		private string		pOrderStr;
		/// <summary>
		/// Order by 句の指定
		/// </summary>
		public string		OrderStr
		{
			get { return this.pOrderStr; }
			set { 
				if( value != null )
				{
					this.pOrderStr = value.Trim(); 
				}
				else
				{
					this.pOrderStr = value; 
				}
			}
		}

		/// <summary>
		/// 別名の設定
		/// </summary>
		private string		pAliasStr;
		/// <summary>
		/// 別名の設定
		/// </summary>
		public string AliasStr
		{
			get { return this.pAliasStr; }
			set { this.pAliasStr = value; }
		}

		/// <summary>
		/// 最大件数の指定
		/// </summary>
		private string		pMaxString;
		/// <summary>
		/// 最大件数の指定
		/// </summary>
		public string		MaxStr
		{
			get { return this.pMaxString; }
			set { this.pMaxString = value; }
		}

		/// <summary>
		/// 最大件数の獲得
		/// 空白の場合は0を返す
		/// </summary>
		public int			MaxCount
		{
			get
			{
				if( this.pMaxString == null ||
					this.pMaxString == string.Empty )
				{
					return 0;
				}
				else
				{
					return int.Parse(this.pMaxString,System.Globalization.CultureInfo.CurrentCulture);
				}
			}
		}
		/// <summary>
		/// グリッドにデータを表示するか否かの指定
		/// </summary>
		private bool		pIsDisp;
		/// <summary>
		/// グリッドにデータを表示するか否かの指定
		/// </summary>
		public bool		IsDisp
		{
			get { return this.pIsDisp; }
			set { this.pIsDisp = value; }
		}

		/// <summary>
		/// グリッドにデータを全て表示するか否かの指定
		/// </summary>
		private bool		pIsAllDisp;
		/// <summary>
		/// グリッドにデータを全て表示するか否かの指定
		/// </summary>
		public bool		IsAllDisp
		{
			get { return this.pIsAllDisp; }
			set { this.pIsAllDisp = value; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ProcCondition()
		{
			this.pAliasStr = string.Empty;
			this.pEncoding = System.Text.Encoding.Unicode;
			this.pIsAllDisp = false;
			this.pIsDisp = true;
			this.pMaxString = string.Empty;
			this.pOrderStr = string.Empty;
			this.pOutputDestination = 1;
			this.pOutputFile = string.Empty;
			this.pTableName = new ArrayList();
			this.pWhereStr = string.Empty;
		}
	}

	/// <summary>
	/// 右クリック用＆ボタンクリック時のメニューの情報を管理する
	/// </summary>
	class	qdbeMenuItem 
	{

		private	bool	isSeparater = false;
		/// <summary>
		/// セパレーターか否か
		/// </summary>
		public bool		IsSeparater
		{
			get { return this.isSeparater; }
		}
		private	bool	isObjTarget = true;
		/// <summary>
		/// オブジェクトを対象に処理を行う動作か否か
		/// </summary>
		public  bool	IsObjTarget 
		{
			get { return this.isObjTarget; }
		}
		private	string	callBtnName;
		/// <summary>
		/// ボタンからの呼び出しがある場合、そのボタンのコントロール名称
		/// </summary>
		public	string	CallBtnName
		{
			get { return this.callBtnName; }
		}
		/// <summary>
		/// メニューとして表示する文字列
		/// </summary>
		private	string	menuName = "";
		/// <summary>
		/// クリック時に呼び出されるイベントハンドラ
		/// </summary>
		private	System.EventHandler	 clickHandler;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="iss">セパレーターか否か true: セパレーター false: 通常メニュー</param>
		/// <param name="isot">オブジェクトを対象に処理を行う動作か否か</param>
		/// <param name="btn">ボタンからの呼び出しがある場合、そのボタンのコントロール名称</param>
		/// <param name="text">メニューとして表示する文字列</param>
		/// <param name="clickEv">クリック時に呼び出されるイベントハンドラ</param>
		public	qdbeMenuItem(bool iss, bool isot, string btn, string text, System.EventHandler clickEv)
		{
			this.isSeparater = iss;
			this.isObjTarget = isot;
			this.callBtnName = btn;
			this.menuName = text;
			this.clickHandler = clickEv;
		}

		/// <summary>
		/// メニューITEMを作成する
		/// </summary>
		/// <param name="index">メニューの中でのINDEX</param>
		/// <param name="shortCutNo">0: ショートカットキーはつけない 1以上: 指定された数値でショートカットキーを設定する</param>
		/// <returns></returns>
		public	MenuItem	CreateMenuItem(int index, int shortCutNo)
		{
			MenuItem	it = new MenuItem();
			it.Index = index;
			
			if( this.isSeparater == true )
			{
				it.Text = "-";
			}
			else
			{
				if( shortCutNo > 0 )
				{
					it.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture,"(&{0}) {1}",
						shortCutNo.ToString("x",System.Globalization.CultureInfo.CurrentCulture), this.menuName );
				}
				else
				{
					it.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture,"{0}",
						this.menuName );
				}
				it.Click += this.clickHandler;
			}
			return it;
		}
	}
}
