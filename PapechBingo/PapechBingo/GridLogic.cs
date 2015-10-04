using System;

namespace PapechBingo {
    public class GridLogic {
        public const int GRID_SIZE = 5;
        public static GridLogic instance = new GridLogic();
        
        private bool[,] grid;

        private GridLogic() {
            grid = new bool[GRID_SIZE, GRID_SIZE];
            Reset();
        }

        public Tuple<bool, bool> ToggleButton(int x, int y) {
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
            return Tuple.Create<bool, bool>(grid[x, y], bingo);
        }

        public void Reset() {
            for (var i = 0; i < GRID_SIZE; ++i)
                for (var j = 0; j < GRID_SIZE; ++j)
                    grid[i, j] = false;
        }

        public void FillData(bool[] data) {
            for (var i = 0; i < GRID_SIZE; ++i)
                for (var j = 0; j < GRID_SIZE; ++j)
                    grid[i, j] = data[GRID_SIZE * i + j];
        }

        public bool[] ExtractData() {
            var outData = new bool[GRID_SIZE * GRID_SIZE];
            for (var i = 0; i < GRID_SIZE; ++i)
                for (var j = 0; j < GRID_SIZE; ++j)
                    outData[GRID_SIZE * i + j] = grid[i, j];
            return outData; 
        }
    }
}

