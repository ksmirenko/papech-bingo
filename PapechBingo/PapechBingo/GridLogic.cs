using System.Text;

namespace PapechBingo {
    public class GridLogic {
        public const int GridSize = 5;
        public static GridLogic Instance { get; } = new GridLogic();

        private readonly bool[,] _grid;

        private GridLogic() {
            _grid = new bool[GridSize, GridSize];
        }

        public bool ToggleButton(int index) {
            return ToggleButton(index / GridSize, index % GridSize);
        }
        public bool ToggleButton(int x, int y) {
            // toggling cell state
            _grid[x, y] = !_grid[x, y];
            var bingo = false;
            if (!_grid[x, y]) return bingo;
            // checking bingo
            var bingoRow = true;
            for (var j = 0; j < GridSize; ++j)
                bingoRow &= _grid[x, j];
            var bingoColumn = true;
            for (var i = 0; i < GridSize; ++i)
                bingoColumn &= _grid[i, y];
            var bingoDiag1 = false;
            if (x == y) {
                bingoDiag1 = true;
                for (var i = 0; i < GridSize; ++i)
                    bingoDiag1 &= _grid[i, i];
            }
            var bingoDiag2 = false;
            if (x == GridSize - y - 1) {
                bingoDiag2 = true;
                for (var i = 0; i < GridSize; ++i)
                    bingoDiag2 &= _grid[i, GridSize - i - 1];
            }
            bingo = bingoRow || bingoColumn || bingoDiag1 || bingoDiag2;
            return bingo;
        }
        public void Reset() {
            for (var i = 0; i < GridSize; ++i)
                for (var j = 0; j < GridSize; ++j)
                    _grid[i, j] = false;
        }
        public void FillData(string data) {
            for (var i = 0; i < GridSize; ++i)
                for (var j = 0; j < GridSize; ++j)
                    _grid[i, j] = data[GridSize * i + j] == '1';
        }
        public string ExtractData() {
            var outData = new StringBuilder();
            for (var i = 0; i < GridSize; ++i)
                for (var j = 0; j < GridSize; ++j)
                    outData.Append(_grid[i, j] ? '1' : '0');
            return outData.ToString();
        }
    }
}

