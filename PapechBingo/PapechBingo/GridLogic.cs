using System;
using System.Text;

namespace PapechBingo {
    public class GridLogic {
        public const int GRID_SIZE = 5;
        public static GridLogic Instance {
            get { return instance; }
        }

        private static GridLogic instance = new GridLogic();
        private bool[,] grid;

        private GridLogic() {
            grid = new bool[GRID_SIZE, GRID_SIZE];
            Reset();
        }

        public bool ToggleButton(int index) {
            return ToggleButton(index / GRID_SIZE, index % GRID_SIZE);
        }
        public bool ToggleButton(int x, int y) {
            // toggling cell state
            grid[x, y] = !grid[x, y];
            var bingo = false;
            if (grid[x, y]) {
                // checking bingo
                var bingoRow = true;
                for (var j = 0; j < GRID_SIZE; ++j)
                    bingoRow &= grid[x, j];
                var bingoColumn = true;
                for (var i = 0; i < GRID_SIZE; ++i)
                    bingoColumn &= grid[i, y];
                var bingoDiag1 = false;
                if (x == y) {
                    bingoDiag1 = true;
                    for (var i = 0; i < GRID_SIZE; ++i)
                        bingoDiag1 &= grid[i, i];
                }
                var bingoDiag2 = false;
                if (x == GRID_SIZE - y - 1) {
                    bingoDiag2 = true;
                    for (var i = 0; i < GRID_SIZE; ++i)
                        bingoDiag2 &= grid[i, GRID_SIZE - i - 1];
                }
                bingo = bingoRow || bingoColumn || bingoDiag1 || bingoDiag2;
            }
            return bingo;
        }
        public void Reset() {
            for (var i = 0; i < GRID_SIZE; ++i)
                for (var j = 0; j < GRID_SIZE; ++j)
                    grid[i, j] = false;
        }
        public void FillData(string data) {
            for (var i = 0; i < GRID_SIZE; ++i)
                for (var j = 0; j < GRID_SIZE; ++j)
                    grid[i, j] = data[GRID_SIZE * i + j] == '1';
        }
        public string ExtractData() {
            var outData = new StringBuilder();
            for (var i = 0; i < GRID_SIZE; ++i)
                for (var j = 0; j < GRID_SIZE; ++j)
                    outData.Append(grid[i, j] ? '1' : '0');
            return outData.ToString();
        }
    }
}

