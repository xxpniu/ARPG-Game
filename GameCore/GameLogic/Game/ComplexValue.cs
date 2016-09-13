using System;
using System.Collections.Generic;
using Proto;

namespace GameLogic.Game
{
	public class ValueChanageEventArgs:EventArgs
	{
		public int OldValue{ private set; get;}
		public int NewValue{ private set; get;}
		public int FinalValue{ set; get;}

		public ValueChanageEventArgs(int oldValue, int newValue,int finalValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
			FinalValue = finalValue;
		}
	}

	public sealed class ComplexValue
	{
		public ComplexValue ():this(0,0,0)
		{
			
		}

		public ComplexValue(int baseValue, int appendValue, int rate)
		{
			BaseValue = baseValue;
			AppendValue = appendValue;
			Rate = rate;
		}
			

		public int BaseValue{ private set; get;}
		public int AppendValue{ private set; get;}
		/// <summary>
		/// 万分比 r/10000
		/// </summary>
		/// <value>The rate.</value>
		public int Rate{ private set; get;}

		public void SetBaseValue(int value)
		{
			if (OnBaseValueChange != null) {
				var args = new ValueChanageEventArgs (BaseValue,value,value);
				OnBaseValueChange (this, args);
				BaseValue = args.FinalValue;
                OnValueChange(this, new EventArgs());
			} else {
				BaseValue = value;
			}
		}

		public void SetAppendValue(int value)
		{
			if (OnAppendValueChange != null) {
				var args = new ValueChanageEventArgs (AppendValue,value,value);
				OnAppendValueChange (this, args);
				AppendValue = args.FinalValue;
                OnValueChange(this, new EventArgs());
			} else {
				AppendValue = value;
			}
		}

		public void SetRate(int value)
		{
			if (OnRateChange != null) {
				var args = new ValueChanageEventArgs (Rate,value,value);
				OnRateChange (this, args);
				Rate = args.FinalValue;
                OnValueChange(this, new EventArgs());
			} else {
				Rate = value;
			}
		}

		public int FinalValue{
			get
			{
				float value = (float)(BaseValue + AppendValue) * (1 + ((float)Rate/10000f));
			    return (int)value;
			}
		}

		public  EventHandler<ValueChanageEventArgs> OnBaseValueChange;
		public  EventHandler<ValueChanageEventArgs> OnAppendValueChange;
		public  EventHandler<ValueChanageEventArgs> OnRateChange;
        public  EventHandler<EventArgs> OnValueChange;

		static public implicit operator ComplexValue(int value)
		{
			return new ComplexValue (value, 0, 0);
		}

		static public explicit operator int(ComplexValue value)
		{
			return value.FinalValue;
		}

		static public bool operator ==(ComplexValue r, ComplexValue l)
		{
			return r.FinalValue == l.FinalValue;
		}

		static public bool operator !=(ComplexValue r, ComplexValue l)
		{
			return r.FinalValue != l.FinalValue;
		}

		public override int GetHashCode ()
		{
			return FinalValue.GetHashCode();
		}

		public override bool Equals (object obj)
		{
			if (!(obj is ComplexValue))
				return false;
			var temp = obj as ComplexValue;
			return temp.FinalValue == this.FinalValue;
		}
	}

   
}

