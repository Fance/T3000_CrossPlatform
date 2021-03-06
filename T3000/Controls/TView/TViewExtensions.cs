﻿namespace T3000
{
    using Controls;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class TViewExtensions
    {
        public static readonly Color DisabledBackColor = SystemColors.ControlDark;
        public static readonly Color DisabledForeColor = SystemColors.ControlDarkDark;
        public static readonly Color DisabledSelectionBackColor = SystemColors.ControlDark;

        public static bool IsMono() => Type.GetType("Mono.Runtime") != null;

        public static DataGridViewCell GetCell(this DataGridViewRow row,
            DataGridViewColumn column) =>
            row.Cells[column.Index];

        public static void SetCell(this DataGridViewRow row,
            DataGridViewColumn column, DataGridViewCell cell)
        {
            //Mono fix
            if (IsMono())
            {
                row.Cells.RemoveAt(column.Index);
            }

            row.Cells[column.Index] = cell;
        }

        public static T GetValue<T>(this DataGridViewRow row, DataGridViewColumn column)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var value = row.GetCell(column).Value;
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

        public static void SetValue<T>(this DataGridViewRow row, DataGridViewColumn column, T value = default(T), bool withoutValidation = false)
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
            var view = ((TView) row.DataGridView);
            var temp = view.ValidationEnabled;
            if (withoutValidation)
            {
                view.ValidationEnabled = false;
            }

            column.ValueType = value.GetType();
            row.GetCell(column).Value = value;

            if (withoutValidation)
            {
                view.ValidationEnabled = temp;
            }
        }

        //public static T GetValue<T>(this DataGridViewRow row, DataGridViewColumn column) =>
        //    row.GetValue<T>(column.Name);

        //public static void SetValue<T>(this DataGridViewRow row, DataGridViewColumn column, 
        //    T value = default(T)) =>
        //    row.SetValue(column.Name, value);


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
    }
}
