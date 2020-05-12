namespace CliNetCore.Interfaces
{
    /// <summary>
    /// 명령어 실행 인터페이스.
    /// </summary>
    public interface IAction
    {
        #region Properties

        /// <summary>
        /// 유효성.
        /// </summary>
        bool IsValid { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 명령어 수행.
        /// </summary>
        /// <returns></returns>
        int Action();

        #endregion
    }
}
