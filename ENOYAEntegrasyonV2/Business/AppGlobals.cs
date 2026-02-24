using ENOYAEntegrasyonV2.Models.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ENOYAEntegrasyonV2.Business
{
    public class AppGlobals
    {
        public static bool saveServiceFile = false;
        public static AppSettings appSettings { get; set; }
        public static DateTime malzemeTarihi { get; set; } = DateTime.Today.AddDays(-7);
        public static List<SiloAdlari> siloList { get; set; } = SiloAdlari.siloTanimlari();
    }
    public static class ExtensionMethods
    {
        public static void AddToDataSet(DataSet set, object value)
        {
            if (set == null)
                //throw new ArgumentNullException((set));

                if (value == null)
                    return;

            var type = value.GetType();
            var table = set.Tables[type.FullName];
            if (table == null)
            {
                // create one table per type (name)
                table = new DataTable(type.FullName);
                set.Tables.Add(table);
                foreach (var prop in type.GetProperties().Where(p => p.CanRead))
                {
                    if (IsEnumerable(prop))
                        continue;

                    var col = new DataColumn(prop.Name, prop.PropertyType);
                    table.Columns.Add(col);
                }
            }

            var row = table.NewRow();
            foreach (var prop in type.GetProperties().Where(p => p.CanRead))
            {
                object propValue = prop.GetValue(value);
                if (IsEnumerable(prop))
                {
                    if (propValue != null)
                    {
                        foreach (var child in (ICollection)propValue)
                        {
                            AddToDataSet(set, child);
                        }
                    }
                    continue;
                }

                row[prop.Name] = propValue;
            }
            table.Rows.Add(row);
        }

        private static bool IsEnumerable(PropertyInfo pi)
        {
            // note: we could also use IEnumerable (but string, arrays are IEnumerable...)
            return typeof(ICollection).IsAssignableFrom(pi.PropertyType);
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                try
                {


                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            //ChangeType<>(dr[column.ColumnName])
                            pro.SetValue(obj, EofColumnValue(pro, dr[column.ColumnName]), null); //pro.SetValue(obj, EofColumnValue(column, dr[column.ColumnName]), null);
                        }
                        else
                            continue;
                    }
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show(column.ColumnName);
                    throw;
                }
            }
            return obj;
        }

        private static object EofColumnValue(PropertyInfo pro, object _value)
        {
            try
            {
                if (String.IsNullOrEmpty(_value.ToString()))
                {
                    _value = 0;
                }
                if (pro.PropertyType.Name.ToUpper().Contains("INT64"))
                {
                    try
                    {
                        _value = ExtensionMethods.ChangeType<Int64>(_value);
                    }
                    catch (Exception exc)
                    {
                        _value = 0;
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("INT32"))
                {
                    try
                    {
                        _value = ExtensionMethods.ChangeType<Int32>(_value);
                    }
                    catch (Exception exc)
                    {
                        _value = 0;
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("INT16"))
                {
                    try
                    {
                        _value = ExtensionMethods.ChangeType<Int16>(_value);
                    }
                    catch (Exception exc)
                    {
                        _value = 0;
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("STR"))
                {
                    try
                    {
                        _value = _value == DBNull.Value ? "" : _value.ToString();
                    }
                    catch (Exception exc)
                    {
                        _value = 0;
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("DOU"))
                {
                    try
                    {
                        _value = ExtensionMethods.ChangeType<Double>(_value);
                    }
                    catch (Exception exc)
                    {
                        _value = 0;
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("DEC"))
                {
                    try
                    {
                        _value = ExtensionMethods.ChangeType<Decimal>(_value);//_value.ToDbl();
                    }
                    catch (Exception exc)
                    {
                        _value = 0;
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("DATE"))
                {
                    try
                    {
                        _value = _value.ToDate();
                    }
                    catch (Exception exc)
                    {
                        _value = Convert.ToDateTime("01.01.1901");
                    }
                }
                else
                if (pro.PropertyType.Name.ToUpper().Contains("BOOL"))
                {
                    try
                    {
                        _value = ExtensionMethods.ChangeType<Boolean>(_value);//_value.ToBool();
                    }
                    catch (Exception exc)
                    {
                        _value = false;
                    }
                }
            }
            catch (Exception exc)
            {
                //MethodBase m = MethodBase.GetCurrentMethod();
                //EEL.LogTransactionLineBaseA(m.ReflectedType.Name + "." + m.Name, exc.Message);
            }
            return _value;

        }

        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }
            else if (String.IsNullOrEmpty(value.ToString()))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, t);
        }

        public static DateTime ToDate(this object _obj)
        {
            DateTime rv = Convert.ToDateTime("01.01.1901");
            try
            {
                if (_obj == null || _obj == DBNull.Value)
                    return rv;
                DateTime.TryParse(_obj.ToString(), out rv);
            }
            catch (Exception)
            {
                rv = Convert.ToDateTime("01.01.1901");
            }

            return rv;
        }

        public static Int32 ToInt(this object _obj)
        {
            int rv = 0;
            try
            {
                if (_obj == null || _obj == DBNull.Value)
                    rv = 0;
                else Int32.TryParse(_obj.ToString(), out rv);
            }
            catch (Exception)
            {
                rv = 0;
            }
            return rv;
        }

        public static Int16 ToInt16(this object _obj)
        {
            Int16 rv = 0;
            try
            {
                if (_obj == null || _obj == DBNull.Value)
                    rv = 0;
                else Int16.TryParse(_obj.ToString(), out rv);
            }
            catch (Exception)
            {
                rv = 0;
            }
            return rv;
        }

        public static double ToDbl(this object _obj)
        {
            double rv = 0.00;
            try
            {
                if (_obj == null || _obj == DBNull.Value)
                    return rv = 0.00;
                else Double.TryParse(_obj.ToString(), out rv);
            }
            catch (Exception)
            {
                rv = 0.00;
            }
            return rv;
        }

        public static Decimal ToDecimal(this object _obj)
        {
            Decimal rv = 0;
            try
            {
                if (_obj == null || _obj == DBNull.Value)
                    return rv = 0;
                else Decimal.TryParse(_obj.ToString(), out rv);
            }
            catch (Exception)
            {
                rv = 0;
            }
            return rv;
        }

        public static string TarihStr(this DateTime _tarCevir, string _ayrac)
        {
            string str1 = _tarCevir.Year.ToString();
            if (str1.Length < 4)
                str1 = new string('0', 4 - str1.Length) + str1;
            int num = _tarCevir.Month;
            string str2 = num.ToString();
            if (str2.Length < 2)
                str2 = new string('0', 2 - str2.Length) + str2;
            num = _tarCevir.Day;
            string str3 = num.ToString();
            if (str3.Length < 2)
                str3 = new string('0', 2 - str3.Length) + str3;
            num = _tarCevir.Hour;
            string str4 = num.ToString();
            if (str4.Length < 2)
                str4 = new string('0', 2 - str4.Length) + str4;
            num = _tarCevir.Minute;
            string str5 = num.ToString();
            if (str5.Length < 2)
                str5 = new string('0', 2 - str5.Length) + str5;
            return str3 + _ayrac + str2 + _ayrac + str1;// + " " + str4 + ":" + str5;
        }

        public static string ToStr(this object _obj)
        {
            string rv = "";
            if (_obj == null || _obj == DBNull.Value)
                return rv = "";
            else rv = _obj.ToString();
            return rv;
        }

        public static bool ToBool(this object _obj)
        {
            bool rv = false;
            try
            {
                if (_obj == null || _obj == DBNull.Value)
                    return rv = false;
                Boolean.TryParse(Convert.ToBoolean(_obj).ToString(), out rv);
            }
            catch (Exception)
            {
                rv = false;
            }
            return rv;
        }

    }

}
