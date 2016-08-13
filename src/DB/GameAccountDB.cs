// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from Game_Account_DB on 2016-08-14 00:34:20Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
namespace DataBaseContext
{
	using System;
	using System.ComponentModel;
	using System.Data;
#if MONO_STRICT
	using System.Data.Linq;
#else   // MONO_STRICT
	using DbLinq.Data.Linq;
	using DbLinq.Vendor;
#endif  // MONO_STRICT
	using System.Data.Linq.Mapping;
	using System.Diagnostics;
	
	
	public partial class GameAccountDb : DataContext
	{
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		#endregion
		
		
		public GameAccountDb(string connectionString) : 
				base(connectionString)
		{
			this.OnCreated();
		}
		
		public GameAccountDb(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public GameAccountDb(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public Table<TbaCCount> TbaCCount
		{
			get
			{
				return this.GetTable <TbaCCount>();
			}
		}
	}
	
	#region Start MONO_STRICT
#if MONO_STRICT

	public partial class GameAccountDb
	{
		
		public GameAccountDb(IDbConnection connection) : 
				base(connection)
		{
			this.OnCreated();
		}
	}
	#region End MONO_STRICT
	#endregion
#else     // MONO_STRICT
	
	public partial class GameAccountDb
	{
		
		public GameAccountDb(IDbConnection connection) : 
				base(connection, new DbLinq.MySql.MySqlVendor())
		{
			this.OnCreated();
		}
		
		public GameAccountDb(IDbConnection connection, IVendor sqlDialect) : 
				base(connection, sqlDialect)
		{
			this.OnCreated();
		}
		
		public GameAccountDb(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) : 
				base(connection, mappingSource, sqlDialect)
		{
			this.OnCreated();
		}
	}
	#region End Not MONO_STRICT
	#endregion
#endif     // MONO_STRICT
	#endregion
	
	[Table(Name="game_account_db.TB_Account")]
	public partial class TbaCCount : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private System.DateTime _createDateTime;
		
		private long _id;
		
		private System.DateTime _lastLoginDateTime;
		
		private long _loginCount;
		
		private string _password;
		
		private int _serverID;
		
		private string _userName;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnCreateDateTimeChanged();
		
		partial void OnCreateDateTimeChanging(System.DateTime value);
		
		partial void OnIDChanged();
		
		partial void OnIDChanging(long value);
		
		partial void OnLastLoginDateTimeChanged();
		
		partial void OnLastLoginDateTimeChanging(System.DateTime value);
		
		partial void OnLoginCountChanged();
		
		partial void OnLoginCountChanging(long value);
		
		partial void OnPasswordChanged();
		
		partial void OnPasswordChanging(string value);
		
		partial void OnServerIDChanged();
		
		partial void OnServerIDChanging(int value);
		
		partial void OnUserNameChanged();
		
		partial void OnUserNameChanging(string value);
		#endregion
		
		
		public TbaCCount()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_createDateTime", Name="CreateDateTime", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public System.DateTime CreateDateTime
		{
			get
			{
				return this._createDateTime;
			}
			set
			{
				if ((_createDateTime != value))
				{
					this.OnCreateDateTimeChanging(value);
					this.SendPropertyChanging();
					this._createDateTime = value;
					this.SendPropertyChanged("CreateDateTime");
					this.OnCreateDateTimeChanged();
				}
			}
		}
		
		[Column(Storage="_id", Name="ID", DbType="bigint(20)", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public long ID
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((_id != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_lastLoginDateTime", Name="LastLoginDateTime", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public System.DateTime LastLoginDateTime
		{
			get
			{
				return this._lastLoginDateTime;
			}
			set
			{
				if ((_lastLoginDateTime != value))
				{
					this.OnLastLoginDateTimeChanging(value);
					this.SendPropertyChanging();
					this._lastLoginDateTime = value;
					this.SendPropertyChanged("LastLoginDateTime");
					this.OnLastLoginDateTimeChanged();
				}
			}
		}
		
		[Column(Storage="_loginCount", Name="LoginCount", DbType="bigint(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public long LoginCount
		{
			get
			{
				return this._loginCount;
			}
			set
			{
				if ((_loginCount != value))
				{
					this.OnLoginCountChanging(value);
					this.SendPropertyChanging();
					this._loginCount = value;
					this.SendPropertyChanged("LoginCount");
					this.OnLoginCountChanged();
				}
			}
		}
		
		[Column(Storage="_password", Name="Password", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				if (((_password == value) 
							== false))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[Column(Storage="_serverID", Name="ServerID", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int ServerID
		{
			get
			{
				return this._serverID;
			}
			set
			{
				if ((_serverID != value))
				{
					this.OnServerIDChanging(value);
					this.SendPropertyChanging();
					this._serverID = value;
					this.SendPropertyChanged("ServerID");
					this.OnServerIDChanged();
				}
			}
		}
		
		[Column(Storage="_userName", Name="UserName", DbType="varchar(255)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string UserName
		{
			get
			{
				return this._userName;
			}
			set
			{
				if (((_userName == value) 
							== false))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._userName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
			if ((h != null))
			{
				h(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(string propertyName)
		{
			System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
			if ((h != null))
			{
				h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
