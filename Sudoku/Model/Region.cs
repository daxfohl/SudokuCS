namespace Sudoku.Model {
    public enum RegionType {
        Column,
        Row,
        Square
    }

    public struct Region {
        public readonly int I;
        public readonly RegionType Type;
        public Region(RegionType type, int i) {
            Type = type;
            I = i;
        }
    }
}