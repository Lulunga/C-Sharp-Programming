using System;
using System.Collections.Generic;

namespace Generics.Tables
{
    public class Cell<TypeRows, TypeColumns, TypeCells>
        where TypeRows : IComparable
        where TypeColumns : IComparable
        where TypeCells : IComparable
    {
        public TypeRows Row { get; }
        public TypeColumns Column { get; }
        public TypeCells Data { get; set; }

        public Cell(TypeRows row, TypeColumns column, TypeCells data) =>
            (Row, Column, Data) = (row, column, data);
    }

    public class RowsClass<TypeRows>
        where TypeRows : IComparable
    {
        public HashSet<TypeRows> Indexes { get; } = new HashSet<TypeRows>();

        public int Count() => Indexes.Count;

        public bool HasIndex(TypeRows p) => Indexes.Contains(p);
    }

    public class ColumnsClass<TypeColumns>
        where TypeColumns : IComparable
    {
        public HashSet<TypeColumns> Indexes { get; } = new HashSet<TypeColumns>();

        public int Count() => Indexes.Count;

        public bool HasIndex(TypeColumns p) => Indexes.Contains(p);
    }

    public class COpen<TypeRows, TypeColumns, TypeCells>
        where TypeRows : IComparable
        where TypeColumns : IComparable
        where TypeCells : IComparable
    {
        public Table<TypeRows, TypeColumns, TypeCells> Reference { get; set; }

        public TypeCells this[TypeRows indexRow, TypeColumns indexColumn]
        {
            get
            {
                int index = Reference.GetCellIndex(indexRow, indexColumn);

                if (Reference.Columns.HasIndex(indexColumn) && Reference.Rows.HasIndex(indexRow))
                    return (index != -1) ? Reference.Cells[index].Data : default(TypeCells);

                return default(TypeCells);
            }
            set
            {
                int index = Reference.GetCellIndex(indexRow, indexColumn);

                if (index == -1)
                {
                    Reference.Cells.Add(new Cell<TypeRows, TypeColumns, TypeCells>(indexRow, indexColumn, value));

                    if (!Reference.Rows.HasIndex(indexRow))
                        Reference.Rows.Indexes.Add(indexRow);

                    if (!Reference.Columns.HasIndex(indexColumn))
                        Reference.Columns.Indexes.Add(indexColumn);
                }
                else
                {
                    Reference.Cells[index].Data = value;
                }
            }
        }
    }

    public class CExisted<TypeRows, TypeColumns, TypeCells>
    where TypeRows : IComparable
    where TypeColumns : IComparable
    where TypeCells : IComparable
    {
        public Table<TypeRows, TypeColumns, TypeCells> Reference { get; set; }

        public TypeCells this[TypeRows indexRow, TypeColumns indexColumn]
        {
            get
            {
                if (Reference.Rows.HasIndex(indexRow) && Reference.Columns.HasIndex(indexColumn))
                {
                    int index = Reference.GetCellIndex(indexRow, indexColumn);
                    return (index != -1) ? Reference.Cells[index].Data : default(TypeCells);
                }

                throw new ArgumentException($"No element at [{indexRow}][{indexColumn}]");
            }
            set
            {
                if (Reference.Rows.HasIndex(indexRow) && Reference.Columns.HasIndex(indexColumn))
                {
                    int index = Reference.GetCellIndex(indexRow, indexColumn);

                    if (index == -1)
                    {
                        Reference.Cells.Add(new Cell<TypeRows, TypeColumns, TypeCells>(indexRow, indexColumn, value));

                        if (!Reference.Columns.HasIndex(indexColumn))
                            Reference.Columns.Indexes.Add(indexColumn);
                    }
                    else
                        Reference.Cells[index].Data = value;
                }
                else
                    throw new ArgumentException($"Trying to set a value for a " +
                        $"non-existing row or column at [{indexRow}][{indexColumn}]");
            }
        }
    }

    public class Table<TypeRows, TypeColumns, TypeCells>
        where TypeRows : IComparable
        where TypeColumns : IComparable
        where TypeCells : IComparable
    {
        public RowsClass<TypeRows> Rows { get; } = new RowsClass<TypeRows>();
        public ColumnsClass<TypeColumns> Columns { get; } = new ColumnsClass<TypeColumns>();
        public List<Cell<TypeRows, TypeColumns, TypeCells>> Cells { get; } =
            new List<Cell<TypeRows, TypeColumns, TypeCells>>();
        public COpen<TypeRows, TypeColumns, TypeCells> Open { get; } =
            new COpen<TypeRows, TypeColumns, TypeCells>();
        public CExisted<TypeRows, TypeColumns, TypeCells> Existed { get; } =
            new CExisted<TypeRows, TypeColumns, TypeCells>();

        public void AddColumn(TypeColumns indexColumn)
        {
            Columns.Indexes.Add(indexColumn);
        }

        public void AddRow(TypeRows indexRow)
        {
            Rows.Indexes.Add(indexRow);
        }

        public int GetCellIndex(TypeRows indexRow, TypeColumns indexColumn) =>
            Cells.FindIndex(cell => cell.Row.Equals(indexRow) && cell.Column.Equals(indexColumn));

        public Table()
        {
            Open.Reference = this;
            Existed.Reference = this;
        }
    }
}