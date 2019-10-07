using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace quickDBExplorer
{
	/// <summary>
	/// �T�[�o�[�ʂ̊e��w�藚���f�[�^���Ǘ�����
	/// </summary>
	[Serializable]
	public sealed class ServerData : ISerializable, IDisposable 
	{
		/// <summary>
		/// �ڑ���T�[�o�[��
		/// </summary>
		private string		servername;
		/// <summary>
		/// �ڑ���C���X�^���X��
		/// </summary>
		private string		instancename;
		/// <summary>
		/// �T�[�o�[�{�C���X�^���X�𗘗p�����ڑ���T�[�o�[��Hashtable�̃L�[���
		/// </summary>
		private string		keyname;

		/// <summary>
		/// �Ō�ɗ��p����DB��
		/// </summary>
		private string		pLastDatabase;		
		/// <summary>
		/// �Ō�ɗ��p����DB��
		/// </summary>
		public string		LastDatabase
		{
			get { return this.pLastDatabase; }
			set { this.pLastDatabase = value; }
		}

		/// <summary>
		/// // DB ���̍ŏI���[�U�[���L�^����
		/// ������o�^�����邽�߁A������string �̗v�f��������ArrayList�Ƃ���
		/// </summary>
		private Hashtable		dbopt;	
		/// <summary>
		/// // DB ���̍ŏI���[�U�[���L�^����
		/// ������o�^�����邽�߁A������string �̗v�f��������ArrayList�Ƃ���
		/// </summary>
		public Hashtable		Dbopt
		{
			get { return this.dbopt; }
			//set { this.dbopt = value; }
		}

		/// <summary>
		/// �V�X�e�����[�U�[��\�����邩�ۂ�
		/// </summary>
		private int	isShowsysuser;	
		/// <summary>
		/// �V�X�e�����[�U�[��\�����邩�ۂ�
		/// </summary>
		public int	IsShowsysuser
		{
			get { return this.isShowsysuser; }
			set { this.isShowsysuser = value; }
		}

		/// <summary>
		///table/view ���X�g�̃\�[�g�� 
		/// </summary>
		private int	sortKey;
		/// <summary>
		///table/view ���X�g�̃\�[�g�� 
		/// </summary>
		public int	SortKey
		{
			get { return this.sortKey ; }
			set { this.sortKey = value; }
		}

		/// <summary>
		/// view ��\�������邩�ǂ���
		/// </summary>
		private int	showView;
		/// <summary>
		/// view ��\�������邩�ǂ���
		/// </summary>
		public int	ShowView
		{
			get { return this.showView; }
			set { this.showView = value; }
		}
		
		/// <summary>
		/// �f�[�^�o�͐�̎w��
		/// </summary>
		private Hashtable	outdest;
		/// <summary>
		/// �f�[�^�o�͐�̎w��
		/// </summary>
		public	Hashtable	OutDest
		{
			get { return this.outdest; }
			//set { this.outdest = value; }
		}
		/// <summary>
		/// �f�[�^�o�͐�̃t�@�C���E�t�H���_��
		/// </summary>
		private Hashtable	outfile;
		/// <summary>
		/// �f�[�^�o�͐�̃t�@�C���E�t�H���_��
		/// </summary>
		public	Hashtable	OutFile
		{
			get { return this.outfile; }
			//set { this.outfile = value; }
		}
		/// <summary>
		/// �f�[�^�O���b�h��\�����邩�ۂ�
		/// </summary>
		private Hashtable	showgrid;
		/// <summary>
		/// �f�[�^�O���b�h��\�����邩�ۂ�
		/// </summary>
		public	Hashtable	ShowGrid
		{
			get { return this.showgrid; }
			//set { this.showgrid = value; }
		}

		/// <summary>
		/// �O���b�h�\������
		/// </summary>
		private Hashtable	griddspcnt;
		/// <summary>
		/// �O���b�h�\������
		/// </summary>
		public 	Hashtable	GridDispCnt
		{
			get { return this.griddspcnt; }
			//set { this.griddspcnt = value; }
		}
		
		/// <summary>
		/// �e�L�X�g�o�͎��̕����R�[�h
		/// </summary>
		private Hashtable	txtencode;
		/// <summary>
		/// �e�L�X�g�o�͎��̕����R�[�h
		/// </summary>
		public	Hashtable	TxtEncode
		{
			get { return this.txtencode; }
			//set { this.txtencode = value; }
		}
        /// <summary>
		/// �T�[�o�[�ʏ����L�����邩�ۂ�
		/// </summary>
		private bool	isSaveKey = true;
		/// <summary>
		/// �T�[�o�[�ʏ����L�����邩�ۂ�
		/// </summary>
		public	bool	IsSaveKey
		{
			get { return this.isSaveKey; }
			set { this.isSaveKey = value; }
		}
		/// <summary>
		/// �M���֌W�ڑ��𗘗p���邩�ۂ�
		/// </summary>
		private bool	isUseTrust = false;
		/// <summary>
		/// �M���֌W�ڑ��𗘗p���邩�ۂ�
		/// </summary>
		public	bool	IsUseTrust
		{
			get { return this.isUseTrust; }
			set { this.isUseTrust = value; }
		}
		/// <summary>
		/// ���O�C�����[�U�[��
		/// </summary>
		private string	pLogOnUser = "";
		/// <summary>
		/// ���O�C�����[�U�[��
		/// </summary>
		public	string	LogOnUser
		{
			get { return this.pLogOnUser; }
			set { this.pLogOnUser = value; }
		}

        /// <summary>
        /// �e����͗���
        /// </summary>
        public Dictionary<string, TextHistoryDataSet> InputHistories { get; set; }


		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public ServerData()
		{
			dbopt = new Hashtable();
			isShowsysuser = 0;
			sortKey = 0;
			showView = 0;
			IsUseTrust = false;
			outdest =  new Hashtable();
			outfile =  new Hashtable();
			showgrid =  new Hashtable();
			griddspcnt =  new Hashtable();
			txtencode = new Hashtable();
            InputHistories = new Dictionary<string, TextHistoryDataSet>();
            //whereHistory = new TextHistoryDataSet();
            //sortHistory = new TextHistoryDataSet();
            //aliasHistory = new TextHistoryDataSet();
            //selectHistory = new TextHistoryDataSet();
            //DMLHistory = new TextHistoryDataSet();
            //cmdHistory = new TextHistoryDataSet();
            //pSearchHistory = new TextHistoryDataSet();
		}

		/// <summary>
		/// �������\�[�X��j������
		/// </summary>
		/// <param name="disposing">�j�����邩�ۂ��̃t���O</param>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
                InputHistories.Clear();
			}
			// free native resources
		}

		/// <summary>
		/// �������\�[�X��j������
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// �V���A���C�Y�����p�R���X�g���N�^
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		private ServerData(SerializationInfo info, StreamingContext context)
		{
			servername = "";
			instancename = "";
			keyname = "";
			pLastDatabase = "";
			dbopt = new Hashtable();
			isShowsysuser = 0;
			sortKey = 0;
			showView = 0;
			IsUseTrust = false;
			outdest =  new Hashtable();
			outfile =  new Hashtable();
			showgrid =  new Hashtable();
			griddspcnt =  new Hashtable();
			txtencode = new Hashtable();
            InputHistories = new Dictionary<string, TextHistoryDataSet>();

			try
			{
				servername = info.GetString("servername");
			}
			catch {}
			try
			{
				this.instancename = info.GetString("instancename");
			}
			catch {}
			try
			{
				this.keyname = info.GetString("keyname");
			}
			catch {}
			try
			{
				this.pLastDatabase = info.GetString("pLastDatabase");
			}
			catch {}
			try
			{
				this.dbopt = (Hashtable)info.GetValue("dbopt",typeof(Hashtable));
			}
			catch {}

			try
			{
				this.isShowsysuser = info.GetInt32("isShowsysuser");
			}
			catch {}

			try
			{
				this.sortKey = info.GetInt32("sortKey");
			}
			catch{}

			try
			{
				this.showView = info.GetInt32("showView");
			}
			catch{}

			try
			{
				this.outdest = (Hashtable)info.GetValue("outdest",typeof(Hashtable));
			}
			catch{}

			try
			{
				this.outfile = (Hashtable)info.GetValue("outfile",typeof(Hashtable));
			}
			catch{}

			try
			{
				this.showgrid = (Hashtable)info.GetValue("showgrid",typeof(Hashtable));
			}
			catch{}

			try
			{
				this.griddspcnt = (Hashtable)info.GetValue("griddspcnt",typeof(Hashtable));
			}
			catch{}

			try
			{
				this.txtencode = (Hashtable)info.GetValue("txtencode",typeof(Hashtable));
			}
			catch{}

			try
			{
				this.isSaveKey = info.GetBoolean("isSaveKey");
			}
			catch{}


			try
			{
				this.IsUseTrust = info.GetBoolean("IsUseTrust");
			}
			catch{}

			try
			{
				this.pLogOnUser = info.GetString("pLogOnUser");
			}
			catch{}

			try
			{
                InputHistories.Add("txtWhere", (TextHistoryDataSet)info.GetValue("whereHistory",typeof(TextHistoryDataSet)));
			}
			catch{}
			try
			{
                InputHistories.Add("txtSort", (TextHistoryDataSet)info.GetValue("sortHistory",typeof(TextHistoryDataSet)));
			}
			catch{}
			try
			{
                InputHistories.Add("txtAlias", (TextHistoryDataSet)info.GetValue("aliasHistory",typeof(TextHistoryDataSet)));
			}
			catch{}
			try
			{
                InputHistories.Add("selectHistory",(TextHistoryDataSet)info.GetValue("selectHistory",typeof(TextHistoryDataSet)));
			}
			catch{}
			try
			{
                InputHistories.Add("DMLHistory", (TextHistoryDataSet)info.GetValue("DMLHistory",typeof(TextHistoryDataSet)));
			}
			catch{}
			try
			{
                InputHistories.Add("cmdHistory", (TextHistoryDataSet)info.GetValue("cmdHistory",typeof(TextHistoryDataSet)));
			}
			catch{}
			try
			{
                InputHistories.Add("SearchHistory", (TextHistoryDataSet)info.GetValue("SearchHistory",typeof(TextHistoryDataSet)));
			}
			catch{}

            try
            {
                ArrayList keys = (ArrayList)info.GetValue("InputHistoryKey", typeof(ArrayList));
                ArrayList values = (ArrayList)info.GetValue("InputHistoryValue", typeof(ArrayList));
                for (int i = 0; i < keys.Count; i++)
                {
                    InputHistories.Add((string)keys[i], (TextHistoryDataSet)values[i]);
                }
            }
            catch { }
        }

		/// <summary>
		/// �V���A���C�Y�����p
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(
			SerializationInfo info, StreamingContext context)
		{
			info.AddValue("servername",	servername );
			info.AddValue("instancename", instancename );
			info.AddValue("keyname", keyname );
			info.AddValue("pLastDatabase", pLastDatabase );
			info.AddValue("dbopt", dbopt);
			info.AddValue("isShowsysuser",isShowsysuser);
			info.AddValue("sortKey", sortKey );
			info.AddValue("showView", showView );
			info.AddValue("IsUseTrust", IsUseTrust );
			info.AddValue("outdest", outdest );
			info.AddValue("outfile", outfile );
			info.AddValue("showgrid", showgrid );
			info.AddValue("griddspcnt", griddspcnt );
			info.AddValue("txtencode", txtencode );
			info.AddValue("isSaveKey", isSaveKey );
			info.AddValue("pLogOnUser", pLogOnUser );
            ArrayList keys = new ArrayList();
            ArrayList values = new ArrayList();
            foreach (string each in InputHistories.Keys)
            {
                keys.Add(each);
                values.Add(InputHistories[each]);
            }
            info.AddValue("InputHistoryKey", keys);
            info.AddValue("InputHistoryValue", values);
        }

		/// <summary>
		/// �ڑ���T�[�o�[��
		/// </summary>
		public string Servername
		{
			get { return this.servername; }
			set 
			{ 
				this.servername = value; 
				this.keyname = value + this.instancename;
			}
		}
		/// <summary>
		/// �ڑ���C���X�^���X��
		/// </summary>
		public string InstanceName
		{
			get { return this.instancename; }
			set 
			{
				this.instancename = value;
				this.keyname = this.servername + this.instancename;
			}
		}
		/// <summary>
		/// �T�[�o�[�{�C���X�^���X�𗘗p�����ڑ���T�[�o�[��Hashtable�̃L�[���
		/// </summary>
		public string KeyName
		{
			get 
			{ 
				return this.keyname; 
			}
		}
	}

	/// <summary>
	/// ConditionRecorder �̊T�v�̐����ł��B
	/// </summary>
	[Serializable]
	public sealed class ConditionRecorder : ISerializable
	{
		/// <summary>
		/// �T�[�o�[�ʏ����Ǘ�����n�b�V���e�[�u��
		/// �T�[�o�[���{�C���X�^���X�����L�[�Ƃ���
		/// </summary>
		private Hashtable	perServerData;
		/// <summary>
		/// �T�[�o�[�ʏ����Ǘ�����n�b�V���e�[�u��
		/// �T�[�o�[���{�C���X�^���X�����L�[�Ƃ���
		/// </summary>
		public Hashtable	PerServerData
		{
			get { return this.perServerData; }
			//set { this.perServerData = value; }
		}

		/// <summary>
		/// �Ō�ɐڑ������T�[�o�[��Hashkey
		/// </summary>
		private string lastserverkey = "";
		/// <summary>
		/// �Ō�ɐڑ������T�[�o�[��Hashkey
		/// </summary>
		public string LastServerKey
		{
			get { return this.lastserverkey; }
			set { this.lastserverkey = value; }
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public ConditionRecorder()
		{
			this.perServerData = new Hashtable();
		}

		/// <summary>
		/// �V���A���C�Y�����p�R���X�g���N�^
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		private ConditionRecorder(SerializationInfo info, StreamingContext context)
		{
			try 
			{ 
				this.lastserverkey = info.GetString("LASTSERVERKEY"); 
			} 
			catch
			{
				this.lastserverkey = "";
			}
			try { 
				this.perServerData = (Hashtable)info.GetValue("SERVERDATA",typeof(Hashtable));
			} 
			catch
			{
				this.perServerData = new Hashtable();
			}
		}

		/// <summary>
		/// �V���A���C�Y�����p
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(
			SerializationInfo info, StreamingContext context)
		{
			info.AddValue("LASTSERVERKEY", this.lastserverkey);
			info.AddValue("SERVERDATA", this.perServerData, typeof(Hashtable) );
		}

	}
}