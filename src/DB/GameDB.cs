// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from Game_DB on 2016-09-26 16:35:29Z.
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
	
	
	public partial class GameDb : DataContext
	{
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		#endregion
		
		
		public GameDb(string connectionString) : 
				base(connectionString)
		{
			this.OnCreated();
		}
		
		public GameDb(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public GameDb(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public Table<TBGAmePlayer> TBGAmePlayer
		{
			get
			{
				return this.GetTable <TBGAmePlayer>();
			}
		}
		
		public Table<TBPLayerEquip> TBPLayerEquip
		{
			get
			{
				return this.GetTable <TBPLayerEquip>();
			}
		}
		
		public Table<TBPLayerHero> TBPLayerHero
		{
			get
			{
				return this.GetTable <TBPLayerHero>();
			}
		}
	}
	
	#region Start MONO_STRICT
#if MONO_STRICT

	public partial class GameDb
	{
		
		public GameDb(IDbConnection connection) : 
				base(connection)
		{
			this.OnCreated();
		}
	}
	#region End MONO_STRICT
	#endregion
#else     // MONO_STRICT
	
	public partial class GameDb
	{
		
		public GameDb(IDbConnection connection) : 
				base(connection, new DbLinq.MySql.MySqlVendor())
		{
			this.OnCreated();
		}
		
		public GameDb(IDbConnection connection, IVendor sqlDialect) : 
				base(connection, sqlDialect)
		{
			this.OnCreated();
		}
		
		public GameDb(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) : 
				base(connection, mappingSource, sqlDialect)
		{
			this.OnCreated();
		}
	}
	#region End Not MONO_STRICT
	#endregion
#endif     // MONO_STRICT
	#endregion
	
	[Table(Name="game_db.TB_GamePlayer")]
	public partial class TBGAmePlayer : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private int _coin;
		
		private int _gold;
		
		private int _packageSize;
		
		private long _userID;
		
		private string _userPackage;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnCoinChanged();
		
		partial void OnCoinChanging(int value);
		
		partial void OnGoldChanged();
		
		partial void OnGoldChanging(int value);
		
		partial void OnPackageSizeChanged();
		
		partial void OnPackageSizeChanging(int value);
		
		partial void OnUserIDChanged();
		
		partial void OnUserIDChanging(long value);
		
		partial void OnUserPackageChanged();
		
		partial void OnUserPackageChanging(string value);
		#endregion
		
		
		public TBGAmePlayer()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_coin", Name="Coin", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int Coin
		{
			get
			{
				return this._coin;
			}
			set
			{
				if ((_coin != value))
				{
					this.OnCoinChanging(value);
					this.SendPropertyChanging();
					this._coin = value;
					this.SendPropertyChanged("Coin");
					this.OnCoinChanged();
				}
			}
		}
		
		[Column(Storage="_gold", Name="Gold", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				if ((_gold != value))
				{
					this.OnGoldChanging(value);
					this.SendPropertyChanging();
					this._gold = value;
					this.SendPropertyChanged("Gold");
					this.OnGoldChanged();
				}
			}
		}
		
		[Column(Storage="_packageSize", Name="PackageSize", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int PackageSize
		{
			get
			{
				return this._packageSize;
			}
			set
			{
				if ((_packageSize != value))
				{
					this.OnPackageSizeChanging(value);
					this.SendPropertyChanging();
					this._packageSize = value;
					this.SendPropertyChanged("PackageSize");
					this.OnPackageSizeChanged();
				}
			}
		}
		
		[Column(Storage="_userID", Name="UserID", DbType="bigint(20)", IsPrimaryKey=true, AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public long UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				if ((_userID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._userID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[Column(Storage="_userPackage", Name="UserPackage", DbType="text", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string UserPackage
		{
			get
			{
				return this._userPackage;
			}
			set
			{
				if (((_userPackage == value) 
							== false))
				{
					this.OnUserPackageChanging(value);
					this.SendPropertyChanging();
					this._userPackage = value;
					this.SendPropertyChanged("UserPackage");
					this.OnUserPackageChanged();
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
	
	[Table(Name="game_db.TB_PlayerEquip")]
	public partial class TBPLayerEquip : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _userEquipValues;
		
		private long _userID;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnUserEquipValuesChanged();
		
		partial void OnUserEquipValuesChanging(string value);
		
		partial void OnUserIDChanged();
		
		partial void OnUserIDChanging(long value);
		#endregion
		
		
		public TBPLayerEquip()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_userEquipValues", Name="UserEquipValues", DbType="text", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string UserEquipValues
		{
			get
			{
				return this._userEquipValues;
			}
			set
			{
				if (((_userEquipValues == value) 
							== false))
				{
					this.OnUserEquipValuesChanging(value);
					this.SendPropertyChanging();
					this._userEquipValues = value;
					this.SendPropertyChanged("UserEquipValues");
					this.OnUserEquipValuesChanged();
				}
			}
		}
		
		[Column(Storage="_userID", Name="UserID", DbType="bigint(20)", IsPrimaryKey=true, AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public long UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				if ((_userID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._userID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
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
	
	[Table(Name="game_db.TB_PlayerHero")]
	public partial class TBPLayerHero : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _equips;
		
		private int _exp;
		
		private int _heroID;
		
		private long _id;
		
		private int _level;
		
		private string _magics;
		
		private long _userID;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnEquipsChanged();
		
		partial void OnEquipsChanging(string value);
		
		partial void OnExpChanged();
		
		partial void OnExpChanging(int value);
		
		partial void OnHeroIDChanged();
		
		partial void OnHeroIDChanging(int value);
		
		partial void OnIDChanged();
		
		partial void OnIDChanging(long value);
		
		partial void OnLevelChanged();
		
		partial void OnLevelChanging(int value);
		
		partial void OnMagicsChanged();
		
		partial void OnMagicsChanging(string value);
		
		partial void OnUserIDChanged();
		
		partial void OnUserIDChanging(long value);
		#endregion
		
		
		public TBPLayerHero()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_equips", Name="Equips", DbType="text", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string Equips
		{
			get
			{
				return this._equips;
			}
			set
			{
				if (((_equips == value) 
							== false))
				{
					this.OnEquipsChanging(value);
					this.SendPropertyChanging();
					this._equips = value;
					this.SendPropertyChanged("Equips");
					this.OnEquipsChanged();
				}
			}
		}
		
		[Column(Storage="_exp", Name="Exp", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int Exp
		{
			get
			{
				return this._exp;
			}
			set
			{
				if ((_exp != value))
				{
					this.OnExpChanging(value);
					this.SendPropertyChanging();
					this._exp = value;
					this.SendPropertyChanged("Exp");
					this.OnExpChanged();
				}
			}
		}
		
		[Column(Storage="_heroID", Name="HeroID", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int HeroID
		{
			get
			{
				return this._heroID;
			}
			set
			{
				if ((_heroID != value))
				{
					this.OnHeroIDChanging(value);
					this.SendPropertyChanging();
					this._heroID = value;
					this.SendPropertyChanged("HeroID");
					this.OnHeroIDChanged();
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
		
		[Column(Storage="_level", Name="Level", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if ((_level != value))
				{
					this.OnLevelChanging(value);
					this.SendPropertyChanging();
					this._level = value;
					this.SendPropertyChanged("Level");
					this.OnLevelChanged();
				}
			}
		}
		
		[Column(Storage="_magics", Name="Magics", DbType="text", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string Magics
		{
			get
			{
				return this._magics;
			}
			set
			{
				if (((_magics == value) 
							== false))
				{
					this.OnMagicsChanging(value);
					this.SendPropertyChanging();
					this._magics = value;
					this.SendPropertyChanged("Magics");
					this.OnMagicsChanged();
				}
			}
		}
		
		[Column(Storage="_userID", Name="UserID", DbType="bigint(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public long UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				if ((_userID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._userID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
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
