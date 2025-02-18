using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace quickDBExplorer
{
	/// <summary>
	/// SQLのバージョンによる実装の違いからくるSQL文などを生成する為のクラス
	/// ここでは Interface として定義し、実装は個別に行う
	/// </summary>
	public interface ISqlInterface
	{
		/// <summary>
		/// SQLServerに対するコネクション情報を管理する
		/// </summary>
		/// <param name="sqlConnection">コネクション情報</param>
		/// <param name="timeout">コマンド実行タイムアウト値</param>
		void SetConnection(IDbConnection sqlConnection, int timeout);

		/// <summary>
		/// SQLSERVERに対するコネクション情報を閉じる
		/// </summary>
		void CloseConnection();


		/// <summary>
		/// タイムアウト値を設定しなおす
		/// </summary>
		/// <param name="timeout"></param>
		void SetTimeout(int timeout);

		/// <summary>
		/// DataAdapter を取得する
		/// </summary>
		DbDataAdapter	NewDataAdapter();

		/// <summary>
		/// IDbCommand を新規に作成する。
		/// ただし、コネクション情報とタイムアウト値はすでにセットされている
		/// </summary>
		/// <returns></returns>
		IDbCommand		NewSqlCommand();

		/// <summary>
		/// IDbCommand を新規に作成する。
		/// ただし、コマンド文字列、コネクション情報とタイムアウト値はすでにセットされている
		/// </summary>
		/// <param name="stSql">実行するコマンド文字列</param>
		/// <returns></returns>
		IDbCommand		NewSqlCommand(string stSql);

		/// <summary>
		/// DataAdapter に IDbCommand を SelectCommandとして関連づける
		/// </summary>
		/// <param name="da"></param>
		/// <param name="cmd"></param>
		void	SetSelectCmd(DbDataAdapter da, IDbCommand cmd);

		/// <summary>
		/// トランザクション情報を設定する
		/// </summary>
		IDbTransaction	SetTransaction(IDbCommand cmd);

		/// <summary>
		/// select コマンドから、update, insert, delete コマンドを生成しなおす
		/// </summary>
		/// <param name="da"></param>
		void	SetCommandBuilder(DbDataAdapter da);

		/// <summary>
		/// DataReaderからbyte配列を読み込む。
		/// 指定されたフィールドはもともとバイナリデータであることが前提
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		byte[]	GetDataReaderBytes(IDataReader dr, int col);

		/// <summary>
		/// DataReaderからDateTimeOffset値を読み込む。
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		DateTimeOffset GetDataReaderDateTimeOffSet(IDataReader dr, int col);

		/// <summary>
		/// DBの一覧表示を取得するSQL文を返す
		/// </summary>
		/// <returns></returns>
		string GetDBSelect();

		/// <summary>
		/// 指定されたデータベースへと接続を変更する
		/// </summary>
		/// <param name="dbName">変更先のデータベース名</param>
		/// <returns></returns>
		void SetDatabase(string dbName);

		/// <summary>
		/// オブジェクト一覧のカラムヘッダの表示文字を取得する
		/// </summary>
		/// <returns></returns>
		string GetTableListColumnName();

		/// <summary>
		/// DBオーナーのラベルを返す
		/// </summary>
		/// <returns></returns>
		string GetOwnerLabel1();

		/// <summary>
		/// ラジオボタンのラベルを返す
		/// </summary>
		/// <returns></returns>
		string GetOwnerLabel2();

		/// <summary>
		/// オブジェクト一覧の表示用SQLの取得
		/// </summary>
		/// <param name="isDisplayTable">テーブルを表示させるか否か true: 表示する false: 表示させない</param>
		/// <param name="isDisplayView">View を表示させるか否か true: 表示する false: 表示させない</param>
		/// <param name="isSynonym">シノニムを表示させるか否か true: 表示する false: 表示させない</param>
		/// <param name="isDisplayFunction">Functionを表示させるか否か true: 表示する false: 表示させない</param>
		/// <param name="isDisplaySP">ストアドプロシージャを表示させるか否か true: 表示する false: 表示させない</param>
		/// <param name="ownerList">特定のOwnerのオブジェクトのみ表示する場合は IN句に利用するカンマ区切り文字列を渡す</param>
		/// <returns></returns>
		string GetDisplayObjList(bool isDisplayTable, bool isDisplayView, bool isSynonym, bool isDisplayFunction, bool isDisplaySP, string ownerList);


		/// <summary>
		/// Owner の一覧を取得するSQLを生成する
		/// </summary>
		/// <param name="isDisplaySysUser"></param>
		/// <returns></returns>
		string	GetOwnerList(bool isDisplaySysUser);

		/// <summary>
		/// ISQL を起動する。
		/// </summary>
		/// <param name="serverRealName">サーバー名</param>
		/// <param name="instanceName">インスタンス名</param>
		/// <param name="isUseTrust">信頼関係接続を利用するか否か</param>
		/// <param name="dbName">データベース名</param>
		/// <param name="logOnUserId">ログインID</param>
		/// <param name="logOnPassword">ログインパスワード</param>
		void	CallISQL(string serverRealName, string instanceName, bool isUseTrust, string dbName, string logOnUserId, string logOnPassword);


		/// <summary>
		/// EnterPriseManagerを起動する
		/// </summary>
		/// <param name="serverRealName">サーバー名</param>
		/// <param name="instanceName">インスタンス名</param>
		/// <param name="isUseTrust">信頼関係接続を利用するか否か</param>
		/// <param name="dbName">データベース名</param>
		/// <param name="logOnUserId">ログインID</param>
		/// <param name="logOnPassword">ログインパスワード</param>
		void	CallEPM(string serverRealName, string instanceName, bool isUseTrust, string dbName, string logOnUserId, string logOnPassword);

		/// <summary>
		/// Profilerを起動する
		/// </summary>
		/// <param name="serverRealName">サーバー名</param>
		/// <param name="instanceName">インスタンス名</param>
		/// <param name="isUseTrust">信頼関係接続を利用するか否か</param>
		/// <param name="dbName">データベース名</param>
		/// <param name="logOnUserId">ログインID</param>
		/// <param name="logOnPassword">ログインパスワード</param>
		void	CallProfile(string serverRealName, string instanceName, bool isUseTrust, string dbName, string logOnUserId, string logOnPassword);


		/// <summary>
		/// オブジェクトに対する Create 文を生成する
		/// </summary>
		/// <param name="databaseObjectInfo"></param>
		/// <param name="useParentheses"></param>
		/// <returns></returns>
		string	GetDdlCreateString(DBObjectInfo databaseObjectInfo, bool useParentheses);

		/// <summary>
		/// オブジェクトに対するDROP 文を生成する
		/// </summary>
		/// <param name="databaseObjectInfo"></param>
		/// <returns></returns>
		string	GetDDLDropStr(DBObjectInfo databaseObjectInfo);

		/// <summary>
		/// オブジェクト情報をセットするDataTableを初期化する
		/// </summary>
		/// <param name="objTable"></param>
		void	InitObjTable(DataTable objTable);

		/// <summary>
		/// オブジェクトの情報を、DataTable に追加する
		/// </summary>
		/// <param name="databaseObjectInfo">対象となるオブジェクト</param>
		/// <param name="dt"></param>
		void	AddObjectInfo(DBObjectInfo databaseObjectInfo, DataTable dt);

		/// <summary>
		/// オブジェクトの詳細情報をセットするイベントハンドラを返す
		/// </summary>
		/// <returns></returns>
		DataGetEventHandler ObjectDetailSet();

		/// <summary>
		/// フィールド名を検索する SQL文を生成する
		/// </summary>
		/// <param name="searchCondition">検索対象の文字</param>
		/// <param name="searchType">検索方法</param>
		/// <param name="limitSchema">スキーマの絞込み対象</param>
        /// <param name="condition">検索条件詳細</param>
		/// <returns></returns>
		string	GetSearchFieldSql(string searchCondition, quickDBExplorer.SearchType searchType, List<string> limitSchema, ObjectSearchCondition condition);

        /// <summary>
        /// オブジェクト名を検索する SQL文を生成する
        /// </summary>
        /// <param name="searchCondition">検索対象の文字</param>
        /// <param name="searchType">検索方法</param>
        /// <param name="limitSchema">スキーマの絞込み対象</param>
        /// <param name="condition">検索条件詳細</param>
        /// <returns></returns>
        string GetSearchObjectSql(string searchCondition, quickDBExplorer.SearchType searchType, List<string> limitSchema, ObjectSearchCondition condition);



        /// <summary>
        /// バージョン情報を設定する
        /// </summary>
        /// <param name="version"></param>
        void SetupVersion(SqlVersion version);

        /// <summary>
        /// DBへの再接続を強制する
        /// </summary>
        void ReConnect();
	}
}
