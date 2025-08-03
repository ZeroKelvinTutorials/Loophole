namespace core
{
    public static class BoxUtility
    {
        public static bool CanMove(Board board, Box box, V2Int direction)
        {
            V2Int targetPosition = box.position + direction;

            //If theres another box in target position, box is not movable...
            foreach (V2Int position in board.boxes.Keys)
            {
                if (targetPosition == position)
                {
                    return false;
                }
            }

            //Todo edge cases for self box or player but current width/height wont allow it

            switch (board.GetTile(targetPosition.Loop(board.width, board.height)).tileType)
            {
                case TileType.Rock:
                    return false;
                default:
                    return true;
            }
        }
    }
}
