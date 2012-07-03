using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace quickDBExplorer
{
	/// <summary>
	/// SqlServer2005 の概要の説明です。
	/// </summary>
	public class SqlServerDriver2012 : SqlServerDriver2008R2
	{
        public SqlServerDriver2012()
        {
            this.regkey = @"SOFTWARE\Microsoft\Microsoft SQL Server\110\Tools\ClientSetup\";
        }
    }
}