﻿namespace T3000
{
    using Controls;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;

    public static class TViewExtensions
    {
        public static readonly Color DisabledBackColor = SystemColors.ControlDark;
        public static readonly Color DisabledForeColor = SystemColors.ControlDarkDark;
        public static readonly Color DisabledSelectionBackColor = SystemColors.ControlDark;

        public static T GetValue<T>(this DataGridViewRow row, string columnName)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var value = row.Cells[columnName].Value;
            /* Need, but slowly
            if (value.GetType() != typeof(T))
            {
                throw new InvalidCastException($@"Invalid cast. 
ColumnName: {columnName}
Actual type: {value.GetType()}
Cast type: {typeof(T)}");
            }
            */

            return (T)value;
        }

        public static void SetValue<T>(this DataGridViewRow row, string columnName, T value = default(T))
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            /* Need, but slowly
            var actualValue = row.Cells[columnName].Value;
            if (actualValue != null && actualValue.GetType() != typeof(T))
            {
                throw new ArgumentException($@"row.SetValue: Trying to install a different type. 
ColumnName: {columnName}
Actual type: {row.Cells[columnName].Value.GetType()}
Value type: {typeof(T)}");
            }
            */
            row.Cells[columnName].Value = value;
        }

        public static T GetValue<T>(this DataGridViewRow row, DataGridViewColumn column) =>
            row.GetValue<T>(column.Name);

        public static void SetValue<T>(this DataGridViewRow row, DataGridViewColumn column, T value = default(T)) =>
            row.SetValue(column.Name, value);


        /// <summary>
        /// Returns null if index not valid
        /// </summary>
        /// <param name="view"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static DataGridViewRow GetRow(this TView view, int index)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (!view.RowIndexIsValid(index))
            {
                return null;
            }

            return view.Rows[index];
        }

        public static void Enable(this DataGridViewCell cell, bool enabled)
        {
            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }

            cell.ReadOnly = !enabled;
            cell.Style.BackColor = enabled
                ? cell.OwningColumn.DefaultCellStyle.BackColor
                : DisabledBackColor;
            cell.Style.ForeColor = enabled
                ? cell.OwningColumn.DefaultCellStyle.ForeColor
                : DisabledForeColor;
            cell.Style.SelectionBackColor = enabled
                ? cell.OwningColumn.DefaultCellStyle.SelectionBackColor
                : DisabledSelectionBackColor;
        }

        public static string ColumnName(this DataGridViewCell cell)
        {
            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }
            
            return cell.OwningColumn?.Name;
        }
        public static DataGridViewRow CreateRow(this DataGridView view, Dictionary<string, object> values = null)
        {
            var objects = new List<object>();

            foreach (DataGridViewColumn column in view.Columns)
            {
                objects.Add(values != null && values.ContainsKey(column.Name)
                    ? values[column.Name]
                    : null);
            }

            view.Rows.Add(objects);

            return view.Rows[view.RowCount - 1];
        }
    }
}
