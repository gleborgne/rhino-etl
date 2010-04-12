
namespace Rhino.Etl.Core.Reactive
{
    /// <summary>
    /// Helper methods for join operation on rows
    /// </summary>
    public class RowJoinHelper
    {
        private string _fieldName;

        /// <summary>
        /// Constructor of the helper
        /// </summary>
        /// <param name="fieldName">name of the field to join</param>
        public RowJoinHelper(string fieldName)
        {
            _fieldName = fieldName;    
        }

        /// <summary>
        /// Check for a full join
        /// </summary>
        /// <param name="leftRow">row from main line</param>
        /// <param name="rightRow">row from joined line</param>
        /// <returns>true if join match</returns>
        public bool FullJoinMatch(Row leftRow, Row rightRow)
        {
            return 
                rightRow == null || 
                leftRow[_fieldName] == null || 
                rightRow[_fieldName] == null || 
                Equals(leftRow[_fieldName], rightRow[_fieldName]);
        }

        /// <summary>
        /// Check for an inner join
        /// </summary>
        /// <param name="leftRow">row from main line</param>
        /// <param name="rightRow">row from joined line</param>
        /// <returns>true if join match</returns>
        public bool InnerJoinMatch(Row leftRow, Row rightRow)
        {
            if (rightRow == null)
                return false;

            return Equals(leftRow[_fieldName], rightRow[_fieldName]);
        }

        /// <summary>
        /// Check for a left join
        /// </summary>
        /// <param name="leftRow">row from main line</param>
        /// <param name="rightRow">row from joined line</param>
        /// <returns>true if join match</returns>
        public bool LeftJoinMatch(Row leftRow, Row rightRow)
        {
            return 
                rightRow == null ||
                rightRow[_fieldName] == null || 
                Equals(leftRow[_fieldName], rightRow[_fieldName]);
        }

        /// <summary>
        /// Check for a right join
        /// </summary>
        /// <param name="leftRow">row from main line</param>
        /// <param name="rightRow">row from joined line</param>
        /// <returns>true if join match</returns>
        public bool RightJoinMatch(Row leftRow, Row rightRow)
        {
            if (rightRow == null)
                return false;

            return Equals(leftRow[_fieldName], rightRow[_fieldName]) || leftRow[_fieldName] == null;
        }

        /// <summary>
        /// Merge the content of left and right row
        /// </summary>
        /// <param name="leftRow">row from main line</param>
        /// <param name="rightRow">row from joined line</param>
        /// <returns>merged row</returns>
        public static Row MergeRows(Row leftRow, Row rightRow)
        {
            if (rightRow == null)
                return leftRow;

            if (leftRow == null)
                return rightRow;

            leftRow.Copy(rightRow);

            return leftRow;
        }
    }
}
