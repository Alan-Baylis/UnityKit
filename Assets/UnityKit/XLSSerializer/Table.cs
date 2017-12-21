using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    public class Table {
        public string this[int row, int col] {
            get { return _table[row,col]; }
            set { _table[row, col] = value; }
        }

        public string name { get; set; }
        public int nrows { get; private set; }
        public int ncols { get; private set; }

        private string[,] _table = null;
        public Table(string[,] data) {
            this._table = data;
            this.nrows = _table.GetLength(0);
            this.ncols = _table.GetLength(1);
        }
    }
}
